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
                case "优秀党员评选":
                case "召开支部党员大会":
                case "召开支部委员会":
                case "组织党课":
                case "召开专题组织生活会":
                    {
                        if (userrole == "组织委员")
                            break;
                        else
                        {
                            username = "";
                            return username;
                        }
                    }
                case "优秀党员期间核查":
                    {
                        if (result.user_dept == "中心党群")
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
            else if (FlowType == "发展党员")
            {
                flow_DevYB flow_d = new flow_DevYB();
                flow_d.DevYB_flowid = flowInfo.flow_num;
                db.flow_DevYB.Add(flow_d);
            }

            else if (FlowType == "党员转正")
            {
                flow_NewZGDY flow_d = new flow_NewZGDY();
                flow_d.NewZGDY_flowid = flowInfo.flow_num;
                flow_d.NewZGDY_Delay = 0;
                db.flow_NewZGDY.Add(flow_d);
            }

            else if (FlowType == "优秀党员评选")
            {
                flow_NewYXDY flow_d = new flow_NewYXDY();
                flow_d.NewYXDY_flowid = flowInfo.flow_num;

                db.flow_NewYXDY.Add(flow_d);
            }
            else if (FlowType == "召开支部党员大会")
            {
                flow_DYDH flow_d = new flow_DYDH();
                flow_d.DYDH_flowid = flowInfo.flow_num;

                db.flow_DYDH.Add(flow_d);
            }
            else if (FlowType == "召开支部委员会")
            {
                flow_ZWH flow_d = new flow_ZWH();
                flow_d.ZWH_flowid = flowInfo.flow_num;

                db.flow_ZWH.Add(flow_d);
            }
            else if (FlowType == "组织党课")
            {
                flow_DK flow_d = new flow_DK();
                flow_d.DK_flowid = flowInfo.flow_num;

                db.flow_DK.Add(flow_d);
            }
            else if (FlowType == "召开专题组织生活会")
            {
                flow_ZZSHH flow_d = new flow_ZZSHH();
                flow_d.ZZSHH_flowid = flowInfo.flow_num;

                db.flow_ZZSHH.Add(flow_d);
            }
            db.SaveChanges();
            return username;
        }


        //查看详情的几个函数

        //发展积极分子
        public JsonResult check_newJJFZ(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_Devmem.Find(flow_num);
            var resultp = db.person.Find(result.Devmem_personid);

            string Person_Name1 = result.Devmem_pname;
            string Person_Dept1 = result.Devmem_pdept;
            string Person_Application0 = "http://localhost:54133/Files/" + result.Devmem_aptxt;
            string Person_College1 = resultp.person_college;
            string Person_Phonenum1 = resultp.person_phonenum;
            string Person_Email1 = resultp.person_email;
            string Person_Eduback1 = resultp.person_eduback;
            string Person_Sex1 = resultp.person_sex;
            string Person_Degree1 = resultp.person_degree;
            string Person_Birth1 = resultp.person_birth;
            string Person_CJGZdate1 = resultp.person_CJGZdate;

            dic.Add("Person_Name1", Person_Name1);
            dic.Add("Person_Dept1", Person_Dept1);
            dic.Add("Person_Application0", Person_Application0);
            dic.Add("Person_College1", Person_College1);
            dic.Add("Person_Phonenum1", Person_Phonenum1);
            dic.Add("Person_Email1", Person_Email1);
            dic.Add("Person_Eduback1", Person_Eduback1);
            dic.Add("Person_Sex1", Person_Sex1);
            dic.Add("Person_Degree1", Person_Degree1);
            dic.Add("Person_Birth1", Person_Birth1);
            dic.Add("Person_CJGZdate1", Person_CJGZdate1);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展预备党员
        public JsonResult check_newYBDY(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DevYB.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name2 = result.DevYB_pname;
            string Person_IntroducerA2 = result.DevYB_introname1;
            string Person_IntroducerB2 = result.DevYB_introname2;
            string Person_Dept2 = result.DevYB_pdept;
            string Person_Willing2 = "http://localhost:54133/Files/" + result.DevYB_PWtxt;
            string Person_ZSJL2 = "http://localhost:54133/Files/" + result.DevYB_ZStxt;
            string Person_DeptOpi2 = "http://localhost:54133/Files/" + result.DevYB_DOptxt;
            string Person_DWOpi2 = "http://localhost:54133/Files/" + result.DevYB_DWOptxt;
            string Person_Train2 = "http://localhost:54133/Files/" + result.DevYB_Trprtxt;
            string Conclusion20 = result_flow.flow_brexcon;
            string Opinion20 = result_flow.flow_brexop;
            string Conclusion21 = result_flow.flow_pmexcon;
            string Opinion21 = result_flow.flow_pmexop;
            string Conclusion22 = result_flow.flow_pcexcon;
            string Opinion22 = result_flow.flow_pcexop;

            dic.Add("Person_Name2", Person_Name2);
            dic.Add("Person_IntroducerA2", Person_IntroducerA2);
            dic.Add("Person_IntroducerB2", Person_IntroducerB2);
            dic.Add("Person_Dept2", Person_Dept2);
            dic.Add("Person_Willing2", Person_Willing2);
            dic.Add("Person_ZSJL2", Person_ZSJL2);
            dic.Add("Person_DeptOpi2", Person_DeptOpi2);
            dic.Add("Person_Train2", Person_Train2);
            dic.Add("Conclusion20", Conclusion20);
            dic.Add("Opinion20", Opinion20);
            dic.Add("Conclusion21", Conclusion21);
            dic.Add("Opinion21", Opinion21); 
            dic.Add("Conclusion22", Conclusion22);
            dic.Add("Opinion22", Opinion22);
       
            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //预备党员转正
        public JsonResult check_newZGDY(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewZGDY.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name3 = result.NewZGDY_pname;
            string Person_ZZSQ3 = "http://localhost:54133/Files/" + result.NewZGDY_ZZSQtxt;
            string Person_ZBDHTL3 = "http://localhost:54133/Files/" + result.NewZGDY_ZBDHTLtxt;
            string Conclusion30 = result_flow.flow_brexcon;
            string Opinion30 = result_flow.flow_brexop;
            string Conclusion31 = result_flow.flow_pmexcon;
            string Opinion31 = result_flow.flow_pmexop;

            dic.Add("Person_Name3", Person_Name3);
            dic.Add("Person_ZZSQ3", Person_ZZSQ3);
            dic.Add("Person_ZBDHTL3", Person_ZBDHTL3);
            dic.Add("Conclusion30", Conclusion30);
            dic.Add("Opinion30", Opinion30);
            dic.Add("Conclusion31", Conclusion31);
            dic.Add("Opinion31", Opinion31);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //优秀党员评选
        public JsonResult check_PMchoose(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewYXDY.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name4 = result.NewYXDY_pname;
            string Person_Dept4 = result.NewYXDY_pdept;
            string Person_Opi4 =  result.NewYXDY_PYopitxt;
            string Conclusion40 = result_flow.flow_brexcon;
            string Opinion40 = result_flow.flow_brexop;
            string Conclusion41 = result_flow.flow_pmexcon;
            string Opinion41 = result_flow.flow_pmexop;
            string Conclusion42 = result_flow.flow_pcexcon;
            string Opinion42 = result_flow.flow_pcexop;

            dic.Add("Person_Name4", Person_Name4);
            dic.Add("Person_Dept4", Person_Dept4);
            dic.Add("Person_Opi4", Person_Opi4);
            dic.Add("Conclusion40", Conclusion40);
            dic.Add("Opinion40", Opinion40);
            dic.Add("Conclusion41", Conclusion41);
            dic.Add("Opinion41", Opinion41);
            dic.Add("Conclusion42", Conclusion42);
            dic.Add("Opinion42", Opinion42);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //优秀党员期间核查
        public JsonResult check_PMtest(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_QJHC.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name5 = result.QJHC_pname;
            string Person_Dept5 = result.QJHC_pdept;
            string Person_DeptOpi5 = result.QJHC_DeptOpitxt;
            string IfZZXY5 = result.QJHC_IfZZXY;
            string IfGZYJ5 = result.QJHC_IfGZYJ;
            string IfMFZY5 = result.QJHC_IfMFZY;
            string Conclusion50 = result_flow.flow_brexcon;
            string Opinion50 = result_flow.flow_brexop;
            string Conclusion51 = result_flow.flow_pmexcon;
            string Opinion51 = result_flow.flow_pmexop;

            dic.Add("Person_Name5", Person_Name5);
            dic.Add("Person_Dept5", Person_Dept5);
            dic.Add("Person_DeptOpi5", Person_DeptOpi5);
            dic.Add("IfZZXY5", IfZZXY5);
            dic.Add("IfGZYJ5", IfGZYJ5); 
            dic.Add("IfMFZY5", IfMFZY5);
            dic.Add("Conclusion50", Conclusion50);
            dic.Add("Opinion50", Opinion50);
            dic.Add("Conclusion51", Conclusion51);
            dic.Add("Opinion51", Opinion51);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }
    }
}