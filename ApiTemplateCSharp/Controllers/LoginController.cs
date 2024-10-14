using ApiTemplateCSharp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplateCSharp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserAccountsDbContext _db;

        public LoginController(UserAccountsDbContext db)
        {
            _db = db;
        }

        private async Task<UserAccounts> GetSignInCred(string username, string password)
        {
            return await _db.UserAccounts.FirstOrDefaultAsync(ua => ua.Username == username && ua.Password == password);
        }

        [HttpPost]
        public IActionResult SignIn(IFormCollection fc)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var user_accounts = GetSignInCred(fc["username"], fc["password"]);

            if (user_accounts.Result != null)
            {
                var username = user_accounts.Result.Username;
                var full_name = user_accounts.Result.FullName;
                var section = user_accounts.Result.Section;
                var role = user_accounts.Result.Role;

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                // G7x3FaKN5+QgsYe@
                var key = Encoding.ASCII.GetBytes("Bd00NXIrhM6ttxnH6JjOXBE2jWM+OFTRQktHx/9p1uk=\r\n"); // Use the same key as in ConfigureServices
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1), // Set token expiration
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                data.Add("token", tokenString);
                data.Add("username", username);
                data.Add("full_name", full_name);
                data.Add("section", section);
                data.Add("role", role);
                data.Add("message", "success");
            }
            else
            {
                data.Add("message", "failed");
            }

            return Json(data);
        }

        [HttpGet]
        public IActionResult SigningOut()
        {
            //HttpContext.Session.Remove(NameSessionKey);
            //HttpContext.Session.Remove(UsernameSessionKey);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
