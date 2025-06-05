using API.Controllers.Database.Broker.Table;
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
                "Broker" => GetBrokerBrokerSummaryOkObjectResult(),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult GetBrokerBrokerSummaryOkObjectResult()
        {
            var brokerAttributeController = new BrokerAttributeController();
            var nameBrokerAttributeEntity = brokerAttributeController.GetActiveEntityByDescription("Name");

            var brokerDetailController = new BrokerDetailController();
            var nameBrokerDetailEntityList = brokerDetailController.GetActiveEntityListByAttributeId(nameBrokerAttributeEntity.Id)
                .OrderBy(d => d.Description)
                .ToList();

            var brokerSummaryEntityList = new List<BrokerBrokerSummaryEntity>();

            foreach(var nameBrokerDetailEntity in nameBrokerDetailEntityList)
            {
                var brokerSummaryEntity = new BrokerBrokerSummaryEntity
                {
                    BrokerId = nameBrokerDetailEntity.BrokerId,
                    Name = nameBrokerDetailEntity.Description
                };

                brokerSummaryEntityList.Add(brokerSummaryEntity);
            }

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(brokerSummaryEntityList) });
        }
    }
}
