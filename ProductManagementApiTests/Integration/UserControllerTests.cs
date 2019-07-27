using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagementApiTests.Integration
{
    [TestFixture]
    public class UserControllerTests
    {
        private ProductManagementApiFactory _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new ProductManagementApiFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task When_client_create_a_new_user_then_the_result_is_ok()
        {
            //Arrange
            string createUserJson = "{'Username':'user', 'Password':'password'}";
            var body = new StringContent(createUserJson, Encoding.UTF8, "application/json");

            //Act
            var result = await _client.PostAsync("/api/users", body);

            //Assert
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task When_client_authenticate_then_the_result_is_ok_and_token_is_returned()
        {
            //Arrange
            string createUserJson = "{'Username':'user', 'Password':'password'}";
            var body = new StringContent(createUserJson, Encoding.UTF8, "application/json");
            await _client.PostAsync("/api/users", body);

            //Act
            var result = await _client.PostAsync("/api/users/authenticate", body);

            //Assert
            result.EnsureSuccessStatusCode();
            var resultContent = await result.Content.ReadAsStringAsync();
            var content = JObject.Parse(resultContent);
            Assert.IsNotEmpty(content["token"].ToString());
        }
    }
}
