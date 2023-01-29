using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvRaspored.Models;
using TvRaspored.Selection;
using TvRaspored.ViewModels;

namespace TvRaspored.Controllers
{
    [Authorize(Roles = "administrator, moderator")]
    public class TvpostajeController : Controller
    {
        private readonly AppDbContext context;

        public TvpostajeController(AppDbContext context)
        {
            this.context = context;
        }

        [AllowAnonymous]
        public IActionResult Izbornik()
        {
            var tvPostaje = context.Tvpostaja.Include(t => t.Moderator).AsEnumerable();
            TvpostajeIzbornikViewModel model = new TvpostajeIzbornikViewModel()
            {
                Tvpostaje = tvPostaje
            };
            return View(model);
        }

        [Authorize(Roles = "administrator")]
        public IActionResult Statistika()
        {
            var tvPostaje = context.Tvpostaja
                .Join(context.Emisija,
                t => t.TvpostajaId, e => e.TvpostajaId,
                (t, e) => new
                {
                    t.Naziv,
                    e.EmisijaId
                })
                .Join(context.Pretplata,
                t => t.EmisijaId, p => p.EmisijaId,
                (t, p) => new TvPostajeBrojPretplataSelection()
                {
                    Naziv = t.Naziv,
                    Status = p.Status
                }).Where(p => p.Status == 1).ToList();
            var korisnici = context.Korisnik.Where(k => k.DatumPrijave.Value.AddYears(2) < DateTime.Now).ToList();

            TvpostajeStatistikaViewModel model = new TvpostajeStatistikaViewModel()
            {
                Tvpostaje = tvPostaje,
                Korisnici = korisnici
            };
            return View(model);
        }

        public IActionResult Detalji(int id)
        {
            if (!TvpostajaExists(id))
            {
                return View("NotFound");
            }
            var tvpostaja = context.Tvpostaja
                .Include(t => t.Moderator)
                .First(m => m.TvpostajaId == id);
            TvpostajeDetaljiViewModel model = new TvpostajeDetaljiViewModel()
            {
                Naziv = tvpostaja.Naziv,
                TvpostajaId = id,
                ModeratorIme = $"{tvpostaja.Moderator.Ime} {tvpostaja.Moderator.Prezime}"
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            var tipoviKorisnika = context.TipKorisnika;
            var moderatori = context.Korisnik
                .Where(k => k.TipId == tipoviKorisnika.First(t => t.Naziv == "moderator").TipId
                || k.TipId == tipoviKorisnika.First(t => t.Naziv == "administrator").TipId);
            TvpostajeDodajUrediViewModel model = new TvpostajeDodajUrediViewModel()
            {
                Moderatori = moderatori
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Dodaj(TvpostajeDodajUrediViewModel model)
        {
            if (ModelState.IsValid)
            {
                Tvpostaja tvpostaja = new Tvpostaja()
                {
                    Naziv = model.Naziv,
                    ModeratorId = model.ModeratorId
                };
                context.Add(tvpostaja);
                context.SaveChanges();
                return RedirectToAction("Izbornik");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            
            if (!TvpostajaExists(id))
            {
                return View("NotFound");
            }
            var tvpostaja = context.Tvpostaja.Find(id);
            var tipoviKorisnika = context.TipKorisnika;
            var moderatori = context.Korisnik
                .Where(k => k.TipId == tipoviKorisnika.First(t => t.Naziv == "moderator").TipId 
                || k.TipId == tipoviKorisnika.First(t => t.Naziv == "administrator").TipId);
            TvpostajeDodajUrediViewModel model = new TvpostajeDodajUrediViewModel()
            {
                Moderatori = moderatori,
                ModeratorId = tvpostaja.ModeratorId,
                Naziv = tvpostaja.Naziv,
                TvpostajaId = id
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Uredi(TvpostajeDodajUrediViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tvpostaja tvpostaja = context.Tvpostaja.Find(model.TvpostajaId);
                    tvpostaja.Naziv = model.Naziv;
                    tvpostaja.ModeratorId = model.ModeratorId;
                    context.SaveChanges();
                    return RedirectToAction("Izbornik");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TvpostajaExists(model.TvpostajaId))
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

        private bool TvpostajaExists(int id)
        {
            return context.Tvpostaja.Any(t => t.TvpostajaId == id);
        }
    }
}
