using Newtonsoft.Json;
using NUnit.Framework;
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
            // insert 2 products

            var result = await _client.GetAsync("/products", HttpCompletionOption.ResponseContentRead);
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic[] content = JsonConvert.DeserializeObject<dynamic[]>(resultContent);
            Assert.AreEqual(2, content.Length);
        }

        [Test]
        public async Task When_client_request_get_specific_products_then_the_result_is_ok_and_correct_data_is_returned()
        {
            // insert 2 products with id=PR001 and id=PR002

            var result = await _client.GetAsync("/products/PR001", HttpCompletionOption.ResponseContentRead);
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(resultContent);
            Assert.AreEqual("PR001", content.Id);
        }

        [Test]
        public async Task When_client_create_a_new_product_then_the_result_is_ok_and_data_is_created_successfully()
        {
            string createProductJson = "{'Id': 'PR003', 'Description':'Running Shoes', 'Model':'Female', 'Brand':'Adidas'}";

            var body = new StringContent(createProductJson, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/products", body);
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(resultContent);
            Assert.IsTrue(content.IsSuccessful);
        }

        [Test]
        public async Task When_client_update_a_product_then_the_result_is_ok_and_data_is_updated_successfully()
        {
            string editProductJson = "{'Description':'Walking Shoes', 'Model':'Female', 'Brand':'Nike'}";

            var body = new StringContent(editProductJson, Encoding.UTF8, "application/json");

            var result = await _client.PutAsync("/products/PR003", body);
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(resultContent);
            Assert.IsTrue(content.IsSuccessful);
        }

        [Test]
        public async Task When_client_delete_a_product_then_the_result_is_ok_and_data_is_deleted_successfully()
        {
            var result = await _client.DeleteAsync("/products/PR002");
            result.EnsureSuccessStatusCode();

            var resultContent = await result.Content.ReadAsStringAsync();
            dynamic content = JsonConvert.DeserializeObject<dynamic>(resultContent);
            Assert.IsTrue(content.IsSuccessful);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _factory.Dispose();
            _client.Dispose();
        }
    }
}
