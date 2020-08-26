using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApi.Controllers
{
    public class HomeController:Controller
    {

        [Authorize]
        public string Secret()
        {
            return "This is secret message";
        }
    }
}
