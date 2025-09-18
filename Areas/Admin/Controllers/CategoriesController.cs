using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Route("admin/danh-muc")]
    public class CategoriesController : SessionController
    {
        private readonly IWebHostEnvironment _environment;
        private string RootPath => _environment.WebRootPath;
        private string CategoriesPath => Path.Combine(RootPath, "admin_page/uploads/categories/");
        private string VirtualPath => "/admin_page/uploads/categories/";
        public CategoriesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// Lấy danh sách danh mục và hiển thị trên trang phân trang.
        /// </summary>
        /// <param name="page">Trang hiện tại cần lấy (mặc định là 1).</param>
        /// <param name="size">Kích thước của trang (số lượng danh mục mỗi trang, mặc định là 10).</param>
        /// <returns>Trang danh sách danh mục.</returns>
        [HttpGet("tat-ca-danh-muc")]
        public IActionResult List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            ViewBag.Title = "Tất cả danh mục";
            IPagedList<Danhmuc> danhMuc = FastFood_DanhMuc.getDanhMuc().OrderBy(m => m.Madanhmuc).ToPagedList(page, size);
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
        [HttpPost("them-danh-muc")]
        [ValidateAntiForgeryToken]
        public string Create(FastFood_DanhMuc_TaoMoi a)
        {
            int contentLength = 10000000;
            if (a.AnhDaiDien == null || a.AnhNen == null || a.AnhDaiDien.Length == 0 || a.AnhNen.Length == 0)
                return JsonMessage(message: "Chưa chọn tệp tải lên !");

            if (a.AnhDaiDien.Length > contentLength || a.AnhNen.Length > contentLength)
                return JsonMessage(message: "Kích thước các tệp phải <= 10MB !");

            if (!a.AnhDaiDien.ContentType.Contains("image/") || !a.AnhNen.ContentType.Contains("image/"))
                return JsonMessage(message: "Các tệp tải lên phải là định dạng ảnh !");

            string randName = Guid.NewGuid().ToString();
            string bgFileName = randName + Path.GetExtension(a.AnhNen.FileName);
            string thumbFileName = randName + Path.GetExtension(a.AnhDaiDien.FileName);

            string thumbSavePath = Path.Combine(this.CategoriesPath, "thumbnail", thumbFileName);
            string bgSavePath = Path.Combine(this.CategoriesPath, "background", bgFileName);

            string thumbVirtualPath = Path.Combine(this.VirtualPath, "thumbnail", thumbFileName);
            string bgVirtualPath = Path.Combine(this.VirtualPath, "background", bgFileName);

            using (FileStream anhDD = new FileStream(thumbSavePath, FileMode.Create))
            {
                a.AnhDaiDien.CopyTo(anhDD);
            }

            using (FileStream anhNen = new FileStream(bgSavePath, FileMode.Create))
            {
                a.AnhNen.CopyTo(anhNen);
            }

            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (e.Danhmucs.Any(x => x.Tendanhmuc.Equals(a.TenDanhMuc)))
                    return JsonMessage(message: "Tên danh mục đã tồn tại !");

                Danhmuc dm = new Danhmuc()
                {
                    Tendanhmuc = a.TenDanhMuc,
                    Mota = a.MoTa,
                    Nguoitao = Convert.ToInt32(this.MaNhanVien),
                    Ngaytao = DateTime.Now,
                    Anhdaidien = thumbVirtualPath,
                    Anhnen = bgVirtualPath
                };

                e.Danhmucs.Add(dm);
                e.SaveChanges();

                return JsonMessage(true, "Thêm danh mục thành công !");
            }
        }
        /// <summary>
        /// Xóa danh mục dựa vào mã danh mục.
        /// </summary>
        /// <param name="categoryId">Mã của danh mục cần xóa.</param>
        /// <returns>Kết quả JSON thông báo thành công hay thất bại.</returns>
        [HttpPost("xoa-danh-muc")]
        public string Delete([FromForm] int categoryId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Danhmuc? dm = e.Danhmucs.FirstOrDefault(x => x.Madanhmuc == categoryId);

                if (dm == null)
                    return JsonMessage(message: "ID Không hợp lệ !");

                if (dm.Sanphams.Any())
                    return JsonMessage(message: "Danh mục này đã có sản phẩm. Không thể xóa !");

                FastFood_Tools.DeleteFile(dm.Anhnen!);
                FastFood_Tools.DeleteFile(dm.Anhdaidien!);

                e.Danhmucs.Remove(dm);
                e.SaveChanges();
            }

            return JsonMessage(true, $"Xóa danh mục {categoryId} thành công !");

        }
        /// <summary>
        /// Xóa nhiều danh mục cùng lúc.
        /// </summary>
        /// <param name="categoryIds">Danh sách mã danh mục cần xóa.</param>
        /// <returns>Kết quả JSON thông báo thành công hay thất bại với số lượng danh mục xóa được.</returns>
        [HttpPost("xoa-nhieu-danh-muc")]
        public string DeleteMultiple([FromForm] IEnumerable<int> categoryIds)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                List<int> failedDeletes = new List<int>();
                int totalCount = categoryIds.Count();

                foreach (int categoryId in categoryIds)
                {
                    Danhmuc? dm = e.Danhmucs.FirstOrDefault(x => x.Madanhmuc == categoryId);

                    if (dm == null)
                        return JsonMessage(message: "ID không hợp lệ !");

                    if (dm.Sanphams.Any())
                    {
                        failedDeletes.Add(categoryId);
                        continue;
                    }

                    FastFood_Tools.DeleteFile(dm.Anhnen!);
                    FastFood_Tools.DeleteFile(dm.Anhdaidien!);

                    e.Danhmucs.Remove(dm);
                }

                e.SaveChanges();

                if (failedDeletes.Count > 0)
                {
                    return JsonMessage(message: $"Một số danh mục không thể xóa do đã có sản phẩm. {failedDeletes}");
                }

                return JsonMessage(true, $"{totalCount - failedDeletes.Count}/{totalCount} danh mục được xóa thành công!");
            }

        }
        [HttpPost("sua-danh-muc")]
        [ValidateAntiForgeryToken]
        public string Edit(FastFood_DanhMuc_SuaDanhMuc a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Danhmuc? dm = e.Danhmucs.Where(x => x.Madanhmuc == a.MaDanhMuc).FirstOrDefault();

                if (dm == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                dm.Tendanhmuc = a.TenDanhMuc;
                dm.Mota = a.MoTa;

                e.SaveChanges();
                return JsonMessage(true, "Sửa danh mục thành công !");
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
