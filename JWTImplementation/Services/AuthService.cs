using JWTImplementation.Context;
using JWTImplementation.Interfaces;
using JWTImplementation.Models;
using JWTImplementation.RequestModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTImplementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _jwtContext;
        private readonly IConfiguration _configuration;

        public AuthService(JwtContext jwtContext, IConfiguration configuration)
        {
            _jwtContext = jwtContext;
            _configuration = configuration;
        }

        public User AddUser(User user)
        {
            var addedUser = _jwtContext.Users.Add(user);
            _jwtContext.SaveChanges();
            return addedUser.Entity;
        }

        public string Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest.Username != null && loginRequest.Pasword != null)
                {
                    var user = _jwtContext.Users.SingleOrDefault(x => x.Email == loginRequest.Username && x.Password == loginRequest.Pasword);

                    if (user != null)
                    {
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subjects"]),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("UserName", user.Name),
                            new Claim("Email", user.Email)
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );

                        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                        return jwtToken;
                    }
                    else
                    {
                        throw new Exception("User is invalid");
                    }
                }
                else
                {
                    throw new Exception("Credentials are invalid");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
