using AutoMapper;
using Microsoft.EntityFrameworkCore;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<EmployeeDto> Details(int? id);
        Task<EmployeeDto> Create(EmployeeCreateDto model);
        Task<Employee> GetById(int? id);
        Task DeleteConfirmed(int id);
    }

    public class EmployeeService: IEmployeeService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "employees";


        public EmployeeService(ApplicationDbContext context,
                                IMapper mapper,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var _employee = await _context.Employees
                .AsNoTracking()
                .OrderByDescending(x => x.DateCreate)
                .ToListAsync();
            return (_employee);
        }

        public async Task<EmployeeDto> Details(int? id)
        {
            var _employee = _mapper.Map<EmployeeDto>(
                    await _context.Employees
                    .FirstOrDefaultAsync(m => m.EmployeeId == id)
                );

            return _mapper.Map<EmployeeDto>(_employee);
        }

        public async Task<EmployeeDto> Create(EmployeeCreateDto model)
        {
            var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);

            var _employee = new Employee
            {
                EmployeeId = model.EmployeeId,
                Name = model.Name,
                Occupation = model.Occupation,
                CoverPage = _coverPage,

            };

            await _context.AddAsync(_employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(_employee);
        }

        public async Task<Employee> GetById(int? id)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);

        }

        public async Task DeleteConfirmed(int id)
        {
            var _shearchCoverPage = await _context.Employees
                .AsNoTracking()
                .SingleAsync(x => x.EmployeeId == id);

            _uploadedFileIIS.DeleteConfirmed(_shearchCoverPage.CoverPage, _account);

            _context.Remove(new Employee
            {
                EmployeeId = id
            });

            await _context.SaveChangesAsync();

        }

    }
}
