using API.Entity.Database.Information.Table.Process;

namespace API.Controllers.Database.Information.Table.Process
{
    public class ProcessDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Information";
        private readonly string _table = "ProcessDetail";

        public ProcessDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<ProcessDetailEntity>();
        }

        internal ProcessDetailEntity GetActiveEntityByDetailId(long detailId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {detailId}");
            return new ProcessDetailEntity(dataRow);
        }

        internal ProcessDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"ProcessId\" = {id} AND \"ProcessAttributeId\" = {attributeId}");
            return new ProcessDetailEntity(dataRow);
        }

        internal List<ProcessDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"ProcessAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new ProcessDetailEntity(dataRow));
        }

        internal List<ProcessDetailEntity> GetActiveEntityListById(long id)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"ProcessId\" = {id}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new ProcessDetailEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"EffectiveFromDateTime\", \"ProcessId\", \"ProcessAttributeId\", \"Description\") VALUES ({createdByUserId}, NOW() AT TIME ZONE 'UTC', {id}, {attributeId}, '{description}')");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
