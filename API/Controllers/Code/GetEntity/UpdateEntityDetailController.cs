using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Broker.Table.Broker;
using API.Controllers.Database.Customer.Table.Customer;
using API.Controllers.Database.Information.Table.Process;
using API.Controllers.Database.Information.Table.Setting;
using API.Entity.Code.GetEntityDetail;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.GetEntity
{
    public class UpdateEntityDetailController : WebsiteRequestController
    {
        public UpdateEntityDetailController()
        {

        }

        internal OkObjectResult UpdateEntityDetail(JObject json)
        {
            var updateEntityDetailEntity = JsonConvert.DeserializeObject<UpdateEntityDetailEntity>(json.ToString());

            var securityController = new SecurityController();
            var userId = securityController.ValidateSecurityToken(updateEntityDetailEntity.SecurityToken);

            if (userId == 0)
            {
                return Ok(new { valid = false });
            }

            if (string.IsNullOrWhiteSpace(updateEntityDetailEntity.NewAttribute))
            {
                return Ok(new { valid = false, errorMessage = "Invalid value selected. Attribute cannot be empty." });
            }

            if (string.IsNullOrWhiteSpace(updateEntityDetailEntity.NewDescription))
            {
                return Ok(new { valid = false, errorMessage = "Invalid value entered. Value cannot be empty." });
            }

            return updateEntityDetailEntity.Entity switch
            {
                "Broker" => UpdateBrokerDetailEntity(updateEntityDetailEntity, userId),
                "Customer" => UpdateCustomerDetailEntity(updateEntityDetailEntity, userId),
                "Process" => UpdateProcessDetailEntity(updateEntityDetailEntity, userId),
                "Setting" => UpdateSettingDetailEntity(updateEntityDetailEntity, userId),
                "User" => UpdateUserDetailEntity(updateEntityDetailEntity, userId),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult UpdateBrokerDetailEntity(UpdateEntityDetailEntity updateEntityDetailEntity, long userId)
        {
            var brokerDetailController = new BrokerDetailController();

            if (updateEntityDetailEntity.Id == -1)
            {
                var brokerAttributeController = new BrokerAttributeController();
                var brokerAttributeEntity = brokerAttributeController.GetActiveEntityByDescription(updateEntityDetailEntity.NewAttribute);

                brokerDetailController.Insert(userId, updateEntityDetailEntity.EntityId, brokerAttributeEntity.Id, updateEntityDetailEntity.NewDescription);
            }
            else
            {
                var brokerDetailEntity = brokerDetailController.GetActiveEntityByDetailId(updateEntityDetailEntity.Id);

                if (brokerDetailEntity.Description == updateEntityDetailEntity.NewDescription)
                {
                    return Ok(new { valid = true });
                }

                brokerDetailController.UpdateEffectiveToDateTime(brokerDetailEntity.Id, userId);
                brokerDetailController.Insert(userId, brokerDetailEntity.BrokerId, brokerDetailEntity.BrokerAttributeId, updateEntityDetailEntity.NewDescription);
            }

            return Ok(new { valid = true });
        }

        private OkObjectResult UpdateCustomerDetailEntity(UpdateEntityDetailEntity updateEntityDetailEntity, long userId)
        {
            var customerDetailController = new CustomerDetailController();

            if (updateEntityDetailEntity.Id == -1)
            {
                var customerAttributeController = new CustomerAttributeController();
                var customerAttributeEntity = customerAttributeController.GetActiveEntityByDescription(updateEntityDetailEntity.NewAttribute);

                customerDetailController.Insert(userId, updateEntityDetailEntity.EntityId, customerAttributeEntity.Id, updateEntityDetailEntity.NewDescription);
            }
            else
            {
                var customerDetailEntity = customerDetailController.GetActiveEntityByDetailId(updateEntityDetailEntity.Id);

                if (customerDetailEntity.Description == updateEntityDetailEntity.NewDescription)
                {
                    return Ok(new { valid = true });
                }

                customerDetailController.UpdateEffectiveToDateTime(customerDetailEntity.Id, userId);
                customerDetailController.Insert(userId, customerDetailEntity.CustomerId, customerDetailEntity.CustomerAttributeId, updateEntityDetailEntity.NewDescription);
            }

            return Ok(new { valid = true });
        }

        private OkObjectResult UpdateProcessDetailEntity(UpdateEntityDetailEntity updateEntityDetailEntity, long userId)
        {
            var processDetailController = new ProcessDetailController();

            if (updateEntityDetailEntity.Id == -1)
            {
                var processAttributeController = new ProcessAttributeController();
                var processAttributeEntity = processAttributeController.GetActiveEntityByDescription(updateEntityDetailEntity.NewAttribute);

                processDetailController.Insert(userId, updateEntityDetailEntity.EntityId, processAttributeEntity.Id, updateEntityDetailEntity.NewDescription);
            }
            else
            {
                var processDetailEntity = processDetailController.GetActiveEntityByDetailId(updateEntityDetailEntity.Id);

                if (processDetailEntity.Description == updateEntityDetailEntity.NewDescription)
                {
                    return Ok(new { valid = true });
                }

                processDetailController.UpdateEffectiveToDateTime(processDetailEntity.Id, userId);
                processDetailController.Insert(userId, processDetailEntity.ProcessId, processDetailEntity.ProcessAttributeId, updateEntityDetailEntity.NewDescription);
            }

            return Ok(new { valid = true });
        }

        private OkObjectResult UpdateSettingDetailEntity(UpdateEntityDetailEntity updateEntityDetailEntity, long userId)
        {
            var settingDetailController = new SettingDetailController();

            if (updateEntityDetailEntity.Id == -1)
            {
                var settingAttributeController = new SettingAttributeController();
                var settingAttributeEntity = settingAttributeController.GetActiveEntityByDescription(updateEntityDetailEntity.NewAttribute);

                settingDetailController.Insert(userId, updateEntityDetailEntity.EntityId, settingAttributeEntity.Id, updateEntityDetailEntity.NewDescription);
            }
            else
            {
                var settingDetailEntity = settingDetailController.GetActiveEntityByDetailId(updateEntityDetailEntity.Id);

                if (settingDetailEntity.Description == updateEntityDetailEntity.NewDescription)
                {
                    return Ok(new { valid = true });
                }

                settingDetailController.UpdateEffectiveToDateTime(settingDetailEntity.Id, userId);
                settingDetailController.Insert(userId, settingDetailEntity.SettingId, settingDetailEntity.SettingAttributeId, updateEntityDetailEntity.NewDescription);
            }

            return Ok(new { valid = true });
        }

        private OkObjectResult UpdateUserDetailEntity(UpdateEntityDetailEntity updateEntityDetailEntity, long userId)
        {
            var userDetailController = new UserDetailController();

            if (updateEntityDetailEntity.Id == -1)
            {
                var userAttributeController = new UserAttributeController();
                var userAttributeEntity = userAttributeController.GetActiveEntityByDescription(updateEntityDetailEntity.NewAttribute);

                userDetailController.Insert(userId, updateEntityDetailEntity.EntityId, userAttributeEntity.Id, updateEntityDetailEntity.NewDescription);
            }
            else
            {
                var userDetailEntity = userDetailController.GetActiveEntityByDetailId(updateEntityDetailEntity.Id);

                if (userDetailEntity.Description == updateEntityDetailEntity.NewDescription)
                {
                    return Ok(new { valid = true });
                }

                userDetailController.UpdateEffectiveToDateTime(userDetailEntity.Id, userId);
                userDetailController.Insert(userId, userDetailEntity.UserId, userDetailEntity.UserAttributeId, updateEntityDetailEntity.NewDescription);
            }

            return Ok(new { valid = true });
        }
    }
}
