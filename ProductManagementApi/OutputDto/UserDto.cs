namespace ProductManagementApi.OutputDto
{
    public class UserDto
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
