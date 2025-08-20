using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoGrupo1.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        private const int AdminRoleId = 2;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var roleId = context.HttpContext.Session.GetInt32("RolID");
            if (roleId != AdminRoleId)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
