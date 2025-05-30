using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "User";

        public UserController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<UserEntity>();
        }

        internal UserEntity GetActiveEntityById(long id)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {id}");
            return new UserEntity(dataRow);
        }

        internal UserEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new UserEntity(dataRow);
        }

        internal UserEntity GetActiveEntityByGuid(string guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new UserEntity(dataRow);
        }
    }
}
