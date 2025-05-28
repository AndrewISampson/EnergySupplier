using System.Data;

namespace API.Entity.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeDetailEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long ValidationCodeId;
        public long ValidationCodeAttributeId;
        public string Description;

        public ValidationCodeDetailEntity()
        {

        }

        public ValidationCodeDetailEntity(DataRow dataRow)
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
            ValidationCodeId = Convert.ToInt64(dataRow["ValidationCodeId"].ToString());
            ValidationCodeAttributeId = Convert.ToInt64(dataRow["ValidationCodeAttributeId"].ToString());
            Description = dataRow["Description"].ToString();
        }
    }
}
