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
using Microsoft.Ajax.Utilities;

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
            if (!(flow_num.IsEmpty() && flow_dept.IsEmpty() && flow_type.IsEmpty()))
            {
                var result = db.flow.Where(f => f.flow_num.Contains(flow_num) && f.flow_dept.Contains(flow_dept) && f.flow_type.Contains(flow_type));
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

        //发起流程函数  传入流程类型和流程所在部门，返回值为流程所在人
        public string addflow(string FlowType, string FlowDept)
        {

            flow flowInfo = new flow();

            string username = Session["username"].ToString();    //获取返回值 所在人

            var result = db.user.Find(username);
            var userrole = result.user_role;

            //判断发起流程权限，如果无权限，返回空值
            switch (FlowType)
            {
                case "发展积极分子":
                case "发展党员":
                case "党员转正":
                    {
                        if (userrole == "组织委员")
                            break;
                        else
                        {
                            username = "";
                            return username;
                        }
                    }
                default: break;
            }

            //建立流程编号，由14位时间戳+2位随机数组成
            Random random = new Random();
            int n = random.Next(10, 99);
            string num = System.DateTime.Now.ToString("yyMMddhhmmss");

            flowInfo.flow_dept = FlowDept;
            flowInfo.flow_type = FlowType;
            flowInfo.flow_stime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            flowInfo.flow_state = "待处理";
            flowInfo.flow_order = 0;
            flowInfo.flow_founder = username;                                     //流程所在人，根据流程编号和
            flowInfo.flow_etime = "";                                           //流程结束时间，归档时写入
                                                                                //           flowInfo.flow_id = Guid.NewGuid().ToString();                       //流程ID，主键，自增
            flowInfo.flow_num = num + n;                                          //流程编号
            db.flow.Add(flowInfo);
            if (FlowType == "发展积极分子")
            {
                flow_Devmem flow_d = new flow_Devmem();
                flow_d.Devmem_flowid = flowInfo.flow_num;
                db.flow_Devmem.Add(flow_d);
            }
            if (FlowType == "发展党员")
            {
                flow_DevYB flow_d = new flow_DevYB();
                flow_d.DevYB_flowid = flowInfo.flow_num;
                db.flow_DevYB.Add(flow_d);
            }
            db.SaveChanges();

            return username;
        }
    }
}