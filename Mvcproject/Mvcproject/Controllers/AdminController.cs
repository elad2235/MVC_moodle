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
    public class AdminController : Controller
    {
        // GET: Admin
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

                try
                {
                    dal.Users.Add(objUsers); // in memory adding
                    dal.SaveChanges();
                    asvm.user = new Users();
                }
                catch(Exception e)
                {
                    Console.WriteLine("{0}",e);
                }

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


        [HttpPost]
        public ActionResult Assign()
        {


            CoursesDal cdal = new CoursesDal();
            StudentsDal sdal = new StudentsDal();
            StudiesDal stdal = new StudiesDal();
            string course_name = Request.Form["course.course_name"];
            int student_id = Convert.ToInt32(Request.Form["student.student_id"]);
            List<Students> objStudents = (from x in sdal.Students
                                          where x.student_id.Equals(student_id)
                                          select x).ToList<Students>();

            List<Courses> objCourses = (from x in cdal.Courses
                                        where x.course_name.Equals(course_name)
                                        select x).ToList<Courses>();

            if (objCourses.Count == 0 || objStudents.Count == 0)
            {
                return View("NoStudent"); /// Needs Pop Error
            }


            List<int> attending = (from x in stdal.Studies
                                       where x.student_id.Equals(student_id)
                                       select x.course_id).ToList<int>();

            CurrDal currDal = new CurrDal();

            int current_course = Convert.ToInt32(from x in currDal.Curriculum
                                     where x.course_id.Equals(objCourses[0].course_id)
                                     select x.time);

            Boolean flag = false;
            foreach (int att in attending)
            {
               if((from x in currDal.Curriculum
                   where x.course_id.Equals(att) select x.time).ToString().Equals(current_course)){
                   flag = true;
                }
            }

            if(flag)
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

            return View("AssignStudent", stvm);
        }
        public ActionResult AssignStudent()
        {


            return View();
        }
        public ActionResult AddStudent()
        {


            return View();
        }


        public ActionResult HomePageAdmin()
        {

            return View();
        }

    }
}