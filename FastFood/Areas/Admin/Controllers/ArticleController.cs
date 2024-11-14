using FastFood.Areas.Admin.Models;
using FastFood.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    public class ArticleController : SessionController
    {
        /// <summary>
        /// Hiển thị giao diện để tạo bài viết mới.
        /// </summary>
        /// <returns>Giao diện thêm bài viết mới.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 5))
                return HttpNotFound();

            FastFood_BaiViet_ThemBaiViet bv = new FastFood_BaiViet_ThemBaiViet()
            {
                MaNguoiTao = Convert.ToInt32(Session["MaNhanVien"] as string)
            };
            ViewBag.Title = "Thêm bài viết";
            return View(bv);
        }
        /// <summary>
        /// Xử lý việc tạo mới bài viết với thông tin từ người dùng.
        /// </summary>
        /// <param name="a">Đối tượng bài viết chứa thông tin nhập vào từ người dùng.</param>
        /// <returns>Kết quả JSON báo thành công hoặc thất bại khi tạo bài viết.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(FastFood_BaiViet_ThemBaiViet a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (e.BaiViets.Any(x => x.TieuDe.Equals(a.TieuDe)))
                    return JsonMessage(false, "Tiêu đề bài viết đã tồn tại trên hệ thống !");

                if (a.HinhAnh == null)
                    return JsonMessage(false, "Chưa chọn tệp tải lên !");

                if (a.HinhAnh.ContentLength == 0 || a.HinhAnh.ContentLength > 10000000)
                    return JsonMessage(false, "Kích thước các tệp phải khác 0 & <= 10MB !");

                if (!a.HinhAnh.ContentType.Contains("image/"))
                    return JsonMessage(false, "Các tệp tải lên phải là định dạng ảnh !");

                string savePath = "~/Content/admin_page/uploads/articles/";
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(a.HinhAnh.FileName);
                string fileSavePath = Path.Combine(Server.MapPath(savePath), fileName);
                a.HinhAnh.SaveAs(fileSavePath);
                string fileDBPath = Path.Combine(savePath, fileName).Replace("\\", "/").Replace("~", "");

                BaiViet bv = new BaiViet()
                {
                    TieuDe = a.TieuDe,
                    NoiDung = a.NoiDung,
                    MoTaNgan = a.MoTaNgan,
                    HinhAnh = fileDBPath,
                    NgayTao = DateTime.Now,
                    NguoiTao = a.MaNguoiTao
                };
                e.BaiViets.Add(bv);
                e.SaveChanges();
                return JsonMessage(true, "Thêm bài viết thành công !");
            }
        }
        /// <summary>
        /// Hiển thị danh sách tất cả bài viết đã được duyệt.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số bài viết trên mỗi trang.</param>
        /// <returns>Giao diện danh sách bài viết.</returns>
        [HttpGet]
        public ActionResult List(int page = 1, int size = 10)
        {
            ViewBag.Title = "Tất cả bài viết";
            X.PagedList.IPagedList<BaiViet> baiViet = FastFood_BaiViet.getBaiVietDaDuyet().OrderBy(m => m.MaBaiViet).ToPagedList(page, size);
            ViewBag.BaiViet = baiViet;
            ViewBag.CurrentPage = baiViet.PageNumber;
            ViewBag.TotalPages = baiViet.PageCount;
            return View();
        }
        /// <summary>
        /// Xóa một bài viết theo mã bài viết.
        /// </summary>
        /// <param name="articleId">Mã bài viết cần xóa.</param>
        /// <returns>Kết quả JSON báo thành công hoặc thất bại khi xóa bài viết.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int articleId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                BaiViet a = e.BaiViets.FirstOrDefault(x => x.MaBaiViet == articleId);
                if (a == null)
                    return HttpNotFound();

                string filePath = Server.MapPath(a.HinhAnh);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                e.BaiViets.Remove(a);
                e.SaveChanges();
                return JsonMessage(true, $"Xóa bài viết {articleId} thành công !");
            }
        }

        /// <summary>
        /// Xóa nhiều bài viết dựa trên danh sách mã bài viết.
        /// </summary>
        /// <param name="articleIds">Danh sách mã bài viết cần xóa.</param>
        /// <returns>Kết quả JSON báo thành công hoặc thất bại khi xóa các bài viết.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(IEnumerable<int> articleIds)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                List<int> failedDeletes = new List<int>();
                int totalCount = articleIds.Count();
                foreach (int articleId in articleIds)
                {
                    BaiViet a = e.BaiViets.FirstOrDefault(x => x.MaBaiViet == articleId);
                    if (a == null)
                        return HttpNotFound();
                    string filePath = Server.MapPath(a.HinhAnh);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    e.BaiViets.Remove(a);
                }
                e.SaveChanges();
                return JsonMessage(true, $"{articleIds.Count()} bài viết được xóa thành công!");
            }
        }
        /// <summary>
        /// Hiển thị danh sách bài viết chờ duyệt.
        /// </summary>
        /// <param name="page">Số trang hiện tại.</param>
        /// <param name="size">Số bài viết trên mỗi trang.</param>
        /// <returns>Giao diện danh sách bài viết chờ duyệt.</returns>
        [HttpGet]
        public ActionResult Approve(int page = 1, int size = 10)
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 3))
                return HttpNotFound();

            X.PagedList.IPagedList<BaiViet> baiViet = FastFood_BaiViet.getBaiVietChuaDuyet().OrderBy(m => m.MaBaiViet).ToPagedList(page, size);
            ViewBag.BaiViet = baiViet;
            ViewBag.CurrentPage = baiViet.PageNumber;
            ViewBag.TotalPages = baiViet.PageCount;
            ViewBag.Title = "Phê duyệt bài viết";
            return View();
        }
        /// <summary>
        /// Phê duyệt hoặc từ chối một bài viết.
        /// </summary>
        /// <param name="articleId">Mã bài viết.</param>
        /// <param name="employeeId">Mã nhân viên thực hiện phê duyệt.</param>
        /// <param name="action">Hành động ("accept" để phê duyệt, "refuse" để từ chối).</param>
        /// <returns>Kết quả JSON báo thành công hoặc thất bại của hành động.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int articleId, int employeeId, string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                BaiViet a = e.BaiViets.FirstOrDefault(x => x.MaBaiViet == articleId);

                if (a == null)
                    return HttpNotFound();

                switch (action)
                {
                    case "accept":
                        a.NguoiDuyet = employeeId;
                        a.NgayDuyet = DateTime.Now;
                        a.DaDuyet = true;
                        e.SaveChanges();
                        return JsonMessage(true, $"Đã phê duyệt & xuất bản bài viết {articleId}!");

                    case "refuse":
                        string filePath = Server.MapPath(a.HinhAnh);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        e.BaiViets.Remove(a);
                        e.SaveChanges();
                        return JsonMessage(true, $"Đã từ chối & xóa bài viết {articleId} khỏi hệ thống!");
                }
            }
            return HttpNotFound();
        }
        /// <summary>
        /// Phê duyệt hoặc từ chối nhiều bài viết dựa trên danh sách mã bài viết.
        /// </summary>
        /// <param name="articleIds">Danh sách mã bài viết cần phê duyệt hoặc từ chối.</param>
        /// <param name="employeeId">Mã nhân viên thực hiện phê duyệt.</param>
        /// <param name="action">Hành động ("accept" để phê duyệt, "refuse" để từ chối).</param>
        /// <returns>Kết quả JSON báo thành công hoặc thất bại của hành động trên các bài viết.</returns>
        [HttpPost]
        public ActionResult ApproveMultiple(IEnumerable<int> articleIds, int employeeId, string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                foreach (int articleId in articleIds)
                {
                    BaiViet product = e.BaiViets.FirstOrDefault(x => x.MaBaiViet == articleId);
                    if (product != null)
                    {
                        switch (action)
                        {
                            case "accept":
                                product.NguoiDuyet = employeeId;
                                product.NgayDuyet = DateTime.Now;
                                product.DaDuyet = true;
                                break;
                            case "refuse":
                                string filePath = Server.MapPath(product.HinhAnh);
                                if (System.IO.File.Exists(filePath))
                                    System.IO.File.Delete(filePath);
                                e.BaiViets.Remove(product);
                                break;

                        }
                    }
                }
                e.SaveChanges();

                if (action == "accept")
                    return JsonMessage(true, "Đã phê duyệt & xuẩt bản các bài viết được chọn !");
                if (action == "refuse")
                    return JsonMessage(true, "Đã từ chối & xóa các bài viết được chọn !");
            }
            return HttpNotFound();
        }
        /// <summary>
        /// Lấy danh sách các bài viết chưa được duyệt.
        /// </summary>
        /// <returns>Dữ liệu JSON chứa danh sách các bài viết chưa được duyệt.</returns>
        [HttpGet]
        public ActionResult GetListNonApprove()
        {
            var bv = FastFood_BaiViet.getBaiVietChuaDuyet()
                .AsEnumerable()
                .Where(x => x.NguoiTao.ToString().Equals(Session["MaNhanVien"] as string))
                .Select(x => new { x.MaBaiViet, x.HinhAnh, x.TieuDe, x.MoTaNgan });
            return Json(bv, JsonRequestBehavior.AllowGet);
        }
    }
}