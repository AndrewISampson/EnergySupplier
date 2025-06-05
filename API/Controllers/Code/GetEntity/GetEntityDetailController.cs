using API.Controllers.Database.Broker.Table;
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
                "Broker" => GetBrokerBrokerDetailOkObjectResult(getEntityDetailEntity.EntityId),
                _ => Ok(new { valid = false }),
            };
        }

        private OkObjectResult GetBrokerBrokerDetailOkObjectResult(long entityId)
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

            var brokerBrokerDetailEntityList = new List<BrokerBrokerDetailEntity>();

            foreach (var brokerDetailEntity in brokerDetailEntityList.OrderBy(d => brokerAttributeDictionary[d.BrokerAttributeId]))
            {
                var brokerBrokerDetailEntity = new BrokerBrokerDetailEntity
                {
                    Id = brokerDetailEntity.Id,
                    Attribute = brokerAttributeDictionary[brokerDetailEntity.BrokerAttributeId],
                    Description = brokerDetailEntity.Description
                };

                brokerBrokerDetailEntityList.Add(brokerBrokerDetailEntity);
            }

            return Ok(new { valid = true, entityList = JsonConvert.SerializeObject(brokerBrokerDetailEntityList) });
        }
    }
}
