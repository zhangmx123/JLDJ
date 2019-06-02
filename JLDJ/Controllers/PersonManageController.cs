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
    public class PersonManageController : Controller
    {
        // GET: PersonManage
        private DJXTEntities2 db = new DJXTEntities2();
        //[HttpGet]
        //public JsonResult Sessionget()
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    string username = Session["username"].ToString();
        //    string dept = Session["dept"].ToString();
        //    string role = Session["role"].ToString();
        //    dic.Add("username", username);
        //    dic.Add("dept", dept);
        //    dic.Add("role", role);
        //    return Json(dic, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PersonManage()
        {
            return View();
        }
        public JsonResult GetPerson(string person_name)
        {
            if (!person_name.IsEmpty())
            {
                var result = db.people.Where(u => u.person_name.Contains(person_name));
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var result = from b in db.people select b;
                var re1 = Json(result, JsonRequestBehavior.AllowGet).ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public int addperson(string Person_Name, string Person_Sex, string Person_Birth, string Person_CJGZdate, string Person_ZGDYdate,
            string Person_YBDYdate, string Person_Eduback, string Person_Degree, string Person_College, string Person_Phonenum,
            string Person_Dept, string Person_Email)
        {
            person personInfo = new person()
            {
                person_name = Person_Name,
                person_sex = Person_Sex,
                person_birth = Person_Birth,
                person_JJFZdate = Person_CJGZdate,
                person_CJGZdate = Person_CJGZdate,
                person_ZGDYdate = Person_ZGDYdate,
                person_YBDYdate = Person_YBDYdate,
                person_eduback = Person_Eduback,
                person_degree = Person_Degree,
                person_college = Person_College,
                person_phonenum = Person_Phonenum,
                person_dept = Person_Dept,
                person_email = Person_Email,
            };
            //try
            //{
                db.people.Add(personInfo);
                db.SaveChanges();
            //}
            //catch (Exception ex) {
            //    throw;
            //}
            return 1;
        }   
        [HttpGet]
        public int delperson(string Person_Id)
            {
                person personInfo = db.people.Find(Convert.ToInt32(Person_Id));
                if (personInfo != null)
                {
                    db.people.Remove(personInfo);
                    db.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
        [HttpGet]
        public int updperson(string Person_Id,string Person_Name, string Person_Sex, string Person_Birth, string Person_CJGZdate, string Person_ZGDYdate,
            string Person_YBDYdate, string Person_Eduback, string Person_Degree, string Person_College, string Person_Phonenum,
            string Person_Dept, string Person_Email)
        {
            person personInfo = db.people.Find(Convert.ToInt32(Person_Id));
            if (personInfo != null)
            {
                personInfo.person_name = Person_Name;
                personInfo.person_sex = Person_Sex;
                personInfo.person_birth = Person_Birth;
                personInfo.person_CJGZdate = Person_CJGZdate;
                personInfo.person_ZGDYdate = Person_ZGDYdate;
                personInfo.person_YBDYdate = Person_YBDYdate;
                personInfo.person_eduback = Person_Eduback;
                personInfo.person_degree = Person_Degree;
                personInfo.person_college = Person_College;
                personInfo.person_phonenum = Person_Phonenum;
                personInfo.person_dept = Person_Dept;
                personInfo.person_email = Person_Email;
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }

        }
    }
}