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
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult HomePageTeacher()
        {
            string username = Session["username"].ToString();
            TeachersDal tdal = new TeachersDal();
            Teachers t = (from x in tdal.Teachers
                           where x.username.Equals(username)
                           select x).FirstOrDefault();
            return View(t);
        }
        public ActionResult View_TSchedule(Teachers t)
        {
         
            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.curriculums = t.get_Courses_Schedule();
            return View(cvm);
        }
        public ActionResult View_Courses(Teachers t)
        {
            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.courses = t.get_Courses_List();
            return View(cvm);
            
        }
        
        public ActionResult View_Students(Courses c)
        {
            TempData["coursename"] = c.course_name;
            TempData["courseid"] = c.course_id;

            StudentsViewModel svm = new StudentsViewModel();
            svm.students = c.get_Students_Of_Course();
            return View(svm);

        }
        public ActionResult UpdateGrades(Grades g)
        {  //Add test for exam date if erlier than today error message will be presented
            CoursesDal cdal = new CoursesDal();
            GradesDal gdal = new GradesDal();           
            int course = Convert.ToInt32(TempData["courseid"]);
            int student = Convert.ToInt32(TempData["studentid"]);

            Courses c = (from x in cdal.Courses
                        where x.course_id == course
                        select x).FirstOrDefault();

            Grades g1 = (from x in gdal.Grades
                        where x.course_id == course && x.student_id == student
                        select x).FirstOrDefault();
            if(g1 == null)
            {
                g.student_id = student;
                g.course_id = course;
                if (ModelState.IsValid)
                {
                    gdal.Grades.Add(g);
                    gdal.SaveChanges();
                }
                
            }
            else
            {
                g1.moedA = g.moedA;
                g1.moedB = g.moedB;
                if (ModelState.IsValid)
                {
                    gdal.SaveChanges();
                }
            }

           
            StudentsViewModel svm = new StudentsViewModel();
            svm.students = c.get_Students_Of_Course();
            return View("View_Students", svm);

        }
        public ActionResult EditGrades(Students s)
        {
            int course = Convert.ToInt32(TempData["courseid"]);
            TempData["fname"] = s.firstname;
            TempData["lname"] = s.lastname;
            GradesDal gdal = new GradesDal();
            Grades g = (from x in gdal.Grades
                        where x.student_id == s.student_id && x.course_id == course
                        select x).FirstOrDefault();
            if (g == null)
            {
                g = new Grades();
                g.student_id = s.student_id;
            }
            TempData["courseid"] = course;
            return View(g);
        }

    }
}