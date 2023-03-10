using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Student_Model
    {
        public HttpPostedFileBase student_file { get; set; }
        public int Class_ID { get; set; }
        public int Division_ID { get; set; }
        public string Division { get; set; }
        public string GR_No { get; set; }
        public string Name { get; set; }
        public string Roll_No { get; set; }
        public int Year_ID { get; set; }
        public double Mobile_No { get; set; }
        public string Email_ID { get; set; }
    
    }
}