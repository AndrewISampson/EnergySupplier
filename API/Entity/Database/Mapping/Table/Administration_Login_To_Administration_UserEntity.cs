﻿using System.Data;

namespace API.Entity.Database.Mapping.Table
{
    public class Administration_Login_To_Administration_UserEntity
    {
        public long Id;
        public DateTime CreatedDateTime;
        public long CreatedByUserId;
        public DateTime EffectiveFromDateTime;
        public DateTime EffectiveToDateTime;
        public bool IsActiveRecord;
        public long ClosedByUserId;
        public long LoginId;
        public long UserId;

        public Administration_Login_To_Administration_UserEntity(DataRow dataRow)
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
            UserId = Convert.ToInt64(dataRow["UserId"].ToString());
        }
    }
}
