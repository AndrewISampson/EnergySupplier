using API.Entity.Database.Customer.Table;

namespace API.Controllers.Database.Customer.Table.Customer
{
    public class CustomerAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Customer";
        private readonly string _table = "CustomerAttribute";

        public CustomerAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<CustomerAttributeEntity>();
        }

        internal CustomerAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new CustomerAttributeEntity(dataRow);
        }

        internal List<CustomerAttributeEntity> GetActiveEntityList()
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new CustomerAttributeEntity(dataRow));
        }
    }
}
