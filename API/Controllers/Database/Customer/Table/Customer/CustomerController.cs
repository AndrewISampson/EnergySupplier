using API.Entity.Database.Customer.Table;

namespace API.Controllers.Database.Customer.Table.Customer
{
    public class CustomerController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Customer";
        private readonly string _table = "Customer";

        public CustomerController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<CustomerEntity>();
        }

        internal CustomerEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new CustomerEntity(dataRow);
        }

        internal CustomerEntity InsertNewAndGetEntity(long createdByUserId)
        {
            var guid = Guid.NewGuid();

            while (GetActiveEntityByGuid(guid).Id != 0)
            {
                guid = Guid.NewGuid();
            }

            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"Guid\") VALUES ({createdByUserId}, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }
    }
}
