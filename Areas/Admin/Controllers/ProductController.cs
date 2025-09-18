using System.Collections.Immutable;
using FastFood.DB;
using FastFood.Models;
using FastFood.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;
namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/san-pham")]
    public class ProductController : SessionController
    {
        private readonly IWebHostEnvironment _environment;
        private string RootPath => _environment.WebRootPath;
        private string ProductsPath => Path.Combine(RootPath, "admin_page/uploads/products/");
        private string VirtualPath => "/admin_page/uploads/products/";
        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// Lấy danh sách tất cả sản phẩm.
        /// </summary>
        /// <returns>Trả về danh sách sản phẩm dưới dạng JSON.</returns>
        [HttpPost("danh-sach-san-pham")]
        public IActionResult GetAllProducts()
        {
            return Json(FastFood_SanPham.getSanPham());
        }
        /// <summary>
        /// Hiển thị danh sách sản phẩm đã duyệt, phân trang.
        /// </summary>
        /// <param name="page">Số trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng sản phẩm trên mỗi trang (mặc định là 10).</param>
        /// <returns>Trả về trang danh sách sản phẩm đã duyệt.</returns>
        [HttpGet("san-pham-da-duyet")]
        public IActionResult List([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            IPagedList<Sanpham> sanPham = FastFood_SanPham.getSanPhamDaDuyet().OrderBy(m => m.Masanpham).ToPagedList(page, size);
            ViewBag.SanPham = sanPham;
            ViewBag.CurrentPage = sanPham.PageNumber;
            ViewBag.TotalPages = sanPham.PageCount;
            return View();

        }
        /// <summary>
        /// Hiển thị form tạo mới sản phẩm.
        /// </summary>
        /// <returns>Trả về form tạo sản phẩm.</returns>
        [HttpGet("them-san-pham")]
        public IActionResult Create()
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(this.MaNhanVien, 6))
                return BadRequest();

            FastFood_SanPham_ThemSanPham sanPham = new FastFood_SanPham_ThemSanPham()
            {
                NguoiTao = FastFood_NhanVien.getHoTen(this.MaNhanVien),
                DanhMuc = new SelectList(FastFood_DanhMuc.getDanhMuc()
                                                    .Select(p => new SelectListItem
                                                    {
                                                        Text = p.Tendanhmuc,
                                                        Value = p.Madanhmuc.ToString()
                                                    }).ToList(), "Value", "Text")
            };
            return View(sanPham);
        }
        /// <summary>
        /// Xử lý lưu trữ dữ liệu sản phẩm mới.
        /// </summary>
        /// <param name="duLieuGuiDi">Dữ liệu gửi lên từ form.</param>
        /// <returns>Trả về kết quả thực thi (thành công hay lỗi).</returns>
        [HttpPost("them-san-pham")]
        [ValidateAntiForgeryToken]
        public string Create([FromForm] FastFood_SanPham_ThemSanPham_Post duLieuGuiDi)
        {
            Tuple<List<string>, string> isValidFiles = uploadFile(duLieuGuiDi.HinhAnh);

            if (isValidFiles.Item1 == null)
                return isValidFiles.Item2;

            List<string> dsHinhAnh = isValidFiles.Item1;

            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (e.Sanphams.Any(x => x.Tensanpham.Contains(duLieuGuiDi.SanPham.TenSanPham)))
                {
                    return JsonMessage(message: "Sản phẩm đã tồn tại trên hệ thống !");
                }

                if (!e.Danhmucs.Any(x => x.Madanhmuc == duLieuGuiDi.SanPham.MaDanhMuc))
                {
                    return JsonMessage(message: "Vui lòng chọn danh mục cho sản phẩm !");
                }

                Sanpham sp = new Sanpham()
                {
                    Tensanpham = duLieuGuiDi.SanPham.TenSanPham,
                    Madanhmuc = duLieuGuiDi.SanPham.MaDanhMuc,
                    Giagoc = duLieuGuiDi.SanPham.GiaGoc,
                    Khuyenmai = duLieuGuiDi.SanPham.KhuyenMai,
                    Motangan = duLieuGuiDi.SanPham.MoTaNgan,
                    Motadai = duLieuGuiDi.SanPham.MoTaDai,
                    Hinhanh = string.Join(",", dsHinhAnh),
                    Ngaytao = DateTime.Now,
                    Ngaycapnhat = null,
                    Nguoitao = Convert.ToInt32(this.MaNhanVien),
                    Daduyet = false,
                    Ngayduyet = null
                };

                e.Sanphams.Add(sp);
                e.SaveChanges();

                if (duLieuGuiDi.DanhSachNguyenLieu != null && duLieuGuiDi.DanhSachNguyenLieu.Any())
                {
                    foreach (FastFood_SanPham_NguyenLieuDaChon nguyenLieu in duLieuGuiDi.DanhSachNguyenLieu)
                    {
                        SanphamNguyenlieu sanPhamNguyenLieu = new SanphamNguyenlieu()
                        {
                            Masanpham = sp.Masanpham,
                            Manguyenlieu = Convert.ToInt32(nguyenLieu.MaNguyenLieu),
                            Soluongcan = nguyenLieu.SoLuong,
                            Donvitinh = nguyenLieu.DonViTinh
                        };

                        e.SanphamNguyenlieus.Add(sanPhamNguyenLieu);
                    }
                    e.SaveChanges();
                }
                else
                {
                    return JsonMessage(message: "Chọn ít nhất 1 nguyên liệu !");
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
        [HttpGet("chi-tiet-san-pham/{id}")]
        public IActionResult Detail(int id)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Sanpham? sp = e.Sanphams.FirstOrDefault(x => x.Masanpham == id);
                if (sp == null)
                    return BadRequest();

                string trangThaiDuyet = sp.Daduyet ? "Đã duyệt" : "Chưa duyệt";

                FastFood_SanPham_ChiTietSanPham a = new FastFood_SanPham_ChiTietSanPham
                {
                    MaSanPham = sp.Masanpham,
                    TenSanPham = sp.Tensanpham,
                    GiaGoc = sp.Giagoc,
                    KhuyenMai = sp.Khuyenmai ?? 0,
                    GiaSauKhuyenMai = sp.Giasaukhuyenmai ?? 0,
                    MoTaNgan = sp.Motangan ?? string.Empty,
                    MoTaDai = sp.Motadai ?? string.Empty,
                    NgayTao = sp.Ngaytao,
                    MaDanhMuc = sp.MadanhmucNavigation?.Madanhmuc ?? 0,
                    NguoiTao = sp.Nguoitao.HasValue ? FastFood_NhanVien.getHoTen(sp.Nguoitao.Value.ToString()) : string.Empty,
                    TrangThaiDuyet = trangThaiDuyet,
                    NguoiDuyet = sp.NguoiduyetNavigation != null ? sp.NguoiduyetNavigation.Hodem + " " + sp.NguoiduyetNavigation.Tennhanvien : string.Empty,
                    NgayDuyet = sp.Ngayduyet,
                    DanhMuc = new SelectList(FastFood_DanhMuc.getDanhMuc()
                                                .Select(p => new SelectListItem
                                                {
                                                    Text = p.Tendanhmuc,
                                                    Value = p.Madanhmuc.ToString()
                                                }).ToList(), "Value", "Text")
                };
                ViewBag.MaSanPham = sp.Masanpham.ToString();
                ViewBag.ReturnURL = GetAbsoluteUri();
                ViewBag.HinhAnh = FastFood_Tools.SplitAnh(sp.Hinhanh!);


                return View(a);
            }


        }
        /// <summary>
        /// Cập nhật thông tin chi tiết sản phẩm.
        /// </summary>
        /// <param name="duLieuGuiDi">Dữ liệu gửi từ form.</param>
        /// <returns>Trả về kết quả thực thi.</returns>
        [HttpPost("chi-tiet-san-pham/{id}")]
        public string Detail([FromForm] FastFood_SanPham_ChiTietSanPham_Post duLieuGuiDi)
        {
            List<string> dsHinhAnh = new List<string>();
            if (duLieuGuiDi.HinhAnh != null)
            {
                Tuple<List<string>, string> isValidFiles = uploadFile(duLieuGuiDi.HinhAnh);

                if (isValidFiles.Item2 != null)
                    return isValidFiles.Item2;

                dsHinhAnh = isValidFiles.Item1;
            }

            using (FastFoodEntities e = new FastFoodEntities())
            {
                int maSanPham = duLieuGuiDi.SanPham.MaSanPham;
                Sanpham? sp = e.Sanphams
                    .Include(x => x.SanphamNguyenlieus)
                    .FirstOrDefault(m => m.Masanpham == maSanPham);

                if (sp == null)
                    return JsonMessage(message: "ID không tồn tại !");

                if (duLieuGuiDi.SanPham.TenSanPham.Length > 100)
                    return JsonMessage(message: "Tên sản phẩm không được vượt quá 100 kí tự !");


                if (duLieuGuiDi.SanPham.MoTaNgan.Length > 100)
                    return JsonMessage(message: "Mô tả ngắn không được vượt quá 100 kí tự !");

                if (duLieuGuiDi.SanPham.MoTaDai.Length > 255)
                    return JsonMessage(message: "Mô tả dài không được vượt quá 255 kí tự !");

                sp.Tensanpham = duLieuGuiDi.SanPham.TenSanPham;
                sp.Motangan = duLieuGuiDi.SanPham.MoTaNgan;
                sp.Motadai = duLieuGuiDi.SanPham.MoTaDai;
                sp.Ngaycapnhat = duLieuGuiDi.SanPham.NgayCapNhat;
                sp.Giagoc = duLieuGuiDi.SanPham.GiaGoc;
                sp.Khuyenmai = duLieuGuiDi.SanPham.KhuyenMai;
                sp.Daduyet = false;
                sp.Nguoiduyet = null;
                sp.Ngayduyet = null;
                if (dsHinhAnh.Count > 0)
                {
                    sp.Hinhanh = string.IsNullOrEmpty(sp.Hinhanh)
                        ? string.Join(",", dsHinhAnh)
                        : string.Join(",", sp.Hinhanh, string.Join(",", dsHinhAnh));
                }

                if (duLieuGuiDi.DanhSachNguyenLieu == null || !duLieuGuiDi.DanhSachNguyenLieu.Any())
                    return JsonMessage(message: "Vui lòng chọn ít nhất 1 nguyên liệu !");

                List<SanphamNguyenlieu> nguyenLieuHienCoTrongDb = e.SanphamNguyenlieus.Where(x => x.Masanpham == maSanPham).ToList();
                List<int> maNguyenLieuTrongDanhSach = duLieuGuiDi.DanhSachNguyenLieu.Select(x => Convert.ToInt32(x.MaNguyenLieu)).ToList();

                foreach (SanphamNguyenlieu nlDb in nguyenLieuHienCoTrongDb)
                {
                    if (!maNguyenLieuTrongDanhSach.Contains(nlDb.Manguyenlieu))
                    {
                        e.SanphamNguyenlieus.Remove(nlDb);
                    }
                }

                foreach (FastFood_SanPham_NguyenLieuDaChon nguyenLieu in duLieuGuiDi.DanhSachNguyenLieu)
                {
                    int maNguyenLieu = Convert.ToInt32(nguyenLieu.MaNguyenLieu);
                    SanphamNguyenlieu? existingIngredient = nguyenLieuHienCoTrongDb.Find(x => x.Manguyenlieu == maNguyenLieu);

                    if (existingIngredient != null)
                    {
                        if (existingIngredient.Soluongcan != nguyenLieu.SoLuong || existingIngredient.Donvitinh != nguyenLieu.DonViTinh)
                        {
                            existingIngredient.Soluongcan = nguyenLieu.SoLuong;
                            existingIngredient.Donvitinh = nguyenLieu.DonViTinh;
                        }
                    }
                    else
                    {
                        SanphamNguyenlieu nl = new SanphamNguyenlieu()
                        {
                            Manguyenlieu = maNguyenLieu,
                            Donvitinh = nguyenLieu.DonViTinh,
                            Masanpham = maSanPham,
                            Soluongcan = nguyenLieu.SoLuong
                        };
                        e.SanphamNguyenlieus.Add(nl);
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
        [HttpPost("xoa-san-pham")]
        [ValidateAntiForgeryToken]
        public string Delete([FromForm] int productId)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Sanpham? sp = e.Sanphams
                    .Include(x => x.Chitietdonhangs)
                    .Include(x => x.SanphamNguyenlieus)
                    .FirstOrDefault(m => m.Masanpham == productId);

                if (sp == null)
                    return JsonMessage(message: "ID không tồn tại !");

                if (sp.Chitietdonhangs.Any(x => x.Masanpham == productId))
                    return JsonMessage(message: "Sản phẩm này đã có đơn hàng. Không thể xóa !");

                if (sp.SanphamNguyenlieus.Any(x => x.Masanpham == productId))
                {
                    IQueryable<SanphamNguyenlieu> nguyenLieuLienQuan = e.SanphamNguyenlieus.Where(x => x.Masanpham == productId);
                    e.SanphamNguyenlieus.RemoveRange(nguyenLieuLienQuan);
                }

                FastFood_Tools.DeleteFile(sp.Hinhanh!);

                e.Sanphams.Remove(sp);
                e.SaveChanges();
            }
            return JsonMessage(true, $"Xóa sản phẩm {productId} thành công !");



        }
        /// <summary>
        /// Xóa nhiều sản phẩm cùng lúc dựa trên danh sách các ID sản phẩm.
        /// </summary>
        /// <param name="productIds">Danh sách các ID sản phẩm cần xóa.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost("xoa-nhieu-san-pham")]
        [ValidateAntiForgeryToken]
        public string DeleteMultiple([FromForm] IEnumerable<int> productIds)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                List<int> failedDeletes = new List<int>();
                int totalCount = productIds.Count();
                foreach (int productId in productIds)
                {
                    Sanpham? sp = e.Sanphams
                        .Include(x => x.Chitietdonhangs)
                        .Include(x => x.SanphamNguyenlieus)
                        .FirstOrDefault(m => m.Masanpham == productId);

                    if (sp != null)
                    {
                        if (sp.Chitietdonhangs.Any(x => x.Masanpham == productId))
                        {
                            failedDeletes.Add(productId);
                            continue;
                        }
                        if (sp.SanphamNguyenlieus.Any(x => x.Masanpham == productId))
                        {
                            List<SanphamNguyenlieu> nguyenLieuLienQuan = e.SanphamNguyenlieus.Where(x => x.Masanpham == productId).ToList();
                            e.SanphamNguyenlieus.RemoveRange(nguyenLieuLienQuan);
                        }

                        FastFood_Tools.DeleteFile(sp.Hinhanh!);

                        e.Sanphams.Remove(sp);
                    }
                }

                e.SaveChanges();

                if (failedDeletes.Count > 0)
                    return JsonMessage(message: $"Một số sản phẩm không thể xóa do đã có đơn hàng. {failedDeletes}");

                return JsonMessage(message: $"{totalCount - failedDeletes.Count}/{totalCount} sản phẩm được xóa thành công!");
            }

        }
        /// <summary>
        /// Hiển thị sản phẩm chưa duyệt.
        /// </summary>
        /// <param name="page">Trang hiện tại (mặc định là 1).</param>
        /// <param name="size">Số lượng sản phẩm trên mỗi trang (mặc định là 10).</param>
        /// <returns>Trả về danh sách sản phẩm chưa duyệt.</returns>
        [HttpGet("phe-duyet-san-pham")]
        public IActionResult Approve([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(this.MaNhanVien, 1))
                return BadRequest();

            IPagedList<Sanpham> sanPham = FastFood_SanPham.getSanPhamChuaDuyet().OrderBy(m => m.Daduyet).ToPagedList(page, size);
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
        [HttpPost("phe-duyet-san-pham")]
        [ValidateAntiForgeryToken]
        public string Approve([FromForm] int productId, [FromForm] string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Sanpham? a = e.Sanphams
                        .Include(x => x.SanphamNguyenlieus)
                        .FirstOrDefault(m => m.Masanpham == productId);

                if (a == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                switch (action)
                {
                    case "accept":
                        a.Nguoiduyet = Convert.ToInt32(this.MaNhanVien);
                        a.Ngayduyet = DateTime.Now;
                        a.Daduyet = true;
                        break;

                    case "refuse":
                        IEnumerable<SanphamNguyenlieu> ingredients = a.SanphamNguyenlieus.Where(x => x.Masanpham == productId);
                        e.SanphamNguyenlieus.RemoveRange(ingredients);

                        if (System.IO.File.Exists(a.Hinhanh))
                            System.IO.File.Delete(a.Hinhanh);

                        e.Sanphams.Remove(a);
                        break;

                }
                e.SaveChanges();

                if (action == "accept")
                    return JsonMessage(true, $"Đã phê duyệt sản phẩm {a.Masanpham} !");

                if (action == "refuse")
                    return JsonMessage(true, $"Đã từ chối và xóa các sản phẩm {a.Masanpham} !");
            }
            return JsonMessage();
        }
        /// <summary>
        /// Phê duyệt hoặc từ chối nhiều sản phẩm cùng lúc.
        /// </summary>
        /// <param name="productIds">Danh sách các ID sản phẩm cần phê duyệt hoặc từ chối.</param>
        /// <param name="employeeId">ID của nhân viên thực hiện phê duyệt.</param>
        /// <param name="action">Hành động phê duyệt hoặc từ chối.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost("phe-duyet-nhieu-san-pham")]
        public string ApproveMultiple([FromForm] IEnumerable<int> productIds, [FromForm] string action)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                foreach (int productId in productIds)
                {
                    Sanpham? product = e.Sanphams
                            .Include(x => x.SanphamNguyenlieus)
                            .FirstOrDefault(m => m.Masanpham == productId);

                    if (product != null)
                    {
                        switch (action)
                        {
                            case "accept":
                                product.Nguoiduyet = Convert.ToInt32(this.MaNhanVien);
                                product.Ngayduyet = DateTime.Now;
                                product.Daduyet = true;
                                break;

                            case "refuse":
                                List<SanphamNguyenlieu> ingredients = e.SanphamNguyenlieus.Where(x => x.Masanpham == productId).ToList();
                                e.SanphamNguyenlieus.RemoveRange(ingredients);


                                if (System.IO.File.Exists(product.Hinhanh))
                                    System.IO.File.Delete(product.Hinhanh);

                                e.Sanphams.Remove(product);
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
            return JsonMessage();

        }
        /// <summary>
        /// Xóa một hình ảnh khỏi sản phẩm.
        /// </summary>
        /// <param name="productId">ID của sản phẩm chứa hình ảnh cần xóa.</param>
        /// <param name="imagePath">Đường dẫn hình ảnh cần xóa.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        [HttpPost("xoa-anh-san-pham")]
        public string RemoveImage([FromForm] int productId, [FromForm] string imagePath)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                Sanpham? sp = e.Sanphams.FirstOrDefault(m => m.Masanpham == productId);

                if (sp == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                string[] dsHinhAnh = FastFood_Tools.SplitAnh(sp.Hinhanh);
                string[] dsAnhMoi = dsHinhAnh.Where(x => !x.Equals(imagePath)).ToArray();
                string result = string.Join(",", dsAnhMoi);
                sp.Hinhanh = result;

                e.SaveChanges();
                return JsonMessage(true, "Xóa ảnh thành công !");
            }

        }
        /// <summary>
        /// Xử lý tải lên các tệp hình ảnh cho sản phẩm.
        /// </summary>
        /// <param name="hinhAnh">Danh sách các tệp hình ảnh cần tải lên.</param>
        /// <returns>Trả về kết quả dưới dạng JSON với thông báo thành công hoặc thất bại.</returns>
        private Tuple<List<string>, string> uploadFile(IEnumerable<IFormFile> hinhAnh)
        {

            List<string> dsHinhAnh = new List<string>();

            if (hinhAnh == null || !hinhAnh.Any())
                return Tuple.Create<List<string>, string>(null!, JsonMessage(message: "Chưa chọn tệp tải lên !"));

            if (hinhAnh.Count() > 5)
                return Tuple.Create<List<string>, string>(null!, JsonMessage(message: "Chỉ được phép tải lên tối đa 5 tệp ảnh !"));

            byte i = 1;
            int total = hinhAnh.Count();

            foreach (IFormFile file in hinhAnh)
            {
                if (file == null)
                    return Tuple.Create<List<string>, string>(null!, JsonMessage(message: $"Tệp {i}/{total} bị lỗi !"));

                if (file.Length == 0 || file.Length > 10000000)
                    return Tuple.Create<List<string>, string>(null!, JsonMessage(message: $"Kích thước tệp {i}/{total} phải khác 0 & <= 10MB !"));

                if (!file.ContentType.Contains("image/"))
                    return Tuple.Create<List<string>, string>(null!, JsonMessage(message: $"Tệp {i}/{total} phải là định dạng ảnh !"));

                string newFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(this.ProductsPath, newFileName);
                string productVirtualPath = Path.Combine(this.VirtualPath, newFileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                dsHinhAnh.Add(productVirtualPath);
                i++;
            }
            return Tuple.Create<List<string>, string>(dsHinhAnh, null);
        }
    }

}