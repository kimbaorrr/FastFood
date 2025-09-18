using FastFood.DB;
using FastFood.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Areas.Controllers
{
    [Route("tin-tuc")]
    public class ArticleController : SessionController
    {
        [HttpGet("")]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int size = 6)
        {
            IPagedList<Baiviet> baiViet = FastFood_BaiViet.GetBaiVietDaDuyet().OrderBy(m => m.Mabaiviet).ToPagedList(page, size);
            ViewBag.BaiViet = baiViet;
            ViewBag.CurrentPage = baiViet.PageNumber;
            ViewBag.TotalPages = baiViet.PageCount;
            ViewBag.Title = "Tin tức";
            return View();

        }
        [HttpGet("chi-tiet/{id}")]
        public IActionResult Detail(int id)
        {
            Baiviet? bv = FastFood_BaiViet.GetBaiVietDaDuyet().FirstOrDefault(x => x.Mabaiviet == id);
            if (bv == null)
                return BadRequest();
            ViewBag.Title = "Tin tức";
            ViewBag.BaiViet = bv;
            ViewBag.ReturnUrl = GetAbsoluteUri();
            return View();

        }
    }
}