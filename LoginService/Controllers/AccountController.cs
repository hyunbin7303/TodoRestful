using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AccountController:Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
             UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {

                var jwt=await BuildToken(user);
                
                return Ok(new
                {
                    token = jwt,
                });
            }

            return Unauthorized();
        }


        [HttpPost]
        public async Task<IActionResult> Signin([FromBody] SigninModel model)
        {
            //is model valid?
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //is user registered?
            var userExists = await _userManager.FindByNameAsync(model.Username);

            //if user exist return 500 error
            if (userExists != null)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "User already exists!" });

            //create user
            AppUser user = new AppUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            //return ok if create success
            if (result.Succeeded)
            {
                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }
            //return 500 error if create fail
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                new { Status = "Error", Message = "User creation failed! Please check user details and try again." });
        }

        public async Task<string> BuildToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            //claim collection
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }




            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                  Constants.Issuer,
                  Constants.Audiance,
                  claims,
                  notBefore: DateTime.Now,
                  expires: DateTime.Now.AddHours(1),
                  signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenJson;
        }

    }
}
