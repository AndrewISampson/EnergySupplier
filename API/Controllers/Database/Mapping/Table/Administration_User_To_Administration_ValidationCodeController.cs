using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_User_To_Administration_ValidationCodeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Mapping";
        private readonly string _table = "Administration.UserToAdministration.ValidationCode";

        public Administration_User_To_Administration_ValidationCodeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<Administration_User_To_Administration_ValidationCodeEntity>();
        }

        internal List<Administration_User_To_Administration_ValidationCodeEntity> GetActiveEntityByUserId(long userId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new Administration_User_To_Administration_ValidationCodeEntity(dataRow));
        }

        internal Administration_User_To_Administration_ValidationCodeEntity GetActiveEntityByValidationCodeId(long validationCodeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"ValidationCodeId\" = '{validationCodeId}'");
            return new Administration_User_To_Administration_ValidationCodeEntity(dataRow);
        }

        internal Administration_User_To_Administration_ValidationCodeEntity GetActiveEntityByUserIdAndValidationCodeId(long userId, long validationCodeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}' AND \"ValidationCodeId\" = '{validationCodeId}'");
            return new Administration_User_To_Administration_ValidationCodeEntity(dataRow);
        }

        internal void Insert(long createdByUserId, long userId, long validationCodeId)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"UserId\", \"ValidationCodeId\") VALUES ({createdByUserId}, {userId}, {validationCodeId})");
        }
    }
}
