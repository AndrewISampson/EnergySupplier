using System.Data;

namespace API.Entity.Database.Administration.Table.Login
{
    public class LoginDetailEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long LoginId;
        public long LoginAttributeId;
        public string Description;

        public LoginDetailEntity()
        {

        }

        public LoginDetailEntity(DataRow dataRow)
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
            LoginId = Convert.ToInt64(dataRow["LoginId"].ToString());
            LoginAttributeId = Convert.ToInt64(dataRow["LoginAttributeId"].ToString());
            Description = dataRow["Description"].ToString();
        }
    }
}
