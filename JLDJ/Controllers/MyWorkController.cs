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
            //string userrname = "";
            //var user = db.user.Where(f => f.user_name.Contains(username));
            //foreach(var u in user)
            //{
            //    userrname = u.user_rname;
            //}
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

            //上传附件

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
            //判断人员id是否存在,不存在则返回null
            if (!(db.person.Find(Person_Id) != null && db.person.Find(Person_IntroducerA) != null
                                                    && db.person.Find(Person_IntroducerB) != null))
                return "1";

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
            //添加附件
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
            if (Conclusion=="通过") {
                person.person_YBDYdate = System.DateTime.Now.ToString("yyyy-MM-dd");
                person.person_plstatus = "预备党员";
            }
            flow.flow_pcexop = Opinion;
            flow.flow_pcexcon = Conclusion;

            db.SaveChanges();

            EndFlow(flow_num);

            return "0";
        }
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
            db.SaveChanges();
            return null;
        }

    }
}