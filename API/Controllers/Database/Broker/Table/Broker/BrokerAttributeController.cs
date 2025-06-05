using API.Entity.Database.Broker.Table;

namespace API.Controllers.Database.Broker.Table.Broker
{
    public class BrokerAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Broker";
        private readonly string _table = "BrokerAttribute";

        public BrokerAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<BrokerAttributeEntity>();
        }

        internal BrokerAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new BrokerAttributeEntity(dataRow);
        }

        internal List<BrokerAttributeEntity> GetActiveEntityList()
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new BrokerAttributeEntity(dataRow));
        }
    }
}
