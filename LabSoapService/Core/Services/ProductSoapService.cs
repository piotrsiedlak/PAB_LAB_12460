using LabSoapService.Core.Interfaces;
using LabWebApi.Core.Entities;
using LabWebApi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSoapService.Core.Services
{
    public class ProductSoapService : IProductSoapService
    {
        private readonly IProductService _productService;

        public ProductSoapService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return product != null ? new ProductDto(product) : null;
        }

        public async Task<ProductDto[]> GetAllProducts()
        {
            var products = await _productService.GetProductsAsync();
            return products.Select(p => new ProductDto(p)).ToArray();
        }

        public async Task AddProduct(ProductDto product)
        {
            await _productService.AddProductAsync(new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            });
        }

        public async Task UpdateProduct(ProductDto product)
        {
            await _productService.UpdateProductAsync(new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            });
        }

        public async Task DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
        }
    }
}