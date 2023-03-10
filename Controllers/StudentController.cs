using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Question_Bank.Helper;
using Question_Bank.Models;
using Question_Bank.Areas.Exam.Controllers;

namespace Question_Bank.Controllers
{
    public class StudentController : BaseController
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();
        // GET: Student


        public ActionResult Add_Student_By_Excel()
        {
            Login_Model login_Model = common.Get_Login_Details();

            ViewBag.Class = new SelectList(common.Get_Class(login_Model.Index_No), "Value", "Text");

            List<Tbl_Year> YearList = db.Tbl_Year.ToList();
            ViewBag.Year = new SelectList(YearList, "ID", "Year");
            return View();
        }
        [HttpPost]
        public JsonResult Add_Student_By_Excel(Student_Model model)
        {
            string Success = "", Failure = "";
            try
            {
                Login_Model login_Model = common.Get_Login_Details();
                ExamController ex = new ExamController();
                if (model.Class_ID == 0) { return Json(new { Result = false, Message = "Please Select Class" }, JsonRequestBehavior.AllowGet); }
                if (model.Division_ID == 0) { return Json(new { Result = false, Message = "Please Select Division" }, JsonRequestBehavior.AllowGet); }
                if (model.Year_ID == 0) { return Json(new { Result = false, Message = "Please Select Year" }, JsonRequestBehavior.AllowGet); }

                string extension = Path.GetExtension(model.student_file.FileName);
                if (extension == ".xls")
                {
                    string Path = Server.MapPath("../Upload/Student/student_file.xls");
                    model.student_file.SaveAs(Path);
                    DataTable dt = common.Import_To_Grid(Path, ".xls", "Yes");

                    if (dt.Rows.Count > 0)
                    {
                        List<Student_Model> list = common.ConvertDataTable_To_Model<Student_Model>(dt);
                        foreach (Student_Model item in list)
                        {
                            if (item.GR_No != null && item.GR_No != "")
                            {
                                if (!db.Tbl_Student.Where(a => a.GR_No == item.GR_No).Any())
                                {
                                    try
                                    {
                                        Tbl_Student tbl = new Tbl_Student();
                                        tbl.DateTime = DateTime.Now;
                                        tbl.IP_Address = ex.GetIp();
                                        tbl.Index_No = login_Model.Index_No;
                                        tbl.Class_ID = model.Class_ID;
                                        tbl.Division_ID = model.Division_ID;
                                        tbl.GR_No = Convert.ToString(item.GR_No);
                                        tbl.Name = item.Name.ToUpper();
                                        tbl.Seat_No = item.Roll_No;
                                        tbl.Mobile_No = item.Mobile_No;
                                        tbl.Email_ID = item.Email_ID;
                                        tbl.Active = 1;
                                        db.Tbl_Student.Add(tbl);
                                        db.SaveChanges();
                                        int Record_ID = tbl.ID;
                                        if (Record_ID > 0)
                                        {
                                            Tbl_Student_Details tbl_Student_Details = new Tbl_Student_Details();
                                            tbl_Student_Details.DateTime = DateTime.Now;
                                            tbl_Student_Details.IPAddress = ex.GetIp();
                                            tbl_Student_Details.Record_ID = Record_ID;
                                            tbl_Student_Details.Index_No = login_Model.Index_No;
                                            tbl_Student_Details.Class_ID = model.Class_ID;
                                            tbl_Student_Details.Division_ID = model.Division_ID;
                                            tbl_Student_Details.GR_No = Convert.ToString(item.GR_No);
                                            tbl_Student_Details.Year_ID = Convert.ToInt32(model.Year_ID);

                                            tbl_Student_Details.Active = 1;
                                            db.Tbl_Student_Details.Add(tbl_Student_Details);
                                            db.SaveChanges();
                                            Success += item.Roll_No + ",";
                                        }

                                    }
                                    catch (Exception exe)
                                    {
                                        Failure += item.Roll_No + ",";
                                    }
                                }
                            }
                        }

                    }
                }
                return Json(new { Result = true, Message = "Record Save Successfuly", Success = "Successfully Inserted Roll no:- " + Success, Failure = "Fail to add Roll no:- " + Failure }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Unable to Save Record" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Get_Division_List(int Class_ID)
        {
            try
            {
                Login_Model login_Model = common.Get_Login_Details();

                var tbl = db.Tbl_Division.Where(a => a.Class_ID == Class_ID && login_Model.Index_No == a.Index_No).ToList();

                return Json(new { Result = true, List = tbl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Unable To Delete Record" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit_Student()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Edit_Student(Tbl_Student model)
        {
            try
            {
                if (model.ID != 0)
                {
                    var oldrecord = db.Tbl_Student.Where(x => x.ID == model.ID).FirstOrDefault();

                    oldrecord.Index_No = model.Index_No;
                    oldrecord.Class_ID = model.Class_ID;
                    oldrecord.Division_ID = model.Division_ID;
                    oldrecord.GR_No = model.GR_No;
                    oldrecord.Name = model.Name.ToUpper();
                    oldrecord.Seat_No = model.Seat_No;
                    oldrecord.Mobile_No = model.Mobile_No;
                    oldrecord.Email_ID = model.Email_ID;

                    db.Tbl_Student.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Index_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Class_ID).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Division_ID).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.GR_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Name).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Seat_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Mobile_No).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Email_ID).IsModified = true;

                    db.SaveChanges();
                 
                    return Json(new { Result = true, Response = "Record Updated successfully." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = false, Response = "Something Went Wrong!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_Data()
        {
            try
            {
                Login_Model login_Model = common.Get_Login_Details();
                var tbl = db.Tbl_Student.Where(x => x.Index_No == login_Model.Index_No).ToList();

                return Json(new { Result = true, Response = tbl }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exe)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + exe }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult Get_Class_Record(int id)
        {
            var record = db.Tbl_Student.Where(x => x.ID == id).FirstOrDefault();

            return Json(new { Result = true, Response = record }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete_Class_Record(int id)
        {
            try
            {
                var oldrecord = db.Tbl_Student.Where(x => x.ID == id).FirstOrDefault();

                db.Tbl_Student.Remove(oldrecord);
                db.SaveChanges();

                return Json(new { Result = true, Response = "Record Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Fail To Delete Record." + ex }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}