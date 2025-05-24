using System.Data;
using API.Entity.Database.Administration.Table.ValidationCode;

namespace API.Controllers.Database.Administration.Table.ValidationCode
{
    public class ValidationCodeDetailController
    {
        private readonly DatabaseController databaseController;

        public ValidationCodeDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal ValidationCodeDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"ValidationCodeDetail\" WHERE \"IsActiveRecord\" = '1' AND \"ValidationCodeId\" = {id} AND \"ValidationCodeAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new ValidationCodeDetailEntity(d))
                .First();
        }

        internal ValidationCodeDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"ValidationCodeDetail\" WHERE \"IsActiveRecord\" = '1'AND \"ValidationCodeAttributeId\" = {attributeId} AND \"Description\" = '{description}' ")
                .Rows.Cast<DataRow>()
                .Select(d => new ValidationCodeDetailEntity(d))
                .FirstOrDefault();
        }

        internal void BulkInsert(List<ValidationCodeDetailEntity> ValidationCodeDetailEntityList)
        {
            ValidationCodeDetailEntityList.ForEach(l => Insert(l.ValidationCodeId, l.ValidationCodeAttributeId, l.Description));
        }

        internal void Insert(long id, long attributeId, string description)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"ValidationCodeDetail\" (\"CreatedByUserId\", \"ValidationCodeId\", \"ValidationCodeAttributeId\", \"Description\") VALUES (1, {id}, {attributeId}, '{description}')");
        }
    }
}
