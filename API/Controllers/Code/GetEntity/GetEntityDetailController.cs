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
    public class GetEntityDetailController : WebsiteRequestController
    {
        public GetEntityDetailController()
        {

        }

        internal OkObjectResult GetEntityDetail(JObject json)
        {
            var getEntityDetailEntity = JsonConvert.DeserializeObject<GetEntityDetailEntity>(json.ToString());

            return getEntityDetailEntity.Entity switch
            {
                "Broker" => GetBrokerDetailOkObjectResult(getEntityDetailEntity.EntityId),
                "Customer" => GetCustomerDetailOkObjectResult(getEntityDetailEntity.EntityId),
                "Process" => GetProcessDetailOkObjectResult(getEntityDetailEntity.EntityId),
                "Setting" => GetSettingDetailOkObjectResult(getEntityDetailEntity.EntityId),
                "User" => GetUserDetailOkObjectResult(getEntityDetailEntity.EntityId),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult GetBrokerDetailOkObjectResult(long entityId)
        {
            var brokerAttributeController = new BrokerAttributeController();
            var brokerAttributeEntityList = brokerAttributeController.GetActiveEntityList();
            var brokerAttributeDictionary = brokerAttributeEntityList.ToDictionary
                (
                    a => a.Id,
                    a => a.Description
                );

            var brokerDetailController = new BrokerDetailController();
            var brokerDetailEntityList = brokerDetailController.GetActiveEntityListById(entityId);
            var brokerDetailDictionary = brokerAttributeDictionary.ToDictionary
                (
                    a => a.Value,
                    a => brokerDetailEntityList.Where(d => d.BrokerAttributeId == a.Key).ToDictionary(d => d.Id, d => d.Description)
                );

            return GetDetailList(brokerDetailDictionary);
        }

        private OkObjectResult GetCustomerDetailOkObjectResult(long entityId)
        {
            var customerAttributeController = new CustomerAttributeController();
            var customerAttributeEntityList = customerAttributeController.GetActiveEntityList();
            var customerAttributeDictionary = customerAttributeEntityList.ToDictionary
                (
                    a => a.Id,
                    a => a.Description
                );

            var customerDetailController = new CustomerDetailController();
            var customerDetailEntityList = customerDetailController.GetActiveEntityListById(entityId);
            var customerDetailDictionary = customerAttributeDictionary.ToDictionary
                (
                    a => a.Value,
                    a => customerDetailEntityList.Where(d => d.CustomerAttributeId == a.Key).ToDictionary(d => d.Id, d => d.Description)
                );

            return GetDetailList(customerDetailDictionary);
        }

        private OkObjectResult GetProcessDetailOkObjectResult(long entityId)
        {
            var processAttributeController = new ProcessAttributeController();
            var processAttributeEntityList = processAttributeController.GetActiveEntityList();
            var processAttributeDictionary = processAttributeEntityList.ToDictionary
                (
                    a => a.Id,
                    a => a.Description
                );

            var processDetailController = new ProcessDetailController();
            var processDetailEntityList = processDetailController.GetActiveEntityListById(entityId);
            var processDetailDictionary = processAttributeDictionary.ToDictionary
                (
                    a => a.Value,
                    a => processDetailEntityList.Where(d => d.ProcessAttributeId == a.Key).ToDictionary(d => d.Id, d => d.Description)
                );

            return GetDetailList(processDetailDictionary);
        }

        private OkObjectResult GetSettingDetailOkObjectResult(long entityId)
        {
            var settingAttributeController = new SettingAttributeController();
            var settingAttributeEntityList = settingAttributeController.GetActiveEntityList();
            var settingAttributeDictionary = settingAttributeEntityList.ToDictionary
                (
                    a => a.Id,
                    a => a.Description
                );

            var settingDetailController = new SettingDetailController();
            var settingDetailEntityList = settingDetailController.GetActiveEntityListById(entityId);
            var settingDetailDictionary = settingAttributeDictionary.ToDictionary
                (
                    a => a.Value,
                    a => settingDetailEntityList.Where(d => d.SettingAttributeId == a.Key).ToDictionary(d => d.Id, d => d.Description)
                );

            return GetDetailList(settingDetailDictionary);
        }

        private OkObjectResult GetUserDetailOkObjectResult(long entityId)
        {
            var userAttributeController = new UserAttributeController();
            var userAttributeEntityList = userAttributeController.GetActiveEntityList();
            var userAttributeDictionary = userAttributeEntityList.ToDictionary
                (
                    a => a.Id,
                    a => a.Description
                );

            var userDetailController = new UserDetailController();
            var userDetailEntityList = userDetailController.GetActiveEntityListById(entityId);
            var userDetailDictionary = userAttributeDictionary.ToDictionary
                (
                    a => a.Value,
                    a => userDetailEntityList.Where(d => d.UserAttributeId == a.Key).ToDictionary(d => d.Id, d => d.Description)
                );

            return GetDetailList(userDetailDictionary);
        }

        private OkObjectResult GetDetailList(Dictionary<string, Dictionary<long, string>> detailDictionary)
        {
            var entityDetailEntityList = new List<EntityDetailEntity>();

            foreach(var detailKeyValuePair in detailDictionary.OrderBy(d => d.Key))
            {
                foreach(var attributeKeyValuePair in detailKeyValuePair.Value.OrderBy(d => d.Value))
                {
                    entityDetailEntityList.Add(new EntityDetailEntity(attributeKeyValuePair.Key, detailKeyValuePair.Key, attributeKeyValuePair.Value));
                }
            }

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(entityDetailEntityList) });
        }
    }
}
