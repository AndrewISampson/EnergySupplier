using System.Data;
using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_Login_To_Administration_UserController
    {
        private readonly DatabaseController databaseController;

        public Administration_Login_To_Administration_UserController()
        {
            databaseController = new DatabaseController();
        }

        internal List<Administration_Login_To_Administration_UserEntity> GetActiveEntityListByUserId(long userId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.LoginToAdministration.User\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_Login_To_Administration_UserEntity(d))
                .ToList();
        }

        internal void Insert(long loginId, long userId)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Mapping\".\"Administration.LoginToAdministration.User\" (\"CreatedByUserId\", \"LoginId\", \"UserId\") VALUES (1, {loginId}, {userId}");
        }
    }
}
