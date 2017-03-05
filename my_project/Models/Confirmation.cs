using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my_project.Models
{
    public class Confirmation
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public bool confirm(String reg_num,String Install)
        {
            try
            {

                if ((from i in dc.ConfirmInstallments where i.reg_num.Equals(reg_num) select i).Count() == 0)
                {
                    ConfirmInstallment con = new ConfirmInstallment();
                    con.reg_num = reg_num;
                    con.installment_No = Install;
                    dc.ConfirmInstallments.InsertOnSubmit(con);
                    dc.SubmitChanges();
                    return true;
                }
                else
                {
                    var query = (from i in dc.ConfirmInstallments where i.reg_num.Equals(reg_num) select i).First();
                    query.installment_No = Install;
                    dc.SubmitChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

              
        }
        public bool get_challan(string reg,string install,out string tution,out string pre,out string late,out string total)
        {
            
            var query = (from i in dc.registers where i.reg_num.Equals(reg) select i).First();
            string fees="";
            if (query.fee_type == "Monthly Fee")
            {
                fees = (from i in dc.Fees where i.Degree.Equals(query.degree) select i.MonthFee.ToString()).First();
            }
            else
            {
                fees = (from i in dc.Fees where i.Degree.Equals(query.degree) select i.InstallFee.ToString()).First();
            }
            
                string con = (from i in dc.ConfirmInstallments where i.reg_num.Equals(reg) select i.installment_No.ToString()).First();
                
                string[] inst = con.Split('/');
                int diff = Convert.ToInt32(install) - Convert.ToInt32(inst[0]);
                
                if (diff == 0 || diff < 0 || diff==1)
                {
                    tution = fees;
                    pre = "0";
                    late = "0";
                    total = fees;
                }
                else
                {
                    String fines=(from i in dc.fines where i.cat.Equals("Late Fee Fine") select i.total_fee.ToString()).First();
                    tution = fees;
                    int previous = 0;
                    for (int i = 1; i < diff; i++)
                    {
                        previous = previous + Convert.ToInt32(tution);   
                    }
                    pre = Convert.ToString(previous);
                    late = fines;
                    total = Convert.ToString(Convert.ToInt32(tution) + Convert.ToInt32(previous) + Convert.ToInt32(late));
                }

            
            return true;
        }

        public bool reissue_challan(string reg, string install,string check_late,string re_issue, out string fine,out string tution, out string pre, out string late, out string total)
    
        {
            var query = (from i in dc.registers where i.reg_num.Equals(reg) select i).First();
            string fees = "";
           
            if (query.fee_type == "Monthly Fee")
            {
                fees = (from i in dc.Fees where i.Degree.Equals(query.degree) select i.MonthFee.ToString()).First();
            }
            else
            {
                fees = (from i in dc.Fees where i.Degree.Equals(query.degree) select i.InstallFee.ToString()).First();
            }

            string con = (from i in dc.ConfirmInstallments where i.reg_num.Equals(reg) select i.installment_No.ToString()).First();

            string[] inst = con.Split('/');
            int diff = Convert.ToInt32(install) - Convert.ToInt32(inst[0]);

            if (diff == 0 || diff < 0 || diff == 1)
            {
                tution = fees;
                pre = "0";
                if (check_late == "y")
                {
                    late = "500";
                }
                else
                {
                    late = "0";
                }
                if(re_issue=="y")
                {
                    fine="500";
                }
                else {
                    fine = "0";
                }


                total =Convert.ToString( Convert.ToInt32(tution) + Convert.ToInt32(late) + Convert.ToInt32(fine));
            }
            else
            {
               
                tution = fees;
                int previous = 0;
                for (int i = 1; i < diff; i++)
                {
                    previous = previous + Convert.ToInt32(tution);
                }
                pre = Convert.ToString(previous);
                
                if (check_late == "y")
                {
                    late = "500";
                }
                else
                {
                    late = "0";
                }
                if (re_issue == "y")
                {
                    fine = "500";
                }
                else
                {
                    fine = "0";
                }
                total = Convert.ToString(Convert.ToInt32(tution) + Convert.ToInt32(previous) + Convert.ToInt32(late));
            }



            return true;

        }

    }
}