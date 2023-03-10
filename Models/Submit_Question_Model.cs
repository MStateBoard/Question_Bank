using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Q1Model
    {
        public int ID { get; set; }
        public string Ans { get; set; }
        public string Ans1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }
        public string QNO { get; set; }
    }
    public class Submit_Question_Model
    {
        public string Seat_No { get; set; }
        public string Sub_Code { get; set; }
        public string Index_No { get; set; }
        public string Paper_No { get; set; }
        public List<Q1Model> Question1_List { get; set; }
    }
}