using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Repositories
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        private readonly UserManager<User> userManager;

        private readonly SymmetricSecurityKey key;

        private readonly IPermissionService permissionService;

        public TokenService(IConfiguration configuration, UserManager<User> userManager, IPermissionService permissionService)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            //this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTConfiguration: Key"]));
            var keyString = configuration["JWTConfiguration:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                throw new ArgumentNullException("JWTConfiguration:Key", "JWT Key cannot be null or empty. Please check your appsettings.json file.");
            }

            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            this.permissionService = permissionService;
        }

        public async Task<string> CreateToken(User user)
        {
            if (user.PhoneNumber == null) return null;
            
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.DisplayName ?? ""),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id ?? "" ),
                new Claim("PhoneNumber", user.PhoneNumber ?? ""),
            };

            var permissions = await permissionService.GetUserPermissionsAsync(user);
            if (permissions != null && permissions.Any())
            {
                claims.Add(new Claim("permissions", JsonConvert.SerializeObject(permissions)));
            }
                
            var roles = await userManager.GetRolesAsync(user);

            if (roles != null && roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = configuration["JWTConfiguration:Issuer"],
                Audience = configuration["JWTConfiguration:Audience"],
                IssuedAt = DateTime.Now,
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = credential,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return await Task.Run(() => tokenHandler.WriteToken(token)).ConfigureAwait(false); 
        }
    }
}
