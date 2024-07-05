using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace vormer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class excelController : ControllerBase
    {
        [HttpPost(Name = "upload")]
        public object Upload([FromForm] ExcelUpload uploadData)
        {
            var ExcelConfig = JsonSerializer.Deserialize<ExcelConfig[]>(uploadData.ExcelConfig);
            return ExcelConfig;
        }
    }
}
