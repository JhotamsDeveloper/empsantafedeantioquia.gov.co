using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using modelDTOs;
using prjESPSantaFeAnt.Models;
using services;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Controllers
{
    public class BrigadesController : Controller
    {
        private readonly IBrigadeService _brigadeService;

        public BrigadesController(IBrigadeService brigadeService)
        {
            _brigadeService = brigadeService;
        }

        // GET: Brigades
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            var _brigada = from a in await _brigadeService.GetAll()
                           select new ModelViewBrigade
                           {
                               Id = a.Id,
                               NameMaster = a.NameMaster,
                               DateBrigade = a.DateBrigade.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                               DateCreate = a.DateCreate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                           };

            return View(_brigada);
        }

        // GET: Brigades/Details/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _brigade = await _brigadeService.GetById(id);
            
            if (_brigade == null)
            {
                return NotFound();
            }

            var _model = new ModelViewBrigade
            {
                Id = _brigade.Id,
                NameMaster = _brigade.NameMaster,
                UrlMaster = _brigade.UrlMaster,
                Description = _brigade.Description,
                CoverPage = _brigade.CoverPage,
                Statud = _brigade.Statud,
                Author = _brigade.Author,
                DateBrigade = _brigade.DateBrigade.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))

            };

            ViewData["detail"] = true;

            return View(_model);
        }

        // GET: Brigades/Details/5
        [Route("brigadas/{nameBrigade}")]
        public async Task<IActionResult> Details(string nameBrigade)
        {
            if (nameBrigade == string.Empty)
            {
                return NotFound();
            }

            var _brigade = await _brigadeService.GetById(nameBrigade);

            if (_brigade == null)
            {
                return NotFound();
            }

            var _model = new ModelViewBrigade
            {
                Id = _brigade.Id,
                NameMaster = _brigade.NameMaster,
                UrlMaster = _brigade.UrlMaster,
                Description = _brigade.Description,
                CoverPage = _brigade.CoverPage,
                Statud = _brigade.Statud,
                Author = _brigade.Author,
                DateBrigade = _brigade.DateBrigade.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))

            };

            ViewData["detail"] = false;

            return View(_model);
        }

        // GET: Brigades/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Brigades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrigadeCreateDto model)
        {
            if (_brigadeService.DuplicaName(model.NameMaster))
            {
                ViewData["DuplicaName"] = "El nombre ya ha sido utilizado, por favor cambielo";
                View(model);
            }

            if (ModelState.IsValid)
            {
                await _brigadeService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Brigades/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _brigade = await _brigadeService.GetById(id);

            if (_brigade == null)
            {
                return NotFound();
            }

            return View(_brigade);
        }

        // POST: Brigades/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _brigadeService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(int id)
        {
            return _brigadeService.BrigadeExists(id);
        }

        // GET: Brigades
        [Route("brigadas")]
        public async Task<IActionResult> BrigadesGetAll()
        {
            var _brigada = from a in await _brigadeService.GetAll()
                           select new ModelViewBrigade
                           {
                               Id = a.Id,
                               NameMaster = a.NameMaster,
                               UrlMaster = a.UrlMaster,
                               Description = a.Description,
                               CoverPage = a.CoverPage,
                               DateBrigade = a.DateBrigade.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                               DateCreate = a.DateCreate.ToString("MMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                           };

            return View(_brigada);
        }
    }
}
