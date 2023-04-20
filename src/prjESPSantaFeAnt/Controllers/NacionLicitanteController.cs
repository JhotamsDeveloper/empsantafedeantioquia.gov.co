using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;

namespace prjESPSantaFeAnt.Controllers
{
    public class NacionLicitanteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INacionLicitanteService _nacionLicitante;

        public NacionLicitanteController(ApplicationDbContext context,
            INacionLicitanteService nacionLicitante)
        {
            _context = context;
            _nacionLicitante = nacionLicitante;
        }

        // GET: NacionLicitante
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            var _model = from a in await _nacionLicitante.GetAll()
                         select new ModelViewNacionLicitante
                         {
                             Id = a.Id,
                             NameMaster = a.NameMaster,
                             UrlMaster = a.UrlMaster,
                             NacionLicitantegStartDate = a.NacionLicitantegStartDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             NacionLicitanteEndDate = a.NacionLicitanteEndDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateUpdate = a.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                         };

            return View(_model);

        }

        // GET: NacionLicitante/Details/5
        [Route("convocatorias/{nameNacionLicitante}")]
        public async Task<IActionResult> Details(string nameNacionLicitante)
        {
            if (nameNacionLicitante == null)
            {
                return NotFound();
            }

            var _master = await _nacionLicitante.GetById(nameNacionLicitante);
            var _filesDocuments = from a in await _nacionLicitante.FilesDocuments()
                                  where a.MasterId == _master.Id
                                  select a;

            var _nacionLicitantegStartDate = _master.NacionLicitantegStartDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _nacionLicitanteEndDate = _master.NacionLicitanteEndDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            var _delatedDocuments = await _nacionLicitante.DelatedDocuments(_master.Id);

            var _dateCreate = _master.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _dateUpdate = _master.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            var dateTime = DateTime.Now;
            var statud = false;

            if (dateTime >= _master.NacionLicitantegStartDate)
            {
                if (dateTime <= _master.NacionLicitanteEndDate)
                {
                    statud = true;
                }
            }

            if (_master == null)
            {
                return NotFound();
            }

            var _model = new ModelViewNacionLicitante
            {
                Id = _master.Id,
                NameMaster = _master.NameMaster,
                UrlMaster = _master.UrlMaster,
                Description = _master.Description,
                CoverPage = _master.CoverPage,
                NacionLicitantegStartDate = _nacionLicitantegStartDate,
                NacionLicitanteEndDate = _nacionLicitanteEndDate,
                FilesDocuments = _filesDocuments,
                DelatedDocuments = _delatedDocuments,
                DateCreate = _dateCreate,
                DateUpdate = _dateUpdate,
                Statud = statud
            };

            ViewData["detail"] = false;
            ViewData["idConvocatoria"] = _master.Id;
            return View(_model);
        }

        // GET: NacionLicitante/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: NacionLicitante/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NacionLicitanteCreateDto model)
        {
            var _urlName = _nacionLicitante.DuplicaName(model.NameMaster);

            if (_urlName)
            {
                ViewData["DuplicaName"] = $"El Nombre {model.NameMaster} ya ha sido utilizado, cambielo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                await _nacionLicitante.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: NacionLicitante/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _master = await _nacionLicitante.GetById(id);
            var _filesDocuments = from a in await _nacionLicitante.FilesDocuments()
                                  where a.MasterId == _master.Id
                                  select a;

            var _nacionLicitantegStartDate = _master.NacionLicitantegStartDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _nacionLicitanteEndDate = _master.NacionLicitanteEndDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            var _delatedDocuments = await _nacionLicitante.DelatedDocuments(_master.Id);

            var _dateCreate = _master.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            var _dateUpdate = _master.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            if (_master == null)
            {
                return NotFound();
            }

            var _model = new ModelViewNacionLicitante
            {
                Id = _master.Id,
                NameMaster = _master.NameMaster,
                UrlMaster = _master.UrlMaster,
                Description = _master.Description,
                CoverPage = _master.CoverPage,
                NacionLicitantegStartDate = _nacionLicitantegStartDate,
                NacionLicitanteEndDate = _nacionLicitanteEndDate,
                FilesDocuments = _filesDocuments,
                DelatedDocuments = _delatedDocuments,
                DateCreate = _dateCreate,
                DateUpdate = _dateUpdate
            };

            ViewData["detail"] = false;
            ViewData["idConvocatoria"] = _master.Id;
            return View(_model);

        }


        // POST: NacionLicitante/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _nacionLicitante.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MasterExists(int id)
        {
            return _nacionLicitante.NacionLicitantegExists(id);
        }

        [Authorize(Roles = "UserApp")]
        [AllowAnonymous]
        [ActionName("Documento-Oficial-de-la-convocatoria")]
        public FileResult OfficialLicitante(string routeFile)
        {
            return File($"/images/filesDocuments/{routeFile}", "application/pdf", $"{routeFile}.pdf");
        }

        // GET: NacionLicitante/grid/5
        [Route("convocatorias")]
        public async Task<IActionResult> ListGetAll()
        {

            var _model = from a in await _nacionLicitante.GetAll()
                         where a.Statud == true
                         select new ModelViewNacionLicitante
                         {
                             Id = a.Id,
                             NameMaster = a.NameMaster,
                             UrlMaster = a.UrlMaster,
                             CoverPage = a.CoverPage,
                             Description = a.Description,
                             NacionLicitantegStartDate = a.NacionLicitantegStartDate.ToString(),
                             NacionLicitanteEndDate = a.NacionLicitanteEndDate.ToString(),
                             DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateUpdate = a.DateUpdate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                         };

            return View(_model);
        }
    }
}