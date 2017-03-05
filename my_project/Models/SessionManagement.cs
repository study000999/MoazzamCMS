using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace my_project.Models
{
 
    public class SessionManagement
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();

        public void Add_Session(String sess)
        {
            Session s = new Session();
            s.session_name = sess;
            dc.Sessions.InsertOnSubmit(s);
            dc.SubmitChanges();
        }

    }
}