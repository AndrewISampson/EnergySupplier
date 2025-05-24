using System.Data;
using API.Entity.Database.Mapping.Table;

namespace API.Controllers.Database.Mapping.Table
{
    public class Administration_User_To_Administration_ValidationCodeController
    {
        private readonly DatabaseController databaseController;

        public Administration_User_To_Administration_ValidationCodeController()
        {
            databaseController = new DatabaseController();
        }

        internal List<Administration_User_To_Administration_ValidationCodeEntity> GetActiveEntityByUserId(long userId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.UserToAdministration.ValidationCode\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_User_To_Administration_ValidationCodeEntity(d))
                .ToList();
        }

        internal Administration_User_To_Administration_ValidationCodeEntity GetActiveEntityByValidationCodeId(long validationCodeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.UserToAdministration.ValidationCode\" WHERE \"IsActiveRecord\" = '1' AND \"ValidationCodeId\" = '{validationCodeId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_User_To_Administration_ValidationCodeEntity(d))
                .FirstOrDefault();
        }

        internal Administration_User_To_Administration_ValidationCodeEntity GetActiveEntityByUserIdAndValidationCodeId(long userId, long validationCodeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Mapping\".\"Administration.UserToAdministration.ValidationCode\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = '{userId}' AND \"ValidationCodeId\" = '{validationCodeId}'")
                .Rows.Cast<DataRow>()
                .Select(d => new Administration_User_To_Administration_ValidationCodeEntity(d))
                .FirstOrDefault();
        }

        internal void Insert(long userId, long validationCodeId)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Mapping\".\"Administration.UserToAdministration.ValidationCode\" (\"CreatedByUserId\", \"UserId\", \"ValidationCodeId\") VALUES (1, {userId}, {validationCodeId})");
        }
    }
}
