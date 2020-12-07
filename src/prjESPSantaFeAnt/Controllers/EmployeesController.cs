using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using prjESPSantaFeAnt.Models;
using services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjESPSantaFeAnt.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employees
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            var _employees = from a in await _employeeService.GetAll()
                             select new ModelViewEmployees
                             {
                                 EmployeeId = a.EmployeeId,
                                 Name = a.Name,
                                 Occupation = a.Occupation,
                                 CoverPage = a.CoverPage
                             };

            return View(_employees);
        }

        // GET: Employees/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create(EmployeeCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetById(id);

            if (employee == null)
            {
                return NotFound();
            }

            var _model = new ModelViewEmployees
            {
                EmployeeId = employee.EmployeeId,
                Name = employee.Name,
                Occupation = employee.Occupation,
                CoverPage = employee.CoverPage
            };

            return View(_model);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteConfirmed(id);

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees
        [Route("funcionarios")]
        [AllowAnonymous]
        public async Task<IActionResult> ListEmployees()
        {
            var _employees = from a in await _employeeService.GetAll()
                             select new ModelViewEmployees
                             {
                                 EmployeeId = a.EmployeeId,
                                 Name = a.Name,
                                 Occupation = a.Occupation,
                                 CoverPage = a.CoverPage
                             };

            return View(_employees);
        }
    }
}
