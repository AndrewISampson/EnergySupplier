using API.Entity.Database.Administration.Table.Login;

namespace API.Controllers.Database.Administration.Table.Login
{
    public class LoginController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "Login";

        public LoginController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<LoginEntity>();
        }

        internal LoginEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new LoginEntity(dataRow);
        }

        internal LoginEntity InsertNewAndGetEntity(long createdByUserId)
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
