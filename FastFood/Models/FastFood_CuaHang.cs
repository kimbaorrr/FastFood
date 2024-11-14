using FastFood.DB;
using System.Linq;

namespace FastFood.Models
{
    public class FastFood_CuaHang
    {
        private static FastFoodEntities context = new FastFoodEntities();
        private static IQueryable<GioLamViecCuaHang> gioLamViecCuaHangs => context.GioLamViecCuaHangs;
        private static IQueryable<ThongTinCuaHang> thongTinCuaHangs => context.ThongTinCuaHangs;

        public static IQueryable<GioLamViecCuaHang> getGioLamViec()
        {
            return gioLamViecCuaHangs;
        }

        public static ThongTinCuaHang getThongTin()
        {
            return thongTinCuaHangs.SingleOrDefault();
        }
    }
}