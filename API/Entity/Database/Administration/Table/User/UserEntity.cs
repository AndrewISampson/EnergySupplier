using System.Data;

namespace API.Entity.Database.Administration.Table.User
{
    public class UserEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public Guid Guid;

        public UserEntity(DataRow dataRow)
        {
            Id = Convert.ToInt64(dataRow["Id"].ToString());
            CreatedDateTime = Convert.ToDateTime(dataRow["CreatedDateTime"].ToString());
            CreatedByUserId = Convert.ToInt64(dataRow["CreatedByUserId"].ToString());
            EffectiveFromDateTime = Convert.ToDateTime(dataRow["EffectiveFromDateTime"].ToString());
            EffectiveToDateTime = Convert.ToDateTime(dataRow["EffectiveToDateTime"].ToString());
            IsActiveRecord = Convert.ToBoolean(dataRow["IsActiveRecord"].ToString());
            ClosedByUserId = string.IsNullOrWhiteSpace(dataRow["ClosedByUserId"].ToString()) ? 0 : Convert.ToInt64(dataRow["ClosedByUserId"].ToString());
            Guid.TryParse(dataRow["Guid"].ToString(), out Guid);
        }
    }
}
