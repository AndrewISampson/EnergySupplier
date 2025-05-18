using System.Data;
using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_Password_To_Administration_UserController
    {
        private readonly DatabaseController databaseController;

        public Administration_Password_To_Administration_UserController()
        {
            databaseController = new DatabaseController();
        }

        internal List<Administration_Password_To_Administration_UserEntity> GetActiveEntityByPasswordId(long passwordId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.PasswordToAdministration.User\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordId\" = '{passwordId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_Password_To_Administration_UserEntity(d))
                .ToList();
        }

        internal Administration_Password_To_Administration_UserEntity GetActiveEntityByUserId(long userId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.PasswordToAdministration.User\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_Password_To_Administration_UserEntity(d))
                .FirstOrDefault();
        }

        internal Administration_Password_To_Administration_UserEntity GetActiveEntityByPasswordIdAndUserId(long passwordId, long userId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.PasswordToAdministration.User\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordId\" = '{passwordId}' AND \"UserId\" = '{userId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_Password_To_Administration_UserEntity(d))
                .FirstOrDefault();
        }
    }
}
