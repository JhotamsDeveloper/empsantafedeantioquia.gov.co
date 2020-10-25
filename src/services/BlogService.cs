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
    public interface IBlogService
    {
        Task<IEnumerable<Master>> GetAll();
        Task<BlogDto> Details(int? id);
        Task<BlogDto> Create(BlogCreateDto model);
        Task<BlogDto> Edit(int? id);
        Task Edit(int id, BlogEditDto model);
        Task<Master> GetById(int? id);
        Task<Master> GetById(string urBlog);
        Task DeleteConfirmed(int id);
        bool BlogExists(int id);
        bool DuplicaName(string _stringName);
    }

    public class BlogService : IBlogService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;
        private readonly IUploadedFileIIS _uploadedFileIIS;
        private readonly string _account = "blog";

        public BlogService(ApplicationDbContext context,
                                IMapper mapper,
                                IFormatStringUrl formatStringUrl,
                                IUploadedFileIIS uploadedFileIIS)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
            _uploadedFileIIS = uploadedFileIIS;
        }

        public async Task<IEnumerable<Master>> GetAll()
        {
            var _blog = await _context.Masters
                .Where(x => x.Blog == true)
                .ToListAsync();
            return (_blog);
        }

        public async Task<BlogDto> Details(int? id)
        {
            var _blog = _mapper.Map<BlogDto>(
                    await _context.Masters
                    .FirstOrDefaultAsync(m => m.Id == id)
                );

            return _mapper.Map<BlogDto>(_blog);
        }

        public async Task<BlogDto> Create(BlogCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _urlCategory = _formatStringUrl.FormatString(model.NameBlog);
            var _coverPage = _uploadedFileIIS.UploadedFileImage(model.CoverPage, _account);

            var _blog = new Master
            {
                Id = model.Id,
                NameMaster = model.NameBlog.Trim(),
                UrlMaster = _urlCategory,
                Description = model.Description.Trim(),
                CoverPage = _coverPage,
                Statud = model.Statud,
                Blog = true,
                Author = model.Author,
                DateCreate = _dateCreate
            };

            await _context.AddAsync(_blog);
            await _context.SaveChangesAsync();

            return _mapper.Map<BlogDto>(_blog);
        }

        public async Task<BlogDto> Edit(int? id)
        {
            return _mapper.Map<BlogDto>(
                await _context.Masters.FindAsync(id));
        }

        public async Task Edit(int id, BlogEditDto model)
        {
            var _coverPage = "";
            var _dateUpdate = DateTime.Now;

            var _blog = await _context.Masters.SingleAsync(x => x.Id == id);

            if (model.CoverPage != null)
            {
                _coverPage = _uploadedFileIIS.UploadedFileImage(_blog.CoverPage, model.CoverPage, _account);
            }
            else
            {
                _coverPage = _blog.CoverPage;
            }

            _blog.Description = model.Description;
            _blog.CoverPage = _coverPage;
            _blog.Statud = model.Statud;
            _blog.Author = model.Author;
            _blog.DateUpdate = _dateUpdate;

            await _context.SaveChangesAsync();
        }

        public async Task<Master> GetById(int? id)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.Id == id);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task<Master> GetById(string urBlog)
        {
            return await _context.Masters.FirstOrDefaultAsync(x => x.UrlMaster == urBlog);

            //return _mapper.Map<CategoryDto>(
            //await _context.Categorys.FindAsync(id)
            //);
        }

        public async Task DeleteConfirmed(int id)
        {
            var _shearchCoverPage = await _context.Masters
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);

            _uploadedFileIIS.DeleteConfirmed(_shearchCoverPage.CoverPage, _account);

            _context.Remove(new Master
            {
                Id = id
            });

            await _context.SaveChangesAsync();

        }

        public bool BlogExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Masters.Any(e => e.NameMaster == _stringName);
        }

    }
}
