using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question_Bank.Models
{
    public class Question_Bank_Model
    {
        public int ID { get; set; }
        public List<string> Chk { get; set; }
        public string User_ID { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public int Question_ID { get; set; }
        public string Question_Type { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Paper_Name { get; set; }
        public string Marks { get; set; }
        public Nullable<int> Active { get; set; }
        public string Question_No { get; set; }
    }
}