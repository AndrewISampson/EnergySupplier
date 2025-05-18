using System.Data;
using API.Entity.Database.Administration.Table.Login;

namespace API.Controllers.Database.Administration.Table.Login
{
    public class LoginAttributeController
    {
        private readonly DatabaseController databaseController;

        public LoginAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal LoginAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"LoginAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new LoginAttributeEntity(d))
                .First();
        }

        internal List<LoginAttributeEntity> GetActiveEntityList()
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"LoginAttribute\" WHERE \"IsActiveRecord\" = '1'")
                .Rows.Cast<DataRow>()
                .Select(d => new LoginAttributeEntity(d))
                .ToList();
        }
    }
}
