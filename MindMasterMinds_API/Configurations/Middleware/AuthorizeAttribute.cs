using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MindMasterMinds_Data.Models.Internal;

namespace MindMasterMinds_API.Configurations.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public ICollection<string> Roles { get; set; }

        public AuthorizeAttribute(params string[] roles)
        {
            Roles = roles.Select(x => x.ToLower()).ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var auth = (AuthModel?)context.HttpContext.Items["User"];
            if (auth == null)
            {
                context.Result = new JsonResult(new { message = "Đăng nhập cái đã" }) { StatusCode = StatusCodes.Status401Unauthorized };
                //throw new UnauthorizedException("Unauthorized");
            }
            else
            {
                var role = auth.Role;
                var isValid = false;
                if (Roles.Contains(role.ToLower()))
                {
                    isValid = true;
                }
                if (!isValid)
                {
                    context.Result = new JsonResult(new { message = "Quyền của bạn không đủ để vào" }) { StatusCode = StatusCodes.Status403Forbidden };
                    //throw new ForbiddenException("Forbidden");
                }
            }
        }
    }
}
