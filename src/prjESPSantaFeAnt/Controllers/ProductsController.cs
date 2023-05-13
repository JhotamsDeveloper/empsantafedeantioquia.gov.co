using Microsoft.AspNetCore.Mvc;
using modelDTOs;
using prjESPSantaFeAnt.Models;
using services;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var _product = from a in await _productService.GetAll()
                           select new ModelViewProduct
                           {
                               ProductId = a.ProductId,
                               Icono = a.Icono,
                               Name = a.Name,
                               UrlProduct = a.UrlProduct
                           };

            return View(_product);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _product = await _productService.Details(id);

            if (_product == null)
            {
                return NotFound();
            }

            var _model = new ModelViewProduct
            {
                ProductId = _product.ProductId,
                Icono = _product.Icono,
                Name = _product.Name,
                Description = _product.Description,
                UrlProduct = _product.UrlProduct,
                DateCreate = _product.DateCreate.ToString("MMMM dd, yyyy", CultureInfo.CreateSpecificCulture("es-CO"))
            };

            ViewData["admin"] = true;
            return View(_model);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto model)
        {
            var _urlName = _productService.DuplicaName(model.Name);

            if (_urlName)
            {
                ViewData["DuplicaName"] = $"El Nombre {model.Name} ya ha sido utilizado, cambielo";
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var result = await _productService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _productService.ProductExists(id);
        }

        // GET: Products
        [Route("servicios")]
        public async Task<IActionResult> ListProducts()
        {
            var _product = from a in await _productService.GetAll()
                           select new ModelViewProduct
                           {
                               ProductId = a.ProductId,
                               Icono = a.Icono,
                               Name = a.Name,
                               Description = a.Description,
                               UrlProduct = a.UrlProduct
                           };

            return View(_product);
        }
    }
}
