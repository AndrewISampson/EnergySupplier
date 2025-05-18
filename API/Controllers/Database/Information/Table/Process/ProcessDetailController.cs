using System.Data;
using API.Entity.Database.Information.Table.Process;

namespace API.Controllers.Database.Information.Table.Process
{
    public class ProcessDetailController
    {
        private readonly DatabaseController databaseController;

        public ProcessDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal ProcessDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"ProcessDetail\" WHERE \"IsActiveRecord\" = '1' AND \"ProcessId\" = {id} AND \"ProcessAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new ProcessDetailEntity(d))
                .FirstOrDefault();
        }
    }
}
