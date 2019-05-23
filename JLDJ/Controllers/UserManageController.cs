﻿using JLDJ.Models;
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
    public class UserManageController : DefaultController
    {
        // GET: UserManage
        private DJXTEntities2 db = new DJXTEntities2();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserManage()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetUser(string user_name)
        {


            //if (!string.IsNullOrEmpty(user_name))
                if (!user_name.IsEmpty())
            {
                var result = db.users.Where(u => u.user_name.Contains(user_name));
                return Json(result, JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                var result =  from b in db.users select b;
                var re1 = Json(result, JsonRequestBehavior.AllowGet).ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }           
        }
        [HttpGet]
        public int adduser(string Username, string Password, string Name, string Role, string Department)
        {

            /*SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Data Source=(local);Initial Catalog=JLDJXT;User ID=sa;Password=123456";

            conn.Open();

            string strsql = "insert into [User] (Username,Password,Name,Role,Department) values ('" + Username + "','" + Password + "','" + Name + "','" + Role + "','" + Department + "')";

            SqlCommand cmd = new SqlCommand(strsql, conn);

            int result = cmd.ExecuteNonQuery();

            conn.Close();*/
            user isnewuUser = db.users.Find(Username);
            if (isnewuUser != null)
            {
                return 0;        
            }
            else
            {
                user userInfo = new user()
                {
                    user_name = Username,
                    user_pwd = Password,
                    user_rname = Name,
                    user_dept =Department,
                    user_role = Role
                };
                db.users.Add(userInfo);
                db.SaveChanges();
                return 1;
            }            
        }
        [HttpGet]
        public int deluser(string Username)
        {

            /*SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Data Source=(local);Initial Catalog=JLDJXT;User ID=sa;Password=123456";

            conn.Open();

            string strsql = "delete from [User] where Username='" + Username + "'";

            SqlCommand cmd = new SqlCommand(strsql, conn);

            int result = cmd.ExecuteNonQuery();

            conn.Close();
            return result;*/

            user userInfo = db.users.Find(Username);
            if (userInfo != null)
            {
                db.users.Remove(userInfo);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }

        }

        [HttpGet]
        public int upduser(string Username, string Name, string Role, string Department)
        {

            /*SqlConnection conn = new SqlConnection();

            conn.ConnectionString = "Data Source=(local);Initial Catalog=JLDJXT;User ID=sa;Password=123456";

            conn.Open();

            string strsql = "update [User] set Password='" + Password + "',Name='" + Name + "',Role='" + Role + "',Department='" + Department + "' where Username='" + Username + "'";

            SqlCommand cmd = new SqlCommand(strsql, conn);

            int result = cmd.ExecuteNonQuery();

            conn.Close();
            return result;*/

            user userInfo = db.users.Find(Username);
            if (userInfo != null)
            {
                userInfo.user_name = Username;
               
                userInfo.user_role = Role;
                userInfo.user_dept = Department;
                userInfo.user_rname = Name;

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