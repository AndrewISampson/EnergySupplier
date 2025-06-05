using System.Data;

namespace API.Entity.Database.Broker.Table
{
    public class BrokerDetailEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long BrokerId;
        public long BrokerAttributeId;
        public string Description;

        public BrokerDetailEntity(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return;
            }

            Id = Convert.ToInt64(dataRow["Id"].ToString());
            CreatedDateTime = Convert.ToDateTime(dataRow["CreatedDateTime"].ToString());
            CreatedByUserId = Convert.ToInt64(dataRow["CreatedByUserId"].ToString());
            EffectiveFromDateTime = Convert.ToDateTime(dataRow["EffectiveFromDateTime"].ToString());
            EffectiveToDateTime = Convert.ToDateTime(dataRow["EffectiveToDateTime"].ToString());
            IsActiveRecord = Convert.ToBoolean(dataRow["IsActiveRecord"].ToString());
            ClosedByUserId = string.IsNullOrWhiteSpace(dataRow["ClosedByUserId"].ToString()) ? 0 : Convert.ToInt64(dataRow["ClosedByUserId"].ToString());
            BrokerId = Convert.ToInt64(dataRow["BrokerId"].ToString());
            BrokerAttributeId = Convert.ToInt64(dataRow["BrokerAttributeId"].ToString());
            Description = dataRow["Description"].ToString();
        }
    }
}
