using System.Data;
using API.Entity.Database.Administration.Table.Login;

namespace API.Controllers.Database.Administration.Table.Login
{
    public class LoginController
    {
        private readonly DatabaseController databaseController;

        public LoginController()
        {
            databaseController = new DatabaseController();
        }

        internal LoginEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"Login\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new LoginEntity(d))
                .FirstOrDefault();
        }

        internal LoginEntity InsertNewAndGetEntity()
        {
            var guid = Guid.NewGuid();
            var loginEntity = GetActiveEntityByGuid(guid);

            while (loginEntity != null)
            {
                guid = Guid.NewGuid();
                loginEntity = GetActiveEntityByGuid(guid);
            }

            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"Login\" (\"CreatedByUserId\", \"Guid\") VALUES (1, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }
    }
}
