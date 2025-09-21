using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json;
using FastFood.DB.Entities;

namespace FastFood.Areas.Controllers
{
    public abstract class BaseController : Controller
    {
        protected int _customerId => this.GetCustomerFromClaim()?.CustomerId ?? -1;
        protected Dictionary<int, CustomerCartViewModel> _customerCartViewModel => this.GetCartFromClaim() ?? new();

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Dictionary<int, CustomerCartViewModel> customerCartViewModel = new();
            await this.AddCartToClaim(customerCartViewModel);

            await base.OnActionExecutionAsync(context, next);
        }

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

        protected async Task AddCustomerToClaim(string customerId, Customer customerInfo, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customerId ?? string.Empty),
                new Claim("CustomerInfo", JsonConvert.SerializeObject(customerInfo))
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

        protected async Task AddCartToClaim(Dictionary<int, CustomerCartViewModel> customerCartViewModel)
        {
            var claims = new List<Claim>(User?.Claims ?? Enumerable.Empty<Claim>());

            var oldCartClaim = claims.FirstOrDefault(c => c.Type == "CartInfo");
            if (oldCartClaim != null)
                claims.Remove(oldCartClaim);

            claims.Add(new Claim("CartInfo", JsonConvert.SerializeObject(customerCartViewModel)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties
            );
        }

        protected Dictionary<int, CustomerCartViewModel>? GetCartFromClaim()
        {
            var claimData = User?.FindFirst("CartInfo");
            if (claimData != null)
                return JsonConvert.DeserializeObject<Dictionary<int, CustomerCartViewModel>>(claimData.Value);

            return null;
        }

        protected async Task ClearClaims()
            => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


        protected async Task AddPaymentSummaryToClaim(PaymentSummaryViewModel paymentSummaryViewModel)
        {
            var claims = new List<Claim>(User?.Claims ?? Enumerable.Empty<Claim>());

            var oldClaim = claims.FirstOrDefault(c => c.Type == "CheckoutSummary");
            if (oldClaim != null)
                claims.Remove(oldClaim);

            claims.Add(new Claim("PaymentSummary", JsonConvert.SerializeObject(paymentSummaryViewModel)));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        protected PaymentSummaryViewModel? GetPaymentSummaryFromClaim()
        {
            var claimData = User?.FindFirst("PaymentSummary");
            if (claimData != null)
                return JsonConvert.DeserializeObject<PaymentSummaryViewModel>(claimData.Value)
                    ;
            return null;
        }

    }
}