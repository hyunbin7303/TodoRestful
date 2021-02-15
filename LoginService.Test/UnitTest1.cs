using IdentityServer;
using IdentityServer.Controllers;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace LoginService.Test
{
    public class Tests
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        [SetUp]
        public void Setup(UserManager<AppUser> userManager)
        {
            //_userManager = userManager;
        }

        [Test]
        public async System.Threading.Tasks.Task Test1Async()
        {
            LoginModel loginModel = new LoginModel()
            {
                Username = "hyunbin7303",
                Password = ""
            };

            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {

                //var jwt = await BuildToken(user);

                //return Ok(new
                //{
                //    token = jwt,
                //});
            
            
            }



            Assert.Pass();
        }
    }
}