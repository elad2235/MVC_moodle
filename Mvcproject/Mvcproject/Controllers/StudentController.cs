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
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult HomePageStudent()
        {
            string username = Session["username"].ToString();
            StudentsDal sdal = new StudentsDal();
            Students st = (from x in sdal.Students
                           where x.username.Equals(username)
                           select x).FirstOrDefault();  
            return View(st);
        }
        public ActionResult View_Schedule(Students s)
        {
            StudiesDal stdal = new StudiesDal();
            CurrDal curdal = new CurrDal();
            CoursesDal cdal = new CoursesDal();
            CurriculumViewModel cvm = new CurriculumViewModel();
            List<int> course_list = (from x in stdal.Studies
                                     where x.student_id == s.student_id
                                     select x.course_id).ToList<int>();
            cvm.curriculums = new List<Curriculum>();
            cvm.courses = new List<Courses>();

            foreach(int c in course_list)
            {
                cvm.curriculums.Add((from x in curdal.Curriculum
                               where x.course_id == c
                               select x).FirstOrDefault());
            }

            foreach (int c in course_list)
            {
                cvm.courses.Add((from x in cdal.Courses
                                     where x.course_id == c
                                     select x).FirstOrDefault());
            }
            return View(cvm);
        }
        

    }
}