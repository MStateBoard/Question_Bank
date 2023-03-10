using Question_Bank.Helper;
using Question_Bank.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Question_Bank.Controllers
{
    public class QuestionController : BaseController
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();
        // GET: Question
        public JsonResult Delete_Question_Record(int id)
        {
            try
            {
                var oldrecord = db.Tbl_Question_Paper.Where(x => x.ID == id).FirstOrDefault();
                var deact = db.Tbl_Question_Data.Where(a => a.ID == oldrecord.Question_ID).FirstOrDefault();

                deact.Active = 0;
                
                db.Tbl_Question_Data.Add(deact);
                db.Entry(deact).State = EntityState.Modified;
                
                db.SaveChanges();

                db.Tbl_Question_Paper.Remove(oldrecord);
                db.SaveChanges();

                return Json(new { Result = true, Response = "Record Deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Fail To Delete Record." + ex }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get_Question_Record(int id)
        {
            var record = db.Tbl_Question_Paper.Where(x => x.ID == id).FirstOrDefault();

            return Json(new { Result = true, Response = record }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit_Question_Paper()
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

        public ActionResult Create_Question_Bank()
        {
            Login_Model login_model = common.Get_Login_Details();
            var model = db.Tbl_Create_School_College.Where(x => x.User_ID == login_model.User_ID).FirstOrDefault();
            var qmodel = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID).ToList();
            TempData["User_ID"] = login_model.User_ID;
            Get_Que_Count();

            if (model != null && model.Mobile_No != null && model.Email_ID != null && model.Index_No != null && model.Address != null && model.Institute_Name != null && model.Name != null && model.Password != null)
            {
                if (qmodel.Count == 0) { return RedirectToAction("Create_Class", "Master"); }
                foreach (var item in qmodel)
                {
                    if (item.Subject == null)
                    {
                        return RedirectToAction("Create_Subject", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Create_User", "Master");
            }

            return View(qmodel);
        }

        public ActionResult Edit_Question_Bank()
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

        [HttpPost]
        public JsonResult Add_Question(Question_Model model)
        {
            try
            {
                var oldrecord = db.Tbl_Question_Data.Where(x => x.ID == model.ID).FirstOrDefault();

                if (oldrecord.Question_Type == null || oldrecord.Question_Type == "0" || oldrecord.Question_Type == "" || oldrecord.Question == null || oldrecord.Question == "0" || oldrecord.Question == "")
                {
                    if (model.Question == null || model.Question == "0" || model.Question == "") { return Json(new { Result = false, Response = "Please Enter Question." }, JsonRequestBehavior.AllowGet); }
                    if (model.Question_Type == null || model.Question_Type == "0" || model.Question_Type == "") { return Json(new { Result = false, Response = "Please Enter Question Type." }, JsonRequestBehavior.AllowGet); }

                    oldrecord.Question_Type = model.Question_Type;

                    if (model.Question_Type == "Single Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        if (model.Ans1 == "A") { oldrecord.Answer1 = model.Option1; } else { oldrecord.Answer1 = model.Option2; }
                    }
                    if (model.Question_Type == "Double Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "" || model.Ans2 == null || model.Ans2 == "0" || model.Ans2 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        if (model.Ans1 == "A") { oldrecord.Answer1 = model.Option1; } else if(model.Ans1 == "B") { oldrecord.Answer1 = model.Option2; } else if(model.Ans1 == "C") { oldrecord.Answer1 = model.Option3; } else if (model.Ans1 == "D") { oldrecord.Answer1 = model.Option4; }
                        if (model.Ans2 == "A") { oldrecord.Answer2 = model.Option1; } else if(model.Ans2 == "B") { oldrecord.Answer2 = model.Option2; } else if(model.Ans2 == "C") { oldrecord.Answer2 = model.Option3; } else if (model.Ans2 == "D") { oldrecord.Answer2 = model.Option4; }
                        
                    }
                    if (model.Question_Type == "Triple Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "" || model.Option5 == null || model.Option5 == "0" || model.Option5 == "" || model.Option6 == null || model.Option6 == "0" || model.Option6 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "" || model.Ans2 == null || model.Ans2 == "0" || model.Ans2 == "" || model.Ans3 == null || model.Ans3 == "0" || model.Ans3 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = model.Option5;
                        oldrecord.Option6 = model.Option6;
                        if (model.Ans1 == "A") { oldrecord.Answer1 = model.Option1; } else if (model.Ans1 == "B") { oldrecord.Answer1 = model.Option2; } else if (model.Ans1 == "C") { oldrecord.Answer1 = model.Option3; } else if (model.Ans1 == "D") { oldrecord.Answer1 = model.Option4; } else if (model.Ans1 == "E") { oldrecord.Answer1 = model.Option5; } else if (model.Ans1 == "F") { oldrecord.Answer1 = model.Option6; }
                        if (model.Ans2 == "A") { oldrecord.Answer2 = model.Option1; } else if (model.Ans2 == "B") { oldrecord.Answer2 = model.Option2; } else if (model.Ans2 == "C") { oldrecord.Answer2 = model.Option3; } else if (model.Ans2 == "D") { oldrecord.Answer2 = model.Option4; } else if (model.Ans2 == "E") { oldrecord.Answer2 = model.Option5; } else if (model.Ans2 == "F") { oldrecord.Answer2 = model.Option6; }
                        if (model.Ans3 == "A") { oldrecord.Answer3 = model.Option1; } else if (model.Ans3 == "B") { oldrecord.Answer3 = model.Option2; } else if (model.Ans3 == "C") { oldrecord.Answer3 = model.Option3; } else if (model.Ans3 == "D") { oldrecord.Answer3 = model.Option4; } else if (model.Ans3 == "E") { oldrecord.Answer3 = model.Option5; } else if (model.Ans3 == "F") { oldrecord.Answer3 = model.Option6; }
                       
                    }
                    if (model.Question_Type == "Fill in the Blanks")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "") { return Json(new { Result = false, Response = "Please Enter the Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Answer1 = model.Option1;
                    }
                    if (model.Question_Type == "True/False")
                    {
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = "True";
                        oldrecord.Option2 = "False";
                        oldrecord.Answer1 = model.Ans1;
                    }
                    if (model.Question_Type == "Match the Following")
                    {
                        if (model.Heading1 == null || model.Heading1 == "0" || model.Heading1 == "" || model.Heading2 == null || model.Heading2 == "0" || model.Heading2 == "" || model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "" || model.Option5 == null || model.Option5 == "0" || model.Option5 == "" || model.Option6 == null || model.Option6 == "0" || model.Option6 == "" || model.Option7 == null || model.Option7 == "0" || model.Option7 == "" || model.Option8 == null || model.Option8 == "0" || model.Option8 == "") { return Json(new { Result = false, Response = "Please Enter Headings OR Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Enter Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = "(H1) " + model.Heading1 + " (Op1) " + model.Option1 + " (Op2) " + model.Option2 + " (Op3) " + model.Option3 + " (Op4) " + model.Option4 + " (H2) " + model.Heading2 + " (OpA) " + model.Option5 + " (OpB) " + model.Option6 + " (OpC) " + model.Option7 + " (OpD) " + model.Option8;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        if (model.Ans1 == "Option I") { oldrecord.Answer1 = model.Option1; } else if(model.Ans1 == "Option II") { oldrecord.Answer1 = model.Option2; } else if (model.Ans1 == "Option III") { oldrecord.Answer1 = model.Option3; } else if (model.Ans1 == "Option IV") { oldrecord.Answer1 = model.Option4; }
                        
                    }
                    if (model.Question_Type == "Rearrange")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        if (model.Ans1 == "A") { oldrecord.Answer1 = model.Option1; } else if (model.Ans1 == "B") { oldrecord.Answer1 = model.Option2; } else if (model.Ans1 == "C") { oldrecord.Answer1 = model.Option3; } else if (model.Ans1 == "D") { oldrecord.Answer1 = model.Option4; }
                        
                    }
                    if (model.Question_Type == "Theory Question")
                    {
                        if (model.TAns1 == null || model.TAns1 == "0" || model.TAns1 == "" || model.Marks == null || model.Marks == "0" || model.Marks == "") { return Json(new { Result = false, Response = "Please Enter Answer OR Marks!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Answer1 = model.TAns1;
                        oldrecord.Marks = model.Marks;
                    }
                    db.Tbl_Question_Data.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Question_Type).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Question).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option1).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option2).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option3).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option4).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option5).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option6).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer1).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer2).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer3).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Marks).IsModified = true;

                    db.SaveChanges();
                    return Json(new { Result = true, Response = "Question Added Successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (model.Question_Type == null || model.Question_Type == "0" || model.Question_Type == "") { return Json(new { Result = false, Response = "Please Enter Question Type." }, JsonRequestBehavior.AllowGet); }
                    if (model.Question == null || model.Question == "0" || model.Question == "") { return Json(new { Result = false, Response = "Please Enter Question." }, JsonRequestBehavior.AllowGet); }
                    oldrecord.Question_Type = model.Question_Type;
                    if (model.Question_Type == "Single Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Double Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "" || model.Ans2 == null || model.Ans2 == "0" || model.Ans2 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = model.Ans2;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Triple Correct")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "" || model.Option5 == null || model.Option5 == "0" || model.Option5 == "" || model.Option6 == null || model.Option6 == "0" || model.Option6 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "" || model.Ans2 == null || model.Ans2 == "0" || model.Ans2 == "" || model.Ans3 == null || model.Ans3 == "0" || model.Ans3 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = model.Option5;
                        oldrecord.Option6 = model.Option6;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = model.Ans2;
                        oldrecord.Answer3 = model.Ans3;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Fill in the Blanks")
                    {
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Enter the Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = null;
                        oldrecord.Option2 = null;
                        oldrecord.Option3 = null;
                        oldrecord.Option4 = null;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "True/False")
                    {
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = "True";
                        oldrecord.Option2 = "False";
                        oldrecord.Option3 = null;
                        oldrecord.Option4 = null;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Match the Following")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter Question OR Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Enter Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Rearrange")
                    {
                        if (model.Option1 == null || model.Option1 == "0" || model.Option1 == "" || model.Option2 == null || model.Option2 == "0" || model.Option2 == "" || model.Option3 == null || model.Option3 == "0" || model.Option3 == "" || model.Option4 == null || model.Option4 == "0" || model.Option4 == "") { return Json(new { Result = false, Response = "Please Enter the Options!" }, JsonRequestBehavior.AllowGet); }
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "") { return Json(new { Result = false, Response = "Please Select Answer!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = model.Option1;
                        oldrecord.Option2 = model.Option2;
                        oldrecord.Option3 = model.Option3;
                        oldrecord.Option4 = model.Option4;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    if (model.Question_Type == "Theory Question")
                    {
                        if (model.Ans1 == null || model.Ans1 == "0" || model.Ans1 == "" || model.Marks == null || model.Marks == "0" || model.Marks == "") { return Json(new { Result = false, Response = "Please Enter Answer OR Marks!" }, JsonRequestBehavior.AllowGet); }
                        oldrecord.Question = model.Question;
                        oldrecord.Option1 = null;
                        oldrecord.Option2 = null;
                        oldrecord.Option3 = null;
                        oldrecord.Option4 = null;
                        oldrecord.Option5 = null;
                        oldrecord.Option6 = null;
                        oldrecord.Answer1 = model.Ans1;
                        oldrecord.Answer2 = null;
                        oldrecord.Answer3 = null;
                        oldrecord.Marks = model.Marks;
                    }
                    db.Tbl_Question_Data.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Question_Type).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Question).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option1).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option2).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option3).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option4).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option5).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option6).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer1).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer2).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Answer3).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Marks).IsModified = true;

                    db.SaveChanges();
                    return Json(new { Result = true, Response = "Question Edited Successfully." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public void Get_Count()
        {
            Login_Model login_model = common.Get_Login_Details();
            ViewData["Que1"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Fill in the Blanks" && x.Paper_Name == null).Count();
            ViewData["Que2"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "True/False" && x.Paper_Name == null).Count();
            ViewData["Que3"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Single Correct" && x.Paper_Name == null).Count();
            ViewData["Que4"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Double Correct" && x.Paper_Name == null).Count();
            ViewData["Que5"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Triple Correct" && x.Paper_Name == null).Count();
            ViewData["Que6"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Match the Following" && x.Paper_Name == null).Count();
            ViewData["Que7"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Rearrange" && x.Paper_Name == null).Count();
            ViewData["Que8"] = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Theory Question" && x.Paper_Name == null).Count();
        }

        public void Get_Que_Count()
        {
            Login_Model login_model = common.Get_Login_Details();
            ViewData["Q1"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Fill in the Blanks").Count();
            ViewData["Q2"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "True/False").Count();
            ViewData["Q3"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Single Correct").Count();
            ViewData["Q4"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Double Correct").Count();
            ViewData["Q5"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Triple Correct").Count();
            ViewData["Q6"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Match the Following").Count();
            ViewData["Q7"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Rearrange").Count();
            ViewData["Q8"] = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == "Theory Question").Count();
        }
      

        [HttpPost]
        public JsonResult Load_Subject(string Class)
        {
            Login_Model login_model = common.Get_Login_Details();
            List<SelectListItem> list = common.Get_Subject(login_model.Index_No , Class);
            ViewBag.ClassList = new SelectList(list, "Value", "Text");

            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public ActionResult Create_Question_Paper()
        {
            Login_Model login_model = common.Get_Login_Details();
            Get_Count();

            List<SelectListItem> List = common.Get_Class(login_model.Index_No);
            ViewBag.ClassList = new SelectList(List, "Value", "Text");

            var model = db.Tbl_Create_School_College.Where(x => x.User_ID == login_model.User_ID).FirstOrDefault();
            var qmodel = db.Tbl_Question_Data.Where(x => x.User_ID == login_model.User_ID).ToList();
       
            if (model != null && model.Mobile_No != null && model.Email_ID != null && model.Index_No != null && model.Address != null && model.Institute_Name != null && model.Name != null && model.Password != null)
            {
                if (qmodel.Count == 0) { return RedirectToAction("Create_Class", "Master"); }
                foreach (var item in qmodel)
                {
                    if (item.Subject == null)
                    {
                        return RedirectToAction("Create_Subject", "Master");
                    }
                }
            }
            else
            {
                return RedirectToAction("Create_User", "Master");
            }

            return View();
        }

        [HttpPost]
        public JsonResult Create_Question_Bank(Question_Bank_Model _Model)
        {
            try 
            {
                var model = db.Tbl_Question_Data.Where(x => x.Class == _Model.Class && x.Subject == _Model.Subject && x.Question_Type == _Model.Question_Type && x.Active == 0).ToList();

                return Json(new { Result = true, Response = model }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!"+ex}, JsonRequestBehavior.AllowGet);
            }           
        }

        [HttpPost]
        public ActionResult addQuestions(Question_Bank_Model Question_Data)
        {
            try
            {
                Login_Model login_model = common.Get_Login_Details();
                Tbl_Question_Paper _Question_Paper = new Tbl_Question_Paper();

                if(Question_Data.ID == 0)
                {
                    if (Question_Data.Chk == null || Question_Data.Class == "0" || Question_Data.Class == "") { return Json(new { Result = false, Response = "Select Edit Option!" }, JsonRequestBehavior.AllowGet); }
                    foreach (var item in Question_Data.Chk)
                    {
                        Tbl_Question_Data exists = db.Tbl_Question_Data.Find(Convert.ToInt32(item));

                        int qid = Convert.ToInt32(item);

                        int check_id = db.Tbl_Question_Paper.Where(x => x.User_ID == login_model.User_ID && x.Question_Type == exists.Question_Type && x.Paper_Name == null && x.Question_ID == qid).Count();

                        if (check_id == 0)
                        {
                            _Question_Paper.Question_ID = exists.ID;
                            _Question_Paper.Index_No = login_model.Index_No;
                            _Question_Paper.User_ID = login_model.User_ID;
                            _Question_Paper.Class = exists.Class;
                            _Question_Paper.Subject = exists.Subject;
                            _Question_Paper.Question_Type = exists.Question_Type;
                            _Question_Paper.Question = exists.Question;

                            _Question_Paper.Option1 = exists.Option1;
                            _Question_Paper.Option2 = exists.Option2;
                            _Question_Paper.Option3 = exists.Option3;
                            _Question_Paper.Option4 = exists.Option4;
                            _Question_Paper.Option5 = exists.Option5;
                            _Question_Paper.Option6 = exists.Option6;

                            _Question_Paper.Marks = exists.Marks;
                            _Question_Paper.Active = 1;

                            db.Tbl_Question_Paper.Add(_Question_Paper);
                            db.SaveChanges();

                            exists.Active = 1;
                            db.Tbl_Question_Data.Attach(exists);
                            db.Entry(exists).Property(x => x.Active).IsModified = true;
                            db.SaveChanges();
                            TempData["Temp"] = "Question Added Successfully";
                        }
                        else
                        {
                            TempData["Temp"] = "Question Already Exists";
                            return RedirectToAction("Create_Question_Paper");
                        }
                        
                    }
                    return RedirectToAction("Create_Question_Paper");
                }
                else
                {
                    if(Question_Data.Class == null || Question_Data.Class == "0" || Question_Data.Class == "") { return Json(new { Result = false, Response = "Select Class" }, JsonRequestBehavior.AllowGet); }
                    if(Question_Data.Subject == null || Question_Data.Subject == "0" || Question_Data.Subject == ""){ return Json(new { Result = false, Response = "Select Subject" }, JsonRequestBehavior.AllowGet); }
                    if(Question_Data.Question_Type == null || Question_Data.Question_Type == "0" || Question_Data.Question_Type == "") { return Json(new { Result = false, Response = "Enter Question_Type" }, JsonRequestBehavior.AllowGet); }
                    if(Question_Data.Question == null || Question_Data.Question == "0" || Question_Data.Question == "") { return Json(new { Result = false, Response = "Enter Question" }, JsonRequestBehavior.AllowGet); }

                    var oldrecord = db.Tbl_Question_Paper.Where(x => x.ID == Question_Data.ID).FirstOrDefault();

                    oldrecord.Class = Question_Data.Class;
                    oldrecord.Subject = Question_Data.Subject;
                    oldrecord.Question_Type = Question_Data.Question_Type;
                    oldrecord.Question = Question_Data.Question;
                    oldrecord.Option1 = Question_Data.Option1;
                    oldrecord.Option2 = Question_Data.Option2;
                    oldrecord.Option3 = Question_Data.Option3;
                    oldrecord.Option4 = Question_Data.Option4;
                    oldrecord.Option5 = Question_Data.Option5;
                    oldrecord.Option6 = Question_Data.Option6;
                    oldrecord.Marks = Question_Data.Marks;
                    oldrecord.Active = 1;

                    db.Tbl_Question_Paper.Attach(oldrecord);
                    db.Entry(oldrecord).Property(x => x.Class).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Subject).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Question_Type).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Question).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option1).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option2).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option3).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option4).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option5).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Option6).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Marks).IsModified = true;
                    db.Entry(oldrecord).Property(x => x.Active).IsModified = true;

                    db.SaveChanges();
                }
                return Json(new { Result = true, Response = "Question Edited Successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}