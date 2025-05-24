using System.Data;
using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeController
    {
        private readonly DatabaseController databaseController;

        public ValidationCodeController()
        {
            databaseController = new DatabaseController();
        }

        internal ValidationCodeEntity InsertNewAndGetEntity()
        {
            var guid = Guid.NewGuid();
            var ValidationCodeEntity = GetActiveEntityByGuid(guid);

            while (ValidationCodeEntity != null)
            {
                guid = Guid.NewGuid();
                ValidationCodeEntity = GetActiveEntityByGuid(guid);
            }

            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"ValidationCode\" (\"CreatedByUserId\", \"Guid\") VALUES (1, '{guid}')");
            return GetActiveEntityByGuid(guid);
        }

        internal ValidationCodeEntity GetActiveEntityByGuid(Guid guid)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"ValidationCode\" WHERE \"IsActiveRecord\" = '1' AND \"Guid\" = '{guid}'")
                .Rows.Cast<DataRow>()
                .Select(d => new ValidationCodeEntity(d))
                .FirstOrDefault();
        }
    }
}
