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
        private Entities db = new Entities();
        [HttpGet]
        public int Checklogin(string Username, string Password)
        {

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

                //如果登陆部门为中心党群，则检索是否生成优秀党员核查任务
                if (userInfo.user_dept == "中心党群" && userInfo.user_role == "党群组织专责")
                {
                    var result = from b in db.person_YXDY select b;
                    foreach (var p in result)
                    {
                        if (p.YXDY_iftest == 0)
                        {
                            string t = p.YXDY_date;
                            string now = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                            DateTime n = Convert.ToDateTime(now);
                            string date = t + " 00:00:00";
                            DateTime YXdate = Convert.ToDateTime(date);
                            DateTime d = YXdate.AddDays(180);
                            if (DateTime.Compare(n, d) >= 0)
                            {
                                addflow(p.YXDY_personid);
                            }
                        }
                        if (p.YXDY_iftest == 1)
                        {
                            string t = p.YXDY_ltdate;
                            string now = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                            DateTime n = Convert.ToDateTime(now);
                            string date = t.Substring(0, 9) + " 00:00:00";
                            DateTime YXdate = Convert.ToDateTime(date);
                            DateTime d = YXdate.AddDays(180);
                            if (DateTime.Compare(n, d) >= 0)
                            {
                                addflow(p.YXDY_personid);
                            }
                        }
                    }
                }


                return 1;
            }
        }

        private void addflow(int personid)
        {
            flow flowInfo = new flow();

            string username = Session["username"].ToString();    //获取返回值 所在人

            var result = db.user.Find(username);
            var userrole = result.user_role;

            //建立流程编号，由14位时间戳+2位随机数组成
            Random random = new Random();
            int n = random.Next(10, 99);
            string num = System.DateTime.Now.ToString("yyMMddhhmmss");

            flowInfo.flow_dept = "中心党群";
            flowInfo.flow_type = "优秀党员期间核查";
            flowInfo.flow_stime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            flowInfo.flow_state = "待处理";
            flowInfo.flow_order = 0;
            flowInfo.flow_founder = username;                                     //流程所在人，根据流程编号和
            flowInfo.flow_etime = "";                                           //流程结束时间，归档时写入
                                                                                //           flowInfo.flow_id = Guid.NewGuid().ToString();                       //流程ID，主键，自增
            flowInfo.flow_num = num + n;                                          //流程编号
            db.flow.Add(flowInfo);

            var person = db.person.Find(personid);
            flow_QJHC flow_d = new flow_QJHC();
            flow_d.QJHC_flowid = flowInfo.flow_num;
            flow_d.QJHC_personid = personid;
            flow_d.QJHC_pname = person.person_name;
            flow_d.QJHC_pdept = person.person_dept;
            db.flow_QJHC.Add(flow_d);

            db.SaveChanges();

        }

        public ActionResult Login()
        {
            return View();
        }

    }
}