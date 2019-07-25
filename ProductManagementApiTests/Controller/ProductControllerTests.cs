using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController _productController;

        private IList<CreateAndEditProductDto> _dummyData => new List<CreateAndEditProductDto>()
        {
            new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "11111",
                Brand = "MyProduct"
            },
            new CreateAndEditProductDto()
            {
                Description = "Product 2",
                Model = "22222",
                Brand = "MyProduct"
            }
        };

        [OneTimeSetUp]
        public void Setup()
        {
            _productController = new ProductController();

            foreach(var eachProduct in _dummyData)
            {
                _productController.CreateNewProduct(eachProduct);
            }
        }


        [Test]
        public void When_get_existing_product_then_return_ok_status()
        {
            var result = _productController.GetAllProducts();
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var okValue = okResult.Value as List<ProductDto>;
            var getProduct1 = okValue.FirstOrDefault(x => x.Description == "Product 1");
            Assert.IsTrue(getProduct1 != null);

            var product1Id = getProduct1.Id;

            result = _productController.GetProductById(product1Id);
            Assert.IsInstanceOf<OkObjectResult>(result);

            okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [Test]
        public void When_get_non_existing_product_then_return_not_found_status()
        {
            var result = _productController.GetProductById("RandomId");
            Assert.IsInstanceOf<NotFoundResult>(result);

            var notFoundResult = result as NotFoundResult;
            Assert.AreEqual(404, notFoundResult.StatusCode);
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

        [Test]
        public void When_update_passing_null_data_then_return_bad_request()
        {
            var productId = GetProductByModel("11111").Id;
            var result = _productController.UpdateProduct(productId, null);
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
        public void When_update_cause_duplicate_product_then_return_bad_request()
        {
            var productId = GetProductByModel("11111").Id;
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
            var productId = GetProductByModel("11111").Id;

            var updateProduct = new CreateAndEditProductDto()
            {
                Description = "Update product",
                Model = "Update model",
                Brand = "Update brand"
            };

            var result = _productController.UpdateProduct(productId, updateProduct);
            Assert.IsInstanceOf<CreatedAtRouteResult>(result);

            var routeResult = result as CreatedAtRouteResult;
            Assert.AreEqual(201, routeResult.StatusCode);
            Assert.AreEqual("GetSpecificProduct", routeResult.RouteName);

            var value = routeResult.Value as OperationResponse;
            Assert.IsTrue(value.IsSuccessful);
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
            var productId = GetProductByModel("11111").Id;

            var result = _productController.DeleteProduct(productId);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as OperationResponse;
            Assert.IsTrue(response.IsSuccessful);
        }

        private ProductDto GetProductByModel(string model)
        {
            var getProductsResult = (_productController.GetAllProducts()) as OkObjectResult;
            var allProducts = getProductsResult.Value as List<ProductDto>;
            return allProducts.FirstOrDefault(p => p.Model == model);
        }
    }
}
