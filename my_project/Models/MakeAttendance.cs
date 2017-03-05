using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace my_project.Models
{

    public class MakeAttendance
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        private ArrayList al;
        public ArrayList date;
        public int count;

        public void Mark_Attendance(string roll, string sec, string sub, string date, string attend)
        {
            if (attend == null)
            {
                attend = "absent";
            }
            Attendance b = new Attendance();
            b.Date = date;
            b.subject = sub;
            b.student_id = roll;
            b.attendence = attend;
            b.section = sec;
            dc.Attendances.InsertOnSubmit(b);
            dc.SubmitChanges();
        }

        public ArrayList check_attendance(params string []data)
        {
            var q = from i in dc.Attendances where i.subject.Equals(data[0]) && i.section.Equals(data[1]) select i.student_id;
            string check = "";
            make_array_list();
            
            
            bool flag = false;
            foreach (var i in q)
            {

                if (check == i && flag == true)
                {

                    break;
                }
                if (flag == false)
                {
                    check = i;
                    
                    flag = true;
                    if (data.Length == 3)
                    {
                        populate_date_list(i, data[2]);
                        
                    }
                    else
                    {
                        populate_date_list(i);
                    }
                }
                if (data.Length == 3)
                {
                    count = 0;
                    populate_array_list(i, data[2]);
                }
                else
                {
                    count = 0;
                    populate_array_list(i);
                }
                
               

            }

            return al;


        }

        private void make_array_list()
        {
            al = new ArrayList();
            date = new ArrayList();
        }
        public void populate_array_list(params string []data)
        {
            var query = from i in dc.Attendances where i.student_id.Equals(data[0]) select new { i.attendence,i.Date };
            foreach (var i in query)
            {

                if (data.Length == 2)
                {
                    string[] dats = i.Date.Split('-');
                    if (dats[1].Equals(data[1]))
                    {

                        
                        if (!al.Contains(data[0]))
                        {
                            al.Add(data[0]);
                            
                        }
                        al.Add(i.attendence);
                        count++;
                    }

                }
                else
                {
                    
                    if (!al.Contains(data[0]))
                    {
                        al.Add(data[0]);
                    }
                    al.Add(i.attendence);
                    count++;
                }
            }
            

        }
        public void populate_date_list(params string [] dt)
        {
            
                var query = from i in dc.Attendances where i.student_id.Equals(dt[0]) select i.Date;
                foreach (var i in query)
                {
                    if (dt.Length == 1)
                    {
                        date.Add(i);
                    }
                    else
                    {
                        string[] dates = i.Split('-');
                        if (dates[1].Equals(dt[1]))
                        {
                            date.Add(i);
                        }

                    }
                }
            
            
            
        }

        public ArrayList populate_student_month_datelist(params string[]s)
        {


            var q = from i in dc.Attendances where i.student_id.Equals(s[0]) && i.subject.Equals(s[1]) select new { i.attendence, i.Date };
            make_array_list();
            foreach (var i in q)
            {
                if (s[2]!=null)
                {
                    string[] d = i.Date.Split('-');
                    if (d[1].Equals(s[2]))
                    {
                        al.Add(i.attendence);
                        al.Add(i.Date);
                    }

                }
                else
                {

                    al.Add(i.attendence);
                    al.Add(i.Date);
                }
            }

            return al;
                   

        }

        public void mark_attendance1(string roll, string attend,string date,string sec,string sub)
        {
            string[] rolls_arr = roll.Split(',');
            string[] attend_arr = attend.Split(',');
           int c= rolls_arr.Count();
           int b = rolls_arr.Count();
            List<Attendance> list=new List<Attendance>();

            for (int i = 1; i < (rolls_arr.Count() - 1); i++)
            {
                Attendance a = new Attendance();
                a.attendence = attend_arr[i];




                a.Date = date;
                a.section = sec;

                a.student_id = rolls_arr[i];
                a.subject = sub;
                list.Add(a);

            }
            dc.Attendances.InsertAllOnSubmit(list);
            dc.SubmitChanges();
        }

        public void edit_attendance(List<string> rolls, string sec, string sub,string date)
        {
            
            foreach (var i in rolls)
            {
                Attendance a = new Attendance();
                var query = (from d in dc.Attendances where d.Date.Equals(date) && d.section.Equals(sec) && d.subject.Equals(sub) && d.student_id.Equals(i) select d).First();
                if (query.attendence.Equals("P"))
                {
                    query.attendence = "A";
                }
                else
                {
                    query.attendence = "P";
                }
            }
            dc.SubmitChanges();


                
            }

        }

    }




