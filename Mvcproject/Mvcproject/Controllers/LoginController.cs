using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvcproject.Models;
using Mvcproject.ModelView;
using Mvcproject.Dal;
using Mvcproject.ModelBinders;





namespace Mvcproject.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login


        [HttpPost]
        public ActionResult Submit()
        {
            UsersViewModel asvm = new UsersViewModel();
            Users objUsers = new Users();
            objUsers.username = Request.Form["user.username"].ToString();
            objUsers.password = Request.Form["user.password"].ToString();
            objUsers.permissions = 3;
            UsersDal dal = new UsersDal();



            Students objStudents = new Students();
            objStudents.firstname = Request.Form["student.firstname"].ToString();
            objStudents.lastname = Request.Form["student.lastname"].ToString();
            objStudents.username = objUsers.username;
            StudentsDal sdal = new StudentsDal();


         
            if (ModelState.IsValid)
            { //checks if the post from the form is valid


                dal.Users.Add(objUsers); // in memory adding
                dal.SaveChanges();
                asvm.user = new Users();

                sdal.Students.Add(objStudents); // in memory adding
                sdal.SaveChanges();
                asvm.student = new Students();
                asvm.students = sdal.Students.ToList<Students>();

            }
            else
            {

                asvm.user = objUsers;
                return View("AddStudent", asvm);


            }
            asvm.users = dal.Users.ToList<Users>();


            
           
            
            
     
            return View("AddStudent", asvm);

            
        }
        


        public ActionResult Assign()
        {
           

            CoursesDal cdal = new CoursesDal();
            StudentsDal sdal = new StudentsDal();
            StudiesDal stdal = new StudiesDal();
            string course_name = Request.Form["course.course_name"].ToString();
            string first_name = Request.Form["student.firstname"].ToString();
            string last_name = Request.Form["student.lastname"].ToString();
            List<Students> objStudents = (from x in sdal.Students
                                          where x.firstname.Equals(first_name) && x.lastname.Equals(last_name)
                                          select x).ToList<Students>();

            List<Courses> objCourses = (from x in cdal.Courses
                                          where x.course_name.Equals(course_name)
                                          select x).ToList<Courses>();
            if(objCourses.Count == 0 || objStudents.Count == 0)
            {
                return View("NoStudent");
            }
            Studies objStudies = new Studies();
            StudiesViewModel stvm = new StudiesViewModel();
            objStudies.course_id = objCourses[0].course_id;
            objStudies.student_id = objStudents[0].student_id;
            stdal.Studies.Add(objStudies); // in memory adding
            stdal.SaveChanges();
            stvm.study = new Studies();
            stvm.studies = stdal.Studies.ToList<Studies>();

            return View("Assign",stvm);
        }
        public ActionResult AssignStudent()
        {


            return View();
        }
        public ActionResult AddStudent()
        {

           
            return View();
        }
        public ActionResult HomePageStudent()
        {
          
            return View();
        }

        public ActionResult HomePageTeacher()
        {

            return View();
        }

        public ActionResult HomePageAdmin()
        {

            return View();
        }

        public ActionResult LoginPage()
        {
            UsersDal dal = new UsersDal();
            List<Users> objUsers = dal.Users.ToList<Users>();
            UsersViewModel uvm = new UsersViewModel();
            uvm.user = new Users();
            uvm.users = objUsers;
            return View(uvm);
        }

        [HttpPost]
        public ActionResult Login()
        {

            UsersDal dal = new UsersDal();

            string searchUsername = Request.Form["user.username"].ToString();
            string searchPassword = Request.Form["user.password"].ToString();
            List<Users> objUsers =
                (from x in dal.Users
                 where x.username.Equals(searchUsername) && x.password.Equals(searchPassword)
                 select x).ToList<Users>();
            UsersViewModel uvm = new UsersViewModel();
            uvm.users = objUsers;
            if (objUsers.Count!=0)
            {
                if (objUsers[0].permissions == 1) { return View("HomePageAdmin", uvm); }
                if(objUsers[0].permissions== 2) { return View("HomePageTeacher", uvm); }
                return View("HomePageStudent", uvm);
            }
            return View("LoginPage");

        }
    }
}