using System.Data;
using API.Entity.Database.Administration.Table.Password;

namespace API.Controllers.Database.Administration.Table.Password
{
    public class PasswordController
    {
        private readonly DatabaseController databaseController;

        public PasswordController()
        {
            databaseController = new DatabaseController();
        }

        internal PasswordEntity GetActiveEntityByGuid(string guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"Password\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordEntity(d))
                .FirstOrDefault();
        }
    }
}
