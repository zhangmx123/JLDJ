using JLDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JLDJ.Controllers
{
    public class MenuController : DefaultController
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Menu()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Menuget()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string username = Session["username"].ToString();
            string dept = Session["dept"].ToString();
            string role = Session["role"].ToString();
            dic.Add("username", username);
            dic.Add("dept", dept);
            dic.Add("role", role);
            return Json(dic, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public int Menulogout()
        {
            Session["username"]=null;
            Session["dept"] = null;
            Session["role"] = null;
            return 1;
        }
    }
}