using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementApi.DataStore;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System.Linq;

namespace ProductManagementApi.Controllers
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private ProductsDataStore _productsDataStore;

        public ProductController(ProductsDataStore productsDataStore)
        {
            _productsDataStore = productsDataStore;
        }

        [HttpGet()]
        public IActionResult GetAllProducts(
            [FromQuery(Name = "descr")] string descr,
            [FromQuery(Name = "model")] string model,
            [FromQuery(Name = "brand")] string brand)
        {
            var products = _productsDataStore.Products;

            if(!string.IsNullOrEmpty(descr))
                products = products.Where(p => p.Description.ToLower().Contains(descr.ToLower())).ToList();

            if (!string.IsNullOrEmpty(model))
                products = products.Where(p => p.Model.ToLower().Contains(model.ToLower())).ToList();

            if (!string.IsNullOrEmpty(brand))
                products = products.Where(p => p.Brand.ToLower().Contains(brand.ToLower())).ToList();

            return Ok(products);
        }

        [HttpGet("{productId}", Name = "GetSpecificProduct")]
        public IActionResult GetProductById(string productId)
        {
            var product = _productsDataStore.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        

        [HttpPost()]
        public IActionResult CreateNewProduct([FromBody] CreateAndEditProductDto newProduct)
        {
            if (newProduct == null)
                return BadRequest(OperationResponse.Fail("New product data is required"));

            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));
                return BadRequest(OperationResponse.Fail(errorMessages.ToArray()));
            }

            if (_productsDataStore.IsProductDuplicate(newProduct.Model, newProduct.Brand))
                return BadRequest(OperationResponse.Fail("This product is already existed. Please enter new product."));

            var newCreatedProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct);
            if (newCreatedProduct == null)
            {
                return StatusCode(500, OperationResponse.Fail("Unsuccessful saving data. Something wrong in the server."));
            }

            return CreatedAtRoute("GetSpecificProduct",
                new { productId = newCreatedProduct.Id },
                OperationResponse.Succeed(newCreatedProduct));
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(string productId, [FromBody] CreateAndEditProductDto product)
        {
            if (product == null)
                return BadRequest(OperationResponse.Fail("Product data is required"));

            var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));
            if (!ModelState.IsValid)
                return BadRequest(OperationResponse.Fail(errorMessages.ToArray()));

            if (!_productsDataStore.IsProductExists(productId))
                return BadRequest(OperationResponse.Fail("Product is not exist. Please provide a correct id."));

            var isUpdated = _productsDataStore.UpdateProductAndReturnStatus(productId, product);
            if (!isUpdated)
            {
                return StatusCode(500, OperationResponse.Fail("Unsuccessful saving data. Something wrong in the server."));
            }

            return CreatedAtRoute("GetSpecificProduct",
                productId,
                OperationResponse.Succeed(product));
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest(OperationResponse.Fail("Product Id is required"));

            if (!_productsDataStore.IsProductExists(productId))
                return BadRequest(OperationResponse.Fail("Product is not exist. Please provide a correct id."));

            var isDeleted = _productsDataStore.DeleteProductAndReturnStatus(productId);
            if (!isDeleted)
            {
                return StatusCode(500, OperationResponse.Fail("Unsuccessful deleting data. Something wrong in the server."));
            }

            return Ok(OperationResponse.Succeed());
        }
    }
}
