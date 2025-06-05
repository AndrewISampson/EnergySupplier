using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Broker.Table.Broker;
using API.Controllers.Database.Customer.Table.Customer;
using API.Controllers.Database.Information.Table.Process;
using API.Controllers.Database.Information.Table.Setting;
using API.Entity.Code.GetEntitySummary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.GetEntity
{
    public class GetEntitySummaryController : WebsiteRequestController
    {
        public GetEntitySummaryController()
        {

        }

        internal OkObjectResult GetEntitySummary(JObject json)
        {
            var getEntitySummaryEntity = JsonConvert.DeserializeObject<GetEntitySummaryEntity>(json.ToString());

            return getEntitySummaryEntity.Entity switch
            {
                "Broker" => GetBrokerSummaryOkObjectResult(),
                "Customer" => GetCustomerSummaryOkObjectResult(),
                "Process" => GetProcessSummaryOkObjectResult(),
                "Setting" => GetSettingSummaryOkObjectResult(),
                "User" => GetUserSummaryOkObjectResult(),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult GetBrokerSummaryOkObjectResult()
        {
            var brokerAttributeController = new BrokerAttributeController();
            var nameBrokerAttributeEntity = brokerAttributeController.GetActiveEntityByDescription("Name");

            var brokerDetailController = new BrokerDetailController();
            var nameBrokerDetailDictionary = brokerDetailController.GetActiveEntityListByAttributeId(nameBrokerAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToDictionary
                (
                    d => d.BrokerId,
                    d => d.Description
                );

            return GetSummaryList(nameBrokerDetailDictionary);
        }

        private OkObjectResult GetCustomerSummaryOkObjectResult()
        {
            var customerAttributeController = new CustomerAttributeController();
            var nameCustomerAttributeEntity = customerAttributeController.GetActiveEntityByDescription("Name");

            var customerDetailController = new CustomerDetailController();
            var nameCustomerDetailEntityDictionary = customerDetailController.GetActiveEntityListByAttributeId(nameCustomerAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToDictionary
                (
                    d => d.CustomerId,
                    d => d.Description
                );

            return GetSummaryList(nameCustomerDetailEntityDictionary);
        }

        private OkObjectResult GetProcessSummaryOkObjectResult()
        {
            var processAttributeController = new ProcessAttributeController();
            var nameProcessAttributeEntity = processAttributeController.GetActiveEntityByDescription("Name");

            var processDetailController = new ProcessDetailController();
            var nameProcessDetailDictionary = processDetailController.GetActiveEntityListByAttributeId(nameProcessAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToDictionary
                (
                    d => d.ProcessId,
                    d => d.Description
                );

            return GetSummaryList(nameProcessDetailDictionary);
        }

        private OkObjectResult GetSettingSummaryOkObjectResult()
        {
            var settingAttributeController = new SettingAttributeController();
            var nameSettingAttributeEntity = settingAttributeController.GetActiveEntityByDescription("Name");

            var settingDetailController = new SettingDetailController();
            var nameSettingDetailDictionary = settingDetailController.GetActiveEntityListByAttributeId(nameSettingAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToDictionary
                (
                    d => d.SettingId,
                    d => d.Description
                );

            return GetSummaryList(nameSettingDetailDictionary);
        }

        private OkObjectResult GetUserSummaryOkObjectResult()
        {
            var userAttributeController = new UserAttributeController();
            var emailAddressUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Email Address");

            var userDetailController = new UserDetailController();
            var emailAddressUserDetailDictionary = userDetailController.GetActiveEntityListByAttributeId(emailAddressUserAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToDictionary
                (
                    d => d.UserId,
                    d => d.Description
                );

            return GetSummaryList(emailAddressUserDetailDictionary, "Email Address");
        }

        private OkObjectResult GetSummaryList(Dictionary<long, string> detailDictionary, string dataTableIdentifier = "Name")
        {
            var summaryEntityList = new List<EntitySummaryEntity>();

            foreach (var detailKeyValuePair in detailDictionary)
            {
                var summaryEntity = new EntitySummaryEntity(detailKeyValuePair.Key, detailKeyValuePair.Value, dataTableIdentifier);

                summaryEntityList.Add(summaryEntity);
            }

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(summaryEntityList) });
        }
    }
}
