using API.Entity.Database.Information.Table.Process;

namespace API.Controllers.Database.Information.Table.Process
{
    public class ProcessController
    {
        private readonly DatabaseController _databaseController;
        private readonly GenericController _genericController;

        private readonly string _selectColumns;
        private readonly string _schema = "Information";
        private readonly string _table = "Process";

        public ProcessController()
        {
            _databaseController = new DatabaseController();
            _genericController = new GenericController();

            _selectColumns = _genericController.GetColumnNameStringFromEntity<ProcessEntity>();
        }

        internal ProcessEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new ProcessEntity(dataRow);
        }

        internal ProcessEntity InsertNewAndGetEntity(long createdByUserId)
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
