using API.Entity.Database.Administration.Table.Login;

namespace API.Controllers.Database.Administration.Table.Login
{
    public class LoginDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "LoginDetail";

        public LoginDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<LoginDetailEntity>();
        }

        internal LoginDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"LoginId\" = {id} AND \"LoginAttributeId\" = {attributeId}");
            return new LoginDetailEntity(dataRow);
        }

        internal void BulkInsert(long createdByUserId, List<LoginDetailEntity> loginDetailEntityList)
        {
            loginDetailEntityList.ForEach(l => Insert(createdByUserId, l.LoginId, l.LoginAttributeId, l.Description));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"LoginId\", \"LoginAttributeId\", \"Description\") VALUES ({createdByUserId}, {id}, {attributeId}, '{description}')");
        }
    }
}
