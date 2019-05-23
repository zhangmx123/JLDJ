using JLDJ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using System.Web.WebPages;
namespace JLDJ.Controllers
{
    public class MyWorkController : DefaultController
    {
        // GET: MyWork
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyWork()
        {
            return View();
        }
        private DJXTEntities2 db = new DJXTEntities2();
        [HttpGet]
        public JsonResult GetWork()
        {
            string username = Session["userrname"].ToString();
            var result = db.flows.Where(f => f.flow_founder.Contains(username));
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}