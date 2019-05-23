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
            try
            {
                db.people.Add(personInfo);
                db.SaveChanges();
            }
            catch(Exception ex) {
                throw;
            }

                return 1;

        }
    }
}