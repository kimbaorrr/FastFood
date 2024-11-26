using FastFood.Models;
using System.Web.Mvc;

namespace FastFood.Controllers
{
    public class SessionController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["GioHang"] == null)
            {
                Session["GioHang"] = new FastFood_GioHang();
            }
        }

        /// <summary>
        /// Thông báo dạng JSON
        /// </summary>
        /// <param name="success">Trạng thái thực thi</param>
        /// <param name="message">Nội dung thông báo</param>
        /// <returns></returns>
        public JsonResult JsonMessage(bool success, string message)
        {
            return success
                ? Json(new { success = success, type = "var(--bs-success)", message = message }, JsonRequestBehavior.AllowGet)
                : Json(new { success = success, type = "var(--bs-danger)", message = message }, JsonRequestBehavior.AllowGet);
        }


    }
}