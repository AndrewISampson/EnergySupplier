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

        internal PasswordEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"Password\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordEntity(d))
                .FirstOrDefault();
        }

        internal PasswordEntity InsertNewAndGetEntity()
        {
            var guid = Guid.NewGuid();
            var loginEntity = GetActiveEntityByGuid(guid);

            while (loginEntity != null)
            {
                guid = Guid.NewGuid();
                loginEntity = GetActiveEntityByGuid(guid);
            }

            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"Password\" (\"CreatedByUserId\", \"Guid\") VALUES (1, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }
    }
}
