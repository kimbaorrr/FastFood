using FastFood.DB;
using System.Collections.Generic;
using System.Linq;

namespace FastFood.Models
{
    public class FastFood_BaiViet
    {
        private static FastFoodEntities context => new FastFoodEntities();
        private static IQueryable<BaiViet> baiViets => context.BaiViets;
        public static IQueryable<BaiViet> GetBaiViets()
        {
            return baiViets;
        }
        public static IEnumerable<BaiViet> GetBaiVietDaDuyet()
        {
            return GetBaiViets().Where(x => x.DaDuyet) ?? Enumerable.Empty<BaiViet>();
        }
    }
}