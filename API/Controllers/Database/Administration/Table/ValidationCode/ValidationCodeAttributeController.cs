using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "ValidationCodeAttribute";

        public ValidationCodeAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<ValidationCodeAttributeEntity>();
        }

        internal ValidationCodeAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new ValidationCodeAttributeEntity(dataRow);
        }

        internal List<ValidationCodeAttributeEntity> GetActiveEntityList()
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new ValidationCodeAttributeEntity(dataRow));
        }
    }
}
