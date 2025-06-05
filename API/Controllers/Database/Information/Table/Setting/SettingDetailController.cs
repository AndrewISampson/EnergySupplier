using API.Entity.Database.Information.Table.Setting;

namespace API.Controllers.Database.Information.Table.Setting
{
    public class SettingDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Information";
        private readonly string _table = "SettingDetail";

        public SettingDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<SettingDetailEntity>();
        }

        internal SettingDetailEntity GetActiveEntityByDetailId(long detailId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {detailId}");
            return new SettingDetailEntity(dataRow);
        }

        internal SettingDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"SettingId\" = {id} AND \"SettingAttributeId\" = {attributeId}");
            return new SettingDetailEntity(dataRow);
        }

        internal SettingDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"SettingAttributeId\" = {attributeId} AND \"Description\" = '{description}'");
            return new SettingDetailEntity(dataRow);
        }

        internal List<SettingDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"SettingAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new SettingDetailEntity(dataRow));
        }

        internal List<SettingDetailEntity> GetActiveEntityListById(long id)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"SettingId\" = {id}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new SettingDetailEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"EffectiveFromDateTime\", \"SettingId\", \"SettingAttributeId\", \"Description\") VALUES ({createdByUserId}, NOW() AT TIME ZONE 'UTC', {id}, {attributeId}, '{description}')");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
