//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Question_Bank.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Question_Paper
    {
        public int ID { get; set; }
        public string Index_No { get; set; }
        public string User_ID { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string Topic { get; set; }
        public int Question_ID { get; set; }
        public string Question_Type { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Marks { get; set; }
        public string Paper_Name { get; set; }
        public Nullable<int> Active { get; set; }
    }
}
