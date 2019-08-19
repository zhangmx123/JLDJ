using JLDJ.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private Entities db = new Entities();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MyWork()
        {
            return View();
        }


        [HttpGet]

        public JsonResult GetWork()
        {
            string username = Session["username"].ToString();
  
            var result = db.flow.Where(f => f.flow_founder.Contains(username));
            return Json(result, JsonRequestBehavior.AllowGet);
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



        //更新流程，顺序号+1，更新所在人
        //返回前端需要格式（所在人）
        public string RenewFlow(string flow_num, string dept, string role)
        {
            string name = "";

            var result = db.user.Where(f => f.user_dept.Contains(dept) && f.user_role.Contains(role));

            foreach (var users in result)
            {
                name = users.user_name;
            }

            var mflow = db.flow.Find(flow_num);
            mflow.flow_founder = name;
            mflow.flow_order++;
            db.SaveChanges();

            if (dept == "中心党群")
            {
                return (role + name);
            }
            else
            {
                return (dept + "的" + role + name);
            }
        }

        //流程归档
        public void EndFlow(string flow_num)
        {
            var mflow = db.flow.Find(flow_num);

            mflow.flow_founder = "";
            mflow.flow_state = "已归档";
            mflow.flow_etime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            db.SaveChanges();
        }

        //上传文件函数
        [HttpPost]
        public JsonResult FileUpload()

        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            NameValueCollection nvc = System.Web.HttpContext.Current.Request.Form;

            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            string number = nvc.Get("Id");

            string type = nvc.Get("Type");

            string imgPath = "";

            string filename = "";

            //List<string> imgList = new List<string>();

            //List<string> nameList = new List<string>();

            if (hfc.Count > 0)
            {
                imgPath = "/Files/" + number + "_" + hfc[0].FileName;
                string PhysicalPath = Server.MapPath(imgPath);
                hfc[0].SaveAs(PhysicalPath);
                //nameList.Add(hfc[0].FileName);
                //imgList.Add(imgPath);
                dic.Add("name", number + "_" + hfc[0].FileName);

            }

            filename = number + "_" + hfc[0].FileName;
            if (string.IsNullOrEmpty(type))
            {
                var flow_Devmem = db.flow_Devmem.Find(number);
                flow_Devmem.Devmem_aptxt = filename;
            }
            else if (type == "1")
            {
                var flow_DevYB = db.flow_DevYB.Find(number);
                flow_DevYB.DevYB_PWtxt = filename;
            }
            else if (type == "2")
            {
                var flow_DevYB = db.flow_DevYB.Find(number);
                flow_DevYB.DevYB_ZStxt = filename;
            }
            else if (type == "3")
            {
                var flow_DevYB = db.flow_DevYB.Find(number);
                flow_DevYB.DevYB_DOptxt = filename;
            }
            else if (type == "4")
            {
                var flow_DevYB = db.flow_DevYB.Find(number);
                flow_DevYB.DevYB_DWOptxt = filename;
            }
            else if (type == "5")
            {
                var flow_DevYB = db.flow_DevYB.Find(number);
                flow_DevYB.DevYB_Trprtxt = filename;
            }
            else if (type == "6")
            {
                var flow_NewZGDY = db.flow_NewZGDY.Find(number);
                flow_NewZGDY.NewZGDY_ZZSQtxt = filename;
            }
            else if (type == "7")
            {
                var flow_NewZGDY = db.flow_NewZGDY.Find(number);
                flow_NewZGDY.NewZGDY_ZBDHTLtxt = filename;
            }
            else if (type == "8")
            {
                var flow_NewYXDY = db.flow_NewYXDY.Find(number);
                flow_NewYXDY.NewYXDY_PYopitxt = filename;
            }
            else if (type == "9")
            {
                var flow_QJHC = db.flow_QJHC.Find(number);
                flow_QJHC.QJHC_DeptOpitxt = filename;
            }
            else if (type == "10")
            {
                var flow_DYDH = db.flow_DYDH.Find(number);
                flow_DYDH.DYDH_MtRctxt = filename;
            }
            else if (type == "11")
            {
                var flow_ZWH = db.flow_ZWH.Find(number);
                flow_ZWH.ZWH_MtPltxt = filename;
            }
            else if (type == "12")
            {
                var flow_ZWH = db.flow_ZWH.Find(number);
                flow_ZWH.ZWH_MtRctxt = filename;
            }
            else if (type == "13")
            {
                var flow_DK = db.flow_DK.Find(number);
                flow_DK.DK_pRctxt = filename;
            }
            else if (type == "14")
            {
                var flow_DK = db.flow_DK.Find(number);
                flow_DK.DK_MtRctxt = filename;
            }
            else if (type == "15")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_CkMtxt = filename;
            }
            else if (type == "16")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_CkOtxt = filename;
            }
            else if (type == "17")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_SfAtxt = filename;
            }
            else if (type == "18")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_MtRctxt = filename;
            }
            else if (type == "19")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_RfPltxt = filename;
            }
            else if (type == "20")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_Rptxt = filename;
            }
            else if (type == "21")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_MZPYtxt = filename;
            }
            else if (type == "22")
            {
                var flow_ZZSHH = db.flow_ZZSHH.Find(number);
                flow_ZZSHH.ZZSHH_MZPYStxt = filename;
            }

            db.SaveChanges();

            return null;

        }



        //发展积极分子

        //第一步 顺序号 0
        //存入 flow_Devmem 表相关信息
        //上传入党申请书到 Files 文件夹
        public string newJJFZ_0(string flow_num, string Person_Name, string Person_Dept)
        {
            //将流程添加至数据库
            flow_Devmem flow = new flow_Devmem();
            flow = db.flow_Devmem.Find(flow_num);
            flow.Devmem_pname = Person_Name;
            flow.Devmem_pdept = Person_Dept;
            db.SaveChanges();

            //更新流程
            string re = RenewFlow(flow_num, Person_Dept, "支部书记");
            return re;
        }

        //发展积极分子第二步01 顺序号=1
        //返回人员名/部门/入党申请书存储路径
        public JsonResult newJJFZ_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var person = db.flow_Devmem.Find(flow_num);

            string Person_Name0 = person.Devmem_pname;
            string Person_Dept0 = person.Devmem_pdept;
            string Person_Application0 = "http://localhost:54133/Files/" + person.Devmem_aptxt;

            dic.Add("Person_Name0", Person_Name0);
            dic.Add("Person_Dept0", Person_Dept0);
            dic.Add("Person_Application0", Person_Application0);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展积极分子第二步02 顺序号=1
        //存储党支部意见和建议    

        public string newJJFZ_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string dept = db.flow_Devmem.Find(flow_num).Devmem_pdept;
                string re = RenewFlow(flow_num, dept, "组织委员");

                return re;
            }
            else
                return "";
        }

        //发展积极分子第三步01 顺序号=2
        //返回人员名/部门/入党申请书存储路径
        public JsonResult newJJFZ_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var person = db.flow_Devmem.Find(flow_num);

            string Person_Name0 = person.Devmem_pname;
            string Person_Dept0 = person.Devmem_pdept;
            string Person_Application0 = "http://localhost:54133/Files/" + person.Devmem_aptxt;

            dic.Add("Person_Name1", Person_Name0);
            dic.Add("Person_Dept1", Person_Dept0);
            dic.Add("Person_Application1", Person_Application0);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展积极分子第三步02 顺序号=2
        //储存积极分子信息
        public int newJJFZ_20(string flow_num, string Person_Name, string Person_Sex, string Person_Dept,
            string Person_Birth, string Person_CJGZdate, string Person_Eduback, string Person_Degree,
            string Person_College, string Person_Phonenum, string Person_Email)

        {
            //将新积极分子插入到person表中
            string JJFZDate = System.DateTime.Now.ToString("yyyy-MM-dd");

            person newPerson = new person();

            newPerson.person_CJGZdate = Person_CJGZdate;
            newPerson.person_JJFZdate = JJFZDate;
            newPerson.person_birth = Person_Birth;
            newPerson.person_college = Person_College;
            newPerson.person_degree = Person_Degree;
            newPerson.person_dept = Person_Dept;
            newPerson.person_email = Person_Email;
            newPerson.person_eduback = Person_Eduback;
            newPerson.person_name = Person_Name;
            newPerson.person_phonenum = Person_Phonenum;
            newPerson.person_plstatus = "积极分子";
            newPerson.person_sex = Person_Sex;
            newPerson.person_college = Person_College;
            db.person.Add(newPerson);
            db.SaveChanges();

            //修改flow表中personid
            var result = db.person.Where(f =>
                f.person_dept.Contains(Person_Dept) && f.person_JJFZdate.Contains(JJFZDate)
                                                    && f.person_name.Contains(Person_Name));

            int id = -1;

            foreach (var person in result)
            {
                id = person.person_id;
            }

            if (id == -1)
                return 0;

            else
            {
                var mflow = db.flow_Devmem.Find(flow_num);
                mflow.Devmem_personid = id;
                db.SaveChanges();
            }

            //流程归档
            EndFlow(flow_num);
            return 1;
        }



        //发展预备党员
        //第一步 顺序号0
        //新建流程，储存基本信息
        public string newYBDY_0(string flow_num, int Person_Id, int Person_IntroducerA, int Person_IntroducerB,
            string Person_Dept, string Person_Train)
        {
            //判断人员id是否存在,不存在则返回1
            if (!(db.person.Find(Person_Id) != null && db.person.Find(Person_IntroducerA) != null
                                                    && db.person.Find(Person_IntroducerB) != null))
                return "1";

            //判断plstatus是否为积极分子，不是返回2
            if (db.person.Find(Person_Id).person_plstatus != "积极分子")
                return "2";

            //人员成为积极分子时长是否满足12个月，不满足则返回3           
            DateTime YB = Convert.ToDateTime(db.person.Find(Person_Id).person_JJFZdate);
            DateTime Now = DateTime.Now;
            int Month = (Now.Year - YB.Year) * 12 + (Now.Month - YB.Month);
            if (Month == 12)
            {
                if (Now.Day < YB.Day)
                    return "3";
            }
            if (Month < 12)
                return "3";

            //将流程添加至数据库
            //flow_Devmem flow = new flow_Devmem();
            //flow = db.flow_Devmem.Find(flow_num);
            //flow.Devmem_pname = Person_Name;
            //flow.Devmem_pdept = Person_Dept;
            //db.SaveChanges();

            flow_DevYB flow = new flow_DevYB();
            flow = db.flow_DevYB.Find(flow_num);
            flow.DevYB_personid = Person_Id;
            flow.DevYB_pname = db.person.Find(Person_Id).person_name;
            flow.DevYB_introid1 = Person_IntroducerA;
            flow.DevYB_introname1 = db.person.Find(Person_IntroducerA).person_name;
            flow.DevYB_introid2 = Person_IntroducerB;
            flow.DevYB_introname2 = db.person.Find(Person_IntroducerB).person_name;
            flow.DevYB_pdept = Person_Dept;

            db.SaveChanges();

            string re = RenewFlow(flow_num, Person_Dept, "支部书记");
            return re;
        }

        //发展预备党员第二步01 顺序号=1
        //向前台传递预备党员信息
        public JsonResult newYBDY_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DevYB.Find(flow_num);

            string Person_Name3 = result.DevYB_pname;
            string Person_IntroducerA3 = result.DevYB_introname1;
            string Person_IntroducerB3 = result.DevYB_introname2;
            string Person_Dept3 = result.DevYB_pdept;
            string Person_Willing3 = "http://localhost:54133/Files/" + result.DevYB_PWtxt;
            string Person_ZSJL3 = "http://localhost:54133/Files/" + result.DevYB_ZStxt;
            string Person_DeptOpi3 = "http://localhost:54133/Files/" + result.DevYB_DOptxt;
            string Person_DWOpi3 = "http://localhost:54133/Files/" + result.DevYB_DWOptxt;
            string Person_Train3 = "http://localhost:54133/Files/" + result.DevYB_Trprtxt;

            dic.Add("Person_Name3", Person_Name3);
            dic.Add("Person_IntroducerA3", Person_IntroducerA3);
            dic.Add("Person_IntroducerB3", Person_IntroducerB3);
            dic.Add("Person_Dept3", Person_Dept3);
            dic.Add("Person_Willing3", Person_Willing3);
            dic.Add("Person_ZSJL3", Person_ZSJL3);
            dic.Add("Person_DeptOpi3", Person_DeptOpi3);
            dic.Add("Person_DWOpi3", Person_DWOpi3);
            dic.Add("Person_Train3", Person_Train3);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展预备党员第二步02 顺序号=1
        //存储党支部意见和建议    
        public string newYBDY_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string dept = db.flow_DevYB.Find(flow_num).DevYB_pdept;
                string re = RenewFlow(flow_num, "中心党群", "党群组织专责");
                return re;
            }

            else
                return "1";
        }

        //发展预备党员第三步01 顺序号=2
        //向前台传递预备党员信息+党支部意见和结论
        public JsonResult newYBDY_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DevYB.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name4 = result.DevYB_pname;
            string Person_IntroducerA4 = result.DevYB_introname1;
            string Person_IntroducerB4 = result.DevYB_introname2;
            string Person_Dept4 = result.DevYB_pdept;

            string Person_Willing4 = "http://localhost:54133/Files/" + result.DevYB_PWtxt;
            string Person_ZSJL4 = "http://localhost:54133/Files/" + result.DevYB_ZStxt;
            string Person_DeptOpi4 = "http://localhost:54133/Files/" + result.DevYB_DOptxt;
            string Person_DWOpi4 = "http://localhost:54133/Files/" + result.DevYB_DWOptxt;
            string Person_Train4 = "http://localhost:54133/Files/" + result.DevYB_Trprtxt;

            string Conclusion40 = result_flow.flow_brexcon;
            string Opinion40 = result_flow.flow_brexop;

            dic.Add("Person_Name4", Person_Name4);
            dic.Add("Person_IntroducerA4", Person_IntroducerA4);
            dic.Add("Person_IntroducerB4", Person_IntroducerB4);
            dic.Add("Person_Dept4", Person_Dept4);
            dic.Add("Person_Willing4", Person_Willing4);
            dic.Add("Person_ZSJL4", Person_ZSJL4);
            dic.Add("Person_DeptOpi4", Person_DeptOpi4);
            dic.Add("Person_DWOpi4", Person_DWOpi4);
            dic.Add("Person_Train4", Person_Train4);
            dic.Add("Conclusion40", Conclusion40);
            dic.Add("Opinion40", Opinion40);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展预备党员第三步02 顺序号=2
        //存储党群部意见和建议    
        public string newYBDY_20(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程

                string dept = db.flow_DevYB.Find(flow_num).DevYB_pdept;
                string re = RenewFlow(flow_num, "中心党群", "党委书记");

                return re;
            }

            else
                return "1";

        }

        //发展预备党员第四步01 顺序号=3
        //向前台传递预备党员信息+党支部意见和结论
        public JsonResult newYBDY_3(string flow_num)

        {

            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DevYB.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name5 = result.DevYB_pname;
            string Person_IntroducerA5 = result.DevYB_introname1;
            string Person_IntroducerB5 = result.DevYB_introname2;
            string Person_Dept5 = result.DevYB_pdept;

            string Person_Willing5 = "http://localhost:54133/Files/" + result.DevYB_PWtxt;
            string Person_ZSJL5 = "http://localhost:54133/Files/" + result.DevYB_ZStxt;
            string Person_DeptOpi5 = "http://localhost:54133/Files/" + result.DevYB_DOptxt;
            string Person_DWOpi5 = "http://localhost:54133/Files/" + result.DevYB_DWOptxt;
            string Person_Train5 = "http://localhost:54133/Files/" + result.DevYB_Trprtxt;

            string Conclusion50 = result_flow.flow_brexcon;
            string Opinion50 = result_flow.flow_brexop;
            string Conclusion51 = result_flow.flow_pmexcon;
            string Opinion51 = result_flow.flow_pmexop;

            dic.Add("Person_Name5", Person_Name5);
            dic.Add("Person_IntroducerA5", Person_IntroducerA5);
            dic.Add("Person_IntroducerB5", Person_IntroducerB5);
            dic.Add("Person_Dept5", Person_Dept5);
            dic.Add("Person_Willing5", Person_Willing5);
            dic.Add("Person_ZSJL5", Person_ZSJL5);
            dic.Add("Person_DeptOpi5", Person_DeptOpi5);
            dic.Add("Person_DWOpi5", Person_DWOpi5);
            dic.Add("Person_Train5", Person_Train5);
            dic.Add("Conclusion50", Conclusion50);
            dic.Add("Opinion50", Opinion50);
            dic.Add("Conclusion51", Conclusion51);
            dic.Add("Opinion51", Opinion51);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展预备党员第四步02 顺序号=3
        //存储党委书记意见和建议    
        public string newYBDY_30(string Opinion, string Conclusion, string flow_num)

        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);
            var flow_d = db.flow_DevYB.Find(flow_num);
            var person = db.person.Find(flow_d.DevYB_personid);

            if (Conclusion == "通过")
            {
                person.person_YBDYdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                person.person_plstatus = "预备党员";
            }

            flow.flow_pcexop = Opinion;
            flow.flow_pcexcon = Conclusion;

            db.SaveChanges();

            EndFlow(flow_num);

            return "0";
        }



        //预备党员转正
        //第一步 顺序号0
        //新建流程，储存基本信息
        public string newZGDY_0(int Person_Id, string flow_num)
        {
            //判断人员id是否存在,不存在则返回1
            if (db.person.Find(Person_Id) == null)
                return "1";
            //判断plstatus是否为预备党员，不是返回2
            if (db.person.Find(Person_Id).person_plstatus != "预备党员")
                return "2";
            //判断转正流程是否存在
            //不存在，人员成为预备党员时长是否满足12个月，不满足则返回3
            //存在，延期标记是否为1，是否满足6个月的要求;为0，返回4，不知道咋回事
            DateTime YB = Convert.ToDateTime(db.person.Find(Person_Id).person_YBDYdate);
            DateTime Now = DateTime.Now;
            if (db.flow_NewZGDY.Find(flow_num).NewZGDY_Delay == 0)
            {
                int Month = (Now.Year - YB.Year) * 12 + (Now.Month - YB.Month);
                if (Month == 12)
                {
                    if (Now.Day < YB.Day)
                        return "3";
                }

                if (Month < 12)
                    return "3";
            }
            else
            {
                if (db.flow_NewZGDY.Find(flow_num).NewZGDY_Delay == 1)
                {
                    int Month = (Now.Year - YB.Year) * 12 + (Now.Month - YB.Month);
                    if (Month < 6)
                        return "3";
                    if (Month == 6)
                    {
                        if (Now.Day < YB.Day)
                            return "3";
                    }
                }

            }

            //将流程添加至数据库
            person p = db.person.Find(Person_Id);
            flow_NewZGDY flow = new flow_NewZGDY();
            flow = db.flow_NewZGDY.Find(flow_num);
            flow.NewZGDY_personid = Person_Id;
            flow.NewZGDY_pname = p.person_name;
            flow.NewZGDY_pdept = p.person_dept;

            db.SaveChanges();

            string re = RenewFlow(flow_num, p.person_dept, "支部书记");
            return re;
        }

        //预备党员转正第二步01 顺序号=1
        //向前台传递预备党员信息
        public JsonResult newZGDY_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewZGDY.Find(flow_num);
            string Person_Name7 = result.NewZGDY_pname;

            string Person_ZZSQ7 = "http://localhost:54133/Files/" + result.NewZGDY_ZZSQtxt;
            string Person_ZBDHTL7 = "http://localhost:54133/Files/" + result.NewZGDY_ZBDHTLtxt;

            dic.Add("Person_Name7", Person_Name7);
            dic.Add("Person_ZZSQ7", Person_ZZSQ7);
            dic.Add("Person_ZBDHTL7", Person_ZBDHTL7);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //预备党员转正第二步02 顺序号=1
        //存储党支部意见和建议    
        public string newZGDY_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string dept = db.flow_NewZGDY.Find(flow_num).NewZGDY_pdept;
                string re = RenewFlow(flow_num, "中心党群", "党群组织专责");

                return re;
            }
            else
                return "1";
        }

        //预备党员转正第三步01 顺序号=2
        //向前台传递预备党员信息+党支部意见和结论
        public JsonResult newZGDY_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewZGDY.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name8 = result.NewZGDY_pname;
            string Person_ZZSQ8 = "http://localhost:54133/Files/" + result.NewZGDY_ZZSQtxt;
            string Person_ZBDHTL8 = "http://localhost:54133/Files/" + result.NewZGDY_ZBDHTLtxt;

            string Conclusion80 = result_flow.flow_brexcon;
            string Opinion80 = result_flow.flow_brexop;

            dic.Add("Person_Name8", Person_Name8);
            dic.Add("Person_ZZSQ8", Person_ZZSQ8);
            dic.Add("Person_ZBDHTL8", Person_ZBDHTL8);
            dic.Add("Conclusion80", Conclusion80);
            dic.Add("Opinion80", Opinion80);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //发展预备党员第三步02 顺序号=2
        //存储党群部意见和建议    
        public string newZGDY_20(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);
            var flow_d = db.flow_NewZGDY.Find(flow_num);
            var person = db.person.Find(flow_d.NewZGDY_personid);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为延期，归档并将转正流程表中的延期标志赋为1
            if (Conclusion == "延期")
            {
                EndFlow(flow_num);
                db.flow_NewZGDY.Find(flow_num).NewZGDY_Delay = 1;
                db.SaveChanges();
                return "1";
            }

            //如果结论为通过，修改person表中成为党员时间和政治面貌
            if (Conclusion == "通过")
            {
                person.person_ZGDYdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                person.person_plstatus = "中共党员";
                db.SaveChanges();
                EndFlow(flow_num);
                return "2";
            }

            else
                return "";
        }



        //评选优秀党员
        //第一步 顺序号0
        //新建流程，储存基本信息
        public string PMchoose_0(int Person_Id, string Person_Dept, string flow_num)
        {
            //判断人员id是否存在,不存在则返回1
            if (db.person.Find(Person_Id) == null)
                return "1";
            //判断plstatus是否为党员，不是返回2
            if (db.person.Find(Person_Id).person_plstatus != "中共党员")
                return "2";

            //将流程添加至数据库
            person p = db.person.Find(Person_Id);
            flow_NewYXDY flow = new flow_NewYXDY();
            flow = db.flow_NewYXDY.Find(flow_num);
            flow.NewYXDY_personid = Person_Id;
            flow.NewYXDY_pname = p.person_name;
            flow.NewYXDY_pdept = p.person_dept;

            db.SaveChanges();

            string re = RenewFlow(flow_num, p.person_dept, "支部书记");
            return re;
        }

        //评选优秀党员第二步01 顺序号=1
        //向前台传递党员信息
        public JsonResult PMchoose_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewYXDY.Find(flow_num);
            string Person_Name10 = result.NewYXDY_pname;
            string Person_Dept10 = result.NewYXDY_pdept;
            string Person_Opi10 = "http://localhost:54133/Files/" + result.NewYXDY_PYopitxt;

            dic.Add("Person_Name10", Person_Name10);
            dic.Add("Person_Dept10", Person_Dept10);
            dic.Add("Person_Opi10", Person_Opi10);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //评选优秀党员第二步02 顺序号=1
        //存储党支部意见和建议    
        public string PMchoose_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string dept = db.flow_NewYXDY.Find(flow_num).NewYXDY_pdept;
                string re = RenewFlow(flow_num, "中心党群", "党群组织专责");

                return re;
            }
            else
                return "1";
        }

        //评选优秀党员第三步01 顺序号=2
        //向前台传递预备党员信息+党支部意见和结论
        public JsonResult PMchoose_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewYXDY.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name11 = result.NewYXDY_pname;
            string Person_Dept11 = result.NewYXDY_pdept;
            string Person_Opi11 = "http://localhost:54133/Files/" + result.NewYXDY_PYopitxt;

            string Conclusion110 = result_flow.flow_brexcon;
            string Opinion110 = result_flow.flow_brexop;

            dic.Add("Person_Name11", Person_Name11);
            dic.Add("Person_Dept11", Person_Dept11);
            dic.Add("Person_Opi11", Person_Opi11);
            dic.Add("Conclusion110", Conclusion110);
            dic.Add("Opinion110", Opinion110);

            return Json(dic, JsonRequestBehavior.AllowGet);

        }

        //评选优秀党员第三步02 顺序号=2
        //存储党群部意见和建议    
        public string PMchoose_20(string Opinion, string Conclusion, string flow_num)
        {
            //存储党群部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string re = RenewFlow(flow_num, "中心党群", "党委书记");

                return re;
            }
            else
                return null;
        }

        //评选优秀党员第四步01 顺序号=3
        //向前台传递优秀党员信息+党支部意见和结论
        public JsonResult PMchoose_3(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_NewYXDY.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name12 = result.NewYXDY_pname;
            string Person_Dept12 = result.NewYXDY_pdept;

            string Person_Opi12 = "http://localhost:54133/Files/" + result.NewYXDY_PYopitxt;

            string Conclusion120 = result_flow.flow_brexcon;
            string Opinion120 = result_flow.flow_brexop;
            string Conclusion121 = result_flow.flow_pmexcon;
            string Opinion121 = result_flow.flow_pmexop;

            dic.Add("Person_Name12", Person_Name12);
            dic.Add("Person_Dept12", Person_Dept12);
            dic.Add("Person_Opi12", Person_Opi12);
            dic.Add("Conclusion120", Conclusion120);
            dic.Add("Opinion120", Opinion120);
            dic.Add("Conclusion121", Conclusion121);
            dic.Add("Opinion121", Opinion121);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //评选优秀党员第四步02 顺序号=3
        //存储党委书记意见和建议    
        public string PMchoose_30(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);
            var flow_d = db.flow_NewYXDY.Find(flow_num);
            var person = db.person.Find(flow_d.NewYXDY_personid);

            if (Conclusion == "通过")
            {
                person_YXDY p = new person_YXDY();
                p.YXDY_date = System.DateTime.Now.ToString("yyyy-MM-dd");
                p.YXDY_personid = (int)flow_d.NewYXDY_personid;
                p.YXDY_iftest = 0;
                db.person_YXDY.Add(p);
            }

            flow.flow_pcexop = Opinion;
            flow.flow_pcexcon = Conclusion;

            db.SaveChanges();

            EndFlow(flow_num);

            return "0";
        }



        //优秀党员期间核查
        //第一步 顺序号0
        //新建流程，储存基本信息
        public string PMtest_00(string Iftest13, string flow_num)
        {
            var flow = db.flow_QJHC.Find(flow_num);
            var person = db.person_YXDY.Find(flow.QJHC_personid);
            //判断为不核查，最近核查日期修改为今天，核查校验改为1
            if (Iftest13 == "否")
            {
                person.YXDY_ltdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                if (person.YXDY_iftest == 0)
                {
                    person.YXDY_iftest = 1;
                }
                db.SaveChanges();

                EndFlow(flow_num);

                return "1";
            }

            //判断为核查,发送流程至支部组织委员

            string re = RenewFlow(flow_num, flow.QJHC_pdept, "组织委员");
            return re;
        }

        //核查第二步01 顺序号=1
        //向前台返回基本信息
        public JsonResult PMtest_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_QJHC.Find(flow_num);
            string Person_Name14 = result.QJHC_pname;
            string Person_Dept14 = result.QJHC_pdept;

            dic.Add("Person_Name14", Person_Name14);
            dic.Add("Person_Dept14", Person_Dept14);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //核查第二步02 顺序号=1
        //存储支部意见及达标情况    
        public string PMtest_10(string IfZZXY14, string IfGZYJ14, string IfMFZY14, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow_QJHC.Find(flow_num);

            flow.QJHC_IfZZXY = IfZZXY14;
            flow.QJHC_IfGZYJ = IfGZYJ14;
            flow.QJHC_IfMFZY = IfMFZY14;

            //如果任一不通过，归档并返回0
            //if (IfZZXY14 == "否"||IfGZYJ14 == "否"||IfMFZY14=="否")
            //{
            //   EndFlow(flow_num);
            //   return "0";
            //}

            //如果结论为通过，顺序号+1，并传给下一个人
            //else
            //{
            //更新流程
            string dept = flow.QJHC_pdept;
            string re = RenewFlow(flow_num, dept, "支部书记");

            return re;
            // }
            // else
            //     return "1";
        }

        //核查第三步01 顺序号=2
        //向前台传递组织委员填写的信息
        public JsonResult PMtest_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_QJHC.Find(flow_num);

            string Person_Name15 = result.QJHC_pname;
            string Person_Dept15 = result.QJHC_pdept;
            string Person_DeptOpi15 = "http://localhost:54133/Files/" + result.QJHC_DeptOpitxt;
            string IfZZXY15 = result.QJHC_IfZZXY.ToString();
            string IfGZYJ15 = result.QJHC_IfGZYJ;
            string IfMFZY15 = result.QJHC_IfMFZY;

            dic.Add("Person_Name15", Person_Name15);
            dic.Add("Person_Dept15", Person_Dept15);
            dic.Add("Person_DeptOpi15", Person_DeptOpi15);
            dic.Add("IfZZXY15", IfZZXY15);
            dic.Add("IfGZYJ15", IfGZYJ15);
            dic.Add("IfMFZY15", IfMFZY15);

            return Json(dic, JsonRequestBehavior.AllowGet);

        }

        //核查第三步02 顺序号=2
        //存储党群部意见和建议    
        public string PMtest_20(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                var person = db.person_YXDY.Find(db.flow_QJHC.Find(flow_num).QJHC_personid);
                db.person_YXDY.Remove(person);
                db.SaveChanges();
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string re = RenewFlow(flow_num, "中心党群", "党群组织专责");

                return re;
            }
            else
                return "-1";
        }

        //核查第四步01 顺序号=3
        //向前台传递优秀党员信息+党支部意见和结论
        public JsonResult PMtest_3(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_QJHC.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string Person_Name16 = result.QJHC_pname;
            string Person_Dept16 = result.QJHC_pdept;

            string Person_DeptOpi16 = "http://localhost:54133/Files/" + result.QJHC_DeptOpitxt;

            string IfZZXY16 = result.QJHC_IfZZXY;
            string IfGZYJ16 = result.QJHC_IfGZYJ;
            string IfMFZY16 = result.QJHC_IfMFZY;

            string Conclusion160 = result_flow.flow_pmexop;
            string Opinion160 = result_flow.flow_pmexop;

            dic.Add("Person_Name16", Person_Name16);
            dic.Add("Person_Dept16", Person_Dept16);
            dic.Add("Person_DeptOpi16", Person_DeptOpi16);
            dic.Add("IfZZXY16", IfZZXY16);
            dic.Add("IfGZYJ16", IfGZYJ16);
            dic.Add("IfMFZY16", IfMFZY16);
            dic.Add("Conclusion160", Conclusion160);
            dic.Add("Opinion160", Opinion160);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //核查第四步02 顺序号=3
        //存储党委书记意见和建议    
        public string PMtest_30(string Opinion, string Conclusion, string flow_num)
        {

            //存储党群部意见和结论
            var flow = db.flow.Find(flow_num);
            var person = db.person_YXDY.Find(db.flow_QJHC.Find(flow_num).QJHC_personid);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                db.person_YXDY.Remove(person);
                db.SaveChanges();
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                person.YXDY_ltdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                if (person.YXDY_iftest == 0)
                {
                    person.YXDY_iftest = 1;
                }
                db.SaveChanges();
                string re = RenewFlow(flow_num, "中心党群", "党委书记");

                return re;
            }
            else
                return null;
        }


        //召开支部党员大会
        //第一步 顺序号0
        //组织委员储存大会信息
        public string DYDH_0(string flow_num, string MeetingTopic, string MeetingDate, string MeetingPlace)
        {
                                  
            //将流程添加至数据库
            flow_DYDH flow = new flow_DYDH();
            flow = db.flow_DYDH.Find(flow_num);

            flow.DYDH_flowid = flow_num;
            flow.DYDH_dept = Session["dept"].ToString();
            flow.DYDH_Date = MeetingDate;
            flow.DYDH_Topic = MeetingTopic;
            flow.DYDH_Place = MeetingPlace;

            db.SaveChanges();

            string re = RenewFlow(flow_num, flow.DYDH_dept, "支部书记");
            return re;
        }

        //召开支部党员大会第二步01 顺序号=1
        //向前台传递支部大会信息
        public JsonResult DYDH_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DYDH.Find(flow_num);

            string MeetingTopic17 = result.DYDH_Topic;
            string MeetingDate17 = result.DYDH_Date;
            string MeetingPlace17 = result.DYDH_Place;

            dic.Add("MeetingTopic17", MeetingTopic17);
            dic.Add("MeetingDate17", MeetingDate17);
            dic.Add("MeetingPlace17", MeetingPlace17);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开支部党员大会第二步02 顺序号=1
        //存储党支部意见和建议    
        public string DYDH_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {

                string re = RenewFlow(flow_num, Session["dept"].ToString(), "组织委员");

                return re;
            }
            else
                return "1";
        }

        //召开支部党员大会第三步01 顺序号=2
        //向前台传递大会及审批信息
        public JsonResult DYDH_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DYDH.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string MeetingTopic18 = result.DYDH_Topic;
            string MeetingDate18 = result.DYDH_Date;
            string MeetingPlace18 = result.DYDH_Place;

            string Conclusion18 = result_flow.flow_brexcon;
            string Opinion18 = result_flow.flow_brexop;

            dic.Add("MeetingTopic18", MeetingTopic18);
            dic.Add("MeetingDate18", MeetingDate18);
            dic.Add("MeetingPlace18", MeetingPlace18);
            dic.Add("Conclusion18", Conclusion18);
            dic.Add("Opinion18", Opinion18);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开支部党员大会第三步02 顺序号=2
        //上传会议记录（在文件上传函数中），流程归档    
        public string DYDH_20( string flow_num )
        {        
                EndFlow(flow_num);
                return "1";
        }


        //召开支部委员会
        //第一步 顺序号0
        //组织委员储存大会信息
        public string ZWH_0(string flow_num, string MeetingTopic, string MeetingDate, string MeetingPlace)
        {

            //将流程添加至数据库
            flow_ZWH flow = new flow_ZWH();
            flow = db.flow_ZWH.Find(flow_num);

            flow.ZWH_flowid = flow_num;
            flow.ZWH_dept = Session["dept"].ToString();
            flow.ZWH_Data = MeetingDate;
            flow.ZWH_Topic = MeetingTopic;
            flow.ZWH_Place = MeetingPlace;

            db.SaveChanges();

            string re = RenewFlow(flow_num, flow.ZWH_dept, "支部书记");
            return re;
        }

        //召开支部委员会第二步01 顺序号=1
        //向前台传递支委会信息
        public JsonResult ZWH_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZWH.Find(flow_num);

            string MeetingTopic20 = result.ZWH_Topic;
            string MeetingDate20 = result.ZWH_Data;
            string MeetingPlace20 = result.ZWH_Place;
            string MeetingPlan20 = "http://localhost:54133/Files/" + result.ZWH_MtPltxt;

            dic.Add("MeetingTopic20", MeetingTopic20);
            dic.Add("MeetingDate20", MeetingDate20);
            dic.Add("MeetingPlace20", MeetingPlace20);
            dic.Add("MeetingPlan20", MeetingPlan20);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }


        //召开支部委员会第二步02 顺序号=1
        //存储党支部意见和建议    
        public string ZWH_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {

                string re = RenewFlow(flow_num, Session["dept"].ToString(), "组织委员");

                return re;
            }
            else
                return "1";
        }

        //召开支部委员会第三步01 顺序号=2
        //向前台传递大会及审批信息
        public JsonResult ZWH_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZWH.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string MeetingTopic21 = result.ZWH_Topic;
            string MeetingDate21 = result.ZWH_Data;
            string MeetingPlace21 = result.ZWH_Place;

            string Conclusion21 = result_flow.flow_brexcon;
            string Opinion21 = result_flow.flow_brexop;

            string MeetingPlan21 = "http://localhost:54133/Files/" + result.ZWH_MtPltxt;

            dic.Add("MeetingTopic21", MeetingTopic21);
            dic.Add("MeetingDate21", MeetingDate21);
            dic.Add("MeetingPlace21", MeetingPlace21);
            dic.Add("Conclusion21", Conclusion21);
            dic.Add("Opinion21", Opinion21);
            dic.Add("MeetingPlan21", MeetingPlan21);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开支部委员会第三步02 顺序号=2
        //上传会议记录（在文件上传函数中），流程归档    
        public string ZWH_20(string flow_num)
        {
            EndFlow(flow_num);
            return "1";
        }


        //组织党课
        //第一步 顺序号0
        //组织委员储存课程信息
        public string DK_0(string flow_num, string MeetingTopic, string MeetingDate, string MeetingPlace)
        {

            //将流程添加至数据库
            flow_DK flow = new flow_DK();
            flow = db.flow_DK.Find(flow_num);

            flow.DK_flowid = flow_num;
            flow.DK_dept = Session["dept"].ToString();
            flow.DK_Date = MeetingDate;
            flow.DK_Topic = MeetingTopic;
            flow.DK_Place = MeetingPlace;

            db.SaveChanges();

            string re = RenewFlow(flow_num, flow.DK_dept, "支部书记");
            return re;
        }

        //组织党课第二步01 顺序号=1
        //向前台传递党课信息
        public JsonResult DK_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DK.Find(flow_num);

            string MeetingTopic23 = result.DK_Topic;
            string MeetingDate23 = result.DK_Date;
            string MeetingPlace23 = result.DK_Place;

            dic.Add("MeetingTopic23", MeetingTopic23);
            dic.Add("MeetingDate23", MeetingDate23);
            dic.Add("MeetingPlace23", MeetingPlace23);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //组织党课第二步02 顺序号=1
        //存储党支部意见和建议    
        public string DK_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                string re = RenewFlow(flow_num, Session["dept"].ToString(), "组织委员");

                return re;
            }
            else
                return "1";
        }

        //组织党课第三步01 顺序号=2
        //向前台传递党课及审批信息
        public JsonResult DK_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_DK.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string MeetingTopic24= result.DK_Topic;
            string MeetingDate24 = result.DK_Date;
            string MeetingPlace24 = result.DK_Place;

            string Conclusion24 = result_flow.flow_brexcon;
            string Opinion24 = result_flow.flow_brexop;

            dic.Add("MeetingTopic24", MeetingTopic24);
            dic.Add("MeetingDate24", MeetingDate24);
            dic.Add("MeetingPlace24", MeetingPlace24);
            dic.Add("Conclusion24", Conclusion24);
            dic.Add("Opinion24", Opinion24);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //组织党课第三步02 顺序号=2
        //上传考勤记录和讨论记录（在文件上传函数中），流程归档    
        public string DK_20(string flow_num)
        {
            EndFlow(flow_num);
            return "1";
        }


        //召开专题组织生活会
        //第一步 顺序号0
        //组织委员储存会议信息
        public string ZZSHH_0(string flow_num, string MeetingTopic, string MeetingDate, string MeetingPlace)
        {

            //将流程添加至数据库
            flow_ZZSHH flow = new flow_ZZSHH();
            flow = db.flow_ZZSHH.Find(flow_num);

            flow.ZZSHH_flowid = flow_num;
            flow.ZZSHH_dept = Session["dept"].ToString();
            flow.ZZSHH_Date = MeetingDate;
            flow.ZZSHH_Topic = MeetingTopic;
            flow.ZZSHH_Place = MeetingPlace;

            db.SaveChanges();

            string re = RenewFlow(flow_num, flow.ZZSHH_dept, "支部书记");
            return re;
        }

        //召开专题组织生活会第二步01 顺序号=1
        //向前台传递组织生活会信息
        public JsonResult ZZSHH_1(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZZSHH.Find(flow_num);

            string MeetingTopic26 = result.ZZSHH_Topic;
            string MeetingDate26 = result.ZZSHH_Date;
            string MeetingPlace26 = result.ZZSHH_Place;

            string CheckMaterials26 = "http://localhost:54133/Files/" + result.ZZSHH_CkMtxt;
            string CheckOutline26 = "http://localhost:54133/Files/" + result.ZZSHH_CkOtxt;
            string SelfAssessment26 = "http://localhost:54133/Files/" + result.ZZSHH_SfAtxt;

            dic.Add("MeetingTopic26", MeetingTopic26);
            dic.Add("MeetingDate26", MeetingDate26);
            dic.Add("MeetingPlace26", MeetingPlace26);
            dic.Add("CheckMaterials26", CheckMaterials26);
            dic.Add("CheckOutline26", CheckOutline26);
            dic.Add("SelfAssessment26", SelfAssessment26);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开专题组织生活会第二步02 顺序号=1
        //存储党支部意见和建议    
        public string ZZSHH_10(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_brexop = Opinion;
            flow.flow_brexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                string re = RenewFlow(flow_num, "中心党群", "党群组织专责");

                return re;
            }
            else
                return "1";
        }

        //召开专题组织生活会第三步01 顺序号=2
        //向前台传递会议及支部审批信息
        public JsonResult ZZSHH_2(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZZSHH.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);   

            string MeetingTopic27 = result.ZZSHH_Topic;
            string MeetingDate27 = result.ZZSHH_Date;
            string MeetingPlace27 = result.ZZSHH_Place;

            string CheckMaterials27 = "http://localhost:54133/Files/" + result.ZZSHH_CkMtxt;
            string CheckOutline27 = "http://localhost:54133/Files/" + result.ZZSHH_CkOtxt;
            string SelfAssessment27 = "http://localhost:54133/Files/" + result.ZZSHH_SfAtxt;

            string Conclusion270 = result_flow.flow_brexcon;
            string Opinion270 = result_flow.flow_brexop;
          
            dic.Add("MeetingTopic27", MeetingTopic27);
            dic.Add("MeetingDate27", MeetingDate27);
            dic.Add("MeetingPlace27", MeetingPlace27);
            dic.Add("CheckMaterials27", CheckMaterials27);
            dic.Add("CheckOutline27", CheckOutline27);
            dic.Add("SelfAssessment27", SelfAssessment27);
            dic.Add("Conclusion270", Conclusion270);
            dic.Add("Opinion270", Opinion270);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开专题组织生活会第三步02 顺序号=2
        //存储党群部意见和建议    
        public string ZZSHH_20(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                //更新流程
                string dept = db.flow_ZZSHH.Find(flow_num).ZZSHH_dept;
                string re = RenewFlow(flow_num, dept, "组织委员");

                return re;
            }
            else
                return "1";

        }

        //召开专题组织生活会第四步01 顺序号3
        //向前台传递会议基本信息及支部和党群部的审批意见
        public JsonResult ZZSHH_3(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZZSHH.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string MeetingTopic28 = result.ZZSHH_Topic;
            string MeetingDate28 = result.ZZSHH_Date;
            string MeetingPlace28 = result.ZZSHH_Place;

            string CheckMaterials28 = "http://localhost:54133/Files/" + result.ZZSHH_CkMtxt;
            string CheckOutline28 = "http://localhost:54133/Files/" + result.ZZSHH_CkOtxt;
            string SelfAssessment28 = "http://localhost:54133/Files/" + result.ZZSHH_SfAtxt;

            string Conclusion280 = result_flow.flow_brexcon;
            string Opinion280 = result_flow.flow_brexop;
            string Conclusion281 = result_flow.flow_pmexcon;
            string Opinion281 = result_flow.flow_pmexop;

            dic.Add("MeetingTopic28", MeetingTopic28);
            dic.Add("MeetingDate28", MeetingDate28);
            dic.Add("MeetingPlace28", MeetingPlace28);
            dic.Add("CheckMaterials28", CheckMaterials28);
            dic.Add("CheckOutline28", CheckOutline28);
            dic.Add("SelfAssessment28", SelfAssessment28);
            dic.Add("Conclusion280", Conclusion280);
            dic.Add("Opinion280", Opinion280);
            dic.Add("Conclusion281", Conclusion281);
            dic.Add("Opinion281", Opinion281);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开专题组织生活会第四步02 顺序号3
        //组织委员上传会议记录等四个文件（在文件上传函数中），流程推至党群部组织专责审核
        public string ZZSHH_30(string flow_num)
        {
            string re = RenewFlow(flow_num, "中心党群", "党群组织专责");
            return re;
        }

        //召开专题组织生活会第五步01 顺序号=4
        //上传考勤记录和讨论记录（在文件上传函数中），流程归档    
        public JsonResult ZZSHH_4(string flow_num)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            var result = db.flow_ZZSHH.Find(flow_num);
            var result_flow = db.flow.Find(flow_num);

            string MeetingTopic29 = result.ZZSHH_Topic;
            string MeetingDate29 = result.ZZSHH_Date;
            string MeetingPlace29 = result.ZZSHH_Place;

            string CheckMaterials29 = "http://localhost:54133/Files/" + result.ZZSHH_CkMtxt;
            string CheckOutline29 = "http://localhost:54133/Files/" + result.ZZSHH_CkOtxt;
            string SelfAssessment29 = "http://localhost:54133/Files/" + result.ZZSHH_SfAtxt;

            string Conclusion290 = result_flow.flow_brexcon;
            string Opinion290 = result_flow.flow_brexop;
            string Conclusion291 = result_flow.flow_pmexcon;
            string Opinion291 = result_flow.flow_pmexop;

            string MeetingRecords29 = "http://localhost:54133/Files/" + result.ZZSHH_MtRctxt;
            string RectificationPlan29 = "http://localhost:54133/Files/" + result.ZZSHH_RfPltxt;
            string ZZSHH_Report29 = "http://localhost:54133/Files/" + result.ZZSHH_Rptxt;
            string MZPYDY_Report29 = "http://localhost:54133/Files/" + result.ZZSHH_MZPYtxt;
            string MZPYDY_Statistics29 = "http://localhost:54133/Files/" + result.ZZSHH_MZPYStxt;

            dic.Add("MeetingTopic29", MeetingTopic29);
            dic.Add("MeetingDate29", MeetingDate29);
            dic.Add("MeetingPlace29", MeetingPlace29);
            dic.Add("CheckMaterials29", CheckMaterials29);
            dic.Add("CheckOutline29", CheckOutline29);
            dic.Add("SelfAssessment29", SelfAssessment29);
            dic.Add("Conclusion290", Conclusion290);
            dic.Add("Opinion290", Opinion290);
            dic.Add("Conclusion291", Conclusion291);
            dic.Add("Opinion291", Opinion291);
            dic.Add("MeetingRecords29", MeetingRecords29);
            dic.Add("RectificationPlan29", RectificationPlan29);
            dic.Add("ZZSHH_Report29", ZZSHH_Report29);
            dic.Add("MZPYDY_Report29", MZPYDY_Report29);
            dic.Add("MZPYDY_Statistics29", MZPYDY_Statistics29);

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        //召开专题组织生活会第五步02 顺序号4
        //党群部对上传材料进行审核
        public string ZZSHH_40(string Opinion, string Conclusion, string flow_num)
        {
            //存储党支部意见和结论
            var flow = db.flow.Find(flow_num);

            flow.flow_pmexop = Opinion;
            flow.flow_pmexcon = Conclusion;

            db.SaveChanges();

            //如果结论为不通过，归档并返回0
            if (Conclusion == "不通过")
            {
                EndFlow(flow_num);
                return "0";
            }

            //如果结论为通过，顺序号+1，并传给下一个人
            if (Conclusion == "通过")
            {
                EndFlow(flow_num);
                return "2";
            }
            else
                return " ";
        }

    }
}