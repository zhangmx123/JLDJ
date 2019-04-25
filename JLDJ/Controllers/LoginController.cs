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
            DJXTEntities1 db = new DJXTEntities1();
            user userInfo = new user();      
            userInfo = db.users.Find(Username);
            if (userInfo == null || userInfo.user_pwd != Password)
                return 0;
            else
                return 1;
        }
  
        public ActionResult Login()
        {
            return View();
        }

    }
}