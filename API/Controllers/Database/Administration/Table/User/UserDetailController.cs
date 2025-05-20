using System.Data;
using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Database.Administration.Table.User
{
    public class UserDetailController
    {
        private readonly DatabaseController databaseController;

        public UserDetailController()
        {
            databaseController = new DatabaseController();
        }

        internal UserDetailEntity GetActiveEntityByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"UserDetail\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = {id} AND \"UserAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new UserDetailEntity(d))
                .FirstOrDefault();
        }

        internal List<UserDetailEntity> GetActiveEntityListByIdAndAttributeId(long id, long attributeId)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"UserDetail\" WHERE \"IsActiveRecord\" = '1' AND \"UserId\" = {id} AND \"UserAttributeId\" = {attributeId}")
                .Rows.Cast<DataRow>()
                .Select(d => new UserDetailEntity(d))
                .ToList();
        }

        internal UserDetailEntity GetActiveEntityByAttributeIdAndDescription(long attributeId, string description)
        {
            return databaseController.GetDataTable($"SELECT * FROM \"Administration\".\"UserDetail\" WHERE \"IsActiveRecord\" = '1'AND \"UserAttributeId\" = {attributeId} AND \"Description\" = '{description}' ")
                .Rows.Cast<DataRow>()
                .Select(d => new UserDetailEntity(d))
                .FirstOrDefault();
        }

        internal void Insert(long id, long attributeId, string description)
        {
            databaseController.ExecuteScalar($"INSERT INTO \"Administration\".\"UserDetail\" (\"CreatedByUserId\", \"UserId\", \"UserAttributeId\", \"Description\") VALUES (1, {id}, {attributeId}, '{description}')");
        }
    }
}
