using Question_Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using Question_Bank.Helper;
using System.Data;

namespace Question_Bank.Controllers
{
    public class AdminController : BaseController
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();
        // GET: Admin

        public ActionResult Create_School_College()
        {
            return View();
        }

        public ActionResult Edit()
        {
            Login_Model login_model = common.Get_Login_Details();
            TempData["User_ID"] = login_model.User_ID;
            return View();
        }

        [HttpPost]
        public ActionResult Create_School_College(College_Model model)
        {
            string Success = "", Failure = "";
            try
            {
                if (model.college_file == null)
                {
                    if (model.Index_No == null || model.Index_No == "" || model.Index_No == "0") { return Json(new { Result = false, Response = "Please Enter Index No." }, JsonRequestBehavior.AllowGet); }
                    if (model.User_ID == 0) { return Json(new { Result = false, Response = "Please Enter User ID." }, JsonRequestBehavior.AllowGet); }
                    if (model.Institute_Name == null || model.Institute_Name == "" || model.Institute_Name == "0") { return Json(new { Result = false, Response = "Please Enter Institute Name." }, JsonRequestBehavior.AllowGet); }
                    if (model.Name == null || model.Name == "" || model.Name == "0") { return Json(new { Result = false, Response = "Please Enter Name." }, JsonRequestBehavior.AllowGet); }
                    if (model.Password == null || model.Password == "" || model.Password == "0") { return Json(new { Result = false, Response = "Please Enter Password." }, JsonRequestBehavior.AllowGet); }
                    if (model.Mobile_No == 0 || model.Mobile_No.ToString().Length<10) { return Json(new { Result = false, Response = "Please Enter Mobile No." }, JsonRequestBehavior.AllowGet); }
                    if (model.Email_ID == null || model.Email_ID == "" || model.Email_ID == "0") { return Json(new { Result = false, Response = "Please Enter Email ID." }, JsonRequestBehavior.AllowGet); }
                    if (model.Address == null || model.Address == "" || model.Address == "0") { return Json(new { Result = false, Response = "Please Enter Address." }, JsonRequestBehavior.AllowGet); }

                    try
                    {
                        if (!db.Tbl_Create_School_College.Where(a => a.Index_No == model.Index_No.ToString()).Any())
                        {
                            try
                            {
                                Tbl_Create_School_College tbl = new Tbl_Create_School_College();
                                tbl.Index_No = model.Index_No.ToString();
                                tbl.Institute_Name = model.Institute_Name;
                                tbl.User_ID = model.User_ID.ToString();
                                tbl.Name = model.Name;
                                tbl.Mobile_No = model.Mobile_No.ToString();
                                tbl.Email_ID = model.Email_ID;
                                tbl.Address = model.Address;
                                tbl.Password = model.Password;
                                tbl.Active = 1;
                                db.Tbl_Create_School_College.Add(tbl);
                                db.SaveChanges();
                                Success += model.Index_No + ",";
                            }
                            catch (Exception exe)
                            {
                                Failure += model.Index_No + ",";
                            }
                        }
                        else
                        {
                            Failure += model.Index_No + " Already Exists!";
                        }
                    }
                    catch (Exception exe)
                    {
                        Failure += model.Index_No + ",";
                    }
                }
                else
                {
                    string extension = Path.GetExtension(model.college_file.FileName);
                    if (extension == ".xls")
                    {
                        string Path = Server.MapPath("../Upload/College/college_file.xls");
                        model.college_file.SaveAs(Path);
                        DataTable dt = common.Import_To_Grid(Path, ".xls", "Yes");

                        if (dt.Rows.Count > 0)
                        {
                            List<College_Model> list = common.ConvertDataTable_To_Model<College_Model>(dt);
                            foreach (College_Model item in list)
                            {
                                if (item.Index_No != null && item.Index_No != "" && item.Index_No != "0")
                                {
                                    if (!db.Tbl_Create_School_College.Where(a => a.Index_No == item.Index_No).Any())
                                    {
                                        try
                                        {
                                            Tbl_Create_School_College tbl = new Tbl_Create_School_College();
                                            tbl.Index_No = item.Index_No.ToString();
                                            tbl.Institute_Name = item.Institute_Name.ToUpper();
                                            tbl.User_ID = item.User_ID.ToString();
                                            tbl.Name = item.Name.ToUpper();
                                            tbl.Mobile_No = item.Mobile_No.ToString();
                                            tbl.Email_ID = item.Email_ID;
                                            tbl.Address = item.Address;
                                            tbl.Password = item.Password;
                                            tbl.Active = 1;
                                            db.Tbl_Create_School_College.Add(tbl);
                                            db.SaveChanges();
                                            Success += item.Index_No + ",";
                                        }
                                        catch (Exception exe)
                                        {
                                            Failure += item.Index_No + ",";
                                        }
                                    }
                                }
                                else
                                {
                                    Failure += item.Index_No + "Already Exists!";
                                }
                            }

                        }
                    }
                }

                return Json(new { Result = true, Message = "Record Save Successfully", Success = "Successfully Inserted Index no:- " + Success, Failure = "Fail to add Index no:- " + Failure }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Unable to Save Record" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(Tbl_Create_School_College model)
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
                    oldrecord.User_ID = model.User_ID;
                    oldrecord.Address = model.Address;
                    oldrecord.Institute_Name = model.Institute_Name;
                    oldrecord.Password = model.Password;
                    oldrecord.Name = model.Name;
                    oldrecord.Active = 1;

                    db.Tbl_Create_School_College.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Mobile_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Email_ID).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Index_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.User_ID).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Address).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Active).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Institute_Name).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Password).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Name).IsModified = true;

                    db.SaveChanges();
                }
                return Json(new { Result = true, Response = "Record Updated successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Generate_Result()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Generate_Result(Question_Paper_Model model)
        {
            var list = db.Tbl_Question_Data.Where(x => x.Index_No == model.Index_No).ToList();
           
            Question_Paper_Model qmodel = new Question_Paper_Model();

            qmodel.tbl_Model_Answer = list;

            qmodel.tbl_Q1 = db.Tbl_Question1.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q2 = db.Tbl_Question2.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q3 = db.Tbl_Question3.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q4 = db.Tbl_Question4.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q5 = db.Tbl_Question5.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q6 = db.Tbl_Question6.Where(x => x.Index_No == model.Index_No).ToList();
            qmodel.tbl_Q7 = db.Tbl_Question7.Where(x => x.Index_No == model.Index_No).ToList();

            return View(qmodel);
        }

         
    }
}