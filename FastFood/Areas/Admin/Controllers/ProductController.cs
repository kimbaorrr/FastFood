using FastFood.DB;
using FastFood.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using X.PagedList.Extensions;
namespace FastFood.Areas.Admin.Controllers
{
    public class ProductController : SessionController
    {
        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        /// <returns>Trả về danh sách sản phẩm dưới dạng JSON.</returns>
        [HttpPost]
        public ActionResult GetAllProducts()
        {
            return Json(FastFood_SanPham.getSanPham());
        }
        /// <summary>
        /// Hiển thị danh sách sản phẩm đã duyệt, phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng sản phẩm trên mỗi trang (mặc định là 10).</param>
        /// <returns>Trả về trang danh sách sản phẩm đã duyệt.</returns>
        [HttpGet]
        public ActionResult List(int page = 1, int size = 10)
        {
            IPagedList<SanPham> sanPham = FastFood_SanPham.getSanPhamDaDuyet().OrderBy(m => m.MaSanPham).ToPagedList(page, size);
            ViewBag.SanPham = sanPham;
            ViewBag.CurrentPage = sanPham.PageNumber;
            ViewBag.TotalPages = sanPham.PageCount;
            return View();
        }
        /// <summary>
        /// Hiển thị form tạo mới sản phẩm.
        /// </summary>
        /// <returns>Trả về form tạo sản phẩm.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 6))
                return HttpNotFound();

            string employeeId = Session["MaNhanVien"] as string;
            FastFood_SanPham_ThemSanPham sanPham = new FastFood_SanPham_ThemSanPham()
            {
                NguoiTao = FastFood_NhanVien.getHoTen(employeeId),
                MaNguoiTao = Convert.ToInt32(employeeId),
                DanhMuc = new SelectList(FastFood_DanhMuc.getDanhMuc()
                                            .Select(p => new { p.MaDanhMuc, p.TenDanhMuc })
                                            .ToList(), "MaDanhMuc", "TenDanhMuc")
            };
            return View(sanPham);
        }
        /// <summary>
        /// Xử lý lưu trữ dữ liệu sản phẩm mới.
        /// </summary>
        /// <param name="duLieuGuiDi">Dữ liệu gửi lên từ form.</param>
        /// <returns>Trả về kết quả thực thi (thành công hay lỗi).</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FastFood_SanPham_ThemSanPham_Post duLieuGuiDi)
        {
            Tuple<List<string>, ActionResult> isValidFiles = uploadFile(duLieuGuiDi.HinhAnh);
            if (isValidFiles.Item2 != null)
                return isValidFiles.Item2;
            List<string> dsHinhAnh = isValidFiles.Item1;
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (e.SanPhams.Any(x => x.TenSanPham.Contains(duLieuGuiDi.SanPham.TenSanPham)))
                {
                    return JsonMessage(false, "Sản phẩm đã tồn tại trên hệ thống !");
                }
                if (!e.DanhMucs.Any(x => x.MaDanhMuc == duLieuGuiDi.SanPham.MaDanhMuc))
                {
                    return JsonMessage(false, "Vui lòng chọn danh mục cho sản phẩm !");
                }
                SanPham sp = new SanPham()
                {
                    TenSanPham = duLieuGuiDi.SanPham.TenSanPham,
                    MaDanhMuc = duLieuGuiDi.SanPham.MaDanhMuc,
                    GiaGoc = duLieuGuiDi.SanPham.GiaGoc,
                    KhuyenMai = duLieuGuiDi.SanPham.KhuyenMai,
                    MoTaNgan = duLieuGuiDi.SanPham.MoTaNgan,
                    MoTaDai = duLieuGuiDi.SanPham.MoTaDai,
                    HinhAnh = string.Join(",", dsHinhAnh),
                    NgayTao = DateTime.Now,
                    NgayCapNhat = null,
                    NguoiTao = duLieuGuiDi.SanPham.MaNguoiTao,
                    DaDuyet = false,
                    NgayDuyet = null
                };

                e.SanPhams.Add(sp);
                e.SaveChanges();

                if (duLieuGuiDi.DanhSachNguyenLieu != null && duLieuGuiDi.DanhSachNguyenLieu.Any())
                {
                    foreach (FastFood_SanPham_NguyenLieuDaChon nguyenLieu in duLieuGuiDi.DanhSachNguyenLieu)
                    {
                        SanPham_NguyenLieu sanPhamNguyenLieu = new SanPham_NguyenLieu()
                        {
                            MaSanPham = sp.MaSanPham,
                            MaNguyenLieu = Convert.ToInt32(nguyenLieu.MaNguyenLieu),
                            SoLuongCan = nguyenLieu.SoLuong,
                            DonViTinh = nguyenLieu.DonViTinh
                        };

                        e.SanPham_NguyenLieu.Add(sanPhamNguyenLieu);
                    }

                    e.SaveChanges();
                }
                else
                {
                    return JsonMessage(false, "Chọn ít nhất 1 nguyên liệu !");
                }

                return JsonMessage(true, "Thêm sản phẩm thành công!");
            }
        }
        /// <summary>
        /// Hiển thị chi tiết sản phẩm.
        /// </summary>
        /// <param name="id">Mã sản phẩm.</param>
        /// <param name="return_url">URL quay lại.</param>
        /// <returns>Trả về chi tiết sản phẩm.</returns>
        [HttpGet]
        public ActionResult Detail(int id, string return_url)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                SanPham sp = e.SanPhams.FirstOrDefault(x => x.MaSanPham == id);
                if (sp == null)
                    return HttpNotFound();
                string trangThaiDuyet = string.Empty;
                if (sp.DaDuyet)
                    trangThaiDuyet = "Đã duyệt";
                else
                    trangThaiDuyet = "Chưa duyệt";
                FastFood_SanPham_ChiTietSanPham a = new FastFood_SanPham_ChiTietSanPham
                {
                    MaSanPham = sp.MaSanPham,
                    TenSanPham = sp.TenSanPham,
                    GiaGoc = sp.GiaGoc,
                    KhuyenMai = Convert.ToInt32(sp.KhuyenMai),
                    GiaSauKhuyenMai = Convert.ToInt32(sp.GiaSauKhuyenMai),
                    MoTaNgan = sp.MoTaNgan,
                    MoTaDai = sp.MoTaDai,
                    NgayTao = (DateTime)sp.NgayTao,
                    MaDanhMuc = sp.DanhMuc.MaDanhMuc,
                    NguoiTao = FastFood_NhanVien.getHoTen(sp.NguoiTao.ToString()),
                    MaNguoiTao = Convert.ToInt32(Session["MaNhanVien"]),
                    TrangThaiDuyet = trangThaiDuyet,
                    NguoiDuyet = sp.NhanVien1.HoDem + " " + sp.NhanVien1.TenNhanVien,
                    NgayDuyet = sp.NgayDuyet,
                    DanhMuc = new SelectList(FastFood_DanhMuc.getDanhMuc()
                                                .Select(p => new { p.MaDanhMuc, p.TenDanhMuc })
                                                .ToList(), "MaDanhMuc", "TenDanhMuc")
                };
                ViewBag.MaSanPham = sp.MaSanPham;
                ViewBag.ReturnURL = return_url;
                if (!string.IsNullOrEmpty(sp.HinhAnh))
                {
                    ViewBag.HinhAnh = FastFood_Tools.SplitAnh(sp.HinhAnh);
                }
                else
                    ViewBag.HinhAnh = new string[] { };

                return View(a);
            }

        }
        /// <summary>
        /// Cập nhật thông tin chi tiết sản phẩm.
        /// </summary>
        /// <param name="duLieuGuiDi">Dữ liệu gửi từ form.</param>
        /// <returns>Trả về kết quả thực thi.</returns>
        [HttpPost]
        public ActionResult Detail(FastFood_SanPham_ChiTietSanPham_Post duLieuGuiDi)
        {
            Tuple<List<string>, ActionResult> isValidFiles = uploadFile(duLieuGuiDi.HinhAnh);
            if (isValidFiles.Item2 != null)
                return isValidFiles.Item2;
            List<string> dsHinhAnh = isValidFiles.Item1;

            using (FastFoodEntities e = new FastFoodEntities())
            {
                int maSanPham = duLieuGuiDi.SanPham.MaSanPham;
                SanPham sp = e.SanPhams.FirstOrDefault(m => m.MaSanPham == maSanPham);

                if (sp == null)
                    return HttpNotFound();

                if (duLieuGuiDi.SanPham.TenSanPham.Length > 100)
                    return JsonMessage(false, "Tên sản phẩm không được vượt quá 100 kí tự !");


                if (duLieuGuiDi.SanPham.MoTaNgan.Length > 100)
                    return JsonMessage(false, "Mô tả ngắn không được vượt quá 100 kí tự !");

                if (duLieuGuiDi.SanPham.MoTaDai.Length > 255)
                    return JsonMessage(false, "Mô tả dài không được vượt quá 255 kí tự !");

                sp.TenSanPham = duLieuGuiDi.SanPham.TenSanPham;
                sp.MoTaNgan = duLieuGuiDi.SanPham.MoTaNgan;
                sp.MoTaDai = duLieuGuiDi.SanPham.MoTaDai;
                sp.NgayCapNhat = duLieuGuiDi.SanPham.NgayCapNhat;
                sp.GiaGoc = duLieuGuiDi.SanPham.GiaGoc;
                sp.KhuyenMai = duLieuGuiDi.SanPham.KhuyenMai;
                sp.DaDuyet = false;
                sp.NguoiDuyet = null;
                sp.NgayDuyet = null;
                sp.HinhAnh = string.Join(",", dsHinhAnh);

                if (duLieuGuiDi.DanhSachNguyenLieu == null || !duLieuGuiDi.DanhSachNguyenLieu.Any())
                    return JsonMessage(false, "Vui lòng chọn ít nhất 1 nguyên liệu !");

                List<SanPham_NguyenLieu> nguyenLieuHienCoTrongDb = e.SanPham_NguyenLieu.Where(x => x.MaSanPham == maSanPham).ToList();
                List<int> maNguyenLieuTrongDanhSach = duLieuGuiDi.DanhSachNguyenLieu.Select(x => Convert.ToInt32(x.MaNguyenLieu)).ToList();

                foreach (SanPham_NguyenLieu nlDb in nguyenLieuHienCoTrongDb)
                {
                    if (!maNguyenLieuTrongDanhSach.Contains(nlDb.MaNguyenLieu))
                    {
                        e.SanPham_NguyenLieu.Remove(nlDb);
                    }
                }

                foreach (FastFood_SanPham_NguyenLieuDaChon nguyenLieu in duLieuGuiDi.DanhSachNguyenLieu)
                {
                    int maNguyenLieu = Convert.ToInt32(nguyenLieu.MaNguyenLieu);
                    SanPham_NguyenLieu existingIngredient = nguyenLieuHienCoTrongDb.Find(x => x.MaNguyenLieu == maNguyenLieu);

                    if (existingIngredient != null)
                    {
                        if (existingIngredient.SoLuongCan != nguyenLieu.SoLuong || existingIngredient.DonViTinh != nguyenLieu.DonViTinh)
                        {
                            existingIngredient.SoLuongCan = nguyenLieu.SoLuong;
                            existingIngredient.DonViTinh = nguyenLieu.DonViTinh;
                        }
                    }
                    else
                    {
                        SanPham_NguyenLieu nl = new SanPham_NguyenLieu()
                        {
                            MaNguyenLieu = maNguyenLieu,
                            DonViTinh = nguyenLieu.DonViTinh,
                            MaSanPham = maSanPham,
                            SoLuongCan = nguyenLieu.SoLuong
                        };
                        e.SanPham_NguyenLieu.Add(nl);
                    }
                }

                e.SaveChanges();
            }
            return JsonMessage(true, "Sửa đổi thành công !");
        }
        /// <summary>
        /// Xóa một sản phẩm dựa trên ID sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần xóa.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int productId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                SanPham sp = e.SanPhams.FirstOrDefault(x => x.MaSanPham == productId);
                if (sp == null)
                    return HttpNotFound();

                if (sp.ChiTietDonHangs.Any(x => x.MaSanPham == productId))
                    return JsonMessage(false, "Sản phẩm này đã có đơn hàng. Không thể xóa !");

                if (sp.SanPham_NguyenLieu.Any(x => x.MaSanPham == productId))
                {
                    IQueryable<SanPham_NguyenLieu> nguyenLieuLienQuan = e.SanPham_NguyenLieu.Where(x => x.MaSanPham == productId);
                    e.SanPham_NguyenLieu.RemoveRange(nguyenLieuLienQuan);
                }
                string filePath = Server.MapPath(sp.HinhAnh);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                e.SanPhams.Remove(sp);
                e.SaveChanges();
            }
            return JsonMessage(true, $"Xóa sản phẩm {productId} thành công !");
        }
        /// <summary>
        /// Xóa nhiều sản phẩm cùng lúc dựa trên danh sách các ID sản phẩm.
        /// </summary>
        /// <param name="productIds">Danh sách các ID sản phẩm cần xóa.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(IEnumerable<int> productIds)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                List<int> failedDeletes = new List<int>();
                int totalCount = productIds.Count();
                foreach (int productId in productIds)
                {
                    SanPham sp = e.SanPhams.FirstOrDefault(x => x.MaSanPham == productId);
                    if (sp != null)
                    {
                        if (sp.ChiTietDonHangs.Any(x => x.MaSanPham == productId))
                        {
                            failedDeletes.Add(productId);
                            continue;
                        }
                        if (sp.SanPham_NguyenLieu.Any(x => x.MaSanPham == productId))
                        {
                            List<SanPham_NguyenLieu> nguyenLieuLienQuan = e.SanPham_NguyenLieu.Where(x => x.MaSanPham == productId).ToList();
                            e.SanPham_NguyenLieu.RemoveRange(nguyenLieuLienQuan);
                        }
                        string filePath = Server.MapPath(sp.HinhAnh);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        e.SanPhams.Remove(sp);
                    }
                }

                e.SaveChanges();

                if (failedDeletes.Count > 0)
                    return Json(new { success = false, type = "var(--bs-danger)", message = "Một số sản phẩm không thể xóa do đã có đơn hàng.", failedIds = failedDeletes });
                return Json(new { success = true, type = "var(--bs-success)", message = $"{totalCount - failedDeletes.Count}/{totalCount} sản phẩm được xóa thành công!" });
            }
        }
        /// <summary>
        /// Hiển thị sản phẩm chưa duyệt.
        /// </summary>
        /// <param name="page">Trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng sản phẩm trên mỗi trang (mặc định là 10).</param>
        /// <returns>Trả về danh sách sản phẩm chưa duyệt.</returns>
        [HttpGet]
        public ActionResult Approve(int page = 1, int size = 10)
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 1))
                return HttpNotFound();

            X.PagedList.IPagedList<SanPham> sanPham = FastFood_SanPham.getSanPhamChuaDuyet().OrderBy(m => m.DaDuyet).ToPagedList(page, size);
            ViewBag.SanPham = sanPham;
            ViewBag.CurrentPage = sanPham.PageNumber;
            ViewBag.TotalPages = sanPham.PageCount;
            ViewBag.Title = "Phê duyệt sản phẩm";
            return View();
        }
        /// <summary>
        /// Phê duyệt hoặc từ chối một sản phẩm theo ID.
        /// </summary>
        /// <param name="productId">ID của sản phẩm cần phê duyệt hoặc từ chối.</param>
        /// <param name="employeeId">ID của nhân viên thực hiện phê duyệt.</param>
        /// <param name="action">Hành động phê duyệt hoặc từ chối.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int productId, int employeeId, string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                SanPham a = e.SanPhams.FirstOrDefault(x => x.MaSanPham == productId);

                if (a == null)
                    return HttpNotFound();
                switch (action)
                {
                    case "accept":
                        a.NguoiDuyet = employeeId;
                        a.NgayDuyet = DateTime.Now;
                        a.DaDuyet = true;
                        e.SaveChanges();
                        return JsonMessage(true, $"Đã phê duyệt sản phẩm {productId}!");

                    case "refuse":
                        IEnumerable<SanPham_NguyenLieu> ingredients = a.SanPham_NguyenLieu.Where(x => x.MaSanPham == productId);
                        e.SanPham_NguyenLieu.RemoveRange(ingredients);
                        string filePath = Server.MapPath(a.HinhAnh);
                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                        e.SanPhams.Remove(a);
                        e.SaveChanges();
                        return JsonMessage(true, $"Đã từ chối và xóa sản phẩm {productId} khỏi hệ thống!");
                }
            }
            return Json(new { });
        }
        /// <summary>
        /// Phê duyệt hoặc từ chối nhiều sản phẩm cùng lúc.
        /// </summary>
        /// <param name="productIds">Danh sách các ID sản phẩm cần phê duyệt hoặc từ chối.</param>
        /// <param name="employeeId">ID của nhân viên thực hiện phê duyệt.</param>
        /// <param name="action">Hành động phê duyệt hoặc từ chối.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost]
        public ActionResult ApproveMultiple(IEnumerable<int> productIds, int employeeId, string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                foreach (int productId in productIds)
                {
                    SanPham product = e.SanPhams.FirstOrDefault(x => x.MaSanPham == productId);
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
                                List<SanPham_NguyenLieu> ingredients = e.SanPham_NguyenLieu.Where(x => x.MaSanPham == productId).ToList();
                                e.SanPham_NguyenLieu.RemoveRange(ingredients);
                                string filePath = Server.MapPath(product.HinhAnh);
                                if (System.IO.File.Exists(filePath))
                                    System.IO.File.Delete(filePath);
                                e.SanPhams.Remove(product);
                                break;

                        }
                    }
                }
                e.SaveChanges();

                if (action == "accept")
                    return JsonMessage(true, "Đã phê duyệt các sản phẩm được chọn !");
                if (action == "refuse")
                    return JsonMessage(true, "Đã từ chối và xóa các sản phẩm được chọn !");
            }
            return JsonMessage(true, "");
        }
        /// <summary>
        /// Xóa một hình ảnh khỏi sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm chứa hình ảnh cần xóa.</param>
        /// <param name="imagePath">Đường dẫn hình ảnh cần xóa.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        public ActionResult RemoveImage(int productId, string imagePath)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                SanPham sp = e.SanPhams.Where(x => x.MaSanPham == productId).FirstOrDefault();
                if (sp == null)
                    return HttpNotFound();
                string[] dsHinhAnh = FastFood_Tools.SplitAnh(sp.HinhAnh);
                string[] dsAnhMoi = dsHinhAnh.Where(x => !x.Equals(imagePath)).ToArray();
                string result = string.Join(",", dsAnhMoi);
                sp.HinhAnh = result;
                e.SaveChanges();
                return JsonMessage(true, "Xóa ảnh thành công !");
            }
        }
        /// <summary>
        /// Xử lý tải lên các tệp hình ảnh cho sản phẩm.
        /// </summary>
        /// <param name="hinhAnh">Danh sách các tệp hình ảnh cần tải lên.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        private Tuple<List<string>, ActionResult> uploadFile(IEnumerable<HttpPostedFileBase> hinhAnh)
        {

            List<string> dsHinhAnh = new List<string>();

            if (hinhAnh == null || !hinhAnh.Any())
                return Tuple.Create<List<string>, ActionResult>(dsHinhAnh, null);

            if (hinhAnh.Count() > 5)
                return Tuple.Create<List<string>, ActionResult>(null, JsonMessage(false, "Chỉ được phép tải lên tối đa 5 tệp ảnh !"));

            byte i = 1;
            int total = hinhAnh.Count();

            foreach (HttpPostedFileBase file in hinhAnh)
            {
                if (file == null)
                    return Tuple.Create<List<string>, ActionResult>(null, JsonMessage(false, $"Tệp {i}/{total} bị lỗi !"));

                if (file.ContentLength == 0 || file.ContentLength > 10000000)
                    return Tuple.Create<List<string>, ActionResult>(null, JsonMessage(false, $"Kích thước tệp {i}/{total} phải khác 0 & <= 10MB !"));

                if (!file.ContentType.Contains("image/"))
                    return Tuple.Create<List<string>, ActionResult>(null, JsonMessage(false, $"Tệp {i}/{total} phải là định dạng ảnh !"));

                string path = "~/Content/admin_page/uploads/products";
                string newFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                string savePath = Path.Combine(Server.MapPath(path), newFileName);
                file.SaveAs(savePath);
                string dbPath = Path.Combine(path, newFileName).Replace("\\", "/").Replace("~", "");
                dsHinhAnh.Add(dbPath);
                i++;
            }
            return Tuple.Create<List<string>, ActionResult>(dsHinhAnh, null);
        }


    }

}