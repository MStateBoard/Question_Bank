using Newtonsoft.Json;
using Question_Bank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Question_Bank.Helper
{
    public class Common
    {
        Question_Bank_2022Entities db = new Question_Bank_2022Entities();
        public Login_Model Get_Login_Details()
        {
            string login_string = HttpContext.Current.User.Identity.Name;
            Login_Model login_model = JsonConvert.DeserializeObject<Login_Model>(login_string);
            return login_model;
        }

        public Exam_Login Get_Exam_Login_Details()
        {
            string login_string = HttpContext.Current.User.Identity.Name;
            Exam_Login login_model = JsonConvert.DeserializeObject<Exam_Login>(login_string);
            return login_model;
        }

        public List<SelectListItem> Get_Subject(string Index, string Class)
        {
            var model = db.Tbl_Subject.Where(x => x.Index_No == Index && x.Class == Class).ToList();
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Text = "--Select Subject--", Value = "0" });
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.Subject, Value = item.Subject });
                }
            }

            return list;
        }

        public List<SelectListItem> Get_Class(string Index)
        {
            var model = db.Tbl_Subject.Where(x => x.Index_No == Index).Select(n => n.Class).Distinct().ToList();
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem { Text = "--Select Class--", Value = "0" }
            };
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item, Value = item });
                }
            }

            return list;
        }
       
        public DataTable Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR={1}'";
                    break;
                case ".xlsx": //Excel 07
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR={1}'";
                    break;
            }
            conStr = String.Format(conStr, FilePath, isHDR);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            DataTable dt = new DataTable();
            cmdExcel.Connection = connExcel;

            //Get the name of First Sheet
            connExcel.Open();
            DataTable dtExcelSchema;
            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
            connExcel.Close();

            // Read Data from First Sheet
            connExcel.Open();
            cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);
            connExcel.Close();

            return dt;


            ////Bind Data to GridView
            //GridView1.Caption = Path.GetFileName(FilePath);
            //GridView1.DataSource = dt;
            //GridView1.DataBind();
        }
        public List<T> ConvertDataTable_To_Model<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    try
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            if (column.ColumnName == "GR_No")
                            {
                                pro.SetValue(obj, Convert.ToString(dr[column.ColumnName]), null);
                            }
                            else
                            {
                                pro.SetValue(obj, dr[column.ColumnName], null);
                            }
                        }
                        else
                            continue;
                    }
                    catch (Exception exe)
                    {

                    }

                }
            }
            return obj;
        }
        public DataTable ConvertModel_To_DataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}