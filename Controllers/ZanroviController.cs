using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TvRaspored.Models;
using TvRaspored.ViewModels;

namespace TvRaspored.Controllers
{
    public class ZanroviController : Controller
    {
        private readonly AppDbContext context;

        public ZanroviController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Izbornik(int tvPostaja_id)
        {
            var emisije = context.Emisija
                .Where(e => e.TvpostajaId == tvPostaja_id).ToList();
            var zanrovi = context.Zanr.ToList();
            var tvpostaja = context.Tvpostaja.Include(t => t.Moderator).First(t => t.TvpostajaId == tvPostaja_id);
            ZanroviIzbornikViewModel model = new ZanroviIzbornikViewModel()
            {
                TvPostajaId = tvPostaja_id,
                TvPostajaNaziv = tvpostaja.Naziv,
                ModeratorIme = tvpostaja.Moderator.KorisnickoIme,
                Zanrovi = zanrovi,
                Emisije = emisije
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "administrator, moderator")]
        public IActionResult Izbornik(ZanroviIzbornikViewModel model)
        {
            if (ModelState.IsValid)
            {
                Zanr zanr = new Zanr()
                {
                    Naziv = model.Naziv
                };
                context.Add(zanr);
                context.SaveChanges();
                var routeValues = new RouteValueDictionary {
                    { "tvPostaja_id", model.TvPostajaId },
                    { "zanr_id", context.Zanr.First(z => z.Naziv == model.Naziv).ZanrId }
                };
                return RedirectToAction("Izbornik", "Emisije", routeValues);
            }
            model.Emisije = context.Emisija
                .Where(e => e.TvpostajaId == model.TvPostajaId).ToList();
            model.Zanrovi = context.Zanr.ToList();
            return View(model);
        }
    }
}
