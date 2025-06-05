using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserAttributeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "UserAttribute";

        public UserAttributeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<UserAttributeEntity>();
        }

        internal UserAttributeEntity GetActiveEntityByDescription(string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'");
            return new UserAttributeEntity(dataRow);
        }

        internal List<UserAttributeEntity> GetActiveEntityList()
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new UserAttributeEntity(dataRow));
        }
    }
}
