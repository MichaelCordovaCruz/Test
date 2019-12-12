using System.Diagnostics;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using RegistroDeCompetencia2019.Models;
using RegistroDeCompetencia2019.Data;
using RegistroDeCompetencia2019.ViewModels;

namespace RegistroDeCompetencia2019.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(new CreateHomeVM{ 
                Recintos = await _context.Recintos.ToListAsync()
            });
        }

        public async Task<IActionResult> Create()
        {
            return View(new CreateHomeVM{ 
                Recintos = await _context.Recintos.ToListAsync()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Estudiante")] CreateHomeVM homeVM)
        {
            if(ModelState.IsValid)
            {
                if(await _context.Estudiantes.FindAsync(homeVM.Estudiante.Id) == null)
                {
                    try
                    {
                        _context.Estudiantes.Add(homeVM.Estudiante);
                        _context.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch(DbUpdateException e)
                    {
                        ModelState.AddModelError("", "Error, información entrada no pudo ser guardada. "
                        + "Contacte a el administrador poder ayudarle.");
                    }
                }
                else
                {
                } 
                    ModelState.AddModelError("", "Ya este número de estudiante esta registrado");
            }
            else
            {
                ModelState.AddModelError("", "La información entrada no es valida. Por favor, inténtelo de nuevo");   
            }

            homeVM.Recintos = await _context.Recintos.ToListAsync();
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
