using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Model.Study;

namespace TodoApi.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Study> Get()
        {
            return null;
        }
        [HttpGet]
        public IEnumerable<Study> Get(string userId, string studyId)
        {

            return null;
        }


    }
}