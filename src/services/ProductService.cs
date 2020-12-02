using AutoMapper;
using model;
using modelDTOs;
using persistenDatabase;
using services.Commons;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<ProductDto> Details(int? id);
        Task<ProductDto> Details(string urlProduct);
        Task<ProductDto> Create(ProductCreateDto model);
        Task<Product> GetById(int? id);
        Task DeleteConfirmed(int id);
        bool ProductExists(int id);
        bool DuplicaName(string _stringName);
    }
    public class ProductService : IProductService
    {
        //Variables
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFormatStringUrl _formatStringUrl;

        public ProductService(ApplicationDbContext context,
            IMapper mapper,
            IFormatStringUrl formatStringUrl)
        {
            _context = context;
            _mapper = mapper;
            _formatStringUrl = formatStringUrl;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return (await _context.Products.ToListAsync());
        }

        public async Task<ProductDto> Details(int? id)
        {
            var _product = _mapper.Map<ProductDto>(
                    await _context.Products
                    .FirstOrDefaultAsync(m => m.ProductId == id)
                );

            //return (Product);
            return _mapper.Map<ProductDto>(_product);
        }

        public async Task<ProductDto> Details(string urlProduct)
        {
            var _product = _mapper.Map<ProductDto>(
                    await _context.Products
                    .FirstOrDefaultAsync(m => m.UrlProduct == urlProduct)
                );

            //return (Product);
            return _mapper.Map<ProductDto>(_product);
        }

        public async Task<ProductDto> Create(ProductCreateDto model)
        {
            var _dateCreate = DateTime.Now;
            var _urlProduct = _formatStringUrl.FormatString(model.Name);

            var _icono = string.Empty;

            var _product = new Product
            {
                ProductId = model.ProductId,
                Name = model.Name,
                UrlProduct = _urlProduct.Trim(),
                Icono = _icono,
                Description = model.Description,
                DateCreate = _dateCreate
            };

            await _context.AddAsync(_product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(_product);
        }

        public async Task<Product> GetById(int? id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == id);

        }

        public async Task DeleteConfirmed(int id)
        {
            _context.Remove(new Product
            {
                ProductId = id
            });

            await _context.SaveChangesAsync();
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public bool DuplicaName(string _stringName)
        {
            return _context.Products.Any(e => e.Name == _stringName);
        }
    }
}
