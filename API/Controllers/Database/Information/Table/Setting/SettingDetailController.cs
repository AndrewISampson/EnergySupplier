using System.Data;
using API.Entity.Database.Information.Table.Setting;

namespace API.Controllers.Database.Information.Table.Setting
{
    public class SettingDetailController
    {
        private readonly DatabaseController databaseController;

        public SettingDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal SettingDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"SettingDetail\" WHERE \"IsActiveRecord\" = '1' AND \"SettingId\" = {id} AND \"SettingAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new SettingDetailEntity(d))
                .FirstOrDefault();
        }

        internal SettingDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"SettingDetail\" WHERE \"IsActiveRecord\" = '1' AND \"SettingAttributeId\" = {attributeId} AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new SettingDetailEntity(d))
                .FirstOrDefault();
        }
    }
}
