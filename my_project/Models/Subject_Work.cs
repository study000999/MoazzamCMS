using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace my_project.Models
{
    public class Subject_Work : System.Web.UI.Page
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();

        public ArrayList get_student_subjects(string rollno)
        {
            ArrayList al = new ArrayList();

            if (rollno[2].Equals('E'))
            {
                var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                al.Add(query.sub1);
                al.Add(query.sub2);
                al.Add(query.sub3);
                al.Add(query.sub4);
                al.Add(query.sub5);
                al.Add(query.sub6);
            }
            if (rollno[2].Equals('M'))
            {
                var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                al.Add(query.sub1);
                al.Add(query.sub2);
                al.Add(query.sub3);
                al.Add(query.sub4);
                al.Add(query.sub5);
                al.Add(query.sub6);
            }

            if (rollno[2].Equals('C'))
            {
                var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                al.Add(query.sub1);
                al.Add(query.sub2);
                al.Add(query.sub3);
                al.Add(query.sub4);
                al.Add(query.sub5);
                al.Add(query.sub6);
            }
            if (rollno[2].Equals('I'))
            {
                var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                al.Add(query.sub1);
                al.Add(query.sub2);
                al.Add(query.sub3);
                al.Add(query.sub4);
                al.Add(query.sub5);
                al.Add(query.sub6);
                al.Add(query.sub7);
            }
            if (rollno[2].Equals('B'))
            {
                if (rollno[3].Equals('A'))
                {
                    var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                    al.Add(query.sub1);
                    al.Add(query.sub2);
                    al.Add(query.sub3);
                    al.Add(query.sub4);
                    al.Add(query.sub5);
                    al.Add(query.sub6);
                    al.Add(query.sub7);
                }
                else if (rollno[3].Equals('-'))
                {
                    var query = dc.student_subjects.First(a => a.student_id.Equals(rollno));
                    al.Add(query.sub1);
                    al.Add(query.sub2);
                    al.Add(query.sub3);
                    al.Add(query.sub4);
                    al.Add(query.sub5);
                    al.Add(query.sub6);
                    al.Add(query.sub7);
                    al.Add(query.sub8);
                }
            }



            return al;

        }
        public ArrayList get_Attendance(string sub, string id)
        {
            var query = from i in dc.Attendances
                        where i.student_id.Equals(id) && i.subject.Equals(sub)
                        select new
                        {
                            i.Date,
                            i.attendence
                        };
            ArrayList al = new ArrayList();
            foreach (var i in query)
            {
                al.Add(i.attendence);
                al.Add(i.Date);
            }

            return al;

        }


        //*************This function is used to get teacher subjects.....So that Teacher can add student mark****************

        public ArrayList get_teacher_subjects(string id)
        {
            var query = from i in dc.teacher_sections
                        where i.teacher_id.Equals(id)
                        select new { i.teacher_subject, i.teacher_sec };
            ArrayList al = new ArrayList();
            foreach (var i in query)
            {
                al.Add(i.teacher_subject);
                al.Add(i.teacher_sec);
            }
            return al;
        }

        public Boolean add_subjects(String id, String subs, string session)
        {

            try
            {
                String[] sub = subs.Split(',');
                foreach (String i in sub)
                {
                    if ((from d in dc.Subjects where d.subjects.Equals(i) && d.degree.Equals(id) && d.session.Equals(session) select d).Count() > 0)
                    {
                        return false;
                    }
                }

                foreach (String i in sub)
                {

                    Subject ss = new Subject();
                    ss.subjects = i;
                    ss.degree = id;
                    ss.session = session;
                    dc.Subjects.InsertOnSubmit(ss);
                    dc.SubmitChanges();

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool update_subject(string deg, string sess, string sub, string new_sub)
        {
            try
            {
                if ((from i in dc.Subjects where i.degree.Equals(deg) && i.session.Equals(sess) 
                    && i.subjects.Equals(new_sub) select i).Count() == 0)
                {
                    var query = (from i in dc.Subjects where i.degree.Equals(deg) && i.session.Equals(sess) 
                    && i.subjects.Equals(sub) select i).First();
                    query.subjects = new_sub;
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


        public bool delete_subject(string deg, string sess, string sub)
        {
            try
            {

                var query = (from i in dc.Subjects where i.degree.Equals(deg) && i.session.Equals(sess) && i.subjects.Equals(sub) select i).First();
                dc.Subjects.DeleteOnSubmit(query);
                dc.SubmitChanges();

                if ((from i in dc.teacher_sections where i.teacher_subject.Equals(sub) select i).Count() == 0)
                {
                    var query1 = from i in dc.teacher_sections where i.teacher_subject.Equals(sub) select i;
                    dc.teacher_sections.DeleteAllOnSubmit(query1);



                }

                return true;
            }

            catch (Exception e)
            {
                return false;
            }

        }


    }




}
