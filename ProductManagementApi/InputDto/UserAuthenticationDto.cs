using System.ComponentModel.DataAnnotations;

namespace ProductManagementApi.InputDto
{
    public class UserAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
