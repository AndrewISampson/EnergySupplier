using API.Entity.Database.Administration.Table.User;

namespace API.Controllers.Code.Login
{
    public class ProcessInternalLoginController : ProcessLoginController
    {
        public ProcessInternalLoginController()
        {

        }

        internal override bool UserHasValidRole(List<UserDetailEntity> roleUserDetailEntityList)
        {
            return roleUserDetailEntityList.Select(u => u.Description).Contains("Internal");
        }
    }
}
