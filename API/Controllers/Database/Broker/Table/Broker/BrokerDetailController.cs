using API.Entity.Database.Broker.Table;

namespace API.Controllers.Database.Broker.Table.Broker
{
    public class BrokerDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Broker";
        private readonly string _table = "BrokerDetail";

        public BrokerDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<BrokerDetailEntity>();
        }

        internal BrokerDetailEntity GetActiveEntityByDetailId(long detailId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {detailId}");
            return new BrokerDetailEntity(dataRow);
        }

        internal BrokerDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"BrokerId\" = {id} AND \"BrokerAttributeId\" = {attributeId}");
            return new BrokerDetailEntity(dataRow);
        }

        internal BrokerDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"BrokerAttributeId\" = {attributeId} AND \"Description\" = '{description}'");
            return new BrokerDetailEntity(dataRow);
        }

        internal List<BrokerDetailEntity> GetActiveEntityListById(long id)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"BrokerId\" = {id}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new BrokerDetailEntity(dataRow));
        }

        internal List<BrokerDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"BrokerAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new BrokerDetailEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"EffectiveFromDateTime\", \"BrokerId\", \"BrokerAttributeId\", \"Description\") VALUES ({createdByUserId}, NOW() AT TIME ZONE 'UTC', {id}, {attributeId}, '{description}')");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
