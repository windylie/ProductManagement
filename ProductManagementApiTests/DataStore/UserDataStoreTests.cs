using NUnit.Framework;
using ProductManagementApi.DataStore;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;

namespace ProductManagementApiTests.DataStore
{
    [TestFixture]
    public class UserDataStoreTests
    {
        private UserDataStore _userDataStore;

        [SetUp]
        public void Setup()
        {
            _userDataStore = new UserDataStore();
        }

        [Test]
        public void When_create_new_user_then_user_is_added_into_the_list_and_return_successful_status()
        {
            var newUser1 = new UserAuthenticationDto()
            {
                Username = "user1",
                Password = "user1"
            };

            var status = _userDataStore.CreateNewUserAndReturnStatus(newUser1.Username, newUser1.Password);
            var user = _userDataStore.GetUserByUsername("user1");
            Assert.IsNotNull(user);
            Assert.IsInstanceOf<UserDto>(user);
            Assert.IsTrue(user.PasswordHash.Length > 0);
            Assert.IsTrue(user.PasswordSalt.Length > 0);
            Assert.IsTrue(status);
        }

        [Test]
        public void When_authenticate_then_return_correct_result()
        {
            var newUser1 = new UserAuthenticationDto()
            {
                Username = "user1",
                Password = "user1"
            };

            _userDataStore.CreateNewUserAndReturnStatus(newUser1.Username, newUser1.Password);

            var user = _userDataStore.Authenticate(newUser1.Username, newUser1.Password);
            Assert.IsNotNull(user);
            Assert.IsInstanceOf<UserDto>(user);
            Assert.AreEqual("user1", user.Username);
        }




    }
}
