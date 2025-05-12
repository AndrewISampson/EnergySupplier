using System.Data;
using API.Entity.Database.Information.Table.Process;

namespace API.Controllers.Database.Information.Table.Process
{
    public class ProcessController
    {
        private readonly DatabaseController databaseController;

        public ProcessController()
        {
            databaseController = new DatabaseController();
        }

        internal ProcessEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"Process\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new ProcessEntity(d))
                .FirstOrDefault();
        }
    }
}
