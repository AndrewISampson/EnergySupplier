using API.Controllers.Database.Administration.Table.Password;
using API.Controllers.Database.Administration.Table.User;
using API.Controllers.Database.Mapping.Table;
using API.Entity.Code;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code.Navbar
{
    public class GetNavbarController : WebsiteRequestController
    {
        public GetNavbarController()
        {

        }

        internal OkObjectResult GetNavbar(JObject json)
        {
            try
            {
                var getNavbarEntity = JsonConvert.DeserializeObject<GetNavbarEntity>(json.ToString());
                var securityToken = JsonConvert.DeserializeObject<SecurityTokenEntity>(getNavbarEntity.SecurityToken);

                var userController = new UserController();
                var userEntity = userController.GetActiveEntityByGuid(securityToken.Id1);

                var passwordController = new PasswordController();
                var passwordEntity = passwordController.GetActiveEntityByGuid(securityToken.Id2);

                var administration_Password_To_Administration_UserController = new Administration_Password_To_Administration_UserController();
                var administration_Password_To_Administration_UserEntity = administration_Password_To_Administration_UserController.GetActiveEntityByUserId(userEntity.Id);

                if (administration_Password_To_Administration_UserEntity.PasswordId != passwordEntity.Id)
                {
                    return Ok(new { valid = false });
                }

                var userAttributeController = new UserAttributeController();
                var roleUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Role");

                var userDetailController = new UserDetailController();
                var roleUserDetailEntityList = userDetailController.GetActiveEntityListByIdAndAttributeId(userEntity.Id, roleUserAttributeEntity.Id);

                if (roleUserDetailEntityList.Select(u => u.Description).Contains("Broker"))
                {
                    return Ok(new { valid = true, navbar = "Broker" });
                }

                if (roleUserDetailEntityList.Select(u => u.Description).Contains("Internal"))
                {
                    return Ok(new { valid = true, navbar = "Broker_Internal" });
                }

                return Ok(new { valid = false });
            }
            catch
            {
                return Ok(new { valid = false });
            }
        }
    }
}
