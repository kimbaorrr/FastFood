using FastFood.DB;
using FastFood.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FastFood.Areas.Admin.Controllers
{
    public class IngredientController : SessionController
    {
        /// <summary>
        /// Lấy danh sách tất cả nguyên liệu.
        /// </summary>
        /// <returns>Danh sách nguyên liệu dưới dạng JSON</returns>
        [HttpGet]
        public ActionResult Get()
        {
            return Json(FastFood_NguyenLieu.getAll(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách nguyên liệu theo mã sản phẩm.
        /// </summary>
        /// <param name="productId">Mã sản phẩm</param>
        /// <returns>Danh sách nguyên liệu liên quan đến sản phẩm dưới dạng JSON</returns>
        [HttpGet]
        public ActionResult GetByProductID(int productId)
        {
            return Json(FastFood_NguyenLieu.getByProductID(productId), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách phiếu nhập kho của nguyên liệu.
        /// </summary>
        /// <returns>Danh sách phiếu nhập kho dưới dạng JSON</returns>
        [HttpGet]
        public ActionResult GetListReceipt()
        {
            return Json(FastFood_NguyenLieu.getNhapKho()
                .AsEnumerable()
                .Select(x => new { x.MaNhapKho, x.MaNguyenLieu, TenNguyenLieu = x.NguyenLieu.TenNguyenLieu ?? string.Empty, NguoiNhap = x.NhanVien.HoDem + " " + x.NhanVien.TenNhanVien, x.SoLuongNhap, NgayNhap = x.NgayNhap.ToString("dd/MM/yyyy HH:mm:ss") }),
                JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Hiển thị trang nhập kho nguyên liệu.
        /// </summary>
        /// <returns>Trang nhập kho nguyên liệu</returns>s
        [HttpGet]
        public ActionResult WareHouse()
        {
            if (!FastFood_NhanVienDangNhap.CheckPermission(Session["MaNhanVien"] as string, 4))
                return HttpNotFound();

            string maNV = Session["MaNhanVien"] as string;
            FastFood_NguyenLieu_ThemPhieuNhap a = new FastFood_NguyenLieu_ThemPhieuNhap()
            {
                NguoiNhap = FastFood_NhanVien.getHoTen(maNV),
                MaNguoiNhap = Convert.ToInt32(maNV)
            };
            ViewBag.NguyenLieu = new SelectList(FastFood_NguyenLieu.getNguyenLieu()
                                            .Select(p => new { p.MaNguyenLieu, p.TenNguyenLieu })
                                            .AsEnumerable(), "MaNguyenLieu", "TenNguyenLieu");
            return View(a);
        }
        /// <summary>
        /// Xử lý thêm phiếu nhập kho nguyên liệu.
        /// </summary>
        /// <param name="a">Đối tượng phiếu nhập kho chứa thông tin nhập kho</param>
        /// <returns>Thông báo kết quả thêm phiếu nhập kho</returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult WareHouse(FastFood_NguyenLieu_ThemPhieuNhap a)
        {
            using (FastFoodEntities e = new FastFoodEntities())
            {
                if (a.MaNguyenLieu == 0 && !string.IsNullOrEmpty(a.TenNguyenLieu))
                {
                    NguyenLieu nlm = new NguyenLieu()
                    {
                        TenNguyenLieu = a.TenNguyenLieu,
                        DonViTinh = a.DonViTinh,
                        MucDatHangLai = a.MucDatHangLai,
                        MoTa = a.MoTa
                    };
                    e.NguyenLieux.Add(nlm);
                    e.SaveChanges();
                    a.MaNguyenLieu = nlm.MaNguyenLieu;
                }
                NguyenLieu nl = e.NguyenLieux.FirstOrDefault(x => x.MaNguyenLieu == a.MaNguyenLieu);
                if (nl == null)
                    return HttpNotFound();
                nl.SoLuongTonKho = nl.SoLuongTonKho + a.SoLuongNhap;
                NhapKho nk = new NhapKho()
                {
                    MaNguyenLieu = a.MaNguyenLieu,
                    NguoiNhap = a.MaNguoiNhap,
                    SoLuongNhap = a.SoLuongNhap,
                    NgayNhap = a.NgayNhap
                };
                e.NhapKhoes.Add(nk);
                e.SaveChanges();
                return JsonMessage(true, "Thêm phiếu nhập kho thành công !");
            }
        }




    }
}