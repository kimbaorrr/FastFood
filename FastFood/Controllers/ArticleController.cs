﻿using FastFood.DB;
using FastFood.Models;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace FastFood.Controllers
{
    public class ArticleController : Controller
    {
        [HttpGet]
        public ActionResult Index(int page = 1, int size = 6)
        {
            IPagedList<BaiViet> baiViet = FastFood_BaiViet.GetBaiVietDaDuyet().OrderBy(m => m.MaBaiViet).ToPagedList(page, size);
            ViewBag.BaiViet = baiViet;
            ViewBag.CurrentPage = baiViet.PageNumber;
            ViewBag.TotalPages = baiViet.PageCount;
            ViewBag.Title = "Tin tức";
            return View();
        }
        [HttpGet]
        public ActionResult Detail(int id, string return_url)
        {
            BaiViet bv = FastFood_BaiViet.GetBaiVietDaDuyet().Where(x => x.MaBaiViet == id).FirstOrDefault();
            if (bv == null)
                return HttpNotFound();
            ViewBag.Title = "Tin tức";
            ViewBag.BaiViet = bv;
            ViewBag.ReturnUrl = return_url;
            return View();
        }
    }
}