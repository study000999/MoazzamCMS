using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace my_project.Models
{
    public class mothly_registeration
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public void registeration(string fee_type,string name, string deg, string reg)
        {
            string query = "";
            var q = dc.Degrees.First(a => a.deg_name.Equals(deg));
            
            string ConString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\asus\Documents\Visual Studio 2012\Projects\my_project\my_project\App_Data\admin_database.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConString);

            if (fee_type.Equals("Monthly Fee"))
            {
                query = "select * from mothly_fee";
            }
            else
            {
                query = "select * from installment";
            }
            SqlCommand cmd = new SqlCommand(query,con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr[1] = reg;
            dr[2] = deg;
            dr[3]=name;

            

            for (int i = 4; i < dt.Columns.Count; i++)
            {
                if (i == dt.Columns.Count-1)
                {
                    dr[i] = "0";
                }
                else if (i % 2 == 0)
                {
                   // dr[i] = q.deg_monthFee;
                }
                else
                {
                    if (i == 5)
                    {
                        dr[i] = "clear";
                    }
                    else
                    {
                        dr[i] = "not clear";
                    }
                }
            }
            dt.Rows.Add(dr);
            con.Open();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            da.InsertCommand = cb.GetInsertCommand();
            da.Update(dt);
            con.Close();


            
        }

        public void confirmation(string reg, string install)
        {
            var query = dc.mothly_fees.First(b => b.reg_num.Equals(reg));
            if (install.Equals("2"))
            {
                query.Confrim_1_installment_2_month = "clear";


            }

            else if (install.Equals("3"))
            {
                query.Confrim_1_installment_3_month = "clear";
            }
            else if (install.Equals("4"))
            {
                query.Confrim_1_installment_4_month = "clear";
            }
            else if (install.Equals("5"))
            {
                query.Confrim_2_installment_1_month = "clear";
            }
            else if (install.Equals("6"))
            {
                query.Confrim_2_installment_2_month = "clear";
            }
            else if (install.Equals("7"))
            {
                query.Confrim_2_installment_3_month = "clear";
            }
            else if (install.Equals("8"))
            {
                query.Confrim_2_installment_4_month = "clear";
            }
            else if (install.Equals("9"))
            {
                query.Confrim_3_installment_1_month = "clear";
            }
            else if (install.Equals("10"))
            {
                query.Confrim_3_installment_2_month = "clear";
            }
            else if (install.Equals("11"))
            {
                query.Confrim_3_installment_3_month = "clear";
            }
            else if (install.Equals("13"))
            {
                query.Confrim_4_installment_1_month = "clear";
            }
            else if (install.Equals("14"))
            {
                query.Confrim_4_installment_2_month = "clear";
            }
            else if (install.Equals("15"))
            {
                query.Confrim_4_installment_3_month = "clear";
            }
            else if (install.Equals("16"))
            {
                query.Confrim_4_installment_4_month = "clear";
            }
            else if (install.Equals("17"))
            {
                query.Confrim_5_installment_1_month = "clear";
            }
            else if (install.Equals("18"))
            {
                query.Confrim_5_installment_2_month = "clear";
            }
            else if (install.Equals("19"))
            {
                query.Confrim_5_installment_3_month = "clear";
            }
            else if (install.Equals("20"))
            {
                query.Confrim_5_installment_4_month = "clear";
            }
            else if (install.Equals("21"))
            {
                query.Confrim_6_installment_1_month = "clear";
            }

            else if (install.Equals("23"))
            {
                query.Confrim_6_installment_3_month = "clear";
            }
            else if (install.Equals("24"))
            {
                query.Confrim_6_installment_4_month = "clear";
            }
            dc.SubmitChanges();
        }

        public string get_challan_form(string reg, string install, out int fine, out int late_fine, out int previous, out int tution_fee)
        {
            var query = dc.mothly_fees.First(a => a.reg_num.Equals(reg));

            var q = dc.fines.First(a => a.cat.Equals("late Fee Fine"));

            string connectionstring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\asus\Documents\Visual Studio 2012\Projects\my_project\my_project\App_Data\admin_database.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionstring);
            con.Open();

            string quer = "select * from mothly_fee";
            SqlCommand cmd = new SqlCommand(quer, con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;

            DataTable targetTable = new DataTable();
            da.Fill(targetTable);




            int j = Convert.ToInt32(install);

            previous = 0;
            tution_fee = 0;
            int index = 0;
            fine = Convert.ToInt32(query.fine);
            late_fine = Convert.ToInt32(q.total_fee);

            string s = Convert.ToString(targetTable.Rows[0][2]);

            for (int i = 0; i < targetTable.Rows.Count; i++)
            {
                if (targetTable.Rows[i][1].Equals(reg))
                {
                    index = i;
                    break;
                }
            }


            if (j == 2)
            {
                if (targetTable.Rows[index][5].ToString().Equals("not clear"))
                {
                    previous = previous + Convert.ToInt32(targetTable.Rows[index][4]);
                }
                tution_fee = Convert.ToInt32(targetTable.Rows[index][4]);
            }
            else if (j == 1)
            {
                tution_fee = Convert.ToInt32(targetTable.Rows[index][6]);
            }

            else
            {
               
                   for (int i = 5; i <= j + (j + 1); i = i + 2)
                        {
                            if (targetTable.Rows[index][i].ToString().Equals("not clear"))
                            {

                                previous = previous + Convert.ToInt32(targetTable.Rows[index][i - 1]);
                            }
                            if(i==j+(j+1))
                            {
                                tution_fee = Convert.ToInt32(targetTable.Rows[index][i + 1]);
                            }

                            
                        }

                        

                        

                    }


                
            

            if (previous == 0)
            {
                late_fine = 0;
            }

            return Convert.ToString(tution_fee + fine + late_fine + previous);


}

        public string installment_fee(string reg, string install, out int fine, out int late_fine, out int previous, out int tution_fee)
        {
            String ConString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\asus\Documents\Visual Studio 2012\Projects\my_project\my_project\App_Data\admin_database.mdf;Integrated Security=True";
            
            int indx =0;
            var q = dc.fines.First(a => a.cat.Equals("late Fee Fine"));
            SqlConnection con = new SqlConnection(ConString);
            con.Open();
            string querys = "select * from installment";
            SqlCommand cmd = new SqlCommand(querys, con);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].Equals(reg))
                {
                    indx = i;
                    break;
                }
                

            }
            tution_fee = 0;
            previous = 0;

            for (int i = 5; i <= (Convert.ToInt32(install)+Convert.ToInt32(install)+1); i=i+2)
            {

                if (Convert.ToString(dt.Rows[indx][i]).Equals("not clear"))
                {
                    previous = previous + Convert.ToInt32(dt.Rows[indx][i - 1]);
                }

                if (i == Convert.ToInt32(install) + Convert.ToInt32(install) + 1)
                {
                    tution_fee=Convert.ToInt32(dt.Rows[indx][i+1]);
                }
            }
            fine = Convert.ToInt32(dt.Rows[indx][16]);
            if (previous == 0)
            {
                late_fine = 0;
            }
            else
            {
                late_fine = Convert.ToInt32(q.total_fee);
            }
            return Convert.ToString(fine + late_fine + previous+tution_fee);

            
        }
    }
}
