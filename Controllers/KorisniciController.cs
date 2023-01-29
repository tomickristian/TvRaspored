using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TvRaspored.Models;
using TvRaspored.ViewModels;
using TvRaspored.Selection;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using TvRaspored.XmlKorisnikClass;
using System.Xml;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TvRaspored.Controllers
{
    public class KorisniciController : Controller
    {
        private readonly AppDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public KorisniciController(AppDbContext context, 
                                    UserManager<IdentityUser> userManager,
                                    SignInManager<IdentityUser> signInManager,
                                    RoleManager<IdentityRole> roleManager,
                                    IWebHostEnvironment hostingEnvironment)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public string EncryptString(string lozinka)
        {
            //enkripcija lozinke
            SHA1CryptoServiceProvider sh = new SHA1CryptoServiceProvider();
            sh.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lozinka));
            var re = sh.Hash;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in re)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult ImportXml()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> ImportXml(XmlViewModel model)
        {
            if (model.XmlFile.ContentType.Equals("application/xml") || model.XmlFile.ContentType.Equals("text/xml"))
            {
                try
                {
                    //spremanje XML datoteke
                    string fileName = model.XmlFile.FileName;
                    var filePath = Path.Combine(hostingEnvironment.WebRootPath, "xmls", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.XmlFile.CopyToAsync(stream);
                        stream.Close();
                    }

                    XDocument xdocument = XDocument.Load(filePath);
                    IEnumerable<XElement> korisnici = xdocument.Elements();
                    foreach (var k in xdocument.Root.Elements())
                    {
                        var email = "";
                        if (k.Element("email").Value != null)
                        {
                            email = k.Element("email").Value;
                        }
                        var korisnicko_ime = k.Element("korisnicko_ime").Value;
                        var ime = k.Element("ime").Value;
                        var prezime = k.Element("prezime").Value;
                        var slika = k.Element("slika").Value;
                        var tip_id = Int32.Parse(k.Element("tip_id").Value);
                        var lozinka = k.Element("lozinka").Value;
                        var datum = DateTime.Parse(k.Element("datum_prijave").Value);

                        //ako korisnik postoji potrebno je update-at tablicu, a u suprotnom je kreiran
                        if (KorisnikExists(korisnicko_ime))
                        {
                            Korisnik Oldkorisnik = context.Korisnik.First(t => t.KorisnickoIme == korisnicko_ime);

                            //ako je korisnik pod tim username-om bio moderator tv postaje zamjenjen je sa admin-om
                            if (Oldkorisnik.TipId != 2 && tip_id == 2)
                            {
                                foreach (var t in context.Tvpostaja)
                                {
                                    if (t.ModeratorId == Oldkorisnik.KorisnikId)
                                    {
                                        t.ModeratorId = 1;
                                        context.Update(t);
                                    }
                                }
                                context.SaveChanges();
                            }
                            Oldkorisnik.Ime = ime;
                            Oldkorisnik.Prezime = prezime;
                            Oldkorisnik.Email = email;
                            Oldkorisnik.Slika = slika;
                            Oldkorisnik.TipId = tip_id;
                            Oldkorisnik.Lozinka = lozinka;
                            Oldkorisnik.DatumPrijave = datum;
                            context.Update(Oldkorisnik); 
                            context.SaveChanges();
                            var user = await userManager.FindByNameAsync(korisnicko_ime);
                            await userManager.DeleteAsync(user);
                        }
                        else
                        {
                            Korisnik korisnik = new Korisnik()
                            {
                                KorisnickoIme = korisnicko_ime,
                                TipId = tip_id,
                                Ime = ime,
                                Prezime = prezime,
                                Lozinka = lozinka,
                                Email = email,
                                Slika = slika,
                                DatumPrijave = datum
                            };
                            context.Add(korisnik);
                            context.SaveChanges();
                        }
                        var NewUser = new IdentityUser { UserName = korisnicko_ime };
                        await userManager.CreateAsync(NewUser, lozinka);
                        await userManager.AddToRoleAsync(NewUser, context.TipKorisnika.Find(tip_id).Naziv);
                    }
                    return RedirectToAction("Lista");
                }
                catch
                {
                    ViewBag.Error = "Pretvorba neuspješna.";
                }
            }
            else
            {
                ViewBag.Error = "Odaberite '.xml' dokument.";
            }
            return View(model);
        }

        [Authorize(Roles = "administrator")]
        private void ExportKorisnikaXml()
        {
            //export liste korisnika u XML
            List<korisnik> korisnici = new List<korisnik>();
            XmlSerializer serial = new XmlSerializer(typeof(List<korisnik>));
            foreach (Korisnik k in context.Korisnik.ToList())
            {
                //class "korisnik" je klasa iz "XmlKorisnikClass" foldera
                
                korisnici.Add(new korisnik()
                {
                    korisnik_id = k.KorisnikId,
                    tip_id = k.TipId,
                    korisnicko_ime = k.KorisnickoIme,
                    lozinka = k.Lozinka,
                    ime = k.Ime,
                    prezime = k.Prezime,
                    email = k.Email,
                    slika = k.Slika,
                    datum_prijave = k.DatumPrijave
                });
            }
            //spremanje XML-a u aplikacijski prostor (~/TvRaspored/wwwroot/xmls/)
            string path = Path.Combine(hostingEnvironment.WebRootPath, "xmls/korisnici.xml");
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                serial.Serialize(stream, korisnici);
                stream.Close();
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> ExportXml()
        {
            //generiranje XML-a
            ExportKorisnikaXml();
            //upload-anje XML-a
            string path = Path.Combine(hostingEnvironment.WebRootPath, "xmls/korisnici.xml");
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "text/xml", "korisnici.xml");
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Lista()
        {
            var korisnici = context.Korisnik.Select(k => new KorisniciListaSelection()
            {
                Ime = k.Ime,
                Prezime = k.Prezime,
                KorisnickoIme = k.KorisnickoIme,
                KorisnikId = k.KorisnikId,
                Slika = k.Slika,
                TipNaziv = k.Tip.Naziv
            }).ToList();
            return View(korisnici);
        }

        public IActionResult Detalji(string korisnickoIme)
        {
            if (!KorisnikExists(korisnickoIme))
            {
                return View("NotFound");
            }
            var korisnik = context.Korisnik.Include(k => k.Tip).First(m => m.KorisnickoIme == korisnickoIme);
            KorisniciDetaljiViewModel model = new KorisniciDetaljiViewModel()
            {
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                KorisnickoIme = korisnik.KorisnickoIme,
                Tip_korisnika = korisnik.Tip.Naziv,
                Slika = korisnik.Slika,
                Email = korisnik.Email,
                Lozinka = korisnik.Lozinka,
                DatumPrijave = korisnik.DatumPrijave
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Uredi(string korisnickoIme)
        {
            if (!KorisnikExists(korisnickoIme))
            {
                return View("NotFound");
            }
            var korisnik = context.Korisnik.Include(k => k.Tip).First(m => m.KorisnickoIme == korisnickoIme);
            KorisniciUrediViewModel model = new KorisniciUrediViewModel()
            {
                KorisnikId = korisnik.KorisnikId,
                TipId = korisnik.TipId,
                Tipovi = context.TipKorisnika.Where(t => t.TipId != 0).AsEnumerable(),
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                KorisnickoIme = korisnik.KorisnickoIme,
                PutanjaSlike = korisnik.Slika,
                Email = korisnik.Email,
                Lozinka = korisnik.Lozinka,
                DatumPrijave = korisnik.DatumPrijave
            };
            return View(model);
        }
        
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> UrediKorisnickoImeZauzeto(string korisnickoIme, int KorisnikId)
        {
            var user = await userManager.FindByNameAsync(korisnickoIme);
            if (user == null || (context.Korisnik.Find(KorisnikId).KorisnickoIme == korisnickoIme))
            {
                return Json(true);
            }
            else
            {
                return Json($"korisničko ime \"{korisnickoIme}\" je već zauzete.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Uredi(KorisniciUrediViewModel model)
        {
            if (ModelState.IsValid)
            {
                Korisnik korisnik = context.Korisnik.Find(model.KorisnikId);
                var user = await userManager.FindByNameAsync(korisnik.KorisnickoIme);
                if (user == null)
                {
                    return View("NotFound");
                }
                try
                {
                    var oldUserName = user.UserName;
                    user.UserName = model.KorisnickoIme;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (model.Slika != null)
                        {
                            string slikeFolder = Path.Combine(hostingEnvironment.WebRootPath, "images/korisnici");
                            string filePath = Path.Combine(slikeFolder, model.Slika.FileName);
                            model.Slika.CopyTo(new FileStream(filePath, FileMode.Create));
                            model.PutanjaSlike = $"korisnici/{model.Slika.FileName}";
                        }
                        korisnik.Ime = model.Ime;
                        korisnik.Prezime = model.Prezime;
                        korisnik.KorisnickoIme = model.KorisnickoIme;
                        korisnik.Email = model.Email;
                        korisnik.TipId = model.TipId;
                        korisnik.Slika = model.PutanjaSlike;
                        korisnik.DatumPrijave = model.DatumPrijave;

                        context.Update(korisnik);
                        context.SaveChanges();

                        if(model.KorisnickoIme != oldUserName && User.Identity.Name == oldUserName)
                        {
                            await signInManager.SignOutAsync();
                            return RedirectToAction("Prijava", "Korisnici");
                        }
                        return RedirectToAction("Detalji", "Korisnici", new { korisnickoIme = model.KorisnickoIme });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KorisnikExists(model.KorisnickoIme))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult PromjenaLozinke(string korisnickoIme)
        {
            KorisnikPromjenaLozinkeViewModel model = new KorisnikPromjenaLozinkeViewModel()
            {
                KorisnickoIme = korisnickoIme
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PromjenaLozinke(KorisnikPromjenaLozinkeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.KorisnickoIme);
                if (user == null)
                {
                    return View("NotFound");
                }
                var encryptedLozinka = EncryptString(model.Lozinka);
                Korisnik korisnik = context.Korisnik.First(Korisnik => Korisnik.KorisnickoIme == user.UserName);
                
                await userManager.RemovePasswordAsync(user);
                var result = await userManager.AddPasswordAsync(user, model.Lozinka);
                if (result.Succeeded)
                {
                    korisnik.Lozinka = encryptedLozinka;
                    context.Update(korisnik);
                    context.SaveChanges();

                    if(User.Identity.Name == user.UserName)
                    {
                        await signInManager.SignOutAsync();
                        return RedirectToAction("Prijava", "Korisnici");
                    }
                    return RedirectToAction("Detalji", "Korisnici", new { korisnickoIme = model.KorisnickoIme });
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Odjava()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Izbornik", "Tvpostaje");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Prijava()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Prijava(KorisniciPrijavaViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!KorisnikExists(model.KorisnickoIme))
                {
                    ModelState.AddModelError(string.Empty, "Nevažeće korisničko ime.");
                    return View(model);
                }
                var result = await signInManager.PasswordSignInAsync(
                    model.KorisnickoIme,
                    model.Lozinka,
                    model.ZapamtiMe,
                    false);
                Korisnik korisnik = context.Korisnik.First(k => k.KorisnickoIme == model.KorisnickoIme);
                korisnik.DatumPrijave = DateTime.Now;
                context.Update(korisnik);
                context.SaveChanges();

                if (result.Succeeded)
                {
                    if (korisnik.TipId == 2)
                    {
                        return RedirectToAction("Raspored", "Emisije");
                    }
                    else if (korisnik.TipId == 1)
                    {
                        return RedirectToAction("Izbornik", "Tvpostaje");
                    }
                    else if (korisnik.TipId == 0)
                    {
                        return RedirectToAction("Statistika", "Tvpostaje");
                    }
                    return View("NotFound");
                }
                ModelState.AddModelError(string.Empty, "Neispravna lozinka.");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult Dodaj()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> DodajKorisnickoImeZauzeto(string korisnickoIme)
        {
            var user = await userManager.FindByNameAsync(korisnickoIme);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"korisničko ime \"{korisnickoIme}\" je već zauzete.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Dodaj(KorisniciDodajViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DatumPrijave = DateTime.Now;
                var user = new IdentityUser{ UserName = model.KorisnickoIme };
                var result = await userManager.CreateAsync(user, model.Lozinka);
                await userManager.AddToRoleAsync(user, context.TipKorisnika.Find(model.TipId).Naziv);

                if (result.Succeeded)
                {
                    if (model.Slika != null)
                    {
                        string slikeFolder = Path.Combine(hostingEnvironment.WebRootPath, "images/korisnici");
                        string filePath = Path.Combine(slikeFolder, model.Slika.FileName);
                        model.Slika.CopyTo(new FileStream(filePath, FileMode.Create));
                        model.PutanjaSlike = $"korisnici/{model.Slika.FileName}";
                    }
                    else { model.PutanjaSlike = "korisnici/nophoto.jpg"; }
                    var encryptedLozinka = EncryptString(model.Lozinka);
                    
                    Korisnik korisnik = new Korisnik()
                    {
                        KorisnickoIme = model.KorisnickoIme,
                        Ime = model.Ime,
                        Prezime = model.Prezime,
                        Email = model.Email,
                        TipId = model.TipId,
                        Lozinka = encryptedLozinka,
                        DatumPrijave = model.DatumPrijave,
                        Slika = model.PutanjaSlike
                    };
                    context.Add(korisnik);
                    context.SaveChanges();

                    if (signInManager.IsSignedIn(User) && User.IsInRole("administrator"))
                    {
                        return RedirectToAction("Detalji", "Korisnici", new { korisnickoIme = model.KorisnickoIme});
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Izbornik", "Tvpostaje");
                }
            }
            return View();
        }

        private bool KorisnikExists(string korisnickoIme)
        {
            return context.Korisnik.Any(e => e.KorisnickoIme == korisnickoIme);
        }
    }
}
