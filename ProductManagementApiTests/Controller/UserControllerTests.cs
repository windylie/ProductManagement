using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using ProductManagementApi.Controllers;
using ProductManagementApi.DataStore;
using ProductManagementApi.Helper;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System.Collections.Generic;

namespace ProductManagementApiTests.Controller
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private UserDataStore _fakeDataStore;
        private IOptions<AppSettings> _fakeAppSettings;

        [SetUp]
        public void Setup()
        {
            var appSettings = new AppSettings()
            {
                Secret = "This is a secret token for testing purpose"
            };

            _fakeAppSettings = Options.Create(appSettings);

            _fakeDataStore = new UserDataStore(new List<UserAuthenticationDto>()
            {
                new UserAuthenticationDto()
                {
                    Username = "user1",
                    Password = "user1"
                }
            });
            _userController = new UserController(_fakeDataStore, _fakeAppSettings);
        }

        #region Test_create_new_user
        [Test]
        public void When_create_new_user_with_duplicate_username_then_return_bad_request()
        {
            var newUser = new UserAuthenticationDto()
            {
                Username = "user2",
                Password = "user2"
            };

            _userController.CreateNewUser(newUser);
            var result = _userController.CreateNewUser(newUser);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void When_user_is_created_successfully_then_return_ok_result()
        {
            var newUser = new UserAuthenticationDto()
            {
                Username = "user2",
                Password = "user2"
            };

            var result = _userController.CreateNewUser(newUser);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        #endregion

        #region Test_authenticate_user
        [Test]
        public void When_authenticate_with_non_existance_username_then_return_bad_request()
        {
            var user = new UserAuthenticationDto()
            {
                Username = "user2",
                Password = "user2"
            };

            var result = _userController.Authenticate(user);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void When_authenticate_successfully_then_return_ok_result()
        {
            var user = new UserAuthenticationDto()
            {
                Username = "user1",
                Password = "user1"
            };

            var result = _userController.Authenticate(user);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        #endregion
    }
}
