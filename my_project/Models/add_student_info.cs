using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;


namespace my_project.Models
{
    public class add_student_info
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();

        public  string  add_student(string fname, string lname, string password, string sec, string degree, string sess,string fee_type,string reg_num)
        {
            try
            {
                var ses = (from i in dc.Sessions where i.Id.Equals(sess) select i.session_name.ToString()).First();
                var sect = (from i in dc.sections where i.Id.Equals(sec) select i.section1.ToString()).First();
                string rollno = "";



                if ((from i in dc.students_tables where i.section.Equals(sec) select i).Count() > 0)
                {
                    var query1 = (from i in dc.students_tables
                                  where i.section.Equals(sec)
                                  orderby i.rollno descending
                                  select i.rollno.ToString()).First();


                    rollno = query1.ToString();


                    string arr = Convert.ToString(rollno[rollno.Length-2]) + Convert.ToString(rollno[rollno.Length-1]);
                    int a = Convert.ToInt32(arr) + 1;

                    if (a == 2 || a == 3 || a == 4 || a == 4 || a == 5 || a == 6 || a == 7 || a == 8 || a == 9)
                    {
                        rollno = "PCE-" + ses + sect + "0" + Convert.ToString(a);
                    }

                    else
                    {


                        rollno = "PCE-" + ses + sect + Convert.ToString(a);
                    }
                }
                else
                {
                    rollno = "PCE-" + ses + sect + "01";
                }

                students_table st = new students_table();
                st.rollno = rollno;
                st.section = sec;
                st.fname = fname;
                st.lname = lname;
                st.fee_type = fee_type;
                st.registeration = reg_num;
                st.password = password;
                st.session = sess;
                st.degree = degree;
                dc.students_tables.InsertOnSubmit(st);
                dc.SubmitChanges();
                return rollno;
            }
            catch (Exception e)
            {
                return "";
            }
            
            



        }



       

        public void change_section(string rollno, string degree, string new_degree, string sec, string new_sec,string sess)
        {
            var query = dc.students_tables.First(a => a.rollno.Equals(rollno));
            
            dc.students_tables.DeleteOnSubmit(query);
            
            
            dc.SubmitChanges();

           

            string newroll=add_student(query.fname, query.lname,  query.password, new_sec,new_degree,sess,query.fee_type,query.registeration);
            add_personalInfo1(rollno, newroll);

            
           

        }

        public void generate_reg_num(string name,string deg,string fee_type)
        {
            try
            {
                var query = from i in dc.registers
                            orderby i.reg_num descending
                            
                            select i.reg_num;
                string reg = "";
                foreach (var i in query)
                {
                    reg = i;
                    break;
                }
                int sum = Convert.ToInt32(reg) + 1;
                if (sum <= 9)
                {
                    reg = "00000000" + Convert.ToString(sum);
                }
                else if (sum >= 10 && sum <= 99)
                {
                    reg = "0000000" + Convert.ToString(sum);
                }
                else if (sum >= 100 && sum <= 999)
                {
                    reg = "000000" + Convert.ToString(sum);
                }
                else if (sum >= 1000 && sum <= 9999)
                {
                    reg = "00000" + Convert.ToString(sum);
                }
                else if (sum >= 10000 && sum <= 99999)
                {
                    reg = "0000" + Convert.ToString(sum);
                }
                



                register r = new register();
                r.reg_num = reg;
                r.stud_name=name;
                r.degree = deg;
                r.fee_type = fee_type;
                dc.registers.InsertOnSubmit(r);
                dc.SubmitChanges();
            }
            catch (Exception E)
            {
                register r = new register();
                r.reg_num = "000000001";
                r.stud_name = name;
                r.degree = deg;
                r.fee_type = fee_type;
                dc.registers.InsertOnSubmit(r);
                dc.SubmitChanges();

            }
        }

        public bool add_personalInfo1(string rollno,string new_roll)
        {
            if ((from i in dc.personal_infos where i.rollno.Equals(rollno) select i).Count() > 0)
            {
                var query = (from i in dc.personal_infos where i.rollno.Equals(rollno) select i).First();
                query.rollno = new_roll;
                dc.SubmitChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public bool add_personal(string rollno, string city, string gender, string add, string email)
        {
            if ((from i in dc.personal_infos where i.rollno.Equals(rollno) select i).Count() == 0)
            {
                personal_info p = new personal_info();
                p.address = add;
                p.city = city;
                p.email = email;
                p.gender = gender;
                p.rollno = rollno;
                
                dc.personal_infos.InsertOnSubmit(p);
                dc.SubmitChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool edit_info(string rollno, string city, string gender, string add, string email)
        {
            var query = (from i in dc.personal_infos where i.rollno.Equals(rollno) select i).First();
            query.rollno = rollno;
            query.city = city;
            query.gender = gender;
            query.address = add;
            query.email = email;

                
                dc.SubmitChanges();
                return true;
            
        }



    }
}
