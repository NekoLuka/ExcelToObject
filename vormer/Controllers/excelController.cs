using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace vormer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class excelController : ControllerBase
    {
        [HttpPost(Name = "upload")]
        public string Upload()
        {
            return "Hello world";
        }
    }
}
