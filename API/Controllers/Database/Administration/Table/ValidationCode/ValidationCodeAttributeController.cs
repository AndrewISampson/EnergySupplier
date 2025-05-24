using System.Data;
using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeAttributeController
    {
        private readonly DatabaseController databaseController;

        public ValidationCodeAttributeController()
        {
            databaseController = new DatabaseController();
        }

        internal ValidationCodeAttributeEntity GetActiveEntityByDescription(string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"ValidationCodeAttribute\" WHERE \"IsActiveRecord\" = '1' AND \"Description\" = '{description}'")
                .Rows.Cast<DataRow>()
                .Select(d => new ValidationCodeAttributeEntity(d))
                .First();
        }

        internal List<ValidationCodeAttributeEntity> GetActiveEntityList()
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"ValidationCodeAttribute\" WHERE \"IsActiveRecord\" = '1'")
                .Rows.Cast<DataRow>()
                .Select(d => new ValidationCodeAttributeEntity(d))
                .ToList();
        }
    }
}
