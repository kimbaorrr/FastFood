using System.Web;
using System.Web.Mvc;

namespace FastFood.Areas.Admin.Controllers
{

    public class SessionController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string employeeId = Session["MaNhanVien"] as string;
            string anhDD = Session["AnhDD"] as string;

            if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(anhDD))
            {
                HttpCookie loginCookie = Request.Cookies["LoginCookie"];

                if (loginCookie != null)
                {
                    employeeId = loginCookie.Values["MaNhanVien"];
                    anhDD = loginCookie.Values["AnhDD"];

                    if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(anhDD))
                    {
                        Session["MaNhanVien"] = employeeId;
                        Session["AnhDD"] = anhDD;
                    }
                }

                if (string.IsNullOrEmpty(Session["MaNhanVien"] as string))
                {
                    filterContext.Result = RedirectToAction("Login", "Account", new { area = "Admin" });
                }
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