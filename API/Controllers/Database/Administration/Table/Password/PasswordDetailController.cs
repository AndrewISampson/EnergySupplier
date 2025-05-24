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

        internal List<PasswordDetailEntity> GetActiveEntityListByAttributeId(long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"PasswordDetail\" WHERE \"IsActiveRecord\" = '1' AND \"PasswordAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordDetailEntity(d))
                .ToList();
        }

        internal PasswordDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"PasswordDetail\" WHERE \"IsActiveRecord\" = '1'AND \"PasswordAttributeId\" = {attributeId} AND \"Description\" = '{description}' ")
                .Rows.Cast<DataRow>()
                .Select(d => new PasswordDetailEntity(d))
                .FirstOrDefault();
        }

        internal void Insert(long createdByUserId, long id, long attributeId, string description)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"PasswordDetail\" (\"CreatedByUserId\", \"PasswordId\", \"PasswordAttributeId\", \"Description\") VALUES ({createdByUserId}, {id}, {attributeId}, '{description}')");
        }
    }
}
