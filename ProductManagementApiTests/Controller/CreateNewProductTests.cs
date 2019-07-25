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
    public class CreateNewProductTests
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
                    Id = "PR002",
                    Description = "Product 2",
                    Model = "22222",
                    Brand = "MyProduct"
                }
            });
            _productController = new ProductController(_fakeDataStore);
        }

        [Test]
        public void When_create_product_with_null_input_then_return_bad_response()
        {
            var result = _productController.CreateNewProduct(null);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObj = result as BadRequestObjectResult;
            var response = badRequestObj.Value as OperationResponse;
            Assert.IsFalse(response.IsSuccessful);
        }

        [Test]
        public void When_create_duplicate_product_then_return_bad_request()
        {
            var duplicateProduct = new CreateAndEditProductDto()
            {
                Description = "Laptop",
                Model = "22222",
                Brand = "MyProduct"
            };

            var result = _productController.CreateNewProduct(duplicateProduct);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObj = result as BadRequestObjectResult;
            var response = badRequestObj.Value as OperationResponse;
            Assert.IsFalse(response.IsSuccessful);
        }

        [Test]
        public void When_create_new_product_then_return_success_and_create_new_product_route()
        {
            var newProduct = new CreateAndEditProductDto()
            {
                Description = "Product 3",
                Model = "33333",
                Brand = "MyProduct"
            };

            var result = _productController.CreateNewProduct(newProduct);
            Assert.IsInstanceOf<CreatedAtRouteResult>(result);

            var routeResult = result as CreatedAtRouteResult;
            Assert.AreEqual(201, routeResult.StatusCode);

            var value = routeResult.Value as OperationResponse;
            Assert.IsTrue(value.IsSuccessful);

            var newProductId = (value.Data as ProductDto).Id;
            Assert.IsTrue(routeResult.RouteValues.Values.Contains(newProductId));
            Assert.AreEqual("GetSpecificProduct", routeResult.RouteName);
        }
    }
}
