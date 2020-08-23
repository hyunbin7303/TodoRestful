using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using IdentityModel.Client;
using IdentityServer4;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Model.Identity;

namespace TodoApi.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            using(HttpClient client = new HttpClient())
            {

                var url = $"{HttpContext.Request.Host}{HttpUtility.UrlEncode(returnUrl)}";
                var get = client.GetAsync($"https://localhost:5001/Account/Login?returnUrl={url}");
                var response = get.Result;
                return Redirect(response.RequestMessage.RequestUri.AbsoluteUri);
            }
            //
        }

        
    }
}
