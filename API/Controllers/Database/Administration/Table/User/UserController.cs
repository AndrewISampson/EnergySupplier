using System.Data;
using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserController
    {
        private readonly DatabaseController databaseController;

        public UserController()
        {
            databaseController = new DatabaseController();
        }

        internal UserEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"User\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new UserEntity(d))
                .FirstOrDefault();
        }
    }
}
