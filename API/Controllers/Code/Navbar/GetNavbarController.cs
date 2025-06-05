using API.Controllers.Database.Administration.Table.User;
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

                if (getNavbarEntity.Path == "/" || string.IsNullOrWhiteSpace(getNavbarEntity.SecurityToken))
                {
                    return Ok(new { valid = true, navbar = "Master" });
                }

                var securityController = new SecurityController();
                var userId = securityController.ValidateSecurityToken(getNavbarEntity.SecurityToken);

                if (userId == 0)
                {
                    return Ok(new { valid = false });
                }

                var userAttributeController = new UserAttributeController();
                var roleUserAttributeEntity = userAttributeController.GetActiveEntityByDescription("Role");

                var userDetailController = new UserDetailController();
                var roleUserDetailEntityList = userDetailController.GetActiveEntityListByIdAndAttributeId(userId, roleUserAttributeEntity.Id);

                if (getNavbarEntity.Path.StartsWith("/broker"))
                {
                    if (roleUserDetailEntityList.Select(u => u.Description).Contains("Broker"))
                    {
                        return Ok(new { valid = true, navbar = "Broker" });
                    }

                    if (roleUserDetailEntityList.Select(u => u.Description).Contains("Internal"))
                    {
                        return Ok(new { valid = true, navbar = "Broker_Internal" });
                    }
                }

                if (getNavbarEntity.Path.StartsWith("/customer"))
                {
                    if (roleUserDetailEntityList.Select(u => u.Description).Contains("Customer"))
                    {
                        return Ok(new { valid = true, navbar = "Customer" });
                    }

                    if (roleUserDetailEntityList.Select(u => u.Description).Contains("Internal"))
                    {
                        return Ok(new { valid = true, navbar = "Customer_Internal" });
                    }
                }

                if (roleUserDetailEntityList.Select(u => u.Description).Contains("Internal"))
                {
                    return Ok(new { valid = true, navbar = "Internal" });
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
