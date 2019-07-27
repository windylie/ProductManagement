using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductManagementApi.DataStore;
using ProductManagementApi.Helper;
using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProductManagementApi.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UserController : Controller
    {
        private UserDataStore _userDataStore;
        private readonly AppSettings _appSettings;

        public UserController(UserDataStore userDataStore, IOptions<AppSettings> appSettings)
        {
            _userDataStore = userDataStore;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateNewUser([FromBody]UserAuthenticationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));
                return BadRequest(OperationResponse.Fail(errorMessages.ToArray()));
            }

            if (_userDataStore.GetUserByUsername(userDto.Username) != null)
                return BadRequest(OperationResponse.Fail("Username is already taken!"));

            var isUserCreated = _userDataStore.CreateNewUserAndReturnStatus(userDto.Username, userDto.Password);

            if (!isUserCreated)
            {
                return StatusCode(500, OperationResponse.Fail("Unsuccessful creating new user. Something wrong in the server."));
            }

            return Ok(OperationResponse.Succeed());
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserAuthenticationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = ModelState.Values.SelectMany(s => s.Errors.Select(e => e.ErrorMessage));
                return BadRequest(OperationResponse.Fail(errorMessages.ToArray()));
            }

            if (_userDataStore.GetUserByUsername(userDto.Username) == null)
                return BadRequest(OperationResponse.Fail("User is not exists! Please register first!"));

            var user = _userDataStore.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(OperationResponse.Fail("Password is not correct!"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString
            });
        }
        
    }
}
