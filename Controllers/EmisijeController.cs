using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvRaspored.Models;
using TvRaspored.ViewModels;
using TvRaspored.Selection;

namespace TvRaspored.Controllers
{
    public class EmisijeController : Controller
    {
        private readonly AppDbContext context;

        public EmisijeController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Izbornik(int tvPostaja_id, int zanr_id, int sortOrder)
        {
            if (!TvpostajaExists(tvPostaja_id))
            {
                return View("NotFound");
            }
            var tvpostaja = context.Tvpostaja.Find(tvPostaja_id);
            var korisnik_id = context.Korisnik.First(Korisnik => Korisnik.KorisnickoIme == User.Identity.Name).KorisnikId;
            string ZanrNaziv = null;
            var emisije = context.Emisija
                    .Join(context.Zanr,
                        e => e.ZanrId,
                        z => z.ZanrId, (e, z) => new EmisijeIzbornikSelection()
                        {
                            Naziv = e.Naziv,
                            EmisijaId = e.EmisijaId,
                            ZanrId = e.ZanrId,
                            DatumVrijemePocetka = e.DatumVrijemePocetka,
                            TvpostajaId = e.TvpostajaId,
                            ZanrNaziv = z.Naziv
                        }).Where(e => e.TvpostajaId == tvPostaja_id).ToList();
            
            if(zanr_id != 0)
            {
                if (!ZanrExists(zanr_id))
                {
                    return View("NotFound");
                }
                ZanrNaziv = context.Zanr.Find(zanr_id).Naziv;
                emisije = emisije.Where(e => e.ZanrId == zanr_id).ToList();
            }
            string korisnicko_ime = User.Identity.Name;
            var korisnik = context.Korisnik.First(k => k.KorisnickoIme == korisnicko_ime);

            if (sortOrder == 0 || korisnik.TipId == 2) 
            { 
                emisije = emisije.OrderBy(e => e.DatumVrijemePocetka).ToList(); 
            }
            else if (sortOrder == 1) 
            { 
                emisije = emisije.OrderBy(e => e.Naziv).ToList(); 
            }
            else if (sortOrder == 2) 
            { 
                emisije = emisije.OrderBy(e => e.ZanrNaziv).ToList(); 
            }

            EmisijeIzbornikViewModel model = new EmisijeIzbornikViewModel()
            {
                emisije = emisije,
                Zanr_id = zanr_id,
                TvPostaja = tvpostaja,
                KorisnikId = korisnik_id,
                ZanrNaziv = ZanrNaziv
            };
            return View(model);
        }

        public IActionResult Raspored(int sortOrder)
        {
            string korisnicko_ime = User.Identity.Name;
            var korisnik = context.Korisnik.First(k => k.KorisnickoIme == korisnicko_ime);
            var emisije = context.Emisija
                .Join(context.Pretplata,
                    e => e.EmisijaId,
                    p => p.EmisijaId, (e, p) => new 
                    {
                        e.Naziv, e.EmisijaId, p.Status, e.DatumVrijemePocetka, p.KorisnikId, e.ZanrId
                    })
                .Join(context.Zanr,
                    e => e.ZanrId,
                    z => z.ZanrId, (e, z) => new EmisijeRasporedSelection()
                    {
                        Naziv = e.Naziv,
                        EmisijaId = e.EmisijaId,
                        Status = e.Status,
                        DatumVrijemePocetka = e.DatumVrijemePocetka,
                        KorisnikId = e.KorisnikId,
                        ZanrNaziv = z.Naziv
                    }
                    ).Where(e => e.KorisnikId == korisnik.KorisnikId && e.Status == 1).ToList();

            if (sortOrder == 0 || korisnik.TipId == 2) { return View(emisije.OrderBy(e => e.DatumVrijemePocetka).ToList()); }
            else if (sortOrder == 1) { return View(emisije.OrderBy(e => e.Naziv).ToList()); }
            else if (sortOrder == 2) { return View(emisije.OrderBy(e => e.ZanrNaziv).ToList()); }

            return View(emisije);
        }

        [HttpGet]
        public IActionResult Detalji(int id)
        {
            if (!EmisijaExists(id))
            {
                return View("NotFound");
            }
            Emisija emisija = context.Emisija.Find(id);
            int korisnik_id = context.Korisnik.First(k => k.KorisnickoIme == User.Identity.Name).KorisnikId;
            if(!context.Pretplata.Any(p => p.KorisnikId == korisnik_id && p.EmisijaId == id))
            {
                Pretplata pretplataEntity = new Pretplata()
                {
                    EmisijaId = id,
                    Status = 0,
                    KorisnikId = korisnik_id
                };
                context.Add(pretplataEntity);
                context.SaveChanges();
            }
            Pretplata pretplata = context.Pretplata.First(p => p.KorisnikId == korisnik_id && p.EmisijaId == id);

            EmisijaDetaljiViewModel model = new EmisijaDetaljiViewModel()
            {
                EmisijaId = id,
                Naziv = emisija.Naziv,
                Opis = emisija.Opis,
                Status = pretplata.Status,
                korisnik_id = korisnik_id,
                PretplataId = pretplata.PretplataId,
                ModeratorId = context.Tvpostaja.Find(emisija.TvpostajaId).ModeratorId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Detalji(EmisijaDetaljiViewModel model)
        {
            if (ModelState.IsValid)
            {
                Emisija emisija = await context.Emisija
                    .FirstAsync(e => e.EmisijaId == model.EmisijaId);
                Korisnik korisnik = await context.Korisnik.FindAsync(model.korisnik_id);

                if (!PretplataExists(model.korisnik_id, model.EmisijaId))
                {
                    Pretplata pretplataEntity = new Pretplata()
                    {
                        EmisijaId = model.EmisijaId,
                        KorisnikId = model.korisnik_id,
                        Status = model.Status
                    };
                    context.Add(pretplataEntity);
                    context.SaveChanges();
                }
                else
                {
                    Pretplata pretplata = context.Pretplata.Where(p => p.KorisnikId == model.korisnik_id)
                    .First(p => p.EmisijaId == model.EmisijaId);

                    pretplata.Status = model.Status;
                    context.SaveChanges();
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult Dodaj(int tvPostaja_id, int zanr_id)
        {
            if (!TvpostajaExists(tvPostaja_id))
            {
                return View("NotFound");
            }
            else if(zanr_id != 0)
            {
                if (!ZanrExists(zanr_id))
                {
                    return View("NotFound");
                }
            }
            int korisnik_id = context.Korisnik.First(k => k.KorisnickoIme == User.Identity.Name).KorisnikId;
            var tvPostaje = context.Tvpostaja.Where(t => t.ModeratorId == korisnik_id);
            EmisijeDodajUrediViewModel model = new EmisijeDodajUrediViewModel()
            {
                TvpostajaId = tvPostaja_id,
                ZanrId = zanr_id,
                tvPostaje = tvPostaje,
                zanrovi = context.Zanr,
                DatumVrijemePocetka = DateTime.Now.Date
            };
            return View(model);
        }

        [AcceptVerbs("Get", "Post")]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult VrijemePocetkaZauzeto(int EmisijaId, DateTime DatumVrijemePocetka, int TvpostajaId)
        {
            var emisije = context.Emisija.Where(e => e.TvpostajaId == TvpostajaId);
            foreach (Emisija e in emisije)
            {
                if (e.EmisijaId != EmisijaId)
                {
                    if (DatumVrijemePocetka.CompareTo(e.DatumVrijemePocetka) >= 0 && DatumVrijemePocetka.CompareTo(e.DatumVrijemeZavrsetka) <= 0)
                    {
                        return Json("Vrijeme poćetka je zauzeto.");
                    }
                }  
            }
            return Json(true);
        }
        
        [AcceptVerbs("Get", "Post")]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult VrijemeTrajanja(int EmisijaId, DateTime DatumVrijemePocetka, int vrijemeTrajanjaSati, int vrijemeTrajanjaMinute, int TvpostajaId)
        {
            if (vrijemeTrajanjaSati == 0 && vrijemeTrajanjaMinute == 0)
            {
                return Json("Upišite vrijeme trajanja.");
            }
            DateTime DatumVrijemeZavrsetka = DatumVrijemePocetka
                .AddHours(Convert.ToDouble(vrijemeTrajanjaSati))
                .AddMinutes(Convert.ToDouble(vrijemeTrajanjaMinute));

            var emisije = context.Emisija.Where(e => e.TvpostajaId == TvpostajaId);
            foreach (Emisija e in emisije)
            {
                if (e.EmisijaId != EmisijaId)
                {
                    if ((DatumVrijemeZavrsetka.CompareTo(e.DatumVrijemePocetka) >= 0 && DatumVrijemeZavrsetka.CompareTo(e.DatumVrijemeZavrsetka) <= 0)
                    || (DatumVrijemePocetka.CompareTo(e.DatumVrijemePocetka) <= 0 && DatumVrijemeZavrsetka.CompareTo(e.DatumVrijemeZavrsetka) >= 0))
                    {
                        return Json("Vrijeme završetka je zauzeto.");
                    }
                }
            }
            return Json(true);
        }

        [HttpPost]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult Dodaj(EmisijeDodajUrediViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DatumVrijemeZavrsetka = model.DatumVrijemePocetka
                    .AddHours(Convert.ToDouble(model.vrijemeTrajanjaSati))
                    .AddMinutes(Convert.ToDouble(model.vrijemeTrajanjaMinute));

                Emisija emisija = new Emisija()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                    TvpostajaId = model.TvpostajaId,
                    ZanrId = model.ZanrId,
                    DatumVrijemePocetka = model.DatumVrijemePocetka,
                    DatumVrijemeZavrsetka = model.DatumVrijemeZavrsetka
                };
                context.Add(emisija);
                context.SaveChanges();
                return RedirectToAction("Detalji", new { id = context.Emisija.First(e => e.Naziv == model.Naziv).EmisijaId });
            }
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "administrator,moderator")]
        public IActionResult Uredi(int id)
        {
            if (!EmisijaExists(id))
            {
                return View("NotFound");
            }
            int korisnik_id = context.Korisnik.First(k => k.KorisnickoIme == User.Identity.Name).KorisnikId;
            var tvPostaje = context.Tvpostaja.Where(t => t.ModeratorId == korisnik_id);
            var emisija = context.Emisija.Find(id);
            DateTime dateTime = new DateTime(emisija.DatumVrijemeZavrsetka.Ticks - emisija.DatumVrijemePocetka.Ticks);
            EmisijeDodajUrediViewModel model = new EmisijeDodajUrediViewModel()
            {
                EmisijaId = id,
                TvpostajaId = emisija.TvpostajaId,
                ZanrId = emisija.ZanrId,
                tvPostaje = tvPostaje,
                zanrovi = context.Zanr,
                Naziv = emisija.Naziv,
                Opis = emisija.Opis,
                DatumVrijemePocetka = emisija.DatumVrijemePocetka,
                vrijemeTrajanjaSati = dateTime.Hour,
                vrijemeTrajanjaMinute = dateTime.Minute
            };
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult Uredi(EmisijeDodajUrediViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.DatumVrijemeZavrsetka = model.DatumVrijemePocetka
                    .AddHours(Convert.ToDouble(model.vrijemeTrajanjaSati))
                    .AddMinutes(Convert.ToDouble(model.vrijemeTrajanjaMinute));
                try
                {
                    Emisija emisija = context.Emisija.Find(model.EmisijaId);
                        emisija.Naziv = model.Naziv;
                        emisija.Opis = model.Opis;
                        emisija.TvpostajaId = model.TvpostajaId;
                        emisija.ZanrId = model.ZanrId;
                        emisija.DatumVrijemePocetka = model.DatumVrijemePocetka;
                        emisija.DatumVrijemeZavrsetka = model.DatumVrijemeZavrsetka;

                    context.Update(emisija);
                    context.SaveChanges();
                    return RedirectToAction("Detalji", new { id = model.EmisijaId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmisijaExists(model.EmisijaId))
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


        private bool EmisijaExists(int id)
        {
            return context.Emisija.Any(e => e.EmisijaId == id);
        }
        private bool PretplataExists(int korisnik_id, int EmisijaId)
        {
            return context.Pretplata.Any(e => e.KorisnikId == korisnik_id && e.EmisijaId == EmisijaId);
        }
        private bool TvpostajaExists(int id)
        {
            return context.Tvpostaja.Any(t => t.TvpostajaId == id);
        }
        private bool ZanrExists(int id)
        {
            return context.Zanr.Any(z => z.ZanrId == id);
        }
    }
}
