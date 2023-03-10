using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Question_Paper_Model
    {
        public List<Tbl_Question_Paper> tbl_Question_Papers { get; set; }
        public List<Tbl_Question_Data> tbl_Model_Answer { get; set; }
        public List<Tbl_Question1> tbl_Q1 { get; set; }
        public List<Tbl_Question2> tbl_Q2 { get; set; }
        public List<Tbl_Question3> tbl_Q3 { get; set; }
        public List<Tbl_Question4> tbl_Q4 { get; set; }
        public List<Tbl_Question5> tbl_Q5 { get; set; }
        public List<Tbl_Question6> tbl_Q6 { get; set; }
        public List<Tbl_Question7> tbl_Q7 { get; set; }
        public Exam_Login login { get; set; }
        public string Index_No { get; set; }
    }
}