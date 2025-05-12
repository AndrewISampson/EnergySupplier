using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code
{
    public class ProcessBrokerLoginController : ControllerBase
    {
        public ProcessBrokerLoginController()
        {

        }

        internal IActionResult ProcessBrokerLogin(JObject json)
        {
            return Ok(new { message = $"I made it to the ProcessBrokerLogin function" });
        }
    }
}
