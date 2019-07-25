using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.DataStore;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class DeleteProductTests
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
                }
            });
            _productController = new ProductController(_fakeDataStore);
        }

        [TestCase("")]
        [TestCase("RandomId")]
        public void When_delete_passing_invalid_product_id_then_return_bad_request(string productId)
        {
            var result = _productController.DeleteProduct(productId);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObj = result as BadRequestObjectResult;
            var response = badRequestObj.Value as OperationResponse;
            Assert.IsFalse(response.IsSuccessful);
        }

        [Test]
        public void When_delete_product_then_return_success()
        {
            var result = _productController.DeleteProduct("PR001");
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as OperationResponse;
            Assert.IsTrue(response.IsSuccessful);
        }
    }
}
