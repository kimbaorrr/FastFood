using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using FastFood.DB.Entities;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public abstract class BaseController : Controller
    {
        protected int? _employeeId => this.GetEmployeeFromClaim()?.EmployeeId ?? -1;
        protected BaseController()
        {

        }
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (!(User.Identity?.IsAuthenticated ?? false) || !User.HasClaim(c => c.Type == "EmployeeInfo"))
        //    {
        //        context.Result = new RedirectToActionResult("Login", "Account", new {area = "Admin"});
        //    }

        //    base.OnActionExecuting(context);
        //}

        protected JsonResult CreateJsonResult(bool success = false, string message = "", object? data = null)
        {

            var json = new
            {
                success = success!,
                type = success ? "var(--bs-success)" : "var(--bs-danger)",
                message = message!,
                data = data ?? new { }
            };

            return new JsonResult(json, new JsonSerializerOptions()
            {
                WriteIndented = true
            });

        }

        protected Employee? GetEmployeeFromClaim()
        {
            var claimData = User?.FindFirst("EmployeeInfo");
            if (claimData != null)
                return JsonConvert.DeserializeObject<Employee>(claimData.Value);

            return null;
        }

        protected async Task AddEmployeeToClaim(string employeeId, Employee employeeInfo, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employeeId ?? string.Empty),
                new Claim("EmployeeInfo", JsonConvert.SerializeObject(employeeInfo))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe
            ? DateTimeOffset.UtcNow.AddDays(7)
            : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            );
        }

        protected async Task ClearClaims()
            => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        protected string GetAbsoluteUri()
        {
            string absoluteUri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            return absoluteUri;
        }
    }
}
