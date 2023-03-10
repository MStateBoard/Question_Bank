using Newtonsoft.Json;
using Question_Bank.Helper;
using Question_Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Question_Bank.Controllers
{
    public class MasterController : BaseController
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();
        // GET: Master

        public ActionResult Student_Answers()
        {
            Exam_Report_Model model = new Exam_Report_Model();

            return View(model);
        }

        [HttpPost]
        public ActionResult Student_Answers(string Seat_No)
        {
            Login_Model login_model = common.Get_Login_Details();
            Exam_Report_Model model = new Exam_Report_Model();

            Tbl_Student isTheir = db.Tbl_Student.Where(x => x.Index_No == login_model.Index_No && x.Seat_No == Seat_No).FirstOrDefault();

            if (isTheir != null)
            {
                model._Question1 = db.Tbl_Question1.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question2 = db.Tbl_Question2.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question3 = db.Tbl_Question3.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question4 = db.Tbl_Question4.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question5 = db.Tbl_Question5.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question6 = db.Tbl_Question6.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();
                model._Question7 = db.Tbl_Question7.Where(x => x.Seat_No == Seat_No && x.Index_No == login_model.Index_No).ToList();

                return View(model);
            }
            else
            {
                TempData["Err"] = "No Record Found";
                return View(model);
            }
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Master");
        }

        [HttpPost]
        public ActionResult Login(Login_Model login_Model)
        {
            try
            {
                if (login_Model.User_ID == "1111122")
                {
                    var admin = db.Tbl_Admin.Where(x => x.User_ID == login_Model.User_ID && x.Password == login_Model.Password).FirstOrDefault();
                    if (admin != null)
                    {
                        login_Model.User_ID = admin.User_ID;
                        string json = JsonConvert.SerializeObject(login_Model);
                        FormsAuthentication.SetAuthCookie(json, false);

                        return RedirectToAction("Create_School_College", "Admin");
                    }
                    else
                    {
                        TempData["Msg"] = "Invalid UserName or Password";
                    }
                }
                else
                {
                    var std_login = db.Tbl_Create_School_College.Where(x => x.User_ID == login_Model.User_ID && x.Password == login_Model.Password).FirstOrDefault();
                    if (std_login != null)
                    {
                        login_Model.User_ID = std_login.User_ID;
                        string json = JsonConvert.SerializeObject(login_Model);
                        FormsAuthentication.SetAuthCookie(json, false);

                        return RedirectToAction("Create_User", "Master");
                    }
                    else
                    {
                        TempData["Msg"] = "Invalid UserName or Password";
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Master");
            }

            return View();
        }

        public ActionResult Create_User()
        {
            Login_Model login_model = common.Get_Login_Details();
            var model = db.Tbl_Create_School_College.Where(x => x.User_ID == login_model.User_ID).FirstOrDefault();
            TempData["User_ID"] = login_model.User_ID;

            if (model != null && model.Mobile_No != null && model.Email_ID != null && model.Index_No != null && model.Address != null && model.Institute_Name != null && model.Name != null && model.Password != null)
            {
                login_model.Index_No = model.Index_No;
                string json = JsonConvert.SerializeObject(login_model);
                FormsAuthentication.SetAuthCookie(json, false);

                return RedirectToAction("Create_Class", "Master");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult Create_User(Tbl_Create_School_College model)
        {
            try
            {
                Login_Model login_Model = common.Get_Login_Details();

                if (model.ID != 0)
                {
                    var oldrecord = db.Tbl_Create_School_College.Where(x => x.ID == model.ID).FirstOrDefault();

                    oldrecord.Mobile_No = model.Mobile_No;
                    oldrecord.Email_ID = model.Email_ID;
                    oldrecord.Index_No = model.Index_No;
                    oldrecord.Address = model.Address;
                    oldrecord.Institute_Name = model.Institute_Name;
                    oldrecord.Password = model.Password;
                    oldrecord.Name = model.Name;
                    oldrecord.Active = 1;

                    db.Tbl_Create_School_College.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Mobile_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Email_ID).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Index_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Address).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Active).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Institute_Name).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Password).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Name).IsModified = true;

                    db.SaveChanges();

                    return Json(new { Result = true, Response = "Record Updated successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (db.Tbl_Create_School_College.Any(x => x.User_ID == login_Model.User_ID))
                    {
                        return Json(new { Result = false, Response = "Record Already Exists! Please Select Edit Option to Make Changes!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.Index_No == null || model.Index_No == "" || model.Index_No == "0") { return Json(new { Result = false, Response = "Please Enter Index No." }, JsonRequestBehavior.AllowGet); }
                        if (model.Institute_Name == null || model.Institute_Name == "" || model.Institute_Name == "0") { return Json(new { Result = false, Response = "Please Enter Institute Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Name == null || model.Name == "" || model.Name == "0") { return Json(new { Result = false, Response = "Please Enter Name." }, JsonRequestBehavior.AllowGet); }
                        if (model.Password == null || model.Password == "" || model.Password == "0") { return Json(new { Result = false, Response = "Please Enter Password." }, JsonRequestBehavior.AllowGet); }
                        if (model.Mobile_No == null || model.Mobile_No == "" || model.Mobile_No == "0") { return Json(new { Result = false, Response = "Please Enter Mobile No." }, JsonRequestBehavior.AllowGet); }
                        if (model.Email_ID == null || model.Email_ID == "" || model.Email_ID == "0") { return Json(new { Result = false, Response = "Please Enter Email ID." }, JsonRequestBehavior.AllowGet); }
                        if (model.Address == null || model.Address == "" || model.Address == "0") { return Json(new { Result = false, Response = "Please Enter Address." }, JsonRequestBehavior.AllowGet); }

                        model.User_ID = login_Model.User_ID;
                        model.Active = 1;

                        db.Tbl_Create_School_College.Add(model);
                        db.SaveChanges();

                        login_Model.Index_No = model.Index_No;
                        string json = JsonConvert.SerializeObject(login_Model);
                        FormsAuthentication.SetAuthCookie(json, false);

                        return Json(new { Result = true, Response = "Record Added Successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Get_Data(string User_ID, int Type)
        {
            try
            {
                if (User_ID == "1111122")
                {
                    var tbl = db.Tbl_Create_School_College.Where(x => x.Active == 1).ToList();

                    return Json(new { Result = true, Response = tbl }, JsonRequestBehavior.AllowGet);
                }
                if (Type == 1)
                {
                    var tbl = db.Tbl_Create_School_College.Where(x => x.User_ID == User_ID && x.Active == 1).ToList();

                    return Json(new { Result = true, Response = tbl }, JsonRequestBehavior.AllowGet);
                }
                if (Type == 2)
                {
                    var tbl = db.Tbl_Question_Data.Where(x => x.User_ID == User_ID).ToList();

                    return Json(new { Result = true, Response = tbl }, JsonRequestBehavior.AllowGet);
                }
                if (Type == 3)
                {
                    var tbl = db.Tbl_Question_Paper.Where(x => x.User_ID == User_ID).ToList();

                    return Json(new { Result = true, Response = tbl }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Result = false, Response = "Something Went Wrong!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exe)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + exe }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Get_User_Record(int id)
        {
            var record = db.Tbl_Create_School_College.Where(x => x.ID == id).FirstOrDefault();

            return Json(new { Result = true, Response = record }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_User_Record(int id)
        {
            try
            {
                var oldrecord = db.Tbl_Create_School_College.Where(x => x.ID == id).FirstOrDefault();

                oldrecord.Active = 0;
                db.Tbl_Create_School_College.Attach(oldrecord);
                db.Entry(oldrecord).Property(x => x.Active).IsModified = true;
                db.SaveChanges();

                return Json(new { Result = true, Response = "Record deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Fail To delete Record." + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create_Class()
        {
            Login_Model login_model = common.Get_Login_Details();

            var model = db.Tbl_Create_School_College.Where(x => x.User_ID == login_model.User_ID).FirstOrDefault();

            TempData["User_ID"] = login_model.User_ID;

            if (model != null && model.Mobile_No != null && model.Email_ID != null && model.Index_No != null && model.Address != null && model.Institute_Name != null && model.Name != null && model.Password != null)
            {
                List<SelectListItem> List = common.Get_Class(login_model.Index_No);
                ViewBag.ClassList = new SelectList(List, "Value", "Text");

                return View();
            }
            else
            {
                return RedirectToAction("Create_User", "Master");
            }
        }

        public JsonResult Get_Class_Record(int id)
        {
            var record = db.Tbl_Question_Data.Where(x => x.ID == id).FirstOrDefault();

            return Json(new { Result = true, Response = record }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_Class_Record(int id)
        {
            try
            {
                var oldrecord = db.Tbl_Question_Data.Where(x => x.ID == id).FirstOrDefault();

                db.Tbl_Question_Data.Remove(oldrecord);
                db.SaveChanges();

                return Json(new { Result = true, Response = "Record deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Fail To delete Record." + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create_Class(Tbl_Question_Data model)
        {
            try
            {
                Login_Model login_Model = common.Get_Login_Details();
                if (model.ID != 0)
                {
                    var oldrecord = db.Tbl_Question_Data.Where(x => x.ID == model.ID).FirstOrDefault();

                    oldrecord.Class = model.Class;
                    oldrecord.Active = 0;

                    db.Tbl_Question_Data.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Class).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Active).IsModified = true;

                    db.SaveChanges();

                    return Json(new { Result = true, Response = "Class Updated Successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (db.Tbl_Question_Data.Any(x => x.User_ID == login_Model.User_ID && x.Class == model.Class))
                    {
                        return Json(new { Result = false, Response = "Class Already Exists! Please Go To Create Subject Page to Add Subject!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (model.Class == null || model.Class == "" || model.Class == "0") { return Json(new { Result = false, Response = "Please Enter Class." }, JsonRequestBehavior.AllowGet); }

                        model.User_ID = login_Model.User_ID;
                        model.Active = 0;

                        db.Tbl_Question_Data.Add(model);
                        db.SaveChanges();

                        return Json(new { Result = true, Response = "Class Added Successfully." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Create_Subject()
        {
            Login_Model login_model = common.Get_Login_Details();

            var model = db.Tbl_Create_School_College.Where(x => x.User_ID == login_model.User_ID).FirstOrDefault();
            TempData["User_ID"] = login_model.User_ID;

            if (model != null && model.Mobile_No != null && model.Email_ID != null && model.Index_No != null && model.Address != null && model.Institute_Name != null && model.Name != null && model.Password != null)
            {
                List<SelectListItem> List = common.Get_Class(login_model.Index_No);
                ViewBag.ClassList = new SelectList(List, "Value", "Text");

                return View();
            }
            else
            {
                return RedirectToAction("Create_User", "Master");
            }
        }

        public JsonResult Delete_Subject_Record(int id)
        {
            try
            {
                var oldrecord = db.Tbl_Question_Data.Where(x => x.ID == id).FirstOrDefault();

                oldrecord.Subject = null;

                db.Tbl_Question_Data.Attach(oldrecord);
                db.Entry(oldrecord).Property(x => x.Subject).IsModified = true;

                db.SaveChanges();

                return Json(new { Result = true, Response = "Subject Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Fail To delete Record." + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Create_Subject(Tbl_Question_Data model)
        {
            try
            {
                if (model.Subject == "0") { return Json(new { Response = "Select Subject!" }); }

                Login_Model login_Model = common.Get_Login_Details();

                if (model.ID != 0)
                {
                    var oldrecord = db.Tbl_Question_Data.Where(x => x.ID == model.ID).FirstOrDefault();

                    oldrecord.Subject = model.Subject;

                    db.Tbl_Question_Data.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Subject).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Active).IsModified = true;

                    db.SaveChanges();

                    return Json(new { Result = true, Response = "Subject Updated Successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (model.Class == null || model.Class == "" || model.Class == "0") { return Json(new { Result = false, Response = "Please Enter Class OR Delete the Class Using Create Class Page!" }, JsonRequestBehavior.AllowGet); }
                    if (model.Subject == null || model.Subject == "" || model.Subject == "0") { return Json(new { Result = false, Response = "Please Enter the Subject." }, JsonRequestBehavior.AllowGet); }

                    model.Index_No = login_Model.Index_No;
                    model.User_ID = login_Model.User_ID;
                    model.Active = 0;

                    db.Tbl_Question_Data.Add(model);
                    db.SaveChanges();

                    return Json(new { Result = true, Response = "Subject Added Successfully!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}