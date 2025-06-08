using API.Entity.Database.Customer.Table;

namespace API.Controllers.Database.Customer.Table.Customer
{
    public class CustomerDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Customer";
        private readonly string _table = "CustomerDetail";

        public CustomerDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<CustomerDetailEntity>();
        }

        internal CustomerDetailEntity GetActiveEntityByDetailId(long detailId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {detailId}");
            return new CustomerDetailEntity(dataRow);
        }

        internal CustomerDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"CustomerId\" = {id} AND \"CustomerAttributeId\" = {attributeId}");
            return new CustomerDetailEntity(dataRow);
        }

        internal CustomerDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"CustomerAttributeId\" = {attributeId} AND \"Description\" = '{description}'");
            return new CustomerDetailEntity(dataRow);
        }

        internal List<CustomerDetailEntity> GetActiveEntityListById(long id)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"CustomerId\" = {id}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new CustomerDetailEntity(dataRow));
        }

        internal List<CustomerDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"CustomerAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new CustomerDetailEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"EffectiveFromDateTime\", \"CustomerId\", \"CustomerAttributeId\", \"Description\") VALUES ({createdByUserId}, NOW() AT TIME ZONE 'UTC', {id}, {attributeId}, '{description}')");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
