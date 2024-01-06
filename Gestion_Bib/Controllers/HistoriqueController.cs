using Gestion_Bib.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gestion_Bib.Controllers
{
    public class HistoriqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoriqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Historique()
        {
            var reservations = _context.Reservations.Include(r => r.Livre).ToList();
            return View(reservations);
        }

        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            var reservationASupprimer = _context.Reservations.Find(id);

            if (reservationASupprimer != null)
            {
                _context.Reservations.Remove(reservationASupprimer);
                _context.SaveChanges();
            }

            return RedirectToAction("Historique");
        }

    }
}
