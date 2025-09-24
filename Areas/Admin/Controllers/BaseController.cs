using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using FastFood.DB.Entities;
using FastFood.Controllers;
using FastFood.Models.ViewModels;

namespace FastFood.Areas.Admin.Controllers
{
    [Area("Admin")]
    public abstract class BaseEmployeeController : BaseAppController
    {
        protected int? _employeeId => this.GetEmployeeFromClaim()?.EmployeeId ?? -1;
        protected BaseEmployeeController()
        {

        }

        protected EmployeeClaimInfoViewModel? GetEmployeeFromClaim()
        {
            var claimData = User?.FindFirst("EmployeeInfo");
            if (claimData != null)
                return JsonConvert.DeserializeObject<EmployeeClaimInfoViewModel>(claimData.Value);

            return null;
        }

        protected async Task AddEmployeeToClaim(string employeeId, EmployeeClaimInfoViewModel employeeInfo, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employeeId ?? string.Empty),
                new Claim("EmployeeInfo", JsonConvert.SerializeObject(employeeInfo))
            };

            var identity = new ClaimsIdentity(claims, "EmployeeScheme");
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe
            ? DateTimeOffset.UtcNow.AddDays(7)
            : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                "EmployeeScheme",
                principal,
                authProperties
            );
        }

        protected async Task ClearClaims()
            => await HttpContext.SignOutAsync("EmployeeScheme");

        protected string GetAbsoluteUri()
        {
            string absoluteUri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            return absoluteUri;
        }
    }
}
