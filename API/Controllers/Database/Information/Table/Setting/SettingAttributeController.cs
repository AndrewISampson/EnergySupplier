using API.Entity.Database.Information.Table.Setting;

namespace API.Controllers.Database.Information.Table.Setting
{
    public class SettingAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Information";
        private readonly string _table = "SettingAttribute";

        public SettingAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<SettingAttributeEntity>();
        }

        internal SettingAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new SettingAttributeEntity(dataRow);
        }

        internal List<SettingAttributeEntity> GetActiveEntityList()
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new SettingAttributeEntity(dataRow));
        }
    }
}
