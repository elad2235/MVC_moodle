using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moodle.Data_Access;
using Moodle.Models;

namespace Moodle.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            if (!isTeacher())
                return RedirectToRoute("Login");

            string name = Session["username"].ToString();
            Session["id"] = new TeachersData().Teachers.Where(x => x.username.Equals(name)).First().teacher_id;
            return View();
        }

        public ActionResult MyCourses()
        {
            if (!isTeacher())
                return RedirectToRoute("Login");
            return View();
        }

        public ActionResult ViewCourse(Courses c)
        {
            if (!isTeacher())
                return RedirectToRoute("Login");
            return View(c);
        }


        public ActionResult edit_course_button(Courses course)
        {
            if (!isTeacher())
                return RedirectToRoute("Login");
            return View("ViewCourse",course);
        }


        public ActionResult edit_grade(Students student)
        {
            if (!isTeacher())
                return RedirectToRoute("Login");
            int c_id = Int32.Parse(Session["course_id"].ToString());
            int s_id = student.student_id;


            GradesData gradesData = new GradesData();
            Grades grade = gradesData.Grades.Where(x => x.student_id == s_id && x.course_id == c_id).FirstOrDefault();
            if(grade == null)
            {
                grade = new Grades();
                grade.course_id = c_id;
                grade.student_id = s_id;
                grade.moedA = -1;
                grade.moedB = -1;
            }
            return View("EditGrade",grade);
        }

        public ActionResult change_grade(Grades g)
        {
            if (!isTeacher())
                return RedirectToRoute("Login");

            GradesData gradesData = new GradesData();
            Grades student_grade = gradesData.Grades.Where(x => g.course_id == x.course_id && x.student_id == g.student_id).FirstOrDefault();
            CoursesData coursesData = new CoursesData();
            Courses student_course = coursesData.Courses.Where(x => x.course_id == g.course_id).FirstOrDefault();
            

            if (student_grade == null)
            {
                
                if(student_course.moed_A > DateTime.Now || (student_course.moed_B!=null && student_course.moed_B > DateTime.Now))
                {
                    ModelState.AddModelError("", "Cant update student grade before the moed date");
                    return View("EditGrade", g);
                }

                student_grade = new Grades();
                student_grade.course_id = g.course_id;
                student_grade.student_id = g.student_id;
                student_grade.moedA = -1;
                student_grade.moedB = -1;
                gradesData.Grades.Add(student_grade);
            }


            student_grade.moedA = g.moedA;
            if ( (student_course.moed_B==null && student_grade.moedB != -1)|| student_course.moed_B==null || student_course.moed_B > DateTime.Now && g.moedB != student_grade.moedB )
            {
                ModelState.AddModelError("", "Cant update student grade B before the moed date");
                return View("EditGrade", g);
            }
            else
            {
                student_grade.moedB = g.moedB;
            }
            gradesData.SaveChanges();

            return View("ViewCourse", new CoursesData().Courses.Where(x => x.course_id==g.course_id).FirstOrDefault());
        }

        private bool isTeacher()
        {
            var perm=0;
            try
            {
                 perm = Int32.Parse(Session["permission"].ToString());
            }
            catch(Exception e)
            {
                return false;
            }

            if ( perm != 2)
                return false;

            return true;

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToRoute("Login");
        }
    }
}