using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoGrupo1.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleId = context.HttpContext.Session.GetInt32("RolID") ?? 0;
            if (roleId != ProyectoGrupo1.Roles.Admin)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
