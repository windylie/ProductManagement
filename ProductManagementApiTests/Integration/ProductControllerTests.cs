using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementApiTests.Integration
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductManagementApiFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new ProductManagementApiFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task When_client_request_get_products_then_the_result_is_ok_and_correct_data_are_returned()
        {
            //Arrange
            var product1 = "{'Description':'This is product 1', 'Model':'12345', 'Brand':'MyProduct'}";
            var product2 = "{'Description':'This is product 2', 'Model':'98765', 'Brand':'MyProduct'}";
            await CreateData(product1);
            await CreateData(product2);

            //Act
            var result = await _client.GetAsync("/api/products", HttpCompletionOption.ResponseContentRead);

            //Assert
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            var content = JArray.Parse(resultContent);

            var isProduct1Exists = content.Children<JObject>().FirstOrDefault(o => o["model"].ToString() == "12345") != null;
            var isProduct2Exists = content.Children<JObject>().FirstOrDefault(o => o["model"].ToString() == "98765") != null;
            Assert.IsTrue(isProduct1Exists);
            Assert.IsTrue(isProduct2Exists);
        }

        [Test]
        public async Task When_client_create_a_new_product_then_the_result_is_ok_and_data_is_created_successfully()
        {
            //Arrange
            string createProductJson = "{'Description':'Running Shoes', 'Model':'RG3432846', 'Brand':'Adidas'}";
            var body = new StringContent(createProductJson, Encoding.UTF8, "application/json");

            //Act
            var result = await _client.PostAsync("/api/products", body);

            //Assert
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            var content = JObject.Parse(resultContent);
            Assert.IsTrue(content["isSuccessful"].Value<bool>());
            Assert.AreEqual("RG3432846", content["data"]["model"].ToString());
            Assert.AreEqual("Adidas", content["data"]["brand"].ToString());

            //Act
            var newProductUrl = result.Headers.Location;
            result = await _client.GetAsync(newProductUrl, HttpCompletionOption.ResponseContentRead);

            //Assert
            result.EnsureSuccessStatusCode();

            resultContent = await result.Content.ReadAsStringAsync();
            content = JObject.Parse(resultContent);
            Assert.AreEqual("RG3432846", content["model"].ToString());
        }

        [Test]
        public async Task When_client_update_a_product_then_the_result_is_ok_and_data_is_updated_successfully()
        {
            //Arrange
            var product1 = "{'Description':'This is product 1', 'Model':'12345', 'Brand':'MyProduct'}";
            var product2 = "{'Description':'This is product 2', 'Model':'98765', 'Brand':'MyProduct'}";
            await CreateData(product1);
            await CreateData(product2);

            var getAllProducts = await _client.GetAsync("/api/products", HttpCompletionOption.ResponseContentRead);
            var allProductsContent = await getAllProducts.Content.ReadAsStringAsync();
            var createdProduct2 = JArray.Parse(allProductsContent).Children<JObject>().FirstOrDefault(o => o["model"].ToString() == "98765");
            var product2Id = createdProduct2["id"].ToString();

            string editProduct2Json = "{'Description':'Walking Shoes', 'Model':'Female', 'Brand':'Nike'}";
            var body = new StringContent(editProduct2Json, Encoding.UTF8, "application/json");

            //Act
            var result = await _client.PutAsync("/api/products/" + product2Id, body);

            //Assert
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            var content = JObject.Parse(resultContent);
            Assert.IsTrue(content["isSuccessful"].Value<bool>());

            //Act
            var updatedProductUrl = result.Headers.Location;
            result = await _client.GetAsync(updatedProductUrl, HttpCompletionOption.ResponseContentRead);
            resultContent = await result.Content.ReadAsStringAsync();
            content = JObject.Parse(resultContent);
            
            //Assert
            Assert.AreEqual("Walking Shoes", content["description"].ToString());
            Assert.AreEqual("Female", content["model"].ToString());
            Assert.AreEqual("Nike", content["brand"].ToString());
        }

        [Test]
        public async Task When_client_delete_a_product_then_the_result_is_ok_and_data_is_deleted_successfully()
        {
            //Arrange
            var product1 = "{'Description':'This is product 1', 'Model':'12345', 'Brand':'MyProduct'}";
            var product2 = "{'Description':'This is product 2', 'Model':'98765', 'Brand':'MyProduct'}";
            await CreateData(product1);
            await CreateData(product2);

            var getAllProducts = await _client.GetAsync("/api/products", HttpCompletionOption.ResponseContentRead);
            var allProductsContent = await getAllProducts.Content.ReadAsStringAsync();
            var createdProduct2 = JArray.Parse(allProductsContent).Children<JObject>().FirstOrDefault(o => o["model"].ToString() == "98765");
            var product2Id = createdProduct2["id"].ToString();

            //Act
            var result = await _client.DeleteAsync("/api/products/" + product2Id);

            //Assert
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            var content = JObject.Parse(resultContent);
            Assert.IsTrue(content["isSuccessful"].Value<bool>());

            //Act
            var getProduct2 = await _client.GetAsync("/api/products/" + product2Id, HttpCompletionOption.ResponseHeadersRead);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, getProduct2.StatusCode);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        private async Task CreateData(string jsonBody)
        {
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/products", body);
        }
    }
}
