using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class MCQ_Question_Model
    {
        public List<Tbl_Question3> tbl_Q3 { get; set; }
        public List<MCQ_Question> questions { get; set; }
        public List<Tbl_Question_Paper> tbl_Question_Papers { get; set; }
        public List<Tbl_Question_Data> tbl_Model_Answer { get; set; }
        public Exam_Login Login { get; set; }
    }

    public class MCQ_Question
    {
        public string Question { get; set; }
        public int ID { get; set; }
        public int Question_ID { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
    }
}