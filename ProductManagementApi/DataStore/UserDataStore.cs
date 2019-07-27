using ProductManagementApi.InputDto;
using ProductManagementApi.OutputDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagementApi.DataStore
{
    public class UserDataStore
    {
        public IList<UserDto> Users { get; private set; }

        public UserDataStore(List<UserAuthenticationDto> initialUsers)
        {
            Users = new List<UserDto>();

            foreach (var user in initialUsers)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

                Users.Add(new UserDto()
                {
                    Username = user.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });
            }
        }

        public UserDataStore() : this(new List<UserAuthenticationDto>())
        {
        }

        public bool CreateNewUserAndReturnStatus(string username, string password)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                Users.Add(new UserDto() {
                    Username = username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                return true;
            } catch(Exception)
            {
                return false;
            }
        }

        public UserDto GetUserByUsername(string username)
        {
            return Users.FirstOrDefault(u => u.Username == username);
        }

        public UserDto Authenticate(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && VerifyPasswordHash(password, u.PasswordHash, u.PasswordSalt));
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64)
                return false;

            if (storedSalt.Length != 128)
                return false;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
