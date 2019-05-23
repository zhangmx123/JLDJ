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
    public class FlowStartController : DefaultController
    {
        // GET: FlowStart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FlowStart()
        {
            return View();
        }
        [HttpGet]
        public int addflow(string FlowType, string FlowDept)
        {
            System.DateTime currentTime = new System.DateTime();
            DJXTEntities2 db = new DJXTEntities2();
            flow flowInfo = new flow()
            {
                flow_dept = FlowDept,
                flow_num = "1",                                            //流程编号，根据已有流程数量，流程所在部门和流程类型等信息进行编号
                flow_type = FlowType,
                flow_stime = currentTime.Second.ToString(),
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
