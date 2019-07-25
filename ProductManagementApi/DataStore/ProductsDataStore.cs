using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementApi.DataStore
{
    public class ProductsDataStore
    {
        public static ProductsDataStore Current { get; } = new ProductsDataStore();
        public IList<ProductDto> Products { get; set; }

        public ProductsDataStore()
        {
            Products = new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = "PR0001",
                    Description = "HP 15-DA0042TX 15.6 inch Laptop [i7]",
                    Model = "4PZ98PA#ABG",
                    Brand = "Hp"
                },
                new ProductDto()
                {
                    Id = "PR0002",
                    Description = "FFalcon 24F1 24 inch HD LED TV",
                    Model = "24F1",
                    Brand = "Falcon"
                },
                new ProductDto()
                {
                    Id = "PR0003",
                    Description = "Apple iPhone XR 64GB (White)",
                    Model = "3801000078",
                    Brand = "Apple"
                },
                new ProductDto()
                {
                    Id = "PR0004",
                    Description = "Panasonic DC-FT7 Tough Camera [4K Video] (Orange)",
                    Model = "DC-FT7GN-D",
                    Brand = "Panasonic"
                },
                new ProductDto()
                {
                    Id = "PR0005",
                    Description = "Cygnett Protectshield Screen Protector for Fitbit Charge 3",
                    Model = "CY2852CPPRO",
                    Brand = "Cygnett"
                }
            };
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
