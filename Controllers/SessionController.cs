using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace FastFood.Areas.Controllers
{
    public class SessionController : Controller
    {
        protected ISession Session => HttpContext.Session;
        protected string CustomerId => Session.GetString("CustomerId") ?? string.Empty;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(Session.GetString("MyCart")))
            {
                Session.SetString("MyCart", JsonConvert.SerializeObject(new FastFood_GioHang()));
            }
        }

        /// <summary>
        /// Thông báo dạng JSON
        /// </summary>
        /// <param name="success">Trạng thái thực thi</param>
        /// <param name="message">Nội dung thông báo</param>
        /// <returns></returns>
        public string JsonMessage(bool success = false, string message = "", object data = null)
        {
            dynamic jsonMessage = new
            {
                success,
                type = success ? "var(--bs-success)" : "var(--bs-danger)",
                message = message ?? string.Empty,
                data = data ?? string.Empty
            };
            string jsonOutput = JsonConvert.SerializeObject(jsonMessage, Formatting.Indented);

            return jsonOutput;

        }

        protected string GetAbsoluteUri()
        {
            string absoluteUri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            return absoluteUri;
        }

        protected FastFood_GioHang? GetGioHang()
        {
            string gioHang = Session.GetString("MyCart") ?? string.Empty;
            return JsonConvert.DeserializeObject<FastFood_GioHang>(gioHang);
        }

        protected void SetGioHang(FastFood_GioHang gh)
        {
            Session.SetString("MyCart", JsonConvert.SerializeObject(gh));
        }

        protected FastFood_ThanhToan_TomTatThanhToan? GetTomTatThanhToan()
        {
            string tomTat = Session.GetString("CheckoutSummary") ?? string.Empty;
            return JsonConvert.DeserializeObject<FastFood_ThanhToan_TomTatThanhToan>(tomTat);
        }

        protected void SetTomTatThanhToan(FastFood_ThanhToan_TomTatThanhToan tt)
        {
            Session.SetString("CheckoutSummary", JsonConvert.SerializeObject(tt));
        }


    }
}