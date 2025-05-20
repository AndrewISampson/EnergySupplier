using System.Data;
using API.Entity.Database.Information.Table.Setting;

namespace API.Controllers.Database.Information.Table.Setting
{
    public class SettingController
    {
        private readonly DatabaseController databaseController;

        public SettingController()
        {
            databaseController = new DatabaseController();
        }

        internal SettingEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"Setting\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new SettingEntity(d))
                .FirstOrDefault();
        }
    }
}
