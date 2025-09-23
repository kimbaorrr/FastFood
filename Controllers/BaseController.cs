using FastFood.DB.Entities;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;

namespace FastFood.Controllers
{
    public abstract class BaseAppController : Controller
    {
        protected IActionResult CreateJsonResult(
            bool success = false,
            string message = "",
            object? data = null,
            int statusCode = 200)
        {
            var json = new
            {
                success,
                type = success ? "var(--bs-success)" : "var(--bs-danger)",
                message,
                data = data ?? new { }
            };

            var jsonSerialized = JsonConvert.SerializeObject(json);

            return statusCode switch
            {
                400 => BadRequest(jsonSerialized),
                401 => Unauthorized(jsonSerialized),
                403 => Forbid(),
                404 => NotFound(jsonSerialized),
                500 => StatusCode(500, jsonSerialized),
                _ => Ok(jsonSerialized)
            };
        }
    }

    public abstract class BaseCustomerController : BaseAppController
    {
        protected int _customerId => this.GetCustomerFromClaim()?.CustomerId ?? -1;

        protected Customer? GetCustomerFromClaim()
        {
            var claimData = User?.FindFirst("CustomerInfo");
            if (claimData != null)
                return JsonConvert.DeserializeObject<Customer>(claimData.Value);

            return null;
        }

        protected string GetAbsoluteUri()
        {
            string absoluteUri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            return absoluteUri;
        }

        protected async Task AddCustomerToClaim(string customerId, CustomerClaimInfoViewModel customerClaimInfo, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customerId ?? string.Empty),
                new Claim("CustomerInfo", JsonConvert.SerializeObject(customerClaimInfo))
            };

            var identity = new ClaimsIdentity(claims, "CustomerScheme");
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe
            ? DateTimeOffset.UtcNow.AddDays(7)
            : DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                "CustomerScheme",
                principal,
                authProperties
            );
        }

        protected async Task ClearClaims()
                => await HttpContext.SignOutAsync("CustomerScheme");


        protected void AddPaymentSummaryToSession(string paymentSummary)
        {
            HttpContext.Session.SetString("PaymentSummary", paymentSummary);
        }

        protected PaymentSummaryViewModel? GetPaymentSummaryFromSession()
        {
            var summaryJson = HttpContext.Session.GetString("PaymentSummary");
            if (!string.IsNullOrEmpty(summaryJson))
            {
                try
                {
                    return JsonConvert.DeserializeObject<PaymentSummaryViewModel>(summaryJson);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

    }
}