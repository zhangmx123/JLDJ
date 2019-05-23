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
    public class FlowSelectController : DefaultController
    {
        // GET: FlowSelect
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FlowSelect()
        {
            return View();
        }
        private DJXTEntities2 db = new DJXTEntities2();
        [HttpGet]
        public JsonResult GetFlow(string flow_num, string flow_dept, string flow_type)
        {
            if (!(flow_num.IsEmpty()&& flow_dept.IsEmpty() && flow_type.IsEmpty()))
            {
                var result = db.flows.Where(f => f.flow_num.Contains(flow_num)&& f.flow_dept.Contains(flow_dept) && f.flow_type.Contains(flow_type));
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from b in db.flows select b;
                var re1 = Json(result, JsonRequestBehavior.AllowGet).ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public int addflow(string FlowType, string FlowDept)
        {
            flow flowInfo = new flow()
            {
                flow_dept = FlowDept,
                flow_num = "1",                                            //流程编号，根据已有流程数量，流程所在部门和流程类型等信息进行编号
                flow_type = FlowType,
                flow_stime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                flow_state = "待处理",
                flow_order = "0",
                flow_founder = "某某",                                     //流程所在人，根据流程编号和
                flow_etime = ""                                            //流程结束时间，归档时写入
            };
            db.flows.Add(flowInfo);
            db.SaveChanges();
            return 1;
        }
    }
}