using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my_project.Models
{
    public class add_teacher_info  
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public void add(string fname, string lname, string email, string password, string gender, string city, string add,string salary)
        {
            string roll = "";
            try
            {
                var query = from i in dc.teacher_tables
                            orderby i.teacher_id descending
                            select i;

                foreach (var i in query)
                {
                    roll = i.teacher_id;
                    break;
                }
                string arr = Convert.ToString(roll[8]) + Convert.ToString(roll[9]) +Convert.ToString(roll[10])+Convert.ToString(roll[11]);
                int a = Convert.ToInt32(arr) + 1;

                if (a <= 9)
                {
                    roll = "PGC-MZD-000" + Convert.ToString(a);
                }
                else if (a>9 && a <= 99)
                {
                    roll = "PGC-MZD-00" + Convert.ToString(a);
                }
                else if (a <= 999)
                {
                    roll = "PGC-MZD-0" + Convert.ToString(a);
                }




            }
            catch (Exception e)
            {
                roll = "PGC-MZD-0001";
               

            }

            teacher_table tt = new teacher_table();
                tt.teacher_id = roll;
                tt.fname = fname;
                tt.lname = lname;
                tt.email = email;
                tt.password = password;
                tt.gender = gender;
                tt.city = city;
                tt.address = add;
                dc.teacher_tables.InsertOnSubmit(tt);
                dc.SubmitChanges();
        }

        public bool AddTeacherSection(string id,string deg,string sec,string sub,string session)
        {
            if ((from i in dc.teacher_sections where i.teacher_degree.Equals(deg) && i.teacher_sec.Equals(sec) && i.teacher_subject.Equals(sub) select i).Count() == 0)
            {
                teacher_section tt = new teacher_section();
                tt.teacher_sec = sec;
                tt.teacher_id = id;
                tt.teacher_subject = sub;
                tt.teacher_degree = deg;
                tt.teacher_session = session;

                dc.teacher_sections.InsertOnSubmit(tt);
                dc.SubmitChanges();
                return true;
            }
            return false;


        }





    }
}