using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.DataStore;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class GetProductTests
    {
        private ProductController _productController;
        private ProductsDataStore _fakeDataStore;

        [SetUp]
        public void Setup()
        {
            _fakeDataStore = new ProductsDataStore(new List<ProductDto>()
            {
                new ProductDto()
                {
                    Id = "PR001",
                    Description = "Product 1",
                    Model = "11111",
                    Brand = "MyProduct"
                },
                new ProductDto()
                {
                    Id = "PR002",
                    Description = "Product 2",
                    Model = "22222",
                    Brand = "MyProduct"
                }
            });
            _productController = new ProductController(_fakeDataStore);
        }

        [Test]
        public void When_get_all_products_then_return_ok_status_with_list_of_products()
        {
            var result = _productController.GetAllProducts();
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var okValue = okResult.Value as List<ProductDto>;
            var ids = okValue.Select(p => p.Id);
            CollectionAssert.AreEqual(new List<string> { "PR001", "PR002" }, ids);
        }

        [Test]
        public void When_get_existing_product_then_return_ok_status()
        {
            var result = _productController.GetProductById("PR001");
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var okValue = okResult.Value as ProductDto;
            Assert.AreEqual("PR001", okValue.Id);
        }

        [Test]
        public void When_get_non_existing_product_then_return_not_found_status()
        {
            var result = _productController.GetProductById("RandomId");
            Assert.IsInstanceOf<NotFoundResult>(result);

            var notFoundResult = result as NotFoundResult;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }
    }
}
