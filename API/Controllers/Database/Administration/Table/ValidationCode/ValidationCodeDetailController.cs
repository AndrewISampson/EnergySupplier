using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeDetailController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "ValidationCodeDetail";

        public ValidationCodeDetailController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnListFromEntity<ValidationCodeDetailEntity>();
        }

        internal ValidationCodeDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"ValidationCodeId\" = {id} AND \"ValidationCodeAttributeId\" = {attributeId}");
            return new ValidationCodeDetailEntity(dataRow);
        }

        internal ValidationCodeDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1'AND \"ValidationCodeAttributeId\" = {attributeId} AND \"Description\" = '{description}'");
            return new ValidationCodeDetailEntity(dataRow);
        }

        internal void BulkInsert(long createdByUserId, List<ValidationCodeDetailEntity> ValidationCodeDetailEntityList)
        {
            ValidationCodeDetailEntityList.ForEach(l => Insert(createdByUserId, l.ValidationCodeId, l.ValidationCodeAttributeId, l.Description));
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"ValidationCodeId\", \"ValidationCodeAttributeId\", \"Description\") VALUES ({createdByUserId}, {id}, {attributeId}, '{description}')");
        }
    }
}
