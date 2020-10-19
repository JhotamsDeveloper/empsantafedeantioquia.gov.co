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
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAll();
        Task<CategoryDto> Details(int? id);
        Task<CategoryDto> Create(CategoryCreateDto model);

        Task<CategoryDto> Edit(int? id);
        Task Edit(int id, CategoryEditDto model);

        Task<Category> GetById(int? id);
        Task<Category> GetById(string nameCategory);
        Task DeleteConfirmed(int id);
        bool CategoryExists(int id);
        bool DuplicaName(string _stringName);
    }

    public class CategoryService : ICategoryService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "categories";

        public CategoryService(ApplicationDbContext context,
                                IMapper mapper,
                                IFormatStringUrl formatStringUrl,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return (await _context.Categories.ToListAsync());
        }

        public async Task<CategoryDto> Details(int? id)
        {
            var category = _mapper.Map<CategoryDto>(
                    await _context.Categories
                    .FirstOrDefaultAsync(m => m.Id == id)
                );

            //return (category);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> Create(CategoryCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _urlCategory = _formatStringUrl.FormatString(model.NameCategory);
            var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);

            var cate = new Category
            {
                Id = model.Id,
                NameCategory = model.NameCategory.Trim(),
                UrlCategory = _urlCategory,
                Description = model.Description.Trim(),
                CoverPage = _coverPage,
                Statud = model.Statud,
                DateCreate = _dateCreate
            };

            await _context.AddAsync(cate);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(cate);
        }

        public async Task<CategoryDto> Edit(int? id)
        {
            return _mapper.Map<CategoryDto>(
                await _context.Categories.FindAsync(id));
        }

        public async Task Edit(int id, CategoryEditDto model)
        {
            var _coverPage = "";
            var _dateUpdate = DateTime.Now;

            var _cate = await _context.Categories.SingleAsync(x => x.Id == id);

            var _shearchCoverPage = await _context.Categories.SingleAsync(x => x.Id == id);

            if (model.CoverPage != null)
            {
                _coverPage = _uploadedFileIIS.UploadedFileImage(_shearchCoverPage.CoverPage, model.CoverPage, _account);
            }
            else
            {
                _coverPage = _cate.CoverPage;
            }

            _cate.Description = model.Description;
            _cate.CoverPage = _coverPage;
            _cate.Statud = model.Statud;
            _cate.DateUpdate = _dateUpdate;

            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetById(int? id)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task<Category> GetById(string nameCategory)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.UrlCategory == nameCategory);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(int id)
        {
            var _shearchCoverPage = await _context.Categories
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            _uploadedFileIIS.DeleteConfirmed(_shearchCoverPage.CoverPage, _account);

            _context.Remove(new Category
            {
                Id = id
            });

            await _context.SaveChangesAsync();

        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Categories.Any(e => e.NameCategory == _stringName);
        }

    }
}
