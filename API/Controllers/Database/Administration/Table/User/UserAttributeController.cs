using System.Data;
using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserAttributeController
    {
        private readonly DatabaseController databaseController;

        public UserAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal UserAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"UserAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new UserAttributeEntity(d))
                .First();
        }
    }
}
