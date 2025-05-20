using System.Data;
using API.Entity.Database.Information.Table.Setting;

namespace API.Controllers.Database.Information.Table.Setting
{
    public class SettingAttributeController
    {
        private readonly DatabaseController databaseController;

        public SettingAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal SettingAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"SettingAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new SettingAttributeEntity(d))
                .FirstOrDefault();
        }
    }
}
