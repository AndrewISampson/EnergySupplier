using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Code.Login
{
    public class ProcessBrokerLoginController : ProcessLoginController
    {
        public ProcessBrokerLoginController()
        {

        }

        internal override bool UserHasValidRole(List<UserDetailEntity> roleUserDetailEntityList)
        {
            return roleUserDetailEntityList.Select(u => u.Description).Contains("Broker")
                || roleUserDetailEntityList.Select(u => u.Description).Contains("Internal");
        }
    }
}
