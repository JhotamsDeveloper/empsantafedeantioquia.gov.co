using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class PQRSDsController : Controller
    {
        private readonly IPQRSDService _iPQSDService;
        private readonly ApplicationDbContext _context;

        [TempData]
        public Guid _StatusMessaje { get; set; }


        public PQRSDsController(IPQRSDService iPQSDService,
            ApplicationDbContext context)
        {
            _iPQSDService = iPQSDService;
            _context = context;
        }

        // GET: PQRSDs
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            var _getAll = _iPQSDService.GetAll();

            var _pqrsd = from a in _getAll
                        select new ModelViewPQRSD
                        {
                            PQRSDID = a.PQRSDID,
                            NamePerson = a.NamePerson,
                            Email = a.Email,
                            PQRSDName = a.PQRSDName,
                            NameSotypeOfRequest = a.NameSotypeOfRequest,
                            IsAnsweredPQRSD = a.IsAnswered,
                            DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                        };

            return View(_pqrsd);
        }

        // GET: PQRSDs/Details/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _pqrsd = _iPQSDService.GetById(id);

            if (_pqrsd == null)
            {
                return NotFound();
            }

            var _model = new ModelViewPQRSD
                         {
                             PQRSDID = _pqrsd.PQRSDID,
                             NamePerson = _pqrsd.NamePerson,
                             Email = _pqrsd.Email,
                             PQRSDName = _pqrsd.PQRSDName,
                             Description = _pqrsd.Description,
                             NameSotypeOfRequest = _pqrsd.NameSotypeOfRequest,
                             IsAnsweredPQRSD = _pqrsd.IsAnswered,
                             ReviewPQRSD = _pqrsd.Reply,
                             AnswerDate = _pqrsd.AnswerDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             DateCreate = _pqrsd.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
                         };

            return View(_model);
        }

        // GET: PQRSDs/Create
        [Route("formular-pqrsd")]
        public IActionResult Create()
        {
            ViewData["CodigoPqrsd"] = "";
            return View();
        }

        // POST: PQRSDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("formular-pqrsd")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PQRSDCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var _create = await _iPQSDService.Create(model);

                _StatusMessaje = _create.PQRSDID;
                return RedirectToAction("TypePQRSD");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Review(ReviewCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var id = model.ID;

                var _stated = _iPQSDService.Review(id, model);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: PQRSDs/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _pqrsd =  _iPQSDService.GetById(id);
            if (_pqrsd == null)
            {
                return NotFound();
            }

            var _model = new ModelViewPQRSD
            {
                PQRSDID = _pqrsd.PQRSDID,
                NamePerson = _pqrsd.NamePerson,
                Email = _pqrsd.Email,
                PQRSDName = _pqrsd.PQRSDName,
                Description = _pqrsd.Description,
                NameSotypeOfRequest = _pqrsd.NameSotypeOfRequest,
                IsAnsweredPQRSD = _pqrsd.IsAnswered,
                ReviewPQRSD = _pqrsd.Reply,
                AnswerDate = _pqrsd.AnswerDate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                DateCreate = _pqrsd.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
            };

            return View(_model);

        }

        // POST: PQRSDs/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _iPQSDService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        [Route("pqrsd")]
        public IActionResult TypePQRSD()
        {
            var _stated = Guid.Parse("00000000-0000-0000-0000-000000000000");

            if ( _StatusMessaje != Guid.Empty || _StatusMessaje != _stated)
            {
                ViewData["CodigoPqrsd"] = _StatusMessaje;
            }
            return View();
        }
    }
}
