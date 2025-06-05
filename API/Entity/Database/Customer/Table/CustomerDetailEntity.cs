using System.Data;

namespace API.Entity.Database.Customer.Table
{
    public class CustomerDetailEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long CustomerId;
        public long CustomerAttributeId;
        public string Description;

        public CustomerDetailEntity(DataRow dataRow)
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
            CustomerId = Convert.ToInt64(dataRow["CustomerId"].ToString());
            CustomerAttributeId = Convert.ToInt64(dataRow["CustomerAttributeId"].ToString());
            Description = dataRow["Description"].ToString();
        }
    }
}
