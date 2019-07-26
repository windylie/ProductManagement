using NUnit.Framework;
using ProductManagementApi.DataStore;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;

namespace ProductManagementApiTests.DataStore
{
    [TestFixture]
    public class ProductDataStoreTests
    {
        private ProductsDataStore _productsDataStore;

        [SetUp]
        public void Setup()
        {
            _productsDataStore = new ProductsDataStore();
        }

        #region Test_create_method
        [Test]
        public void When_create_new_product_then_product_is_added_into_the_list()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct2 = new CreateAndEditProductDto()
            {
                Description = "Product 2",
                Model = "A2",
                Brand = "TheProduct"
            };

            _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            var products = _productsDataStore.Products;
            Assert.AreEqual(1, products.Count);

            _productsDataStore.CreateAndReturnNewProduct(newProduct2);
            Assert.AreEqual(2, products.Count);

            Assert.AreEqual("Product 1", products[0].Description);
            Assert.AreEqual("A1", products[0].Model);
            Assert.AreEqual("TheProduct", products[0].Brand);
        }

        [Test]
        public void When_create_new_product_then_return_the_newly_added_product()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct2 = new CreateAndEditProductDto()
            {
                Description = "Product 2",
                Model = "A2",
                Brand = "TheProduct"
            };

            var product = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            Assert.IsInstanceOf<ProductDto>(product);
            Assert.AreEqual("A1", product.Model);

            product = _productsDataStore.CreateAndReturnNewProduct(newProduct2);
            Assert.AreEqual("A2", product.Model);
        }

        [Test]
        public void When_create_new_product_then_product_id_is_generated()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var product = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            Assert.IsNotNull(product.Id);
            Assert.IsNotEmpty(product.Id);
        }
        #endregion

        #region Test_update_method
        [Test]
        public void When_update_product_then_product_is_updated_and_return_successful_status()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var updateProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 3",
                Model = "A3",
                Brand = "TheProduct"
            };

            var newProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            Assert.AreEqual("A1", newProduct.Model);

            var isUpdated = _productsDataStore.UpdateProductAndReturnStatus(newProduct.Id, updateProduct1);
            Assert.IsTrue(isUpdated);

            var products = _productsDataStore.Products;
            Assert.AreEqual("A3", products[0].Model);
        }

        [Test]
        public void When_there_is_an_exception_during_update_product_then_return_unsuccessful_status()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct1);

            var isUpdated = _productsDataStore.UpdateProductAndReturnStatus(newProduct.Id, null);
            Assert.IsFalse(isUpdated);

            var products = _productsDataStore.Products;
            Assert.AreEqual("A1", products[0].Model);
        }
        #endregion

        #region Test_delete_method
        [Test]
        public void When_delete_product_then_product_id_deleted_and_return_succesful_status()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct2 = new CreateAndEditProductDto()
            {
                Description = "Product 2",
                Model = "A2",
                Brand = "TheProduct"
            };

            var product1 = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            var product2 = _productsDataStore.CreateAndReturnNewProduct(newProduct2);
            var products = _productsDataStore.Products;
            Assert.AreEqual(2, products.Count);

            var isDeleted = _productsDataStore.DeleteProductAndReturnStatus(product1.Id);
            Assert.IsTrue(isDeleted);
            Assert.AreEqual(1, products.Count);
        }

        [Test]
        public void When_there_is_an_exception_during_delete_product_then_return_unsuccessful_status()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            var products = _productsDataStore.Products;
            Assert.AreEqual(1, products.Count);

            var isDeleted = _productsDataStore.DeleteProductAndReturnStatus(null);
            Assert.IsFalse(isDeleted);
            Assert.AreEqual(1, products.Count);
        }
        #endregion

        #region Test_validation_method
        [Test]
        public void When_validate_product_existance_then_return_correct_result()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            Assert.IsTrue(_productsDataStore.IsProductExists(newProduct.Id));
            Assert.IsFalse(_productsDataStore.IsProductExists("randomId"));
        }

        [Test]
        public void When_validate_product_duplication_then_return_correct_result()
        {
            var newProduct1 = new CreateAndEditProductDto()
            {
                Description = "Product 1",
                Model = "A1",
                Brand = "TheProduct"
            };

            var newProduct2 = new CreateAndEditProductDto()
            {
                Description = "Product 2",
                Model = "A2",
                Brand = "TheProduct"
            };

            var newProduct = _productsDataStore.CreateAndReturnNewProduct(newProduct1);
            Assert.IsTrue(_productsDataStore.IsProductDuplicate(newProduct1.Model, newProduct1.Brand));
            Assert.IsFalse(_productsDataStore.IsProductDuplicate(newProduct2.Model, newProduct2.Brand));
        }
        #endregion
    }
}
