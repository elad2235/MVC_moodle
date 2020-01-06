using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Moodle.Data_Access;
using Moodle.Models;

namespace Moodle.Controllers
{
    public class FacultyManagerController : Controller
    {
        // GET: FacultyManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageCourses()
        {
            return View("Courses");
        }

        public ActionResult AddCourse(Courses c)
        {
            CoursesData activeCourses = new CoursesData();
            TeachersData teachers = new TeachersData();
            Teachers teacher = teachers.Teachers.Where(x => x.teacher_id == c.teacher_id).FirstOrDefault();


            //Checks if teacher Exists in DB
            if (!isaTeacher(c))
            {
                return View("courses");
            }

            else
            {
                activeCourses.Courses.Add(c);
                activeCourses.SaveChanges();
            }

            return View("Courses");
        }

        //Edit Course View
        public ActionResult edit_course(Courses c)
        { 
            return View("editCourse",c);
        }

        //Edit Course updates the requested Course

        public ActionResult update_course(Courses c,string moed_a,string moed_b)
        {
            DateTime dateValue = default(DateTime);

            if ( !DateTime.TryParse(c.moed_A.ToString(),out dateValue) && c.moed_A!=null )
            {
                ModelState.AddModelError("", "Moed A Date should be of the format YYYY/MM/DD HH:MM");
                return View("editCourse",c);
            }


            if (!DateTime.TryParse(c.moed_B.ToString(), out dateValue) && c.moed_B != null)
            {
                ModelState.AddModelError("", "Moed B Date should be of the format YYYY/MM/DD HH:MM");
                return View("editCourse", c);
            }

            if(!isaTeacher(c))
            {
                return View("editCourse", c);
            }


            else
            {
                CoursesData activeCourses = new CoursesData();
                var toBeEdited = activeCourses.Courses.Where(x => x.course_id == c.course_id).First();

                toBeEdited.moed_A = c.moed_A;
                toBeEdited.moed_B = c.moed_B;
                toBeEdited.teacher_id = c.teacher_id;



                try
                {
                    activeCourses.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            return View("Courses");
        }

        //Checks if teacher id in c from POST is a Teacher in the DB
        public bool isaTeacher(Courses c)
        {
            TeachersData teachers = new TeachersData();
            Teachers teacher = teachers.Teachers.Where(x => x.teacher_id == c.teacher_id).FirstOrDefault();


            //Checks if teacher Exists in DB
            if (teacher == null)
            {
                ModelState.AddModelError("", "No Teacher with id "+c.teacher_id);
                return false;
            }
            return true;
        }


        public ActionResult delete_course(Courses c)
        {
            if (c != null)
            {
                StudiesData studies = new StudiesData();
                CoursesData activeCourses = new CoursesData();

                foreach (Studies x in studies.Studies.Where(y => y.course_id == c.course_id).ToList())
                {
                    studies.Studies.Remove(x);
                }
                studies.SaveChanges();
                activeCourses.Courses.Remove(activeCourses.Courses.Where(x => x.course_id == c.course_id).FirstOrDefault());
                activeCourses.SaveChanges();
            }

            return View("courses");
        }



        public ActionResult add_study_day(String Classroom, DateTime? time,string dur)
        {
                int course_id = Int32.Parse(TempData["course_id"].ToString());
            int duration=0;
            try
            {
                 duration = Int32.Parse(dur);
            }

            catch(Exception e)
            {
                ModelState.AddModelError("", "Duration needs to be a number");
                return View("courses");
            }

            if(duration==0)
            {
                ModelState.AddModelError("", "Duration of a lesson cannot be zero");
                return View("courses");
            }

            if (time == null)
            {
                ModelState.AddModelError("", "date and time need to be in the format of DD/MM/YYYY HH/MM");
                return View("courses");
            }


            Curriculum curriculum = new Curriculum();
            curriculum.course_id = course_id;
            curriculum.classroom = Classroom;
            curriculum.time = time;
            curriculum.duration = duration;

            CurriculumData curr = new CurriculumData();
            curr.Curriculum.Add(curriculum);
            curr.SaveChanges();



            return View("courses");
        }

    }


}