using API.Entity.Database.Broker.Table;

namespace API.Controllers.Database.Broker.Table
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
    }
}
