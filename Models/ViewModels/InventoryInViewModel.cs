using System.ComponentModel.DataAnnotations;

namespace FastFood.Models.ViewModels
{
    public class InventoryInViewModel
    {
    }

    public class AddInventoryInViewModel
    {
        [Display(Name = "Nguồn nguyên liệu đã có")]
        public int MaNguyenLieu { get; set; } = 0;
        [Display(Name = "Tên nguyên liệu")]
        [DataType(DataType.Text)]
        public string TenNguyenLieu { get; set; } = string.Empty;
        [Display(Name = "Số lượng nhập")]
        [DataType(DataType.Text)]
        public int SoLuongNhap { get; set; } = 0;
        [Display(Name = "Đơn vị tính")]
        [DataType(DataType.Text)]
        public string DonViTinh { get; set; } = "cái";
        [Display(Name = "Mức đặt hàng lại")]
        [DataType(DataType.Text)]
        public int MucDatHangLai { get; set; } = 0;
        [Display(Name = "Mô tả")]
        [DataType(DataType.Text)]
        public string MoTa { get; set; } = string.Empty;
        [Display(Name = "Ngày nhập")]
        [DataType(DataType.Text)]
        public DateTime NgayNhap { get; set; } = DateTime.Now;
        [Display(Name = "Người nhập")]
        [DataType(DataType.Text)]
        public string NguoiNhap { get; set; } = string.Empty;
    }
}
