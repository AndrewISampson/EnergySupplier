using API.Entity.Database.Administration.Table.Password;

namespace API.Controllers.Database.Administration.Table.Password
{
    public class PasswordController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "Password";

        public PasswordController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<PasswordEntity>();
        }

        internal PasswordEntity GetActiveEntityById(long id)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {id}");
            return new PasswordEntity(dataRow);
        }

        internal PasswordEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new PasswordEntity(dataRow);
        }

        internal PasswordEntity GetActiveEntityByGuid(string guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new PasswordEntity(dataRow);
        }

        internal PasswordEntity InsertNewAndGetEntity(long createdByUserId)
        {
            var guid = Guid.NewGuid();

            while (GetActiveEntityByGuid(guid).Id != 0)
            {
                guid = Guid.NewGuid();
            }

            _databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"Password\" (\"CreatedByUserId\", \"Guid\") VALUES ({createdByUserId}, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }
    }
}
