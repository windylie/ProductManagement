using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementApi.DataStore
{
    public class ProductsDataStore
    {
        public IList<ProductDto> Products { get; private set; }

        public ProductsDataStore(List<ProductDto> initialProducts)
        {
            Products = initialProducts;
        }

        public ProductsDataStore() : this(new List<ProductDto>())
        {
        }

        public ProductDto CreateAndReturnNewProduct(CreateAndEditProductDto newProduct)
        {
            var newProductId = GenerateNewProductId();
            Products.Add(new ProductDto()
            {
                Id = newProductId,
                Description = newProduct.Description,
                Model = newProduct.Model,
                Brand = newProduct.Brand

            });

            return Products.SingleOrDefault(p => p.Id == newProductId);
        }

        public bool IsProductExists(string productId)
        {
            return Products.SingleOrDefault(p => p.Id == productId) != null;
        }

        public bool IsProductDuplicate(string model, string brand)
        {
            return Products.Any(p => p.Model == model && p.Brand == brand);
        }

        public bool UpdateProductAndReturnStatus(string productId, CreateAndEditProductDto newProductValue)
        {
            try
            {
                var product = Products.Single(p => p.Id == productId);

                product.Description = newProductValue.Description;
                product.Model = newProductValue.Model;
                product.Brand = newProductValue.Brand;

                return true;
            } catch(Exception)
            {
                return false;
            }

        }

        public bool DeleteProductAndReturnStatus(string productId)
        {
            try
            {
                var product = Products.Single(p => p.Id == productId);

                Products.Remove(product);

                return true;
            } catch(Exception)
            {
                return false;
            }
        }

        private string GenerateNewProductId()
        {
            var randomNumber = new Random();
            return string.Format("PR{0}", randomNumber.Next(0, 1000));
        }
    }
}
