using FastFood.DB;
using FastFood.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{
    public class CategoriesController : SessionController
    {
        /// <summary>
        /// Lấy danh sách danh mục và hiển thị trên trang phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại cần lấy (mặc định là 1).</param>
        /// <param name="size">Kích thước của trang (số lượng danh mục mỗi trang, mặc định là 10).</param>
        /// <returns>Trang danh sách danh mục.</returns>
        public ActionResult List(int page = 1, int size = 10)
        {
            ViewBag.Title = "Tất cả danh mục";
            X.PagedList.IPagedList<DanhMuc> danhMuc = FastFood_DanhMuc.getDanhMuc().OrderBy(m => m.MaDanhMuc).ToPagedList(page, size);
            ViewBag.DanhMuc = danhMuc;
            ViewBag.CurrentPage = danhMuc.PageNumber;
            ViewBag.TotalPages = danhMuc.PageCount;
            return View();
        }
        /// <summary>
        /// Tạo mới danh mục với hình ảnh đại diện và hình nền tải lên từ người dùng.
        /// </summary>
        /// <param name="a">Thông tin chi tiết danh mục và tệp tải lên.</param>
        /// <returns>Kết quả JSON thông báo thành công hay thất bại.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FastFood_DanhMuc_TaoMoi a)
        {
            if (a.AnhDaiDien == null || a.AnhNen == null)
                return JsonMessage(false, "Chưa chọn tệp tải lên !");

            if (a.AnhDaiDien.ContentLength == 0 || a.AnhDaiDien.ContentLength > 10000000 || a.AnhNen.ContentLength == 0 || a.AnhNen.ContentLength > 10000000)
                return JsonMessage(false, "Kích thước các tệp phải khác 0 & <= 10MB !");

            if (!a.AnhDaiDien.ContentType.Contains("image/") || !a.AnhNen.ContentType.Contains("image/"))
                return JsonMessage(false, "Các tệp tải lên phải là định dạng ảnh !");

            string thumbPath = "~/Content/admin_page/uploads/categories/thumbnail/";
            string bgPath = "~/Content/admin_page/uploads/categories/background/";

            string thumbFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(a.AnhDaiDien.FileName);
            string thumbSavePath = Path.Combine(Server.MapPath(thumbPath), thumbFileName);
            a.AnhDaiDien.SaveAs(thumbSavePath);
            string thumbDBPath = Path.Combine(thumbPath, thumbFileName).Replace("\\", "/").Replace("~", "");

            string bgFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(a.AnhNen.FileName);
            string bgSavePath = Path.Combine(Server.MapPath(bgPath), bgFileName);
            a.AnhNen.SaveAs(bgSavePath);
            string bgDBPath = Path.Combine(bgPath, bgFileName).Replace("\\", "/").Replace("~", "");

            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (e.DanhMucs.Any(x => x.TenDanhMuc.Contains(a.TenDanhMuc)))
                    return JsonMessage(false, "Tên danh mục đã tồn tại !");

                DanhMuc dm = new DanhMuc()
                {
                    TenDanhMuc = a.TenDanhMuc,
                    MoTa = a.MoTa,
                    NguoiTao = a.MaNguoiTao,
                    NgayTao = DateTime.Now,
                    AnhDaiDien = thumbDBPath,
                    AnhNen = bgDBPath
                };
                e.DanhMucs.Add(dm);
                e.SaveChanges();
                return JsonMessage(true, "Thêm danh mục thành công !");
            }
        }
        /// <summary>
        /// Xóa danh mục dựa vào mã danh mục.
        /// </summary>
        /// <param name="categoryId">Mã của danh mục cần xóa.</param>
        /// <returns>Kết quả JSON thông báo thành công hay thất bại.</returns>
        [HttpPost]
        public ActionResult Delete(int categoryId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                DanhMuc dm = e.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == categoryId);
                if (dm == null)
                    return HttpNotFound();

                if (dm.SanPhams.Any())
                    return JsonMessage(false, "Danh mục này đã có sản phẩm. Không thể xóa !");

                string thumbFilePath = Server.MapPath(dm.AnhDaiDien);
                string bgFilePath = Server.MapPath(dm.AnhNen);
                DeleteFileIfExists(thumbFilePath);
                DeleteFileIfExists(bgFilePath);

                e.DanhMucs.Remove(dm);
                e.SaveChanges();
            }

            return JsonMessage(true, $"Xóa danh mục {categoryId} thành công !");
        }
        /// <summary>
        /// Xóa nhiều danh mục cùng lúc.
        /// </summary>
        /// <param name="categoryIds">Danh sách mã danh mục cần xóa.</param>
        /// <returns>Kết quả JSON thông báo thành công hay thất bại với số lượng danh mục xóa được.</returns>
        [HttpPost]
        public ActionResult DeleteMultiple(IEnumerable<int> categoryIds)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                List<int> failedDeletes = new List<int>();
                int totalCount = categoryIds.Count();

                foreach (int categoryId in categoryIds)
                {
                    DanhMuc dm = e.DanhMucs.FirstOrDefault(x => x.MaDanhMuc == categoryId);
                    if (dm == null)
                        return HttpNotFound();

                    if (dm.SanPhams.Any())
                    {
                        failedDeletes.Add(categoryId);
                        continue;
                    }

                    string thumbFilePath = Server.MapPath(dm.AnhDaiDien);
                    string bgFilePath = Server.MapPath(dm.AnhNen);
                    DeleteFileIfExists(thumbFilePath);
                    DeleteFileIfExists(bgFilePath);

                    e.DanhMucs.Remove(dm);
                }

                e.SaveChanges();

                if (failedDeletes.Count > 0)
                {
                    return Json(new { success = false, type = "var(--bs-danger)", message = "Một số danh mục không thể xóa do đã có sản phẩm.", failedIds = failedDeletes });
                }

                return JsonMessage(true, $"{totalCount - failedDeletes.Count}/{totalCount} danh mục được xóa thành công!");
            }
        }
        /// <summary>
        /// Xóa tệp trên hệ thống nếu tệp tồn tại.
        /// </summary>
        /// <param name="filePath">Đường dẫn của tệp cần xóa.</param>
        private static void DeleteFileIfExists(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }
    }
}
