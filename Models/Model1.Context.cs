﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Question_Bank_2022Entities : DbContext
    {
        public Question_Bank_2022Entities()
            : base("name=Question_Bank_2022Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Tbl_Division> Tbl_Division { get; set; }
        public virtual DbSet<Tbl_Question_Data> Tbl_Question_Data { get; set; }
        public virtual DbSet<Tbl_Question_Paper> Tbl_Question_Paper { get; set; }
        public virtual DbSet<Tbl_Student_Details> Tbl_Student_Details { get; set; }
        public virtual DbSet<Tbl_Subject> Tbl_Subject { get; set; }
        public virtual DbSet<Tbl_Year> Tbl_Year { get; set; }
        public virtual DbSet<Tbl_Create_School_College> Tbl_Create_School_College { get; set; }
        public virtual DbSet<Tbl_Admin> Tbl_Admin { get; set; }
        public virtual DbSet<Tbl_Flag> Tbl_Flag { get; set; }
        public virtual DbSet<Tbl_Question1> Tbl_Question1 { get; set; }
        public virtual DbSet<Tbl_Question2> Tbl_Question2 { get; set; }
        public virtual DbSet<Tbl_Question3> Tbl_Question3 { get; set; }
        public virtual DbSet<Tbl_Question6> Tbl_Question6 { get; set; }
        public virtual DbSet<Tbl_Question7> Tbl_Question7 { get; set; }
        public virtual DbSet<Tbl_Question4> Tbl_Question4 { get; set; }
        public virtual DbSet<Tbl_Question5> Tbl_Question5 { get; set; }
        public virtual DbSet<Tbl_Student_Login> Tbl_Student_Login { get; set; }
        public virtual DbSet<Tbl_Student> Tbl_Student { get; set; }
        public virtual DbSet<Tbl_Student_Login_Details> Tbl_Student_Login_Details { get; set; }
    }
}