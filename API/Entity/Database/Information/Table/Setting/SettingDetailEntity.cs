using System.Data;

namespace API.Entity.Database.Information.Table.Setting
{
    public class SettingDetailEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long SettingId;
        public long SettingAttributeId;
        public string Description;

        public SettingDetailEntity(DataRow dataRow)
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
            SettingId = Convert.ToInt64(dataRow["SettingId"].ToString());
            SettingAttributeId = Convert.ToInt64(dataRow["SettingAttributeId"].ToString());
            Description = dataRow["Description"].ToString();
        }
    }
}
