using System.Data;
using API.Entity.Database.Administration.Table.Password;

namespace API.Controllers.Database.Administration.Table.Password
{
    public class PasswordAttributeController
    {
        private readonly DatabaseController databaseController;

        public PasswordAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal PasswordAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"PasswordAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordAttributeEntity(d))
                .First();
        }
    }
}
