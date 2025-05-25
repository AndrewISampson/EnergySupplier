using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_Password_To_Administration_UserController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Mapping";
        private readonly string _table = "Administration.PasswordToAdministration.User";

        public Administration_Password_To_Administration_UserController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<Administration_Password_To_Administration_UserEntity>();
        }

        internal List<Administration_Password_To_Administration_UserEntity> GetActiveEntityByPasswordId(long passwordId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordId\" = '{passwordId}'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new Administration_Password_To_Administration_UserEntity(dataRow));
        }

        internal Administration_Password_To_Administration_UserEntity GetActiveEntityByUserId(long userId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'");
            return new Administration_Password_To_Administration_UserEntity(dataRow);
        }

        internal Administration_Password_To_Administration_UserEntity GetActiveEntityByPasswordIdAndUserId(long passwordId, long userId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordId\" = '{passwordId}' AND \"UserId\" = '{userId}'");
            return new Administration_Password_To_Administration_UserEntity(dataRow);
        }

        internal void Insert(long createdByUserId, long passwordId, long userId)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"PasswordId\", \"UserId\") VALUES ({createdByUserId}, {passwordId}, {userId})");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
