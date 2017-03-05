using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI.WebControls; 

using System.Web.Mvc;
using my_project.Models;


using System.Web.Helpers;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;





namespace my_project.Models
{
    public class data :System.Web.UI.Page
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public void making_target(string sec,string sub)
        {
             string connectionString =@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\asus\Documents\Visual Studio 2012\Projects\my_project\my_project\App_Data\admin_database.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionString);
          
            // 1

            con.Open();
            // 2
            string query=" ";

            query = "select * from  Attendance where subject='" + sub + "' and sec="+sec+"' ";

           

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                DataTable targetTable = new DataTable();
                da.Fill(targetTable);
                ExportTableData(targetTable,sub,sec);
            

        }

         public void ExportTableData(DataTable dtdata,string sub,string sec)
        {
            string filename = sub +" "+ sec + ".xls";
            string attach = "attachment;filename=" +filename+ " ";
            
           
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attach);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            if (dtdata != null)
            {
                foreach (DataColumn dc in dtdata.Columns)
                {
                    HttpContext.Current.Response.Write(dc.ColumnName + "\t");
                    //sep = ";";
                }
                HttpContext.Current.Response.Write(System.Environment.NewLine);
                foreach (DataRow dr in dtdata.Rows)
                {
                    for (int i = 0; i < dtdata.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write(dr[i].ToString() + "\t");
                    }
                    HttpContext.Current.Response.Write("\n");
                }
                HttpContext.Current.Response.End();
            }
        }
         
     
    }


    }
