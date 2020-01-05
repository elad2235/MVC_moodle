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
            CoursesDal cdal = new CoursesDal();
            CurrDal curdal = new CurrDal();

            List<int> courses = (from x in cdal.Courses
                                 where x.teacher_id == t.teacher_id
                                 select x.course_id).ToList<int>();

            List<Curriculum> curs = new List<Curriculum>();
            foreach(int c in courses)
            {
                List<Curriculum> tmp = (from x in curdal.Curriculum
                                        where x.course_id == c
                                        select x).ToList<Curriculum>();
                if (tmp != null)
                {
                    foreach (Curriculum c1 in tmp)
                    {
                        curs.Add(c1);
                    }
                }
            
            }
            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.curriculums = curs;
            return View(cvm);
        }
        
              
    }
}