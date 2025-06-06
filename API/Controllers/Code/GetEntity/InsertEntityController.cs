using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Broker.Table.Broker;
using API.Controllers.Database.Customer.Table.Customer;
using API.Controllers.Database.Information.Table.Process;
using API.Controllers.Database.Information.Table.Setting;
using API.Entity.Code.GetEntity.InsertEntity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.GetEntity
{
    public class InsertEntityController : WebsiteRequestController
    {
        public InsertEntityController()
        {

        }

        public OkObjectResult InsertEntity(JObject json)
        {
            var insertEntityEntity = JsonConvert.DeserializeObject<InsertEntityEntity>(json.ToString());

            var securityController = new SecurityController();
            var userId = securityController.ValidateSecurityToken(insertEntityEntity.SecurityToken);

            if (userId == 0)
            {
                return Ok(new { valid = false });
            }

            return insertEntityEntity.Entity switch
            {
                "Broker" => InsertBroker(insertEntityEntity, userId),
                "Customer" => InsertCustomer(insertEntityEntity, userId),
                "Process" => InsertProcess(insertEntityEntity, userId),
                "Setting" => InsertSetting(insertEntityEntity, userId),
                "User" => InsertUser(insertEntityEntity, userId),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult InsertBroker(InsertEntityEntity insertEntityEntity, long userId)
        {
            var requiredAttributeFound = insertEntityEntity.Attributes.Any(a => a == "Name");

            if (!requiredAttributeFound)
            {
                return Ok(new { valid = false, errorMessage = "Required attribute 'Name' is missing." });
            }

            var nameIndex = insertEntityEntity.Attributes.IndexOf("Name");
            var nameDescription = insertEntityEntity.Descriptions[nameIndex];

            var brokerAttributeController = new BrokerAttributeController();
            var nameBrokerAttributeEntity = brokerAttributeController.GetActiveEntityByDescription("Name");

            var brokerDetailController = new BrokerDetailController();
            var existingNameBrokerDetailEntity = brokerDetailController.GetActiveEntityByAttributeIdAndDescription(nameBrokerAttributeEntity.Id, nameDescription);

            if (existingNameBrokerDetailEntity.Id > 0)
            {
                return Ok(new { valid = false, errorMessage = "This broker already exists." });
            }

            var brokerController = new BrokerController();
            var brokerEntity = brokerController.InsertNewAndGetEntity(userId);

            for (var i = 0; i < insertEntityEntity.Attributes.Count; i++)
            {
                var brokerAttributeEntity = brokerAttributeController.GetActiveEntityByDescription(insertEntityEntity.Attributes[i]);
                brokerDetailController.Insert(userId, brokerEntity.Id, brokerAttributeEntity.Id, insertEntityEntity.Descriptions[i]);
            }

            return Ok(new { valid = true, new_entity_id = brokerEntity.Id });
        }

        private OkObjectResult InsertCustomer(InsertEntityEntity insertEntityEntity, long userId)
        {
            var requiredAttributeFound = insertEntityEntity.Attributes.Any(a => a == "Name");

            if (!requiredAttributeFound)
            {
                return Ok(new { valid = false, errorMessage = "Required attribute 'Name' is missing." });
            }

            var nameIndex = insertEntityEntity.Attributes.IndexOf("Name");
            var nameDescription = insertEntityEntity.Descriptions[nameIndex];

            var customerAttributeController = new CustomerAttributeController();
            var nameCustomerAttributeEntity = customerAttributeController.GetActiveEntityByDescription("Name");

            var customerDetailController = new CustomerDetailController();
            var existingNameCustomerDetailEntity = customerDetailController.GetActiveEntityByAttributeIdAndDescription(nameCustomerAttributeEntity.Id, nameDescription);

            if (existingNameCustomerDetailEntity.Id > 0)
            {
                return Ok(new { valid = false, errorMessage = "This customer already exists." });
            }

            var customerController = new CustomerController();
            var customerEntity = customerController.InsertNewAndGetEntity(userId);

            for (var i = 0; i < insertEntityEntity.Attributes.Count; i++)
            {
                var customerAttributeEntity = customerAttributeController.GetActiveEntityByDescription(insertEntityEntity.Attributes[i]);
                customerDetailController.Insert(userId, customerEntity.Id, customerAttributeEntity.Id, insertEntityEntity.Descriptions[i]);
            }

            return Ok(new { valid = true, new_entity_id = customerEntity.Id });
        }

        private OkObjectResult InsertProcess(InsertEntityEntity insertEntityEntity, long userId)
        {
            var requiredAttributeFound = insertEntityEntity.Attributes.Any(a => a == "Name");

            if (!requiredAttributeFound)
            {
                return Ok(new { valid = false, errorMessage = "Required attribute 'Name' is missing." });
            }

            var nameIndex = insertEntityEntity.Attributes.IndexOf("Name");
            var nameDescription = insertEntityEntity.Descriptions[nameIndex];

            var processAttributeController = new ProcessAttributeController();
            var nameProcessAttributeEntity = processAttributeController.GetActiveEntityByDescription("Name");

            var processDetailController = new ProcessDetailController();
            var existingNameProcessDetailEntity = processDetailController.GetActiveEntityByAttributeIdAndDescription(nameProcessAttributeEntity.Id, nameDescription);

            if (existingNameProcessDetailEntity.Id > 0)
            {
                return Ok(new { valid = false, errorMessage = "This process already exists." });
            }

            var processController = new ProcessController();
            var processEntity = processController.InsertNewAndGetEntity(userId);

            for (var i = 0; i < insertEntityEntity.Attributes.Count; i++)
            {
                var processAttributeEntity = processAttributeController.GetActiveEntityByDescription(insertEntityEntity.Attributes[i]);
                processDetailController.Insert(userId, processEntity.Id, processAttributeEntity.Id, insertEntityEntity.Descriptions[i]);
            }

            return Ok(new { valid = true, new_entity_id = processEntity.Id });
        }

        private OkObjectResult InsertSetting(InsertEntityEntity insertEntityEntity, long userId)
        {
            var requiredAttributeFound = insertEntityEntity.Attributes.Any(a => a == "Name");

            if (!requiredAttributeFound)
            {
                return Ok(new { valid = false, errorMessage = "Required attribute 'Name' is missing." });
            }

            var nameIndex = insertEntityEntity.Attributes.IndexOf("Name");
            var nameDescription = insertEntityEntity.Descriptions[nameIndex];

            var settingAttributeController = new SettingAttributeController();
            var nameSettingAttributeEntity = settingAttributeController.GetActiveEntityByDescription("Name");

            var settingDetailController = new SettingDetailController();
            var existingNameSettingDetailEntity = settingDetailController.GetActiveEntityByAttributeIdAndDescription(nameSettingAttributeEntity.Id, nameDescription);

            if (existingNameSettingDetailEntity.Id > 0)
            {
                return Ok(new { valid = false, errorMessage = "This setting already exists." });
            }

            var settingController = new SettingController();
            var settingEntity = settingController.InsertNewAndGetEntity(userId);

            for (var i = 0; i < insertEntityEntity.Attributes.Count; i++)
            {
                var settingAttributeEntity = settingAttributeController.GetActiveEntityByDescription(insertEntityEntity.Attributes[i]);
                settingDetailController.Insert(userId, settingEntity.Id, settingAttributeEntity.Id, insertEntityEntity.Descriptions[i]);
            }

            return Ok(new { valid = true, new_entity_id = settingEntity.Id });
        }

        private OkObjectResult InsertUser(InsertEntityEntity insertEntityEntity, long userId)
        {
            var requiredAttributeFound = insertEntityEntity.Attributes.Any(a => a == "Email Address");

            if (!requiredAttributeFound)
            {
                return Ok(new { valid = false, errorMessage = "Required attribute 'Email Address' is missing." });
            }

            var emailAddressIndex = insertEntityEntity.Attributes.IndexOf("Email Address");
            var emailAddressDescription = insertEntityEntity.Descriptions[emailAddressIndex];

            var userAttributeController = new UserAttributeController();
            var emailAddressUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Email Address");

            var userDetailController = new UserDetailController();
            var existingEmailAddressUserDetailEntity = userDetailController.GetActiveEntityByAttributeIdAndDescription(emailAddressUserAttributeEntity.Id, emailAddressDescription);

            if (existingEmailAddressUserDetailEntity.Id > 0)
            {
                return Ok(new { valid = false, errorMessage = "This user already exists." });
            }

            var userController = new UserController();
            var userEntity = userController.InsertNewAndGetEntity(userId);

            for (var i = 0; i < insertEntityEntity.Attributes.Count; i++)
            {
                var userAttributeEntity = userAttributeController.GetActiveEntityByDescription(insertEntityEntity.Attributes[i]);
                userDetailController.Insert(userId, userEntity.Id, userAttributeEntity.Id, insertEntityEntity.Descriptions[i]);
            }

            return Ok(new { valid = true, new_entity_id = userEntity.Id });
        }
    }
}
