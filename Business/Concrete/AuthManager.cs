using Business.Abstract;
using Dataaccess.Abstract;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {

        private readonly IUsersDal _userDal;
        private readonly IConfiguration _configuration;

        public AuthManager(IUsersDal userDal, IConfiguration configuration)
        {
            _userDal = userDal;
            _configuration = configuration;
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string Login(string email, string password)
        {
            var user = _userDal.Get(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        public string Register(UserRegisterDto userDto, string password)
        {
            var existingUser = _userDal.Get(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Bu kullanıcı zaten mevcut.");
            }

            var (publicKey, privateKey) = GenerateKeyPair();


            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                PublicKey = publicKey,
                EncryptedPrivateKey = privateKey,
                CreatedAt = DateTime.Now
            };

            _userDal.Add(user);

            return GenerateJwtToken(user);
        }


        public (string PublicKey, string PrivateKey) GenerateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    string publicKey = rsa.ToXmlString(false); 
                    string privateKey = rsa.ToXmlString(true); 
                    return (publicKey, privateKey);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false; 
                }
            }
        }
    }
}
