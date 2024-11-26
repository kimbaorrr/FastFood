using FastFood.DB;
using System.Linq;

namespace FastFood.Models
{
    public static class FastFood_CuaHang
    {
        private static FastFoodEntities context = new FastFoodEntities();
        private static IQueryable<GioLamViecCuaHang> gioLamViecCuaHangs => context.GioLamViecCuaHangs;
        private static IQueryable<ThongTinCuaHang> thongTinCuaHangs => context.ThongTinCuaHangs;

        public static IQueryable<GioLamViecCuaHang> GetGioLamViec()
        {
            return gioLamViecCuaHangs;
        }

        public static ThongTinCuaHang GetThongTin()
        {
            return thongTinCuaHangs.SingleOrDefault() ?? new ThongTinCuaHang();
        }
    }
}