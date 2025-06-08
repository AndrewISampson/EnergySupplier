using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Administration";
        private readonly string _table = "ValidationCode";

        public ValidationCodeController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<ValidationCodeEntity>();
        }

        internal ValidationCodeEntity InsertNewAndGetEntity(long createdByUserId)
        {
            var guid = Guid.NewGuid();

            while (GetActiveEntityByGuid(guid).Id != 0)
            {
                guid = Guid.NewGuid();
            }

            _databaseController.ExecuteScalar($"INSERT INTO \"{_schema}\".\"{_table}\" (\"CreatedByUserId\", \"Guid\") VALUES ({createdByUserId}, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }

        internal ValidationCodeEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new ValidationCodeEntity(dataRow);
        }
    }
}
