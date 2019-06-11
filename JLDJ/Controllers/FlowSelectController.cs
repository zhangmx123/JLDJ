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
        private Entities db = new Entities();
        [HttpGet]
        public JsonResult GetFlow(string flow_num, string flow_dept, string flow_type)
        {
            if (!(flow_num.IsEmpty()&& flow_dept.IsEmpty() && flow_type.IsEmpty()))
            {
                var result = db.flow.Where(f => f.flow_num.Contains(flow_num)&& f.flow_dept.Contains(flow_dept) && f.flow_type.Contains(flow_type));
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from b in db.flow select b;
                var re1 = Json(result, JsonRequestBehavior.AllowGet).ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public JsonResult Sessionget()
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
        public int addflow(string FlowType, string FlowDept)
        {
            Random random = new Random();
            int n = random.Next(10, 99);
            string num = System.DateTime.Now.ToString("yyMMddhhmmss");
            flow flowInfo = new flow();
            flowInfo.flow_dept = FlowDept;
            flowInfo.flow_type = FlowType;
            flowInfo.flow_stime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            flowInfo.flow_state = "待处理";
            flowInfo.flow_order = "0";
            flowInfo.flow_founder = "某某";                                     //流程所在人，根据流程编号和
            flowInfo.flow_etime = "";                                           //流程结束时间，归档时写入
            flowInfo.flow_id = Guid.NewGuid().ToString();                       //流程ID
            flowInfo.flow_num = num+n;                                          //流程编号，根据已有流程数量，流程所在部门和流程类型等信息进行编号
            db.flow.Add(flowInfo);
            db.SaveChanges();
            return 1;
        }
    }
}