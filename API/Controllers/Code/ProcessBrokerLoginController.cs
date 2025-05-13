using API.Entity.Code;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var processBrokerLoginEntity = JsonConvert.DeserializeObject<ProcessBrokerLoginEntity>(json.ToString());
            var securityController = new SecurityController();

            var decryptedUsername = securityController.Decrypt(processBrokerLoginEntity.Username, processBrokerLoginEntity.iv_username);

            return Ok(new { authenticated = decryptedUsername == "Andrew.Sampson" });
        }
    }
}
