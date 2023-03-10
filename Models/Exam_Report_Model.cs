using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Exam_Report_Model
    {
        public List<Tbl_Question1> _Question1 { get; set; }
        public List<Tbl_Question2> _Question2 { get; set; }
        public List<Tbl_Question3> _Question3 { get; set; }
        public List<Tbl_Question4> _Question4 { get; set; }
        public List<Tbl_Question5> _Question5 { get; set; }
        public List<Tbl_Question6> _Question6 { get; set; }
        public List<Tbl_Question7> _Question7 { get; set; }
    }
}