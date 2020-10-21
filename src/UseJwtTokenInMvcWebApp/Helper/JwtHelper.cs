using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UseJwtTokenInMvcWebApp.Controllers;

namespace UseJwtTokenInMvcWebApp.Helper
{
    public class JwtHelper
    {

        const string Token = "verversecretToken:)";
        public string CreateAuthenticationTicket(LoginModel user)
        {

            var key = Encoding.ASCII.GetBytes(Token);
            var jwtToken = new JwtSecurityToken(
            issuer: "www.example.com",
            audience: "www.example.com",
            claims: GetUserClaims(user),
            notBefore: new DateTimeOffset(DateTime.Now).DateTime,
            expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return token;
        }


        private IEnumerable<Claim> GetUserClaims(LoginModel user)
        {
            List<Claim> claims = new List<Claim>();
            Claim _claim;
            _claim = new Claim(ClaimTypes.Name, user.UserName);
            claims.Add(_claim);

            _claim = new Claim(ClaimTypes.Role, "Admin");
            claims.Add(_claim);

            return claims.AsEnumerable();
        }
    }
}
