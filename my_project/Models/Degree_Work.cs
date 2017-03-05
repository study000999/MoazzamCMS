using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my_project.Models
{
    public class Degree_Work
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public bool Add_Degree(string degname)
        {


            if ((from i in dc.Degrees where i.deg_name.Equals(degname) select i.deg_name).Count() == 0)
            {
                Degree add = new Degree();
              
                add.deg_name = degname;
                add.year = "1";
               
                dc.Degrees.InsertOnSubmit(add);
                dc.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool Add_fee(String degname, String degfee,String year)
        {
            try
            {
                if ((from i in dc.Fees where i.Degree.Equals(degname) && i.Year.Equals(year) select i).Count() == 0)
                {
                    Fee f = new Fee();
                    f.Year = year;
                    f.Degree = degname;
                    f.TotalFee = degfee;
                    f.InstallFee = Convert.ToString(Convert.ToInt32(degfee) / 3);
                    f.MonthFee = Convert.ToString(Convert.ToInt32(f.InstallFee) / 4);
                    dc.Fees.InsertOnSubmit(f);
                    dc.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public bool Add_Section(string deg, string sect)
        {
            try
            {
                String[] sec = sect.Split(',');
                foreach (string i in sec)
                {
                    if ((from d in dc.sections where d.section1.Equals(i) select d.section1).Count() > 0)
                    {
                        return false;
                    }

                }

                foreach (string i in sec)
                {

                    section s = new section();
                    s.section1 = i;
                    s.degree = deg;
                    dc.sections.InsertOnSubmit(s);
                    dc.SubmitChanges();

                }



                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddCategory(string cat, string amount)
        {
            if ((from i in dc.fines where i.cat.Equals(cat) select i.cat).Count() == 0)
            {
                fine fs = new fine();
                fs.cat = cat;
                fs.total_fee = amount;
                dc.fines.InsertOnSubmit(fs);
                dc.SubmitChanges();
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool edit(string cat, string amount)
        {
            var query = from i in dc.fines where i.cat.Equals(cat) select i;
            foreach (var i in query)
            {
                i.total_fee = amount;
            }
            dc.SubmitChanges();
            return true;


        }
        public bool AddFineToStudent(string reg_num,string cat)
        {
            if ((from i in dc.installments where i.reg_num.Equals(reg_num) select i).Count() == 0)
            {
                var q = dc.mothly_fees.First(a => a.reg_num.Equals(reg_num));
                var q1 = dc.fines.First(a => a.cat.Equals(cat));
                q.fine = q1.total_fee;
                dc.SubmitChanges();



            }

            else
            {
                var q = dc.installments.First(a => a.reg_num.Equals(reg_num));
                var q1 = dc.fines.First(a => a.cat.Equals(cat));
                q.fine = q1.total_fee;
                dc.SubmitChanges();

            }
            return true;

            
        }

        public bool update_degree(String degId,String new_deg)
        {
            try
            {
                var query = (from i in dc.Degrees where i.Id.Equals(degId) select i).First();
                query.deg_name = new_deg;
                query.year = "1";
                
                    
                
                dc.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
              
        }

        public bool update_section(String id, String sec)
        {
            try
            {
                var query = (from i in dc.sections where i.Id.Equals(id) select i).First();
                query.section1 = sec;
                dc.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }

}