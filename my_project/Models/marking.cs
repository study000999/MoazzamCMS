using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;


namespace my_project.Models
{
      
    public class marking
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public ArrayList enter_paper_marks(string sub, string sec)
        {
            

            var query = (from ids in dc.students_tables
                        where ids.section.Equals(sec)
                        select ids.rollno).ToList();
            ArrayList al = new ArrayList(query);
            return al;
        }

        public void submit_paper_marks(string roll,string sec,string sub,string total_marks,string paperType,string marks)
        {
           
            Mark m = new Mark();
            m.student_id = roll;
            m.subject_name = sub;
            m.total_marks = total_marks;
            m.section = sec;
            m.quiz_name = paperType;
            m.obtained_marks = marks;
            dc.Marks.InsertOnSubmit(m);
            dc.SubmitChanges();
        }
        
    }
}