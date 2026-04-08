using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DotNetLearning.Models;

namespace DotNetLearning.Filters;

/// <summary>
/// 要求使用者已註冊。
/// 頁面請求 → 導向 /Account/Register
/// AJAX 請求 → 回傳 JSON { requireRegistration: true }
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequireRegistrationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.Items["CurrentUser"] as SiteUser;
        if (user != null && user.IsRegistered)
        {
            base.OnActionExecuting(context);
            return;
        }

        // AJAX / API request
        if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            || context.HttpContext.Request.ContentType?.Contains("json") == true
            || context.HttpContext.Request.Headers["Accept"].ToString().Contains("json"))
        {
            context.Result = new JsonResult(new
            {
                requireRegistration = true,
                message = "此功能需要註冊帳號才能使用，請先註冊或登入！"
            });
            return;
        }

        // Page request → redirect to register
        var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        context.Result = new RedirectToActionResult("Register", "Account", new { returnUrl });
    }
}
