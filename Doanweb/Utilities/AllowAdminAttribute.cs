using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Doanweb.Utilities;

public class AllowAdminAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      
        if (Function._RoleId != 2)
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Home", new { area = "" });
        }
    }
}
