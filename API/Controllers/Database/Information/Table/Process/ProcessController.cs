using System.Data;
using API.Entity.Database.Information.Table.Process;
using API.Entity.Database.Information.Table.Setting;

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

            _selectColumns = _genericController.GetColumnListFromEntity<ProcessEntity>();
        }

        internal ProcessEntity GetActiveEntityByGuid(Guid guid)
        {
            var dataRow = _databaseController.GetFirstOrDefault($"SELECT {_selectColumns} FROM \"{_schema}\".\"{_table}\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'");
            return new ProcessEntity(dataRow);
        }
    }
}
