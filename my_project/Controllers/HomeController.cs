using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using my_project.Models;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Helpers;
using System.IO;
using System.Web.UI;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Net.Http;
using System.Configuration;


using System.Net;
using System.Net.Mail;
using System.Data;
using System.Data.Entity;



using System.Web.Script.Serialization;
namespace my_project.Controllers
{
    public class HomeController : Controller
    {

        DataClasses1DataContext dc = new DataClasses1DataContext();




        public ActionResult Index()
        {


            return View();


        }





        public ActionResult Gallery()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult Contact()
        {
            return View();
        }




        //ADMIN LOGIN

        public ActionResult login()
        {
            admin_table at = new admin_table();




            string email = Request["username"];
            string password = Request["password"];

            if (email != null && password != null)
            {
                try
                {


                    var v = dc.admin_tables.First(x => x.username == email && x.password == password);
                    if (v.username.Equals(email) && v.password.Equals(password))
                    {
                        Session["username"] = v.username.ToString();
                        return RedirectToAction("adminpage");
                    }


                }
                catch (Exception e)
                {
                    return RedirectToAction("login");
                }


            }



            return View();

        }


        // --------------------------------------------//










        //ADMIN MAIN PAGE


        public ActionResult adminpage()
        {

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }
            return View();
        }


        //******************************************FEE Management **********************************************************//
        public ActionResult fee_management_main()
        {
            return View();
        }


        public ActionResult ceo_page()
        {
            return View();
        }
        public ActionResult ceo_page_1()
        {
            if (Request["username"].Equals("admin") && Request["pass"].Equals("12345"))
            {
                return RedirectToAction("manage_fee");
            }
            else
            {
                return View("ceo_page");
            }
        }

        public ActionResult accountant_page()
        {
            return View();
        }


        public ActionResult Add_degree_Fee()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            ViewBag.session = new SelectList(dc.Sessions, "Id", "session_name");
            return View();
        }

        public JsonResult Add_degree_Fee2(String degname, String degfee, String year)
        {
            try
            {
                Degree_Work d = new Degree_Work();
                bool ans=d.Add_fee(degname, degfee, year);
                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }
           
        }




        public ActionResult manage_fee()
        {

            return View();
        }
        public ActionResult fsc_engg_fee()
        {
            return View();
        }


        public ActionResult fsc_med_fee()
        {
            return View();
        }

        public ActionResult ics_fee()
        {
            return View();
        }

        public ActionResult icom_fee()
        {
            return View();
        }

        public ActionResult bcom_fee()
        {
            return View();
        }

        public ActionResult fa_fee()
        {
            return View();
        }
        public ActionResult ba_fee()
        {
            return View();
        }

        public ActionResult challan_forms()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            ViewBag.session = new SelectList(dc.Sessions, "Id", "session_name");

            return View();
        }


         public JsonResult get_degreefee(String degree,String session)
        {
            try
            {
                var query = (from i in dc.Fees where i.Degree.Equals(degree) && i.Year.Equals(session) select i).First();
                var result = new { install = query.InstallFee, monthly = query.MonthFee };

                return Json(result);
            }
            catch (Exception e)
            {
                return Json(false);
            }
            
        }

        public ActionResult challan_forms2()
        {
            try
            {

                return View(dc.Degrees.First(a => a.deg_name.Equals(Request["degree"])));
            }
            catch (Exception e)
            {
                return RedirectToAction("challan_forms");
            }




        }






       public ActionResult generate_challan_form()
        {
            try
            {

                if (Request.HttpMethod == "POST")
                {
                    add_student_info reg = new add_student_info();
                    reg.generate_reg_num(Request["name"], Request["degree"], Request["fee_type"]);
                    var query = (from i in dc.Fees where i.Degree.Equals(Request["degree"]) && i.Year.Equals(Request["session"]) select i).First();
                    if (Request["fee_type"].Equals("Monthly"))
                    {

                        TempData["fee"] = query.MonthFee;
                    }

                    else
                    {

                        TempData["fee"] = query.InstallFee;

                    }
                    TempData.Keep("fee");
                    ViewBag.fee = TempData["fee"].ToString();


                }
                else
                {
                    ViewBag.fee = TempData["fee"].ToString();
                    TempData.Keep("fee_type");


                }

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("challan_forms");
            }

            
        }


        public ActionResult reg_fee()
        {
            return View();
        }



        public ActionResult Confirmation()
        {
            return View();
        }
        public ActionResult first_installment_confirm()
        {
            return View();
        }

        public ActionResult Confirmation1()
        {


            try
            {
                return View(dc.registers.First(a => a.reg_num.Equals(Request["reg"])));
            }
            catch (Exception e)
            {
                return RedirectToAction("Confirmation");
            }
        }


        public JsonResult Confirm_in_database(String reg,String install)

        {
            try
            {
                bool ans=new Confirmation().confirm(reg,install);
                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }


        public ActionResult Confirmation2()
        {
            if (Request["fee_type"].Equals("Monthly Fee"))
            {
                mothly_registeration mr = new mothly_registeration();
                mr.registeration(Request["fee_type"], Request["name"], Request["deg"], Request["reg"]);
            }
            else
            {
                installment i = new installment();

                dc.installments.InsertOnSubmit(i);
                dc.SubmitChanges();


            }
            return RedirectToAction("Confirmation");
        }




        public ActionResult other_installment()
        {
            return View();
        }

        /* public ActionResult other_installment1()
         {
             try
             {
                 var q = dc.registers.First(b => b.reg_num.Equals(Request["reg"]));
                 if (q.fee_type.Equals("Monthly Fee"))
                 {
                     if (q.deg_name.Equals("Fsc Pre Engg"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("Fsc Pre Engg"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("Fsc Pre Medical"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("Fsc Pre Medical"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("ICS"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("ICS"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("ICOM"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("ICOM"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("BCOM"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("BCOM"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("FA"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("FA"));
                         ViewBag.fee = query.monthly_fee;
                     }
                     else if (q.deg_name.Equals("BA"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("BA"));
                         ViewBag.fee = query.monthly_fee;
                     }

                 }

                 else if (q.fee_type.Equals("Installment"))
                 {
                     if (q.deg_name.Equals("Fsc Pre Engg"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("Fsc Pre Engg"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("Fsc Pre Medical"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("Fsc Pre Medical"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("ICS"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("ICS"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("ICOM"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("ICOM"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("BCOM"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("BCOM"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("FA"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("FA"));
                         ViewBag.fee = query.installment_fee;
                     }
                     else if (q.deg_name.Equals("BA"))
                     {
                         var query = dc.fines.First(b => b.deg_name.Equals("BA"));
                         ViewBag.fee = query.installment_fee;
                     }

                 }



                 return View(dc.registers.First(a => a.reg_num.Equals(Request["reg"])));
             }
             catch (Exception e)
             {
                 return RedirectToAction("other_installment");
             }*/

        //}

        /*public ActionResult other_installment2()
        {

            if (Request["fee_type"] .Equals("Monthly Fee"))
            {
                mothly_registeration mr = new mothly_registeration();
                mr.confirmation(Request["reg"],Request["monthly"]);
            }

            else if (Request["fee_type"] == "Installment")
            {
                if (Request["install"] == "2")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query.Confirm_2nd = "clear";
                    query.fine = "0";
                }
                if (Request["install"] == "3")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query.Confirm_3rd = "clear";
                    query.fine = "0";

                }
                if (Request["install"] == "4")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query.Confirm_3rd = "clear";
                    query.fine = "0";

                }
                if (Request["install"] == "5")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query.Confirm_3rd = "clear";
                    query.fine = "0";

                }
                if (Request["install"] == "6")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query.Confirm_3rd = "clear";
                    query.fine = "0";

                }

                dc.SubmitChanges();
            }
            if (Request["fee_type"] == "monthly")
            {
                if (Request["install"] == "jan")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query._2_installment = Request["amount"];
                }
                else if (Request["install"] == "feb")
                {
                    var query = dc.installments.First(b => b.reg_num.Equals(Request["reg"]));
                    query._3_installment = Request["amount"];

                }
                dc.SubmitChanges();
            }

            return RedirectToAction("Confirmation");
        }*/


        public ActionResult get_challan_form()
        {
            return View();
        }


        public ActionResult get_challan_form1()
        {

            try
            {
                var q = dc.registers.First(b => b.reg_num.Equals(Request["reg"]));
                ViewBag.reg = q.reg_num;
                ViewBag.fee_type = q.fee_type;

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("get_challan_form");
            }
        }

        public ActionResult get_challan_form3()
        {
            var q = dc.fines.First(a => a.cat.Equals("Late Fee Fine"));

            if (Request["fee_type"].Equals("Monthly Fee"))
            {
                mothly_registeration mr = new mothly_registeration();
                int fine = 0, late_fine = 0, previous = 0, tution_fee = 0;

                string total = mr.get_challan_form(Request["reg"], Request["monthly"], out fine, out late_fine, out previous, out tution_fee);
                ViewBag.Late = Convert.ToString(late_fine);
                ViewBag.Fine = Convert.ToString(fine);
                ViewBag.Previous = Convert.ToString(previous);
                ViewBag.tution_fee = Convert.ToString(tution_fee);
                ViewBag.Total = Convert.ToString(total);
                ViewBag.after = Convert.ToString(Convert.ToInt32(total) + 500);
            }
            else
            {
                mothly_registeration mr = new mothly_registeration();
                int fine = 0, late_fine = 0, previous = 0, tution_fee = 0;
                string total = mr.installment_fee(Request["reg"], Request["install"], out fine, out late_fine, out previous, out tution_fee);
                ViewBag.Late = Convert.ToString(late_fine);
                ViewBag.Fine = Convert.ToString(fine);
                ViewBag.Previous = Convert.ToString(previous);
                ViewBag.tution_fee = Convert.ToString(tution_fee);
                ViewBag.Total = Convert.ToString(total);
                ViewBag.after = Convert.ToString(Convert.ToInt32(total) + 500);
            }
            return View();
        }

        public ActionResult get_challan_form2()
        {
            try
            {
                Confirmation con = new Confirmation();

                string tution = "0", pre = "0", late = "0",total="0";
                con.get_challan(Request["reg"], Request["install"], out  tution, out  pre, out  late, out total);
                ViewBag.Late = late;
                
                ViewBag.Previous = pre;
                ViewBag.tution_fee = tution;
                ViewBag.Total = total;
                return View();

            }
            catch (Exception e)
            {
                return RedirectToAction("get_challan_form");
            }

           
        }


        public ActionResult ReissueChallan()
        {
            return View();
        }

        public ActionResult ReissueChallan1()
        {
            try
            {
                var q = dc.registers.First(b => b.reg_num.Equals(Request["reg"]));
                ViewBag.reg = q.reg_num;
                ViewBag.fee_type = q.fee_type;
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("ReisueChallan");
            }
        }

        public ActionResult ReissueChallan2()
        {
            Confirmation con = new Confirmation();

            string tution = "0", pre = "0", late = "0", total = "0",fine ="0";
            con.reissue_challan(Request["reg"], Request["install"], Request["late"],Request["re-issue"],out fine, out tution, out  pre, out  late, out total);
            ViewBag.Previous = pre;
           
            ViewBag.tution_fee = tution;
            ViewBag.Total = total;
            ViewBag.fine = fine;
            ViewBag.late = late;
           
            

            return View();
        }

        // ********************************************* STUDENT   MANAGEMENT *************************************************//

        public ActionResult students()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return View();
        }
        public ActionResult add_student_menu()
        {
            return View();

        }



        public ActionResult first_year_menu()
        {
            return View();
        }

       

        public ActionResult fsc_engg()
        {

            if (Request.HttpMethod == "POST")
            {
                TempData["regnum"] = Request["reg"];
                TempData.Keep("regnum");
                ViewBag.reg_num = TempData["regnum"].ToString();
            }

            else
            {
                ViewBag.reg_num = TempData["regnum"].ToString();
                TempData.Keep("regnum");
            }
                
                
                var query = dc.registers.First(a => a.reg_num.Equals(TempData.Peek("regnum").ToString()));
                ViewBag.session = new SelectList(dc.Sessions, "Id", "session_name");
                List<SelectListItem> deg = new List<SelectListItem>();
                deg.Add(new SelectListItem{Text="--Select", Value="-1"});
                var get_degrees=from i in dc.Degrees select new {i.Id,i.deg_name};
                foreach (var i in get_degrees)
                {
                    deg.Add(new SelectListItem { Text = i.deg_name.ToString(), Value = i.Id.ToString() });
                }

                ViewBag.degree = new SelectList(deg, "Value", "Text");
                ViewBag.fee_type = query.fee_type.ToString();
                
              


                return View();
            }
            
            
        

        public ActionResult fsc_med()
        {
            return View();
        }

        public ActionResult ics()
        {
            return View();
        }

        public ActionResult icom()
        {
            return View();
        }

        public ActionResult bcom()
        {
            return View();
        }

        public ActionResult fa()
        {
            return View();
        }

        public ActionResult ba()
        {
            return View();
        }


         






        // ************    EDIT PASSWORD ******************

        public ActionResult edit_password1()
        {

            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();








            return View();






        }

        public ActionResult edit_password()
        {

            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            string sec = Request["sec"];

            try
            {






                return View(dc.students_tables.First(s => s.rollno == sec));

            }
            catch (Exception e)
            {

                return RedirectToAction("edit_password1");
            }



        }


        public ActionResult edit_final_password()
        {
            string p = Request["pass"];
            string id = Request["rollno"];
            try
            {

                var a = dc.students_tables.First(b => b.rollno == id);
                a.password = p;


                dc.SubmitChanges();


                return RedirectToAction("students");
            }
            catch (Exception e)
            {

                return RedirectToAction("edit_password");
            }

        }


        //*************************** END OF EDIT PASSWORD ************************









        // ************** START  OF   EDIT INFORMATION **************************


        public ActionResult add_information()
        {
            return View(dc.students_tables.ToList());
        }


        public ActionResult add_info1(String id)
        {
            ViewBag.roll = id;
            return View();
        }

        public ActionResult add_info_final()
        {
           
                 new add_student_info().add_personal(Request["roll"], Request["city"], Request["gender"], Request["add"], Request["email"]);
                 return RedirectToAction("add_information");
        }
            
            








        public ActionResult edit_information()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return View(dc.students_tables.ToList());
        }

        


        public ActionResult edit_information1(String id)
        {
            try
            {
                ViewBag.roll = id;
                if ((from i in dc.personal_infos where i.rollno.Equals(id) select i).Count()== 0)
                {
                    return RedirectToAction("add_information");
                }
                else
                {
                    return View(dc.personal_infos.First(a=>a.rollno.Equals(id)));
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("edit_infromation");
            }




        }

        public ActionResult edit_information2()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }


            new add_student_info().edit_info(Request["roll"], Request["city"], Request["gender"], Request["add"],Request["email"]);


            

            
            
            return RedirectToAction("edit_information");
            
            

        }


        //********************** END OF EDIT INFORMATON SECTION ******************************









        //******************* ADD STUDENT **************************************

        public ActionResult Add_Student_main()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return View();
        }
        public ActionResult  Add_Student()
        {
            try
            {

                if (Session["username"] == null)
                {

                    return RedirectToAction("logout");
                }

                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();





                add_student_info a = new add_student_info();
                a.add_student(Request["fname"], Request["lname"], Request["password"], Request["sec"], Request["degree"], Request["session"], Request["fee_type"], Request["reg"]);
                return RedirectToAction("first_year_menu");
            }


            catch (Exception e)
            {

                return RedirectToAction("first_year_menu");
            }
        }




        public ActionResult logout()
        {



            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }
            Session["username"] = null;
            return RedirectToAction("Index");



        }


        //*******************************Change Section**********************************


        public ActionResult change_section()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return View(dc.students_tables.ToList());
        }

        public ActionResult change_section1(string id)
        {
            try
            {
                var query = from i in dc.Degrees select i;
                var check=dc.students_tables.First(a=>a.rollno.Equals(id));
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
                foreach (var i in query)
                {
                    list.Add(new SelectListItem { Text =i.deg_name.ToString(), Value = i.Id.ToString()});

                }
                ViewBag.degree = list;
                ViewBag.session = new SelectList(dc.Sessions, "Id","session_name");
                ViewBag.section = (from i in dc.sections where i.Id.Equals(check.section) select i.section1.ToString()).First();
                ViewBag.deg = (from i in dc.Degrees where i.Id.Equals(check.degree) select i.deg_name.ToString()).First();
                return View(dc.students_tables.First(a=>a.rollno.Equals(id)));

            }
            catch (Exception e)
            {
                return RedirectToAction("change_section");
            }
        }
        public ActionResult change_section2()
        {
            
           
           
            add_student_info a = new add_student_info();
            a.change_section(Request["rollno"], Request["degree"], Request["new_degree"], Request["sec"], Request["new_sec"],Request["session"]);


            return RedirectToAction("change_section");
        }





        // ******************************* END OF ADD STUDENT SECTION *******************************







        //*****************************END OF STUDENT  MANAGEMENT SECTION *************************









        // ****************************** TEACHER MANAGEMENT ***********************************

        public ActionResult teachers()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            return View();
        }


        //********************** TEACHER SECTION ******************************



        public ActionResult Add_Teacher1()
        {
            return View();
        }

        public ActionResult Add_Teacher()
        {

            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();


            string fname = Request["fname"];
            string lname = Request["lname"];
            string email = Request["email"];

            string password = Request["password"];
            string gender = Request["sex"];
            string city = Request["city"];
            string add = Request["add"];
            add_teacher_info at = new add_teacher_info();
            at.add(fname, lname, email, password, gender, city, add, "10000");











            return RedirectToAction("Add_Teacher1");



        }




        //*****************EDIT TEACHER INFORMATION **********************************



        public async Task<ActionResult> edit_teacher_information()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            
            
            return View( await dc.teacher_tables.ToListAsync());
        }



        public ActionResult edit_teacher_information1(int id)
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();



            return View(dc.teacher_tables.First(s => s.Id == id));




        }


        public ActionResult edit_teacher_information2()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            string fname = Request["fname"];
            string lname = Request["lname"];
            string email = Request["email"];

            string password = Request["password"];
            string gender = Request["sex"];
            string city = Request["city"];
            string add = Request["add"];

            string roll = Request["roll"];



            var a = dc.teacher_tables.First(b => b.teacher_id == roll);


            a.fname = fname;
            a.lname = lname;
            a.email = email;
            a.password = password;
            a.gender = gender;
            a.city = city;
            a.address = add;




            dc.SubmitChanges();

            return RedirectToAction("edit_teacher_information");


        }

        //****************** END OF EDIT TEACHER INFORMATIO SECTION ********************






        //************** EDIT TEACHER PASSWORD ***********************




        public ActionResult edit_teacher_password()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();





            string id = Request["id"];




            try
            {



                return View(dc.teacher_tables.First(a => a.teacher_id.Equals(id)));
            }
            catch (Exception e)
            {

                return RedirectToAction("edit_teacher_password1");
            }








        }

        public ActionResult edit_teacher_password1()
        {
            if (Session["username"] == null)
            {

                return RedirectToAction("logout");
            }

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();



            return View(dc.teacher_tables.ToList());
        }


        public ActionResult edit_teacher_password_final()
        {


            string p = Request["pass"];
            string id = Request["sec"];
            try
            {

                var a = dc.teacher_tables.First(b => b.teacher_id == id);
                a.password = p;


                dc.SubmitChanges();


                return RedirectToAction("edit_teacher_password1");
            }
            catch (Exception e)
            {

                return RedirectToAction("edit_password");
            }

        }

        public ActionResult edit_teacher_password_table(string id)
        {

            return View(dc.teacher_tables.First(s => s.teacher_id.Equals(id)));
        }







        //*******************************************MANAGE TEACHER SECTION ************************************************

        public ActionResult manage_teacher_section()
        {


            return View(dc.teacher_tables.ToList());

        }


        public ActionResult add_sections(string id)
        {
            try
            {
                var query = dc.teacher_tables.First(a => a.teacher_id.Equals(id));
                var degs = from i in dc.Degrees select new { i.deg_name, i.Id };
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem { Text = "--select--", Value = "-1" });
                foreach (var c in degs)
                {
                    list.Add(new SelectListItem { Text = c.deg_name, Value = c.Id.ToString() });

                }
                var degs1 = from i in dc.Sessions select new {i.session_name,i.Id };
                List<SelectListItem> lists = new List<SelectListItem>();
                lists.Add(new SelectListItem { Text = "--select--", Value = "-1" });
                foreach (var c in degs1)
                {
                    lists.Add(new SelectListItem { Text = c.session_name, Value = c.Id.ToString() });

                }
                ViewBag.session = lists;
                ViewBag.degree = list;
                ViewBag.fname = query.fname;
                ViewBag.lname = query.lname;

                return View(query);
            }
            catch (Exception e)
            {
                return RedirectToAction("manage_teacher_section");
            }
        }

        public JsonResult get_section(string id)
        {
            try
            {
                var query = from i in dc.sections where i.degree.Equals(id) select new { i.section1, i.Id };
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
                foreach (var i in query)
                {
                    list.Add(new SelectListItem { Text = i.section1.ToString(), Value = i.Id.ToString() });
                }

                return Json(new SelectList(list, "Value", "Text"));
            }
            catch (Exception e)
            {
                return Json(false);
            }
            

            


        }
        public JsonResult get_subjects(string id,string sess)
        {
            var query = from i in dc.Subjects where i.degree.Equals(id) && i.session.Equals(sess) select new { i.Id, i.subjects };
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--Select--", Value = "-1" });
            foreach (var i in query)
            {
                list.Add(new SelectListItem { Text = i.subjects.ToString(), Value=i.Id.ToString()});
                

            }
            return Json(new SelectList(list, "Value", "Text"));

        }





        public ActionResult add_sections1()
        {


            string id = Request["t_id"];

            add_teacher_info at = new add_teacher_info();
            at.AddTeacherSection(Request["t_id"], Request["degree"], Request["section"], Request["subjects"],Request["session"]);

             return RedirectToAction("manage_teacher_section");
        }




        public ActionResult delete_sections(String id)
        {

            try
            {

                List<SelectListItem> lists = new List<SelectListItem>();
                lists.Add(new SelectListItem { Text = "--select--", Value = "-1" });
                var degs1 = from i in dc.teacher_sections where i.teacher_id.Equals(id) select i.teacher_sec;

                foreach (var i in degs1)
                {
                    var get_sess = (from j in dc.sections where j.Id == Convert.ToInt32(i.ToString()) select j.section1.ToString()).First();
                    if (!lists.Any(x => x.Value == i.ToString() && x.Text == get_sess.ToString()))
                    {
                        lists.Add(new SelectListItem { Text = get_sess, Value = i.ToString() });
                    }

                }
                ViewBag.section = lists;
                return View(dc.teacher_tables.First(a => a.teacher_id.Equals(id)));
            }
            catch (Exception e)
            {
                return RedirectToAction("manage_teacher_section");
            }
              
                
                
            }
            
        

        public ActionResult delete_sections2()
        {
            var query = (from i in dc.teacher_sections where (i.teacher_id.Equals(Request["t_id"]) && i.teacher_sec.Equals(Request["section"]) && i.teacher_subject.Equals(Request["subjects"]) ) select i).First();
            dc.teacher_sections.DeleteOnSubmit(query);
            dc.SubmitChanges();
            
            return RedirectToAction("manage_teacher_section");
        }



        public JsonResult get_teacherSection(String id,String deg)

        {

            var query = from i in dc.teacher_sections where i.teacher_id.Equals(id) && i.teacher_degree.Equals(deg) select i.teacher_sec;
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--Select --", Value = "-1" });
            foreach( var i in query){
                if(!list.Any(x=>x.Text==i.ToString() && x.Value==i.ToString()))
                {
            list.Add(new SelectListItem {Text=i.ToString(),Value=i.ToString() });
                }
            }

            return Json(new SelectList(list, "Value","Text"));

        }



        public JsonResult get_teacherSubjects(String id, String sec)
        {

            var query = from i in dc.teacher_sections where i.teacher_id.Equals(id) && i.teacher_sec.Equals(sec) select i.teacher_subject;
            
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "--Select --", Value = "-1" });
            foreach (var i in query)
            {
                var query1 = (from j in dc.Subjects where j.Id.Equals(Convert.ToInt32(i)) select j.subjects.ToString()).First();
                if (! list.Any(x => x.Text == i.ToString() && x.Value == i.ToString()))
                {
                    list.Add(new SelectListItem { Text = query1.ToString(), Value = i.ToString() });
                }
            }
            
            
           

            return Json(new SelectList(list,"Value","Text"));

        }


        public ActionResult update_subjects()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            List<SelectListItem> list = new List<SelectListItem>();
            var query=from i in dc.Sessions select i;
            list.Add(new SelectListItem {Text="--select",Value="-1"});
            foreach (var i in query)
            {
                list.Add(new SelectListItem { Text = i.session_name, Value = i.Id.ToString() });
            }
            ViewBag.sess=list;
            return View();


        }


        public JsonResult get_subjects_for_update(string deg,string sess)
        {
          {
              try
              {
                  var query = (from i in dc.Subjects where i.degree.Equals(deg) && i.session.Equals(sess) select i);
                  List<SelectListItem> sub = new List<SelectListItem>();
                  foreach (var i in query)
                  {
                      sub.Add(new SelectListItem { Text = i.subjects.ToString(), Value = i.subjects.ToString() });
                  }



                  return Json(new SelectList(sub, "Value", "Text"));
              }
              catch (Exception e)
              {
                  return Json(false);
              }
            }
            

        }

        public ActionResult update_finally()
        {
            try
            {

                new Subject_Work().update_subject(Request["degree"], Request["session"], Request["subjects"], Request["new_name"]);
                return RedirectToAction("update_subjects");
            }
            catch (Exception e)
            {
                return RedirectToAction("update_subjects");
            }

            
            
            
        }


        public ActionResult delete_subjects()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            List<SelectListItem> list = new List<SelectListItem>();
            var query = from i in dc.Sessions select i;
            list.Add(new SelectListItem { Text = "--select", Value = "-1" });
            foreach (var i in query)
            {
                list.Add(new SelectListItem { Text = i.session_name, Value = i.Id.ToString() });
            }
            ViewBag.sess = list;
            return View();

        }

        public ActionResult delete_subjects_finally()
        {
            new Subject_Work().delete_subject(Request["degree"], Request["session"], Request["subjects"]);
            return RedirectToAction("delete_subjects");
        }






        //****************** END OF TEACHER MANGEMENT SECTION ************************



        //***************** Attendance Management Section ******************************





        public ActionResult AttendanceManagement()
        {

            List<student_subject> s1 = new List<student_subject>();
            s1 = dc.student_subjects.ToList();


            return View(s1);
        }



        public ActionResult at2()
        {
            string sec = Request["sec"];
            string sub = Request["sub"];

            data D = new data();
            D.making_target(sec, sub);

            return RedirectToAction("check_section_attendance");


        }

        public ActionResult student_attendance_menu()
        {

            return View();
        }





        public ActionResult check_student_Attendance()
        {
            return View();

        }

        public ActionResult check_student_month_attendance()
        {
            return View();
        }

        public ActionResult check_studentAttendance1()
        {
            try
            {
                if (Request.HttpMethod == "POST")
                {


                    TempData["sub"] = Request["sub"];
                    TempData["roll"] = Request["roll"];
                    TempData["month"] = Request["month"];
                }

                    MakeAttendance ch_attend = new MakeAttendance();
                    ViewBag.attend = ch_attend.populate_student_month_datelist(TempData.Peek("roll").ToString(), TempData.Peek("sub").ToString(), TempData.Peek("month").ToString());

                    if (ViewBag.attend.Count == 0)
                    {
                        return RedirectToAction("check_student_Attendance");
                    }
                    return View();


                }
        
                
        
            
            catch (Exception e)
            {
                return RedirectToAction("check_student_Attendance");
            }

        }

        public ActionResult check_section_attendance()
        {
            return View();
        }
        public ActionResult check_section_monthly_attendance()
        {
            return View();
        }

        public ActionResult check_section_attendance1()
        {

            MakeAttendance ch_attend = new MakeAttendance();

            if (Request.HttpMethod == "POST")
            {
                TempData["Sec_sec"] = Request["sec"];
                TempData["Sec_sub"] = Request["sub"];
                ViewBag.attend = ch_attend.check_attendance(TempData.Peek("Sec_sub").ToString(), 
                    TempData.Peek("Sec_sec").ToString());
                ViewBag.date = ch_attend.date;
                ViewBag.count = ch_attend.count;
            }

            else
            {
                ViewBag.attend = ch_attend.check_attendance(TempData.Peek("Sec_sub").ToString(), TempData.Peek("Sec_sec").ToString());
                ViewBag.date = ch_attend.date;
                ViewBag.count = ch_attend.count;
            }


            return View();
        }
        public ActionResult check_section_month_attendance1()
        {
            if (Request.HttpMethod == "POST")
            {
                MakeAttendance ch_attend = new MakeAttendance();
                TempData["mon_sec"] = Request["sec"];
                TempData["mon_sb"] = Request["sub"];
                TempData["month"] = Request["month"];
                ViewBag.attend = ch_attend.check_attendance(TempData.Peek("mon_sb").ToString(), TempData.Peek("mon_sec").ToString(),
                TempData.Peek("month").ToString());
                ViewBag.date = ch_attend.date;
                ViewBag.count = ch_attend.count;
            }
            else
            {
                MakeAttendance ch_attend = new MakeAttendance();

                ViewBag.attend = ch_attend.check_attendance(TempData.Peek("mon_sb").ToString(), TempData.Peek("mon_sec").ToString(), TempData.Peek("month").ToString());
                ViewBag.date = ch_attend.date;
                ViewBag.count = ch_attend.count;
            }


            return View();
        }















        public ActionResult check_teacherAttendance()
        {
            return View();
        }






        //*********************** Section Management Sction *********************************
        public ActionResult SectionManagement()
        {
            try
            {
                if (TempData.Peek("Error").ToString() == "error")
                {
                    ViewBag.error = "The specified section already exists in database.";
                    TempData["Error"] = " ";



                }
                return View();

            }
            catch (Exception e)
            {
                return View();
            }




        }


        public ActionResult AddSection()
        {

            string se = Request["sec"];
            

            var query = from i in dc.sections
                        where i.section1.Equals(se)
                        select i.section1;


            if (query == null)
            {
                section sect = new section();
                sect.section1 = se;
                dc.sections.InsertOnSubmit(sect);
                dc.SubmitChanges();


                return RedirectToAction("SectionManagement");
            }





            TempData["Error"] = "error";
            TempData.Keep();
            return RedirectToAction("SectionManagement");


        }











        // ************************** TEACHER PORTAL ********************************************


        public ActionResult teacher_login()
        {
            teacher_table tt = new teacher_table();




            string email = Request["username"];
            string password = Request["password"];

            if (email != null && password != null)
            {
                try
                {


                    var v = dc.teacher_tables.First(x => x.teacher_id == email && x.password == password);
                    if (v.teacher_id.Equals(email) && v.password.Equals(password))
                    {
                        Session["username"] = v.ToString();


                        TempData["TNAME"] = v.fname;
                        TempData["TID"] = v.teacher_id;
                        TempData.Keep("TID");
                        TempData.Keep("TNAME");
                        //TempData["name"] = query.First();



                        ViewBag.id = email;
                        return RedirectToAction("teacher_mainpage");
                    }


                }
                catch (Exception e)
                {
                    ViewBag.error = "user name or password is incorrect";
                    return View();
                }


            }



            return View();

        }



        //***************************************TEACHER MAIN PAGE ***************************************



        public ActionResult teacher_mainpage()
        {
            ViewBag.mes = TempData.Peek("TNAME").ToString();
            ViewBag.id = TempData.Peek("TID").ToString();
            return View();
        }




        //********************************* STUDENT ATTENDANCE SECTION *************************************


        public ActionResult studentAttendance()
        {
            ViewBag.mes = TempData.Peek("TNAME").ToString();
            TempData.Keep("TNAME");
            TempData.Keep("TID");

            string id = TempData.Peek("TID").ToString();
            ViewBag.id = id;

            teacher_table tt = new teacher_table();

            var query = from i in dc.teacher_sections where i.teacher_id.Equals(id) select i.teacher_sec.ToString();
            List<SelectListItem> list_sec = new List<SelectListItem>();
            list_sec.Add( new SelectListItem{Text="--Select" ,Value="-1"});
            foreach(var i in query)
            {
               string   sections = (from d in dc.sections where d.Id.Equals(i.ToString()) select d.section1.ToString()).First();
               if(!list_sec.Contains(new SelectListItem{Text=sections,Value=i.ToString()}))
               {

               list_sec.Add(new SelectListItem { Text = sections, Value = i.ToString() });
               }
               


            }

            

           
            
            
            ViewBag.sec = list_sec;
            return View();
        }
       

        public ActionResult studentAttendance1(string date,string section,string sub)
        {
            try{
                ViewBag.mes = TempData.Peek("TID");
                
                

                 ViewBag.date = date;
                ViewBag.section = section;
                ViewBag.subject = sub;
                ArrayList al = new ArrayList();

                


                var query = from ids in dc.students_tables
                            where ids.section.Equals(section)
                            select ids.rollno;

                foreach (var idss in query)
                {
                    al.Add(idss);
                }
               
                ViewBag.id = al;
                return View();
            }
        
        catch(Exception e)
    {
        return RedirectToAction("studentAttendance");

    }
            
        }


        public ActionResult editAttendance(string date,string section,string sub)
        {
           ViewBag.sec = section;
           ViewBag.sub = sub;
           ViewBag.date = date;
            return View(dc.Attendances.Where(a=>a.Date.Equals(date)));
        }

        public ActionResult editAttendance1()
        {
            string sec=Request["sec"];
            string sub = Request["sub"];
            string date = Request["date"];

           
            List<string> arr=new List<string>();
            int  c= dc.Attendances.Where(a => a.section.Equals(sec) && a.Date.Equals(Request["date"]) && a.subject.Equals(Request["sub"])).Count();
            for (int i = 0; i < c; i++)
            {
                if (Request["rolls"+i.ToString()] == "checked")
                {
                    arr.Add(Request[i.ToString()]);
                }
                
            }
            new MakeAttendance().edit_attendance(arr, Request["sec"], Request["sub"], Request["date"]);

            return RedirectToAction("studentAttendance");

        }





        public JsonResult MarkAttendence(string ids,string attendence,string sect,string sub,string date)
        {
            
            MakeAttendance b = new MakeAttendance();
            string[] attendences = attendence.Split(',');
            int c = 0;
            foreach (string i in attendences)
            {
                 c = i.Count();
            }
            int d = c + 1;
            
            
          new MakeAttendance().mark_attendance1(ids, attendence,date,sect,sub);


           /* int c = 0;

            foreach (string s in query)
            {

                b.Mark_Attendance(Convert.ToString(s), Request["section"], Request["subject"], Request["date"], Request[Convert.ToString(c)]);
                c++;
            }*/

            return Json(true);
        }


        //************************************END OF STUDENT ATTENDENCE SECTION ************************






        //********************************* MARKS SECTION **************************************************






        //********************************Mid Term MARKS SECTION **************************************************** 


        public ActionResult MidMarks()
        {
            try
            {
                ViewBag.mes = TempData.Peek("TNAME").ToString();
                string id = TempData.Peek("TID").ToString();
                Subject_Work s = new Subject_Work();                                //Subjets class objects so that teacher can add student marks.......

                ViewBag.sel_sub = s.get_teacher_subjects(id);
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("teacher_mainpage");
            }
        }


        public ActionResult MidTermMarks1()
        {
            try
            {
                ViewBag.mes = TempData.Peek("TNAME").ToString();

                string s = Request["section"];
                String[] s1 = s.Split(' ');
                ViewBag.subject = s1[0];
                ViewBag.section = s1[1];
                marking m = new marking();
                ViewBag.marks = m.enter_paper_marks(s1[0], s1[1]);
                return View();

            }

            catch (Exception e)
            {
                return RedirectToAction("MidMarks");
            }

        }

        public ActionResult submit_mid_marks()
        {
            try
            {
                marking m = new marking();
                int c = 0;

                var query = from i in dc.students_tables
                            where i.section.Equals(Request["sec"])
                            select i.rollno;
                foreach (var i in query)
                {
                    m.submit_paper_marks(i, Request["sec"], Request["sub"], Request["t_marks"], Request["paper_type"], Request[Convert.ToString(c)]);
                    c++;
                }

                return RedirectToAction("MidMarks");
            }
            catch (Exception e)
            {

                return RedirectToAction("MidMarks");
            }
        }






        // ****************************** END OF QUIZ MARKS SECTION **************************************



        // ******************************  MID TERM MARKS SECTION **************************************


        public ActionResult Quizes()
        {

            try
            {
                string id = TempData.Peek("TID").ToString();
                ViewBag.mes = TempData.Peek("TNAME").ToString();

                Subject_Work s = new Subject_Work();

                ViewBag.sel_sub = s.get_teacher_subjects(id);

                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("teacher_mainpage");
            }







        }



        public ActionResult Quizes1()
        {
            try
            {
                ViewBag.mes = TempData.Peek("TNAME").ToString();

                string s = Request["section"];
                String[] s1 = s.Split(' ');
                ViewBag.subject = s1[0];
                ViewBag.section = s1[1];
                marking m = new marking();
                ViewBag.id = m.enter_paper_marks(s1[0], s1[1]);
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Quizes");
            }




        }




        // ***************************************** END OF MID TERM MARKS SECTION *************************************




        //***************************************END OF TEACHER PORTAL ****************************************************






        //***************************************** STUDENT LOGIN *********************************************************

        public ActionResult student_login()
        {

            students_table tt = new students_table();




            string email = Request["username"];
            string password = Request["password"];

            if (email != null && password != null)
            {
                try
                {


                    var v = dc.students_tables.First(x => x.rollno == email && x.password == password);
                    if (v.rollno.Equals(email) && v.password.Equals(password))
                    {
                        Session["username"] = v.ToString();
                        TempData["SID"] = email;
                        TempData.Keep("SID");
                        ViewBag.id = email;
                        return RedirectToAction("student_mainpage");
                    }


                }
                catch (Exception e)
                {
                    return RedirectToAction("student_login");
                }


            }



            return View();

        }





        //*************************************** STUDENT MAIN PAGE *******************************************
        public ActionResult student_mainpage()
        {
            string id = Convert.ToString(TempData["SID"]);
            ViewBag.mes = id;
            TempData.Keep("SID");
            return View();
        }
        public ActionResult student_attend_view()
        {
            try
            {
                string id = TempData.Peek("SID").ToString();
                ViewBag.mes = id;
                Subject_Work s = new Subject_Work();

                ViewBag.al = s.get_student_subjects(id);






                return View();
            }
            catch (Exception e)
            { return RedirectToAction("student_mainpage"); }
        }

        public ActionResult student_attend_view1()
        {
            if (Request.HttpMethod == "POST")
            {
                Subject_Work s = new Subject_Work();
                TempData["subject"] = Request["subject"];
                ViewBag.sub = TempData.Peek("subject").ToString();

                ViewBag.id = TempData.Peek("SID").ToString();


                ViewBag.attend = s.get_Attendance(ViewBag.sub, ViewBag.id);
            }
            else
            {
                Subject_Work s = new Subject_Work();
                ViewBag.sub = TempData.Peek("subject").ToString();
                ViewBag.id = TempData.Peek("SID").ToString();
                ViewBag.attend = s.get_Attendance(ViewBag.sub, ViewBag.id);


            }


            return View();
        }







        // ************************************* STUDENT SECTION *******************************************
        public ActionResult Subjects()
        {
            string id = TempData.Peek("SID").ToString();
            ViewBag.mes = id;
            string s = Request["sub"];
            ViewBag.mess = s;



            return View();
        }





        //*******************************************CHECK ATTENDENCE SECTION ***************************************


        public ActionResult check_attendence()
        {
            string id = TempData.Peek("SID").ToString();
            ViewBag.mes = id;


            string sub = Request["subject"];
            ViewBag.s = sub;


            if (sub == "Islamiat")
            {


                var query = from ids in dc.Islamiats
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Islamiats
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }



            if (sub == "Physics")
            {


                var query = from ids in dc.Physics
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Physics
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }



            else if (sub == "Chemistry")
            {


                var query = from ids in dc.Chemistries
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Chemistries
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }

            if (sub == "Biology")
            {


                var query = from ids in dc.Biologies
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Biologies
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }

            if (sub == "English")
            {


                var query = from ids in dc.Englishes
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Englishes
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }



            if (sub == "Urdu")
            {


                var query = from ids in dc.Urdus
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.Urdus
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }



            if (sub == "Maths")
            {


                var query = from ids in dc.mathmatics
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.mathmatics
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }


            if (sub == "PakStudies")
            {


                var query = from ids in dc.PakStudies
                            where ids.student_id.Equals(id)
                            select ids.attendence;
                ArrayList al = new ArrayList();
                foreach (var i in query)
                {
                    al.Add(i);

                }

                var query1 = from ids in dc.PakStudies
                             where ids.student_id.Equals(id)
                             select ids.Date;
                ArrayList al1 = new ArrayList();
                foreach (var i in query1)
                {
                    al1.Add(i);

                }


                ViewBag.a = al;
                ViewBag.b = al1;


            }





            return View();

        }




        //************************************* CHECK QUIZ SECTION *********************************************



        public ActionResult check_quiz()
        {

            string id = TempData.Peek("SID").ToString();

            string sub = Request["subject"];

            var query = from ids in dc.Marks
                        where ids.student_id.Equals(id) && ids.total_marks != "MidTerm" && ids.subject_name == sub
                        select ids.obtained_marks;



            var query1 = from ids in dc.Marks
                         where ids.student_id.Equals(id) && ids.total_marks != "MidTerm" && ids.subject_name == sub
                         select ids.total_marks;


            ArrayList al = new ArrayList();
            ArrayList al1 = new ArrayList();
            foreach (var i in query)
            {
                al.Add(i);
            }

            foreach (var i in query1)
            {
                al1.Add(i);
            }


            ViewBag.total = al1;
            ViewBag.obtain = al;





            return View();
        }




        // *********************************************** CHECK MID TERM MARKS ************************************************

        public ActionResult check_midterm()
        {

            string id = TempData.Peek("SID").ToString();

            string sub = Request["subject"];

            var query = from ids in dc.Marks
                        where ids.student_id.Equals(id) && ids.total_marks == "MidTerm" && ids.total_marks != "FinalTerm" && ids.subject_name.Equals(sub)
                        select ids.obtained_marks;



            var query1 = from ids in dc.Marks
                         where ids.student_id.Equals(id) && ids.total_marks == "MidTerm" && ids.total_marks != "FinalTerm" && ids.subject_name.Equals(sub)
                         select ids.total_marks;


            ArrayList al = new ArrayList();
            ArrayList al1 = new ArrayList();
            foreach (var i in query)
            {
                al.Add(i);
            }

            foreach (var i in query1)
            {
                al1.Add(i);
            }


            ViewBag.total = al1;
            ViewBag.obtain = al;
            return View();
        }



        //********************************Fine Management*****************************************
        public ActionResult Fine_Management_Main()
        {
            return View();
        }

        public ActionResult ManageFine()
        {
            ViewBag.list = new SelectList(dc.fines, "cat", "cat");
            return View();
        }

        public ActionResult add_category()
        {

            return View();

        }
        public ActionResult add_category2()
        {
            Degree_Work d = new Degree_Work();
            d.AddCategory(Request["cat"], Request["amount"]);
            return RedirectToAction("add_category");
        }


        public ActionResult ManageFine2()
        {
            Degree_Work d = new Degree_Work();
            d.edit(Request["cat"], Request["amount"]);
            return RedirectToAction("ManageFine");
        }
        public ActionResult AddFine()
        {

            return View();
        }

        public ActionResult addfine2()
        {
            try
            {
                if ((from i in dc.installments where i.reg_num.Equals(Request["reg"]) select i).Count() == 0)
                {
                    var query = dc.mothly_fees.First(a => a.reg_num.Equals(Request["reg"])).ToString();
                    foreach (var i in query)
                    {
                        ViewBag.reg = Request["reg"];
                    }
                }
                else
                {
                    var query1 = dc.installments.First(a => a.reg_num.Equals(Request["reg"])).ToString();
                    foreach (var i in query1)
                    {
                        ViewBag.reg = Request["reg"];
                    }

                }
                ViewBag.list = new SelectList(dc.fines, "cat", "cat");
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("addfine");
            }
        }
        public ActionResult addfine3()
        {
            try
            {
                Degree_Work d = new Degree_Work();

                d.AddFineToStudent(Request["reg"], Request["fine"]);
                return RedirectToAction("AddFine");

            }
            catch (Exception e)
            {
                return RedirectToAction("AddFine");
            }
        }

        //****************************************deg_name Managemnt ***************************

        public ActionResult Degree_Main()
        {
            return View();
        }

        public ActionResult add_deg_name()
        {

            return View();
        }

        public JsonResult add_degree2(String degname)
        {
            try
            {
                Degree_Work d = new Degree_Work();
                bool res = d.Add_Degree(degname);
                return Json(res);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

        public ActionResult delete_degree()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            return View();
        }

       /* public JsonResult get_degreefee(String deg)
        {
            var query = (from i in dc.Degrees where i.Id.Equals(deg) select i.deg_totalFee.ToString()).First();
            
            return Json(query.ToString());
            
        }*/

        public JsonResult update_degree2(String id,String new_deg)
        {
            try
            {
                Degree_Work d = new Degree_Work();
                bool ans = d.update_degree(id, new_deg);

                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }

       

       /* public ActionResult add_deg_name2()
        {
            if (Request.HttpMethod == "POST")
            {

                Degree_Work d = new Degree_Work();

                bool ans = d.Add_Degree(Request["degname"], Request["degfee"], String.Join(",", Request.Form.GetValues("degsubs")));
                if (ans == false)
                {
                    TempData["error"] = "This deg_name exists in database";
                    return RedirectToAction("add_deg_name");
                }

            }
            TempData["error"] = "The deg_name is added";
            return RedirectToAction("add_deg_name");
        }*/


        public ActionResult Section_Main()
        {
            return View();
        }


        public ActionResult add_section()
        {
            ViewBag.deg_name = new SelectList(dc.Degrees, "Id", "deg_name");
            return View();
        }

        public JsonResult add_section_in_database(String degree, String sections)
        {
            try
            {
                Degree_Work dw = new Degree_Work();
                bool ans = dw.Add_Section(degree, sections);
                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }

        }

        public ActionResult update_section()
        {

            ViewBag.section = new SelectList(dc.sections, "Id", "section1");

            return View();
        }

        public JsonResult update_section2(String id, String new_sec)
        {
            try
            {
                bool ans = new Degree_Work().update_section(id, new_sec);
                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }


        public ActionResult Subject_Main()
        {
            return View();
        }


        public ActionResult Add_Subjects()
        {
            ViewBag.degree = new SelectList(dc.Degrees, "Id", "deg_name");
            ViewBag.session = new SelectList(dc.Sessions, "Id", "session_name");
            return View();
        }

        public JsonResult add_seubjects_in_database(String degree, String subjects,string session)
        {
            try
            {
                bool ans = new Subject_Work().add_subjects(degree, subjects, session);
                return Json(ans);
            }
            catch (Exception e)
            {
                return Json(false);
            }

        }

        /*public ActionResult add_section2()
        {
            Degree_Work d = new Degree_Work();
            bool ans = d.Add_Section(Request["degree"], Request.Form.GetValues("section"));
            return RedirectToAction("add_section");
        }*/
        /*public ActionResult Download()
        {
            String constring = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\asus\Documents\Visual Studio 2012\Projects\my_project\my_project\App_Data\admin_database.mdf;Integrated Security=True";
            SqlConnection con = new SqlConnection(constring);
            //string d = "moazzam";
            // string query = "select * From students_table where fname='"+d+"'";  
            var query ="select * from Attendance where section='"+Request["sec"]+"' and subject ='"+Request["sub"]+"' ";
            DataTable dt = new DataTable();
           
            dt.TableName = "Attendance";
            
            con.Open();
           
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
              
                
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            con.Close();
            return RedirectToAction("check_section_attendance1");
        }*/

        /*public ActionResult  sendmail()
        {
            List<string> list = new List<string>();
            var query=from i in dc.students_tables  select i.email;
            new Thread(()=>
              {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("moazzamjaved0@gmail.com");
            mail.Subject = "Punjab College Muzaffarabad";
         * 
            mail.Body = "Dear Parents/Guardain It is message from Punjab College MZD to 
         *  infrom you that your son/daughter was absent today from college.
         *  For Further detail you can contact us at College Phone Numbers.Thankyou !!!!";
         *  
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "moazzamjaved0@gmail.com",
                Password = "newcampus456"
            };
            smtp.EnableSsl = true;
            foreach (var i in query)
            {


               
               //ssage from Punjab College MZD to infrom you that your son/daughter was absent 
         *     today from college.For Further detail you can contact us at College Phone Numbers.Thankyou !!!!";
                mail.To.Add(i.ToString());
          
            }
            smtp.Send(mail);
                }).Start();
        
            return RedirectToAction("Index");
           
        }*/

        //*********************************** Session Management ************************************

        public ActionResult SessionMain()
        {
            return View();
        }


        public ActionResult CreateSession()
        {
            return View();
        }

        public ActionResult AddSession()

        {
            SessionManagement sm = new SessionManagement();
            sm.Add_Session(Request["session"]);
            return RedirectToAction("SessionMain");
        }

        public ActionResult UpdateSession()
        {

            ViewBag.sessions = new SelectList(dc.Sessions, "session_name", "session_name");
            return View();
        }


           
   


    }
}
