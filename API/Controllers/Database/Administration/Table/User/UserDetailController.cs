using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "UserDetail";

        public UserDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<UserDetailEntity>();
        }

        internal UserDetailEntity GetActiveEntityByDetailId(long detailId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Id\" = {detailId}");
            return new UserDetailEntity(dataRow);
        }

        internal UserDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = {id} AND \"UserAttributeId\" = {attributeId}");
            return new UserDetailEntity(dataRow);
        }

        internal List<UserDetailEntity> GetActiveEntityListByIdAndAttributeId(long id, long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = {id} AND \"UserAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new UserDetailEntity(dataRow));
        }

        internal UserDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'AND \"UserAttributeId\" = {attributeId} AND \"Description\" = '{description}'");
            return new UserDetailEntity(dataRow);
        }

        internal List<UserDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserAttributeId\" = {attributeId}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new UserDetailEntity(dataRow));
        }

        internal List<UserDetailEntity> GetActiveEntityListById(long id)
        {
            var dataRowList = _databaseController.GetList($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = {id}");
            return _genericController.PopulateListFromDataRowList(dataRowList, dataRow => new UserDetailEntity(dataRow));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"UserId\", \"UserAttributeId\", \"Description\") VALUES ({createdByUserId}, {id}, {attributeId}, '{description}')");
        }

        internal void UpdateEffectiveToDateTime(long id, long closedByUserId)
        {
            _databaseController.ExecuteScalar($"UPDATE \"{_schema}\".\"{_table}\" SET \"EffectiveToDateTime\" = NOW() AT TIME ZONE 'UTC', \"ClosedByUserId\" = {closedByUserId} WHERE \"Id\" = {id}");
        }
    }
}
