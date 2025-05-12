using System.Data;
using API.Entity.Database.Information.Table.Process;

namespace API.Controllers.Database.Information.Table.Process
{
    public class ProcessAttributeController
    {
        private readonly DatabaseController databaseController;

        public ProcessAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal ProcessAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Information\".\"ProcessAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new ProcessAttributeEntity(d))
                .FirstOrDefault();
        }
    }
}
