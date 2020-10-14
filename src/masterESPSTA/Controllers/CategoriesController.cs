using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using masterESPSTA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using services;

namespace masterESPSTA.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(ApplicationDbContext context,
                                    ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var _jQuery = from a in await _categoryService.GetAll()
                         select new ModelViewCategory
                         {
                            Id = a.Id,
                            NameCategory = a.NameCategory,
                            DateCreate = a.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO")),
                            Statud = a.Statud
                         };

            return View(_jQuery);
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var _category = await _categoryService.GetById(id);
            var _dateCreate = _category.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            if (_category == null)
            {
                return NotFound();
            }


            var _viewCategory = new ModelViewCategory
            {
                Id = _category.Id,
                NameCategory = _category.NameCategory,
                Description = _category.Description,
                CoverPage = _category.CoverPage,
                DateCreate = _dateCreate,
                Statud = _category.Statud
            };

            ViewData["detail"] = true;

            return View(_viewCategory);
        }

        // GET: Categories/Details/5
        [Route("servicios/{nameCategory}")]
        public async Task<IActionResult> Details(string nameCategory)
        {
            if (nameCategory == null)
            {
                return NotFound();
            }
            var _category = await _categoryService.GetById(nameCategory);
            var _dateCreate = _category.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            if (_category == null)
            {
                return NotFound();
            }


            var _viewCategory = new ModelViewCategory
            {
                Id = _category.Id,
                NameCategory = _category.NameCategory,
                Description = _category.Description,
                CoverPage = _category.CoverPage,
                DateCreate = _dateCreate,
                Statud = _category.Statud
            };

            ViewData["detail"] = false;

            return View(_viewCategory);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDto model)
        {
            var _urlName = _categoryService.DuplicaName(model.NameCategory);

            if (_urlName)
            {
                ViewData["DuplicaName"] = $"El Nombre {model.NameCategory} ya ha sido utilizado, cambielo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _categoryService.Create(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _category = await _categoryService.GetById(id);

            if (_category == null)
            {
                return NotFound();
            }

            var _viewCagory = new CategoryEditDto
            {
                Id = _category.Id,
                NameCategory = _category.NameCategory,
                Description = _category.Description,
                Statud = _category.Statud
            };

            return View(_viewCagory);
        }

        // POST: Categories/Edit/5
        // Para protegerse de ataques de superposición, habilite las propiedades específicas a las que desea enlazar, para
        // más detalles ver http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryEditDto model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.Edit(id, model);
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!CategoryExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _category = await _categoryService.GetById(id);
            var _dateCreate = _category.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"));

            if (_category == null)
            {
                return NotFound();
            }

            var _viewCategory = new ModelViewCategory
            {
                Id = _category.Id,
                NameCategory = _category.NameCategory,
                Description = _category.Description,
                CoverPage = _category.CoverPage,
                DateCreate = _dateCreate,
                Statud = _category.Statud
            };

            return View(_viewCategory);

        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _categoryService.CategoryExists(id);
        }
    }
}
