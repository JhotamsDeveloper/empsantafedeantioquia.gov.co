using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;

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
            if (nameBrigade != "")
            {
                return NotFound();
            }

            var _brigade = await _brigadeService.GetById(nameBrigade);

            if (_brigade == null)
            {
                return NotFound();
            }

            ViewData["detail"] = false;

            return View(_brigade);
        }
        // GET: Brigades/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Brigades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
