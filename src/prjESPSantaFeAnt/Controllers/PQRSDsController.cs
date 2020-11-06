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
    public class PQRSDsController : Controller
    {
        private readonly IPQRSDService _iPQSDService;

        public PQRSDsController(IPQRSDService iPQSDService)
        {
            _iPQSDService = iPQSDService;
        }

        // GET: PQRSDs
        public async Task<IActionResult> Index()
        {
            var _pqrsd = from a in await _iPQSDService.GetAll()
                        select new ModelViewPQRSD
                        {
                            PQRSDID = a.PQRSDID,
                            NamePerson = a.NamePerson,
                            Email = a.Email,
                            PQRSDName = a.PQRSDName,
                            Description = a.Description,
                            NameSotypeOfRequest = a.NameSotypeOfRequest,
                            DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                            File = a.File
                        };

            return View(_pqrsd);
        }

        // GET: PQRSDs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _pqrsd = await _iPQSDService.GetById(id);

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
                             DateCreate = _pqrsd.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                             File = _pqrsd.File
                         };

            return View(_model);
        }

        // GET: PQRSDs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PQRSDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PQRSDCreateDto model)
        {
            if (ModelState.IsValid)
            {
                await _iPQSDService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: PQRSDs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _pqrsd = await _iPQSDService.DeleteConfirmed(id);
            if (pQRSD == null)
            {
                return NotFound();
            }

            return View(pQRSD);
        }

        // POST: PQRSDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pQRSD = await _context.PQRSDs.FindAsync(id);
            _context.PQRSDs.Remove(pQRSD);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PQRSDExists(Guid id)
        {
            return _context.PQRSDs.Any(e => e.PQRSDID == id);
        }
    }
}
