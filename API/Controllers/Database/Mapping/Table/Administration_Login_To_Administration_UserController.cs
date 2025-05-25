using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_Login_To_Administration_UserController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Mapping";
        private readonly string _table = "Administration.LoginToAdministration.User";

        public Administration_Login_To_Administration_UserController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<Administration_Login_To_Administration_UserEntity>();
        }

        internal List<Administration_Login_To_Administration_UserEntity> GetActiveEntityListByUserId(long userId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new Administration_Login_To_Administration_UserEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long loginId, long userId)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"LoginId\", \"UserId\") VALUES ({createdByUserId}, {loginId}, {userId})");
        }
    }
}
