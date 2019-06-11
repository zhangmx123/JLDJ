using JLDJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JLDJ.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public int Checklogin(string Username, string Password)
        {
            Entities db = new Entities();
            user userInfo = new user();      
            userInfo = db.user.Find(Username);
            if (userInfo == null || userInfo.user_pwd != Password)
                return 0;
            else
            {
                Session["username"] = userInfo.user_name;
                Session["userrname"] = userInfo.user_rname;
                Session["role"] = userInfo.user_role;
                Session["dept"] = userInfo.user_dept;


                return 1;
            }
        }
  
        public ActionResult Login()
        {
            return View();
        }

    }
}