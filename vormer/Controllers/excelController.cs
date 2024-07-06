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
            /* Sanetize input data */
            if (!uploadData.ExcelFile.FileName.EndsWith(".xlsx")) return BadRequest("Invalid file type");

            ExcelConfig[]? excelConfig;
            try
            {
                excelConfig = JsonSerializer.Deserialize<ExcelConfig[]>(uploadData.ExcelConfig);
            }
            catch 
            {
                return BadRequest("Invalid value for parameter ExcelConfig");
            }

            if (excelConfig == null) return BadRequest("Invalid value for parameter ExcelConfig");

            /* Process data */
            var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(uploadData.ExcelFile.OpenReadStream());

            // Loop through the data sheets until it landed on the one specified in the request
            while (reader.Name != uploadData.ExcelSheetName) reader.NextResult();

            // Return an error when the excel has more columns than the config, since then there would be no data to match
            if (reader.FieldCount > excelConfig.Length) return BadRequest("Invalid value for parameter ExcelConfig");

            // Populate the return object with the column names and an array to store the values into
            // Also create an easier index to look up column index and name
            Dictionary<string, List<object>> finalObject = new Dictionary<string, List<object>>();
            string[] lookupIndex = new string[excelConfig.Length];
            for (int i = 0; i < excelConfig.Length; i++)
            {
                ExcelConfig column = excelConfig[i];
                finalObject.Add(column.ColName, new List<object>());

                lookupIndex[column.ColIndex] = column.ColName;
            }

            while (reader.Read()) // Loop trough rows untill none are left
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    object value = reader.GetValue(i);
                    finalObject[lookupIndex[i]].Add(value);
                }
            }

            return finalObject;
        }
    }
}
