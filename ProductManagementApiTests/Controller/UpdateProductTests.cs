using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.DataStore;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class UpdateProductTests
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

        [Test]
        public void When_update_passing_null_data_then_return_bad_request()
        {
            var result = _productController.UpdateProduct("PR001", null);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObj = result as BadRequestObjectResult;
            var response = badRequestObj.Value as OperationResponse;
            Assert.IsFalse(response.IsSuccessful);
        }

        [Test]
        public void When_update_non_existing_product_then_return_bad_request()
        {
            var productId = "RandomId";
            var duplicateProduct = new CreateAndEditProductDto()
            {
                Description = "Laptop",
                Model = "11111",
                Brand = "MyProduct"
            };

            var result = _productController.UpdateProduct(productId, duplicateProduct);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObj = result as BadRequestObjectResult;
            var response = badRequestObj.Value as OperationResponse;
            Assert.IsFalse(response.IsSuccessful);
        }

        [Test]
        public void When_update_product_then_return_success_and_return_updated_product_route()
        {
            var updateProduct = new CreateAndEditProductDto()
            {
                Description = "Update product",
                Model = "Update model",
                Brand = "Update brand"
            };

            var result = _productController.UpdateProduct("PR001", updateProduct);
            Assert.IsInstanceOf<CreatedAtRouteResult>(result);

            var routeResult = result as CreatedAtRouteResult;
            Assert.AreEqual(201, routeResult.StatusCode);
            Assert.AreEqual("GetSpecificProduct", routeResult.RouteName);

            var value = routeResult.Value as OperationResponse;
            Assert.IsTrue(value.IsSuccessful);
        }
    }
}
