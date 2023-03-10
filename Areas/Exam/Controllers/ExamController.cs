using Newtonsoft.Json;
using Question_Bank.Helper;
using Question_Bank.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Question_Bank.Areas.Exam.Controllers
{
    public class ExamController : BaseController
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        Common common = new Common();
        // GET: Exam/Exam

        public ActionResult MCQ_Question()
        {
            Exam_Login login_Model = common.Get_Exam_Login_Details();
            return View(login_Model);
        }

        [HttpPost]
        public JsonResult GetTime()
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();

                DateTime log_time = (DateTime)db.Tbl_Student_Login_Details.Where(x => x.Seat_No == login_Model.Seat_No).Select(a => a.Login_Time).FirstOrDefault();
                DateTime exam_time = (DateTime)db.Tbl_Flag.Select(x => x.Total_Exam_Time).FirstOrDefault();

                var l_time = log_time.ToString("hh:mm:ss");
                var e_time = exam_time.ToString("hh:mm:ss");

                TimeSpan time2 = TimeSpan.Parse(e_time);

                DateTime d1 = DateTime.Now;

                DateTime add_dt = log_time + time2;

                TimeSpan sub_dt = add_dt - d1;

                if (d1 <= add_dt)
                {
                    return Json(new { Result = true, Response = sub_dt, End_T = add_dt });
                }
                else
                {
                    return Json(new { Result = false });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        public ActionResult Exam_Login()
        {
            Exam_Login login_Model = common.Get_Exam_Login_Details();
            return View(login_Model);
        }

        public ActionResult Instructions_Page()
        {
            Exam_Login login_Model = common.Get_Exam_Login_Details();

            if (login_Model == null)
            {
                return RedirectToAction("Exam_Login", "Exam", new { area = "Exam" });
            }
            return View(login_Model);
        }

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        [HttpPost]
        public ActionResult Exam_Login(Exam_Login Exam_login_Model)
        {
            try
            {
                var std_login = db.Tbl_Student_Login.Where(x => x.Seat_No == Exam_login_Model.Seat_No && x.Stream == Exam_login_Model.Stream && x.Password == Exam_login_Model.Password).FirstOrDefault();

                if (std_login != null)
                {
                    Exam_login_Model.Seat_No = std_login.Seat_No;
                    Exam_login_Model.User_ID = std_login.User_ID;
                    Exam_login_Model.Index_No = std_login.Index_No;
                    Exam_login_Model.Paper_ID = std_login.Paper_ID;
                    Exam_login_Model.Sub_Code = std_login.Sub_Code;
                    Exam_login_Model.Batch = std_login.Batch;


                    string json = JsonConvert.SerializeObject(Exam_login_Model);
                    FormsAuthentication.SetAuthCookie(json, false);

                    var re_lg = db.Tbl_Student_Login_Details.Where(x => x.Seat_No == std_login.Seat_No).FirstOrDefault();
                    if (re_lg != null)
                    {
                        TempData["Err"] = "Re-Login is Not Allowed!";
                        return RedirectToAction("Exam_Login", "Exam", new { area = "Exam" });
                    }

                    return RedirectToAction("Exam_Screen", "Exam", new { area = "Exam" });
                }
                else
                {
                    TempData["Msg"] = "Invalid Seat Number OR Password OR Stream!";
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something Went Wrong!";
                return RedirectToAction("Exam_Login", "Exam", new { area = "Exam" });
            }

            return RedirectToAction("Exam_Login", "Exam", new { area = "Exam" });
        }

        public ActionResult Exam_Screen()
        {
            Exam_Login login_Model = common.Get_Exam_Login_Details();

            if (login_Model == null)
            {
                return RedirectToAction("Exam_Login", "Exam", new { area = "Exam" });
            }

            if (!db.Tbl_Student_Login_Details.Any(x => x.Seat_No == login_Model.Seat_No))
            {
                Tbl_Student_Login_Details login_Deatils = new Tbl_Student_Login_Details();
                login_Deatils.Index_No = login_Model.Index_No;
                login_Deatils.Seat_No = login_Model.Seat_No;
                login_Deatils.Exam_ID = login_Model.Paper_ID;
                login_Deatils.Login_Time = DateTime.Now;
                db.Tbl_Student_Login_Details.Add(login_Deatils);
                db.SaveChanges();
            }
            var list = db.Tbl_Question_Paper.Where(x => x.User_ID == login_Model.User_ID && x.Index_No == login_Model.Index_No).ToList();

            Question_Paper_Model qmodel = new Question_Paper_Model();

            qmodel.tbl_Question_Papers = list;

            qmodel.tbl_Q1 = db.Tbl_Question1.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q2 = db.Tbl_Question2.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q3 = db.Tbl_Question3.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q4 = db.Tbl_Question4.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q5 = db.Tbl_Question5.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q6 = db.Tbl_Question6.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();
            qmodel.tbl_Q7 = db.Tbl_Question7.Where(x => x.Seat_No == login_Model.Seat_No && x.Index_No == login_Model.Index_No).ToList();

            qmodel.login = login_Model;

            return View(qmodel);
        }

        [HttpPost]
        public JsonResult Submit_Question1(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();

                //if (db.Tbl_Question1.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 1 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}

                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans == null)
                        {
                            cnt++;
                        }

                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 1!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var old_rec = db.Tbl_Question1.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question1 tbl_Q1 = new Tbl_Question1();

                            tbl_Q1.User_ID = login_Model.User_ID;
                            tbl_Q1.DateTime = DateTime.Now;
                            tbl_Q1.IP_Address = GetIp();
                            tbl_Q1.Index_No = model.Index_No.Trim();
                            tbl_Q1.Seat_No = model.Seat_No.Trim();
                            tbl_Q1.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q1.Paper_No = model.Paper_No.Trim();
                            tbl_Q1.Question_ID = answer.ID;
                            tbl_Q1.Question_No = item.QNO;
                            tbl_Q1.Question = qpaper.Question;

                            if (item.Ans != null)
                            {
                                tbl_Q1.Answer = item.Ans;
                                tbl_Q1.Active = 1;
                            }
                            else
                            {
                                tbl_Q1.Active = 0;
                            }

                            db.Tbl_Question1.Add(tbl_Q1);
                            db.SaveChanges();
                        }
                        Match_Answer(1);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question1 _Question1_Edit = old_rec[i];
                                _Question1_Edit.Answer = item.Ans;

                                db.Tbl_Question1.Add(_Question1_Edit);
                                db.Entry(_Question1_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(1);
                    }
                }

                return Json(new { Result = true, Response = "Question Number 1 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question2(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();

                //if (db.Tbl_Question2.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 2 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans == null)
                        {
                            cnt++;
                        }

                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 2!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var old_rec = db.Tbl_Question2.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question2 tbl_Q2 = new Tbl_Question2();

                            tbl_Q2.User_ID = login_Model.User_ID;
                            tbl_Q2.DateTime = DateTime.Now;
                            tbl_Q2.IP_Address = GetIp();
                            tbl_Q2.Index_No = model.Index_No.Trim();
                            tbl_Q2.Seat_No = model.Seat_No.Trim();
                            tbl_Q2.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q2.Paper_No = model.Paper_No.Trim();
                            tbl_Q2.Question_ID = answer.ID;
                            tbl_Q2.Question_No = item.QNO;
                            tbl_Q2.Question = qpaper.Question;

                            if (item.Ans != null)
                            {
                                tbl_Q2.Answer = item.Ans;
                                tbl_Q2.Active = 1;
                            }
                            else
                            {
                                tbl_Q2.Active = 0;
                            }

                            db.Tbl_Question2.Add(tbl_Q2);
                            db.SaveChanges();
                        }
                        Match_Answer(2);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question2 _Question2_Edit = old_rec[i];
                                _Question2_Edit.Answer = item.Ans;

                                db.Tbl_Question2.Add(_Question2_Edit);
                                db.Entry(_Question2_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(2);
                    }
                }
                    return Json(new { Result = true, Response = "Question Number 2 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question3(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();
                //if (db.Tbl_Question3.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 3 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans == null)
                        {
                            cnt++;
                        }

                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 3!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var old_rec = db.Tbl_Question3.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question3 tbl_Q3 = new Tbl_Question3();

                            tbl_Q3.User_ID = login_Model.User_ID;
                            tbl_Q3.DateTime = DateTime.Now;
                            tbl_Q3.IP_Address = GetIp();
                            tbl_Q3.Index_No = model.Index_No.Trim();
                            tbl_Q3.Seat_No = model.Seat_No.Trim();
                            tbl_Q3.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q3.Paper_No = model.Paper_No.Trim();
                            tbl_Q3.Question_ID = answer.ID;
                            tbl_Q3.Question_No = item.QNO;
                            tbl_Q3.Question = qpaper.Question;
                            if (item.Ans != null)
                            {
                                tbl_Q3.Answer = item.Ans;
                                tbl_Q3.Active = 1;
                            }
                            else
                            {
                                tbl_Q3.Active = 0;
                            }
                            db.Tbl_Question3.Add(tbl_Q3);
                            db.SaveChanges();
                        }
                        Match_Answer(3);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question3 _Question3_Edit = old_rec[i];
                                _Question3_Edit.Answer = item.Ans;

                                db.Tbl_Question3.Add(_Question3_Edit);
                                db.Entry(_Question3_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(3);
                    }
                }
                    return Json(new { Result = true, Response = "Question Number 3 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question4(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();
                //if (db.Tbl_Question4.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 4 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans1 == null && check.Ans2 == null)
                        {
                            cnt++;
                        }
                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 4!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var old_rec = db.Tbl_Question4.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question4 tbl_Q4 = new Tbl_Question4();

                            tbl_Q4.User_ID = login_Model.User_ID;
                            tbl_Q4.DateTime = DateTime.Now;
                            tbl_Q4.IP_Address = GetIp();
                            tbl_Q4.Index_No = model.Index_No.Trim();
                            tbl_Q4.Seat_No = model.Seat_No.Trim();
                            tbl_Q4.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q4.Paper_No = model.Paper_No.Trim();
                            tbl_Q4.Question_ID = answer.ID;
                            tbl_Q4.Question_No = item.QNO;
                            tbl_Q4.Question = qpaper.Question;
                            if (item.Ans1 != null && item.Ans2 != null)
                            {
                                tbl_Q4.Answer1 = item.Ans1;
                                tbl_Q4.Answer2 = item.Ans2;
                                tbl_Q4.Active = 1;
                            }
                            else
                            {
                                tbl_Q4.Active = 0;
                            }

                            db.Tbl_Question4.Add(tbl_Q4);
                            db.SaveChanges();
                        }
                        Match_Answer(4);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question4 _Question4_Edit = old_rec[i];
                                _Question4_Edit.Answer1 = item.Ans1;
                                _Question4_Edit.Answer2 = item.Ans2;

                                db.Tbl_Question4.Add(_Question4_Edit);
                                db.Entry(_Question4_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(4);
                    }
                }
                    return Json(new { Result = true, Response = "Question Number 4 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question5(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();
                //if (db.Tbl_Question5.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 5 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans1 == null && check.Ans2 == null && check.Ans3 == null)
                        {
                            cnt++;
                        }
                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 5!" }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    var old_rec = db.Tbl_Question5.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question5 tbl_Q5 = new Tbl_Question5();

                            tbl_Q5.User_ID = login_Model.User_ID;
                            tbl_Q5.DateTime = DateTime.Now;
                            tbl_Q5.IP_Address = GetIp();
                            tbl_Q5.Index_No = model.Index_No.Trim();
                            tbl_Q5.Seat_No = model.Seat_No.Trim();
                            tbl_Q5.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q5.Paper_No = model.Paper_No.Trim();
                            tbl_Q5.Question_ID = answer.ID;
                            tbl_Q5.Question_No = item.QNO;
                            tbl_Q5.Question = qpaper.Question;
                            if (item.Ans1 != null && item.Ans2 != null && item.Ans3 != null)
                            {
                                tbl_Q5.Answer1 = item.Ans1;
                                tbl_Q5.Answer2 = item.Ans2;
                                tbl_Q5.Answer3 = item.Ans3;
                                tbl_Q5.Active = 1;
                            }
                            else
                            {
                                tbl_Q5.Active = 0;
                            }
                            db.Tbl_Question5.Add(tbl_Q5);
                            db.SaveChanges();
                        }
                        Match_Answer(5);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question5 _Question5_Edit = old_rec[i];
                                _Question5_Edit.Answer1 = item.Ans1;
                                _Question5_Edit.Answer2 = item.Ans2;
                                _Question5_Edit.Answer3 = item.Ans3;

                                db.Tbl_Question5.Add(_Question5_Edit);
                                db.Entry(_Question5_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(5);
                    }

                }
                    return Json(new { Result = true, Response = "Question Number 5 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question6(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();
                //if (db.Tbl_Question6.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 6 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;

                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans == null)
                        {
                            cnt++;
                        }
                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 6!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var old_rec = db.Tbl_Question6.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qpaper = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qpaper.Question_ID).FirstOrDefault();

                            Tbl_Question6 tbl_Q6 = new Tbl_Question6();

                            tbl_Q6.User_ID = login_Model.User_ID;
                            tbl_Q6.DateTime = DateTime.Now;
                            tbl_Q6.IP_Address = GetIp();
                            tbl_Q6.Index_No = model.Index_No.Trim();
                            tbl_Q6.Seat_No = model.Seat_No.Trim();
                            tbl_Q6.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q6.Paper_No = model.Paper_No.Trim();
                            tbl_Q6.Question_ID = answer.ID;
                            tbl_Q6.Question_No = item.QNO;
                            tbl_Q6.Question = qpaper.Question;
                            if (item.Ans != null)
                            {
                                tbl_Q6.Answer = item.Ans;
                                tbl_Q6.Active = 1;
                            }
                            else
                            {
                                tbl_Q6.Active = 0;
                            }
                            db.Tbl_Question6.Add(tbl_Q6);
                            db.SaveChanges();
                        }
                        Match_Answer(6);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question6 _Question6_Edit = old_rec[i];
                                _Question6_Edit.Answer = item.Ans;

                                db.Tbl_Question6.Add(_Question6_Edit);
                                db.Entry(_Question6_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(6);
                    }
                }
               
                    return Json(new { Result = true, Response = "Question Number 6 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Submit_Question7(Submit_Question_Model model)
        {
            try
            {
                Exam_Login login_Model = common.Get_Exam_Login_Details();
                //if (db.Tbl_Question7.Any(x => x.Seat_No == model.Seat_No.Trim()))
                //{
                //    return Json(new { Result = false, Response = "Question Number 7 has already been Submitted!" }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                if (model != null)
                {
                    int cnt = 0;
                    foreach (var check in model.Question1_List)
                    {
                        if (check.Ans == null)
                        {
                            cnt++;
                        }
                        if (cnt == model.Question1_List.Count)
                        {
                            return Json(new { Result = false, Response = "Please Enter the Answer for Question Number 7!" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var old_rec = db.Tbl_Question7.Where(x => x.Seat_No == model.Seat_No.Trim()).ToList();

                    if (old_rec.Count == 0)
                    {
                        foreach (var item in model.Question1_List)
                        {
                            var qid = db.Tbl_Question_Paper.Where(x => x.ID == item.ID).FirstOrDefault();
                            var answer = db.Tbl_Question_Data.Where(n => n.ID == qid.Question_ID).FirstOrDefault();

                            Tbl_Question7 tbl_Q7 = new Tbl_Question7();

                            tbl_Q7.User_ID = login_Model.User_ID;
                            tbl_Q7.DateTime = DateTime.Now;
                            tbl_Q7.IP_Address = GetIp();
                            tbl_Q7.Index_No = model.Index_No.Trim();
                            tbl_Q7.Seat_No = model.Seat_No.Trim();
                            tbl_Q7.Sub_Code = model.Sub_Code.Trim();
                            tbl_Q7.Paper_No = model.Paper_No.Trim();
                            tbl_Q7.Question_ID = answer.ID;
                            tbl_Q7.Question_No = item.QNO;
                            tbl_Q7.Question = qid.Question;
                            if (item.Ans != null)
                            {
                                tbl_Q7.Answer = item.Ans;
                                tbl_Q7.Active = 1;
                            }
                            else
                            {
                                tbl_Q7.Active = 0;
                            }
                            db.Tbl_Question7.Add(tbl_Q7);
                            db.SaveChanges();
                        }
                        Match_Answer(7);
                    }
                    else
                    {
                        int i = 0;
                        foreach (var item in model.Question1_List)
                        {
                            if (i <= old_rec.Count)
                            {
                                Tbl_Question7 _Question7_Edit = old_rec[i];
                                _Question7_Edit.Answer = item.Ans;

                                db.Tbl_Question7.Add(_Question7_Edit);
                                db.Entry(_Question7_Edit).State = EntityState.Modified;

                                db.SaveChanges();
                                i++;
                            }
                        }
                        Match_Answer(7);
                    }
                }
                    return Json(new { Result = true, Response = "Question Number 7 Submitted Successfully!" }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Response = "Something Went Wrong!" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public void Match_Answer(int Q_No)
        {
            Exam_Login login_Model = common.Get_Exam_Login_Details();

            if (Q_No == 1)
            {
                var list = db.Tbl_Question1.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if (answer.Answer1 == item.Answer)
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question1.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question1.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 2)
            {
                var list = db.Tbl_Question2.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if (answer.Answer1 == item.Answer)
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question2.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question2.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 3)
            {
                var list = db.Tbl_Question3.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if (answer.Answer1 == item.Answer)
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question3.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question3.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 4)
            {
                var list = db.Tbl_Question4.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if ((answer.Answer1 == item.Answer1 || answer.Answer1 == item.Answer2) && (answer.Answer2 == item.Answer1 || answer.Answer2 == item.Answer2))
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer1 = answer.Answer1;
                        item.Model_Answer2 = answer.Answer2;
                        db.Tbl_Question4.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer1).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer2).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer1 = answer.Answer1;
                        item.Model_Answer2 = answer.Answer2;
                        db.Tbl_Question4.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer1).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer2).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 5)
            {
                var list = db.Tbl_Question5.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if ((answer.Answer1 == item.Answer1 || answer.Answer1 == item.Answer2 || answer.Answer1 == item.Answer3) && (answer.Answer2 == item.Answer1 || answer.Answer2 == item.Answer2 || answer.Answer2 == item.Answer3) && (answer.Answer3 == item.Answer1 || answer.Answer3 == item.Answer2 || answer.Answer3 == item.Answer3))
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer1 = answer.Answer1;
                        item.Model_Answer2 = answer.Answer2;
                        item.Model_Answer3 = answer.Answer3;
                        db.Tbl_Question5.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer1).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer2).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer3).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer1 = answer.Answer1;
                        item.Model_Answer2 = answer.Answer2;
                        item.Model_Answer3 = answer.Answer3;
                        db.Tbl_Question5.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer1).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer2).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer3).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 6)
            {
                var list = db.Tbl_Question6.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if (answer.Answer1 == item.Answer)
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question6.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question6.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
            if (Q_No == 7)
            {
                var list = db.Tbl_Question7.Where(n => n.Seat_No == login_Model.Seat_No && n.Index_No == login_Model.Index_No).ToList();
                foreach (var item in list)
                {
                    var answer = db.Tbl_Question_Data.Where(n => n.ID == item.Question_ID).FirstOrDefault();
                    if (answer.Answer1 == item.Answer)
                    {
                        item.Correct_Answer = "1";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question7.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                    else
                    {
                        item.Correct_Answer = "0";
                        item.Model_Answer = answer.Answer1;
                        db.Tbl_Question7.Attach(item);
                        db.Entry(item).Property(x => x.Correct_Answer).IsModified = true;
                        db.Entry(item).Property(x => x.Model_Answer).IsModified = true;
                        db.SaveChanges();
                    }
                }
            }
        }

    }
}