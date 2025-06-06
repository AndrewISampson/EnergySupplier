using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Broker.Table.Broker;
using API.Controllers.Database.Customer.Table.Customer;
using API.Controllers.Database.Information.Table.Process;
using API.Controllers.Database.Information.Table.Setting;
using API.Entity.Code.GetEntityAttribute;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.GetEntity
{
    public class GetEntityAttributeController : WebsiteRequestController
    {
        public GetEntityAttributeController()
        {

        }

        public OkObjectResult GetEntityAttribute(JObject json)
        {
            var getEntityAttributeEntity = JsonConvert.DeserializeObject<GetEntityAttributeEntity>(json.ToString());

            var securityController = new SecurityController();
            var userId = securityController.ValidateSecurityToken(getEntityAttributeEntity.SecurityToken);

            if (userId == 0)
            {
                return Ok(new { valid = false });
            }

            return getEntityAttributeEntity.Entity switch
            {
                "Broker" => GetBrokerAttributeList(),
                "Customer" => GetCustomerAttributeList(),
                "Process" => GetProcessAttributeList(),
                "Setting" => GetSettingAttributeList(),
                "User" => GetUserAttributeList(),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult GetBrokerAttributeList()
        {
            var brokerAttributeController = new BrokerAttributeController();
            var brokerAttributeEntityList = brokerAttributeController.GetActiveEntityList();

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(brokerAttributeEntityList.Select(a => new EntityAttributeEntity(a.Description))) });
        }

        private OkObjectResult GetCustomerAttributeList()
        {
            var customerAttributeController = new CustomerAttributeController();
            var customerAttributeEntityList = customerAttributeController.GetActiveEntityList();

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(customerAttributeEntityList.Select(a => new EntityAttributeEntity(a.Description))) });
        }

        private OkObjectResult GetProcessAttributeList()
        {
            var processAttributeController = new ProcessAttributeController();
            var processAttributeEntityList = processAttributeController.GetActiveEntityList();

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(processAttributeEntityList.Select(a => new EntityAttributeEntity(a.Description))) });
        }

        private OkObjectResult GetSettingAttributeList()
        {
            var settingAttributeController = new SettingAttributeController();
            var settingAttributeEntityList = settingAttributeController.GetActiveEntityList();

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(settingAttributeEntityList.Select(a => new EntityAttributeEntity(a.Description))) });
        }

        private OkObjectResult GetUserAttributeList()
        {
            var userAttributeController = new UserAttributeController();
            var userAttributeEntityList = userAttributeController.GetActiveEntityList();

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(userAttributeEntityList.Select(a => new EntityAttributeEntity(a.Description))) });
        }
    }
}
