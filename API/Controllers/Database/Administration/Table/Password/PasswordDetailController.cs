using System.Data;
using API.Entity.Database.Administration.Table.Password;

namespace API.Controllers.Database.Administration.Table.Password
{
    public class PasswordDetailController
    {
        private readonly DatabaseController databaseController;

        public PasswordDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal PasswordDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"PasswordDetail\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordId\" = {id} AND \"PasswordAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordDetailEntity(d))
                .FirstOrDefault();
        }
    }
}
