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
            if (!isFaculty())
                return RedirectToRoute("Login");
            return View();
        }

        public ActionResult ManageCourses()
        {
            if (!isFaculty())
                return RedirectToRoute("Login");
            return View("Courses");
        }

        public ActionResult AddCourse(Courses c)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

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
            if (!isFaculty())
                return RedirectToRoute("Login");

            return View("editCourse",c);
        }

        //Edit Course updates the requested Course

        public ActionResult update_course(Courses c,string moed_a,string moed_b)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");




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

            if(checkExamClassDate(c.moed_A,c.examA_class))
            {
                ModelState.AddModelError("", "Exam A Classroom is taken at the selected date and time");

                return View("editCourse", c);
            }

            if(c.moed_B !=null && checkExamClassDate(c.moed_B,c.examB_class))
            {
                ModelState.AddModelError("", "Exam B Classroom is taken at the selected date and time");
                return View("editCourse", c);
            }

            else
            {
                CoursesData activeCourses = new CoursesData();
                var toBeEdited = activeCourses.Courses.Where(x => x.course_id == c.course_id).First();

                if(new GradesData().Grades.Where(x => x.course_id == toBeEdited.course_id && x.moedA!=-1).ToList().Count() > 0)
                {
                    ModelState.AddModelError("", "Students Already have Moed A grade Cant change the date");
                    return View("editCourse", c);
                }


                if (new GradesData().Grades.Where(x => x.course_id == toBeEdited.course_id && x.moedB != -1).ToList().Count() > 0)
                {
                    ModelState.AddModelError("", "Students Already have Moed B grade Cant change the date");
                    return View("editCourse", c);
                }
                
                toBeEdited.moed_A = c.moed_A;
                toBeEdited.moed_B = c.moed_B;
                toBeEdited.teacher_id = c.teacher_id;
                toBeEdited.examA_class = c.examA_class;
                toBeEdited.examB_class = c.examB_class;

                if (toBeEdited.moed_A > toBeEdited.moed_B)
                {
                    ModelState.AddModelError("", "Moed A date needs to be after Moed B date");
                    return View("editCourse", c);
                }

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

        private bool checkExamClassDate(DateTime? moed_A, string examA_class)
        {
            return checkClassroomCrossover(moed_A, examA_class, 2);
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

        //Delete a Course
        public ActionResult delete_course(Courses c)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");
            if (c != null)
            {
                StudiesData studies = new StudiesData();
                CoursesData activeCourses = new CoursesData();
                CurriculumData curriculum = new CurriculumData();
                GradesData gradesData = new GradesData();
                List<Grades> studentGrades = gradesData.Grades.Where(y => y.course_id == c.course_id).ToList();

                if(studentGrades.Count>0)
                {
                    ModelState.AddModelError("", "Cant Delete Course, Students have grades");
                    return View("courses");
                }



                foreach (Studies x in studies.Studies.Where(y => y.course_id == c.course_id).ToList())//Remove students who take this course
                {
                    studies.Studies.Remove(x);
                }
                studies.SaveChanges();

                
                //foreach (Grades g in gradesData.Grades.Where(y => y.course_id == c.course_id).ToList())//Remove students grades from course
                //{
                //    gradesData.Grades.Remove(g);
                //}

                //gradesData.SaveChanges();

                
                foreach (Curriculum curr in curriculum.Curriculum.Where(y => y.course_id == c.course_id).ToList())//Remove lessons of course
                {
                    curriculum.Curriculum.Remove(curr);
                }
                curriculum.SaveChanges();

                activeCourses.Courses.Remove(activeCourses.Courses.Where(x => x.course_id == c.course_id).FirstOrDefault());
                activeCourses.SaveChanges();
            }

            return View("courses");
        }


        //Add study day to a course
        public ActionResult add_study_day(String Classroom, DateTime? time,DateTime? endDate, string dur)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

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



            if (endDate == null)
            {
                Curriculum curriculum = new Curriculum();
                curriculum.course_id = course_id;
                curriculum.classroom = Classroom;
                curriculum.time = time;
                curriculum.duration = duration;


                if(checkClassroomCrossover(time, Classroom,duration))
                {
                    ModelState.AddModelError("", "Other Course is using this classroom in the time entered");
                    return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                }



                if (checkSudentsCrossover(course_id, time, duration))
                {
                    ModelState.AddModelError("", "A student has a class in this date and time");
                    return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                }

                int teacher_id = new CoursesData().Courses.Where(x => x.course_id == curriculum.course_id).Select(y => y.teacher_id).FirstOrDefault();
                if(checkTeacherCrossover(teacher_id,time,duration))
                {
                    ModelState.AddModelError("", "Teacher has another lesson in the selected time");
                    return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                }

                CurriculumData curr = new CurriculumData();
                curr.Curriculum.Add(curriculum);
                curr.SaveChanges();


                return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
            }


            else
            {

                if (time > endDate)
                {
                    ModelState.AddModelError("", "End Date needs to be after starting date");
                    return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                }



                List<Curriculum> lesson_dates = new List<Curriculum>();
                Curriculum curriculum;
                int i = 0;
                endDate = endDate.Value.Date;

                while (time<endDate)
                {
                    curriculum = new Curriculum();
                    curriculum.course_id = course_id;
                    curriculum.classroom = Classroom;
                    curriculum.time = time;
                    curriculum.duration = duration;
                    time = time.Value.AddDays(7 * i);
                    i++;
                    curriculum.time = time;
                    if (checkSudentsCrossover(course_id, curriculum.time, duration))
                    {
                        return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                    }

                    if (checkClassroomCrossover(time, Classroom, duration))
                    {
                        ModelState.AddModelError("", "Other Course is using this classroom in the time entered");
                        return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                    }

                    int teacher_id = new CoursesData().Courses.Where(x => x.course_id == curriculum.course_id).Select(y => y.teacher_id).FirstOrDefault();
                    if (checkTeacherCrossover(teacher_id, time, duration))
                    {
                        ModelState.AddModelError("", "Teacher has another lesson in the selected time");
                        return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
                    }

                    lesson_dates.Add(curriculum);
                }

                CurriculumData curr = new CurriculumData();
                foreach (Curriculum lesson in lesson_dates)
                {
                    
                    curr.Curriculum.Add(lesson);
                }
                curr.SaveChanges();
                return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == course_id).FirstOrDefault());
            }
        }

        private bool checkTeacherCrossover(int teacher_id, DateTime? time, int duration)
        {

            CoursesData coursesData = new CoursesData();
            List<Courses> teaching = coursesData.Courses.Where(x => x.teacher_id == teacher_id).ToList();
            CurriculumData curriculumData = new CurriculumData();
            List<Curriculum> lessons = new List<Curriculum>();
            foreach(Courses c in teaching)
            {
                foreach (Curriculum curr in curriculumData.Curriculum)
                {
                    if (curr.course_id == c.course_id)
                        lessons.Add(curr);
                }
            }

            string original = time?.ToString("dd-MM-yy");
            string toCheck;

            foreach(Curriculum c in lessons)
            {
                toCheck = c.time.Value.ToString("dd-MM-yy");
                if (toCheck == original)
                    if (isDateTimeCrossing((DateTime)time, duration, (DateTime)c.time, c.duration))
                        return true;           
            }

            return false;
        }

        private bool checkClassroomCrossover(DateTime? time, string classroom,int duration)
        {
            CurriculumData curriculumData = new CurriculumData();
            CoursesData coursesData = new CoursesData();


            string original = time?.ToString("dd-MM-yy");
            string toCheck_A, toCheck_B,toCheck;


            foreach(Curriculum c in curriculumData.Curriculum)
            {

                toCheck = c.time.Value.ToString("dd-MM-yy");
                if(toCheck == original)
                    if (isDateTimeCrossing((DateTime)time, duration, (DateTime)c.time, c.duration))
                    {
                        if(c.classroom.Equals(classroom))
                            return true;
                    }
 
            }


            foreach (Courses c in coursesData.Courses)
            {
                if (c.moed_A != null)
                {
                    toCheck_A = c.moed_A.Value.ToString("dd-MM-yy");
                    if (toCheck_A == original)
                        if (isDateTimeCrossing((DateTime)time, duration, (DateTime)c.moed_A, 2))
                        {
                            if (c.examA_class.Equals(classroom))
                                return true;
                        }
                }

                if (c.moed_B!=null)
                {
                    toCheck_B = c.moed_B.Value.ToString("dd-MM-yy");
                    if (toCheck_B == original)
                        if (isDateTimeCrossing((DateTime)time, duration, (DateTime)c.moed_B, 2))
                        {
                            if (c.examB_class.Equals(classroom))
                                return true;
                        }
                }

            }

            return false;
        }


        //check if students have crossing lessons
        private bool checkSudentsCrossover(int course_id, DateTime? time,int dur)
        {

            StudiesData studies = new StudiesData();
            CoursesData coursesData = new CoursesData();
            List<Courses> courses_to_check = new List<Courses>();

            foreach(int s in studies.Studies.Where(x => x.course_id == course_id).Select(y => y.student_id).ToList())
            {
                List<int> student_courses = studies.Studies.Where(y => s == y.student_id).Select(k => k.course_id).ToList();
                foreach(Courses c in coursesData.Courses.Where(j => student_courses.Contains(j.course_id)))
                {
                    if(!courses_to_check.Contains(c))
                    {
                        courses_to_check.Add(c);
                    }
                }
            }


            CurriculumData curriculumData = new CurriculumData();

            
            foreach (Courses c in courses_to_check) 
            {
                //Go through all courses of students who take current course
                //and check if any lesson is crossing
                foreach (Curriculum o in curriculumData.Curriculum.Where(x => c.course_id == x.course_id).ToList())
                {
                    string t2 = o.time?.ToString("dd-MM-yy");
                    string t = time?.ToString("dd-MM-yy");
                    if (t.Equals(t2))
                    {
                        DateTime h = DateTime.Parse(time?.ToString("HH:MM"));
                        DateTime h2 = DateTime.Parse(o.time?.ToString("HH:MM"));

                        if (isDateTimeCrossing(h, dur, h2, o.duration))
                        {
                            ModelState.AddModelError("", "Student Has a course '" + o.course_id + "' lesson at " + o.time);
                            return true;
                        }
                    }
                }

            }

            return false;
        }


        //Adds Student to selected course
        public ActionResult Add_student_fCourse(Students student)
        {
            int c_id = Int32.Parse(Session["course_id"].ToString());
            int s_id = student.student_id;
            if (!isFaculty())
                return RedirectToRoute("Login");

            StudiesData studiesData = new StudiesData();
            CurriculumData curriculum = new CurriculumData();
            List<Curriculum> course_to_add = new List<Curriculum>();
            List<Curriculum> courses_to_check = new List<Curriculum>();

            //get Destination Course Schedule
            course_to_add = curriculum.Curriculum.Where(x => x.course_id == c_id).ToList();

            //get Student Schedule
            foreach(Studies s in studiesData.Studies.Where(y => y.student_id == s_id && y.course_id!=c_id))
            {
                foreach (Curriculum c in curriculum.Curriculum.Where(x => x.course_id == s.course_id).ToList())
                {
                    courses_to_check.Add(c);
                }
            }


            //Check if Student has other lessons in the same time
            foreach(Curriculum c in course_to_add)
            {
                foreach(Curriculum o in courses_to_check)
                {
                    string t = c.time?.ToString("dd-MM-yy");
                    string t2 = o.time?.ToString("dd-MM-yy");

                    if (t.Equals(t2))
                    {
                        DateTime h = DateTime.Parse(c.time?.ToString("HH:MM"));
                        DateTime h2 = DateTime.Parse(o.time?.ToString("HH:MM"));

                        if (isDateTimeCrossing(h,c.duration,h2,o.duration) )
                        {
                            ModelState.AddModelError("", "Student Has a course "+o.course_id +" in date "+ o.time);
                            return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == c.course_id).FirstOrDefault());
                        }
                    }
                }
            }


            Studies new_studies = new Studies();
            new_studies.course_id = c_id;
            new_studies.student_id = s_id;
            studiesData.Studies.Add(new_studies);
            studiesData.SaveChanges();
            return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == c_id).FirstOrDefault());
        }

        //Removes A student from the course 
        public ActionResult delete_student_fCourse(Students student)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

            int c_id = Int32.Parse(Session["course_id"].ToString());
            int s_id = student.student_id;


            StudiesData studies = new StudiesData();

            foreach(Studies s in studies.Studies.Where(x=> x.course_id == c_id && x.student_id==s_id))
            {

                studies.Studies.Remove(s);
            }
            studies.SaveChanges();


            GradesData grades = new GradesData();

            foreach (Grades g in grades.Grades.Where(x => x.course_id == c_id && x.student_id == s_id))
            {
                grades.Grades.Remove(g);
            }
            grades.SaveChanges();

            return View("editCourse", new CoursesData().Courses.Where(x => x.course_id == c_id).FirstOrDefault());
        }


        //Checks if two lessons are crossing
        public bool isDateTimeCrossing(DateTime present,int p_duration,DateTime added,int a_duration)
        {

            DateTime present_end = present.AddHours(p_duration);
            DateTime added_end = present.AddHours(a_duration);

            if ((present.Hour < added.Hour))
            {
                if (present_end.Hour < added.Hour)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else if ((present.Hour > added.Hour))
            {
                if (present.Hour > added_end.Hour)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }


        //Remove a course lesson
        public ActionResult remove_lesson(Curriculum toRemove)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");
            CurriculumData curriculumData = new CurriculumData();
            Curriculum actual_db_lesson = curriculumData.Curriculum.Where(x => x.course_id == toRemove.course_id && x.time == toRemove.time).FirstOrDefault();
            curriculumData.Curriculum.Remove(actual_db_lesson);
            curriculumData.SaveChanges();

            return RedirectToAction("edit_course", "FacultyManager",new CoursesData().Courses.Where(y => y.course_id == toRemove.course_id).FirstOrDefault());
        }

        public ActionResult newUser()
        {
            if (!isFaculty())
                return RedirectToRoute("Login");
            return View();
        }

        public ActionResult new_user(ViewModel.NewUserViewModel u, string permission)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

            UsersData usersData = new UsersData();
            Users isUser = usersData.Users.Where(x => x.username == u.user.username).FirstOrDefault(); ;

            if(isUser !=null)
            {
                ModelState.AddModelError("", "username is taken");
                return View("newUser");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(u.user.password);
            Users newUser = new Users();
            newUser.username = u.user.username;
            newUser.password = hashedPassword;

            if(permission.Equals("Teacher"))
            {
                newUser.permissions = 2;
                if (ModelState.IsValid)
                {

                    usersData.Users.Add(newUser);
                    usersData.SaveChanges();
                }

                TeachersData teachersData = new TeachersData();
                Teachers newTeacher = new Teachers();

                newTeacher.firstname = u.student.firstname;
                newTeacher.lastname = u.student.lastname;
                newTeacher.username = u.user.username;
                teachersData.Teachers.Add(newTeacher);
                teachersData.SaveChanges();
            }

            else
            {
                newUser.permissions = 3;
                if (ModelState.IsValid)
                {
                    usersData.Users.Add(newUser);
                    usersData.SaveChanges();
                }

                StudentsData studentsData = new StudentsData();
                Students newStudent = new Students();

                newStudent.firstname = u.student.firstname;
                newStudent.lastname = u.student.lastname;
                newStudent.username = u.user.username;
                studentsData.Students.Add(newStudent);
                studentsData.SaveChanges();
            }


            return View("Index");
        }

        //Send Student grade to View
        public ActionResult edit_grade(string student_id)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

            int c_id = Int32.Parse(Session["course_id"].ToString());
            int s_id = Int32.Parse(student_id);


            GradesData gradesData = new GradesData();
            Grades grade = gradesData.Grades.Where(x => x.student_id == s_id && x.course_id == c_id).FirstOrDefault();
            if (grade == null)
            {
                grade = new Grades();
                grade.course_id = c_id;
                grade.student_id = s_id;
            }
            return View("EditGrade", grade);
        }

        //update Grade in DB
        public ActionResult change_grade(Grades g)
        {
            if (!isFaculty())
                return RedirectToRoute("Login");

            GradesData gradesData = new GradesData();
            Grades student_grade = gradesData.Grades.Where(x => g.course_id == x.course_id && x.student_id == g.student_id).FirstOrDefault();
            CoursesData coursesData = new CoursesData();
            Courses student_course = coursesData.Courses.Where(x => x.course_id == g.course_id).FirstOrDefault();


            if (student_grade == null)
            {

                if (student_course.moed_A > DateTime.Now || (student_course.moed_B != null && student_course.moed_B > DateTime.Now))
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
            if ((student_course.moed_B == null && student_grade.moedB != -1) || student_course.moed_B == null || student_course.moed_B > DateTime.Now && g.moedB != student_grade.moedB)
            {
                ModelState.AddModelError("", "Cant update student grade B before the moed date");
                return View("EditGrade", g);
            }
            else
            {
                student_grade.moedB = g.moedB;
            }
            gradesData.SaveChanges();

            return View("edit_course", new CoursesData().Courses.Where(x => x.course_id == g.course_id).FirstOrDefault());
        }


        private bool isFaculty()
        {
            var perm = 0;
            try
            {
                perm = Int32.Parse(Session["permission"].ToString());
            }
            catch (Exception e)
            {
                return false;
            }

            if ( perm != 1)
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