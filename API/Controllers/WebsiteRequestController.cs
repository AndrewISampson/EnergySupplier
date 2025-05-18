using API.Controllers.Code;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteRequestController : ControllerBase
    {
        internal ParallelOptions ParallelOptions => new() { MaxDegreeOfParallelism = 2 };

        private readonly GetProcessFunctionController _getProcessFunctionController;

        public WebsiteRequestController()
        {
            _getProcessFunctionController = new GetProcessFunctionController();
        }

        [HttpPost]
        public IActionResult Post([FromBody] dynamic requestData)
        {
            try
            {
                var jsonString = requestData.ToString();
                var json = JObject.Parse(jsonString);

                var functionArray = _getProcessFunctionController.GetProcessFunction(json);

                return _getProcessFunctionController.ProcessFunction(functionArray, json);
            }
            catch (Exception ex)
            {
                // Handle errors in case JSON parsing fails
                return BadRequest(new { error = "Invalid JSON format", details = ex.Message });
            }
        }
    }
}
