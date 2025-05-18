using System.Data;
using API.Entity.Database.Administration.Table.Login;

namespace API.Controllers.Database.Administration.Table.Login
{
    public class LoginDetailController
    {
        private readonly DatabaseController databaseController;

        public LoginDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal LoginDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"LoginDetail\" WHERE \"IsActiveRecord\" = '1' AND \"LoginId\" = {id} AND \"LoginAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new LoginDetailEntity(d))
                .First();
        }

        internal void BulkInsert(List<LoginDetailEntity> loginDetailEntityList)
        {
            loginDetailEntityList.ForEach(l => Insert(l.LoginId, l.LoginAttributeId, l.Description));
        }

        internal void Insert(long id, long attributeId, string description)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"LoginDetail\" (\"CreatedByUserId\", \"LoginId\", \"LoginAttributeId\", \"Description\") VALUES (1, {id}, {attributeId}, '{description}'");
        }
    }
}
