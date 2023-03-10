using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Question_Model
    {
        public int ID { get; set; }
        public string Question_Type { get; set; }
        public string Question { get; set; }

        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }
        public string MOption1 { get; set; } 
        public string MOption2 { get; set; } 
        public string MOption3 { get; set; } 
        public string MOption4 { get; set; }  

        public string Ans1 { get; set; }
        public string TAns1 { get; set; }
        public string Ans2 { get; set; }
        public string Ans3 { get; set; }
      
        public string Heading1 { get; set; }
        public string Heading2 { get; set; }

        public string Marks { get; set; }
    }
}