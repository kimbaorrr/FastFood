using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace FastFood.Areas.Admin.Controllers
{
    [Route("admin/nguyen-lieu")]
    public class IngredientController : SessionController
    {
        /// <summary>
        /// Lấy danh sách tất cả nguyên liệu.
        /// </summary>
        /// <returns>Danh sách nguyên liệu dưới dạng JSON</returns>
        [HttpGet("danh-sach-nguyen-lieu")]
        public string Get()
        {
            return JsonConvert.SerializeObject(FastFood_NguyenLieu.getAll().Select(x => new { MaNguyenLieu = x.MaNguyenLieu, TenNguyenLieu = x.TenNguyenLieu, MoTa = x.MoTa }));
        }
        /// <summary>
        /// Lấy danh sách nguyên liệu theo mã sản phẩm.
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Danh sách nguyên liệu liên quan đến sản phẩm dưới dạng JSON</returns>
        [HttpGet("nguyen-lieu-cua-san-pham")]
        public string GetByProductID([FromQuery] int productId)
        {
            return JsonConvert.SerializeObject(FastFood_NguyenLieu.getByProductID(productId));
        }
        /// <summary>
        /// Lấy danh sách phiếu nhập kho của nguyên liệu.
        /// </summary>
        /// <returns>Danh sách phiếu nhập kho dưới dạng JSON</returns>
        [HttpGet("danh-sach-phieu-nhap")]
        public string GetListReceipt()
        {
            return JsonConvert.SerializeObject(FastFood_NguyenLieu.getNhapKho().AsEnumerable().OrderByDescending(x => x.Ngaynhap)
                .Select(x => new
                {
                    MaNhapKho = x.Manhapkho,
                    MaNguyenLieu = x.Manguyenlieu,
                    TenNguyenLieu = x.ManguyenlieuNavigation.Tennguyenlieu ?? string.Empty,
                    NguoiNhap = x.NguoinhapNavigation.Hodem + " " + x.NguoinhapNavigation.Tennhanvien,
                    SoLuongNhap = x.Soluongnhap,
                    NgayNhap = x.Ngaynhap.ToString("dd/MM/yyyy HH:mm:ss")
                }));
        }
        [HttpGet("danh-sach-ton-kho")]
        public string GetListInventory()
        {
            return JsonConvert.SerializeObject(FastFood_NguyenLieu.getNguyenLieu().AsEnumerable().Select(
                x => new
                {
                    MaNguyenLieu = x.Manguyenlieu,
                    TenNguyenLieu = x.Tennguyenlieu,
                    SoLuongTon = x.Soluongtonkho,
                    MucDatHangLai = x.Mucdathanglai,
                    DatGioiHan = x.Soluongtonkho <= x.Mucdathanglai
                }
                ));
        }
        /// <summary>
        /// Hiển thị trang nhập kho nguyên liệu.
        /// </summary>
        /// <returns>Trang nhập kho nguyên liệu</returns>s
        [HttpGet("nhap-kho")]
        public IActionResult WareHouse()
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(this.MaNhanVien, 4))
                return BadRequest();

            FastFood_NguyenLieu_ThemPhieuNhap a = new FastFood_NguyenLieu_ThemPhieuNhap()
            {
                NguoiNhap = FastFood_NhanVien.getHoTen(this.MaNhanVien)
            };
            ViewBag.NguyenLieu = new SelectList(FastFood_NguyenLieu.getNguyenLieu()
                                            .Select(p => new SelectListItem
                                            {
                                                Text = p.Tennguyenlieu,
                                                Value = p.Manguyenlieu.ToString()
                                            }).ToList(), "Value", "Text");
            return View(a);
        }
        /// <summary>
        /// Xử lý thêm phiếu nhập kho nguyên liệu.
        /// </summary>
        /// <param name="a">Đối tượng phiếu nhập kho chứa thông tin nhập kho</param>
        /// <returns>Thông báo kết quả thêm phiếu nhập kho</returns>
        [ValidateAntiForgeryToken]
        [HttpPost("nhap-kho")]
        public string WareHouse(FastFood_NguyenLieu_ThemPhieuNhap a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (a.MaNguyenLieu == 0 && !string.IsNullOrEmpty(a.TenNguyenLieu))
                {
                    Nguyenlieu nlm = new Nguyenlieu()
                    {
                        Tennguyenlieu = a.TenNguyenLieu,
                        Donvitinh = a.DonViTinh,
                        Mucdathanglai = a.MucDatHangLai,
                        Mota = a.MoTa
                    };

                    e.Nguyenlieus.Add(nlm);
                    e.SaveChanges();

                    a.MaNguyenLieu = nlm.Manguyenlieu;
                }

                Nguyenlieu? nl = e.Nguyenlieus.FirstOrDefault(x => x.Manguyenlieu == a.MaNguyenLieu);

                if (nl == null)
                    return JsonMessage(message: "ID không hợp lệ !");

                nl.Soluongtonkho = nl.Soluongtonkho + a.SoLuongNhap;

                Nhapkho nk = new Nhapkho()
                {
                    Manguyenlieu = a.MaNguyenLieu,
                    Nguoinhap = Convert.ToInt32(this.MaNhanVien),
                    Soluongnhap = a.SoLuongNhap,
                    Ngaynhap = a.NgayNhap
                };

                e.Nhapkhos.Add(nk);
                e.SaveChanges();

                return JsonMessage(true, "Thêm phiếu nhập kho thành công !");
            }

        }




    }
}