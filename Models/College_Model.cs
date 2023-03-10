using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class College_Model
    {
        public HttpPostedFileBase college_file { get; set; }
        public string Index_No { get; set; }
        public string Institute_Name { get; set; }
        public double User_ID { get; set; }
        public string Name { get; set; }
        public double Mobile_No { get; set; }
        public string Email_ID { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}