using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JLDJ.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        //public ActionResult Index()
        //{
        //    return View();
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var username = Session["username"] as String;
            if (String.IsNullOrEmpty(username))
            {
                //重定向至登录页面
                //filterContext.Result = RedirectToAction("Login", "Login", new { url = Request.RawUrl });
                Response.Write("<script>   top.window.location.href = '/Login/Login?r='+Math.random() ;</script>");
                return;
            }
        }
    }
}