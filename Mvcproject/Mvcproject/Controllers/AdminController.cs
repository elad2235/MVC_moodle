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
            objStudents.student_id = 1;
            StudentsDal sdal = new StudentsDal();

            string find_user = (from x in dal.Users
                                where x.username.Equals(objUsers.username)
                                select x.username).FirstOrDefault();



        if(find_user==objUsers.username)
            {
                TempData["message"] = "user already exist";
                ModelState.AddModelError("", new Exception());

            }



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


        [HttpPost]
        public ActionResult Assign()
        {

            CoursesDal cdal = new CoursesDal();
            StudentsDal sdal = new StudentsDal();
            StudiesDal stdal = new StudiesDal();
            int course_id = Convert.ToInt32(TempData["courseid"].ToString());
            int student_id = Convert.ToInt32(Request.Form["student.student_id"]);
            Students objStudents = (from x in sdal.Students
                                          where x.student_id.Equals(student_id)
                                          select x).FirstOrDefault();

            Courses objCourses = (from x in cdal.Courses
                                        where x.course_id.Equals(course_id)
                                        select x).FirstOrDefault();

            Studies stpm = (from x in stdal.Studies
                            where x.course_id == course_id && x.student_id == student_id
                            select x).FirstOrDefault();


            CurrDal currDal = new CurrDal();
            int current_course_id = objCourses.course_id;
            //curr of the course we want to assign
            List<Curriculum> current_course = (from x in currDal.Curriculum
                                               where x.course_id.Equals(current_course_id)
                                               select x).ToList();
            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.course = (from x in cdal.Courses
                          where x.course_id == course_id
                          select x).FirstOrDefault();
            cvm.curriculums = current_course;


            if (stpm != null)
            {
                ModelState.AddModelError("", new Exception());
                TempData["Message"] = "student already takes this course";
                TempData["courseid"] = course_id;
                return View("Curr_list",cvm);
            }
            if (objCourses == null || objStudents == null)
            {
                ModelState.AddModelError("", new Exception());
                TempData["Message"] = "student or course does not exist";
                TempData["courseid"] = course_id;
                return View("Curr_list", cvm);
            }

            //courses the student already study
            List<int> attending = (from x in stdal.Studies
                                   where x.student_id.Equals(student_id)
                                   select x.course_id).ToList<int>();

            Boolean flag = false;
            foreach (int att in attending)
            {
                //cuu for each course the student already study
                List<Curriculum> tmp = (from x in currDal.Curriculum
                                        where x.course_id.Equals(att)
                                        select x).ToList();
                foreach(Curriculum c in tmp)
                {
                    foreach(Curriculum c1 in current_course)
                    {//add check for durations
                        if (c.day.Equals(c1.day) && c.hour.Equals(c1.hour)) flag = true;
                    }
                }

            }

            if (flag)
            {
                ModelState.AddModelError("", new Exception());
                TempData["Message2"] = "Student can't take two overlapping courses";
                TempData["courseid"] = course_id;
                return View("Curr_list", cvm);
            }
       
            Studies objStudies = new Studies();
            
            objStudies.course_id = objCourses.course_id;
            objStudies.student_id = objStudents.student_id;

           
            stdal.Studies.Add(objStudies); // in memory adding
            stdal.SaveChanges();
            TempData["courseid"] = course_id;
            return View("Curr_list", cvm);
        }
        
        public ActionResult Course_list_assign()
        {
            CurriculumViewModel cvm = new CurriculumViewModel();
            CoursesDal cdal = new CoursesDal();
            cvm.courses = cdal.Courses.ToList<Courses>();
            return View(cvm);
        }
        public ActionResult Curr_list(Courses c)
        {
            TempData["courseid"] = c.course_id;
            CurrDal curdal = new CurrDal();
            List<Curriculum> cur = (from x in curdal.Curriculum
                                    where x.course_id == c.course_id
                                    select x).ToList<Curriculum>();

            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.curriculums = cur;
            cvm.course = c;           
            return View(cvm);
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
        public ActionResult ChangeSchedule()
        {
            return View();
        }

        public ActionResult ChangeSchedule1()
        {
            CurriculumViewModel cvm = new CurriculumViewModel();

            cvm.course = new Courses();
            CoursesDal cdal = new CoursesDal();
            List<Courses> objCourses = cdal.Courses.ToList<Courses>();
            cvm.courses = objCourses;
            CurrDal curdal = new CurrDal();
            List<Curriculum> objCurriculums = curdal.Curriculum.ToList<Curriculum>();
            cvm.curriculums = objCurriculums;
            return View(cvm);
            
        }
        public ActionResult ChangeExam()
        {
            //remember to erase expandable
            CurriculumViewModel cvm = new CurriculumViewModel();

            cvm.course = new Courses();
            CoursesDal cdal = new CoursesDal();
            List<Courses> objCourses = cdal.Courses.ToList<Courses>();
            cvm.courses = objCourses;
            CurrDal curdal = new CurrDal();
            List<Curriculum> objCurriculums = curdal.Curriculum.ToList<Curriculum>();
            cvm.curriculums = objCurriculums;
            return View(cvm);
        }
        
        public ActionResult ChangeCourseSchedule()
        {

            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.courses = new List<Courses>();
            cvm.curriculums = new List<Curriculum>();
            cvm.course = new Courses();
            cvm.curriculum = new Curriculum();
            int id = Convert.ToInt32(Request.Form["curriculum.course_id"]);
            string day = Request.Form["curriculum.day"].ToString();
            string hour = Request.Form["curriculum.hour"].ToString();
            string classroom = Request.Form["curriculum.classroom"].ToString();
            int duration = Convert.ToInt32(Request.Form["curriculum.duration"]);
            CurrDal curdal = new CurrDal();
            List<Curriculum> updateCurr = (from x in curdal.Curriculum
                                           where x.course_id == id
                                           select x).ToList<Curriculum>();
           
            updateCurr[0].day = day;
            updateCurr[0].hour = hour;
            updateCurr[0].classroom = classroom;
            updateCurr[0].duration = duration;
            curdal.SaveChanges();
            
     
            return View("ChangeSchedule1", cvm);
        }
        public ActionResult ChangeExamAction()
        {

            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.courses = new List<Courses>();
            cvm.curriculums = new List<Curriculum>();
            cvm.course = new Courses();
            int id = Convert.ToInt32(Request.Form["course.course_id"]);
            string Adate = Request.Form["course.moed_A_date"].ToString();
            string Bdate = Request.Form["course.moed_B_date"].ToString();
            string Atime = Request.Form["course.moed_B_hour"].ToString();
            string Btime = Request.Form["course.moed_B_hour"].ToString();

            CoursesDal cdal = new CoursesDal();
            List<Courses> updateCourse = (from x in cdal.Courses
                                           where x.course_id == id
                                           select x).ToList<Courses>();
           
            updateCourse[0].moed_A_date = Adate;
            updateCourse[0].moed_B_date = Bdate;
            updateCourse[0].moed_A_hour = Atime;
            updateCourse[0].moed_B_hour = Btime;
            cdal.SaveChanges();
            cvm.courses = cdal.Courses.ToList<Courses>();
            
          

            return View("ChangeExam", cvm);
        }

        public ActionResult UpdateGrades()
        {
            CurriculumViewModel cvm = new CurriculumViewModel();

            CoursesDal cdal = new CoursesDal();
            cvm.courses = cdal.Courses.ToList<Courses>();
            return View(cvm);
            
        }
       
        public ActionResult UpdateGradesAction()
        {
            GradesViewModel gvm = new GradesViewModel();
            gvm.grade = new Grades();
            GradesDal gdal = new GradesDal();
            int cid = Convert.ToInt32(TempData["courseid"].ToString());
            int sid = Convert.ToInt32(TempData["studentid"].ToString());
            Grades grade = (from x in gdal.Grades
                            where x.course_id == cid && x.student_id == sid
                            select x).FirstOrDefault();

            int moedAgrade = Convert.ToInt32(Request.Form["moedA"]);
            int moedBgrade = Convert.ToInt32(Request.Form["moedB"]);
            grade.moedA = moedAgrade;
            grade.moedB = moedBgrade;
            gdal.SaveChanges();


            List<Grades> studentList = (from x in gdal.Grades
                                        where x.course_id == cid
                                        select x).ToList<Grades>();
            gvm.grades = studentList;
            return View("edit_grade1", gvm);

        }
        public ActionResult edit_curr(Courses c)
        {
            CurrDal curdal = new CurrDal();
            List<Curriculum> cur = (from x in curdal.Curriculum
                                    where x.course_id == c.course_id
                                    select x).ToList<Curriculum>();

            TempData["courseid"] = c.course_id;
            CurriculumViewModel cvm = new CurriculumViewModel();
            cvm.curriculum = new Curriculum();
            cvm.curriculums = cur;
            cvm.course = c;
            return View(cvm);

        }
        public ActionResult edit_grade(Grades g)
        {
            TempData["courseid"] = g.course_id;
            TempData["studentid"] = g.student_id;

            StudentsDal s = new StudentsDal();
            string fname = (from x in s.Students
                            where x.student_id == g.student_id
                            select x.firstname).FirstOrDefault();

            string lname = (from x in s.Students
                            where x.student_id == g.student_id
                            select x.lastname).FirstOrDefault();

            TempData["fname"] = fname;
            TempData["lname"] = lname;


            return View(g);

        }

        public ActionResult edit_grade1(Courses c)
        {
            GradesDal gdal = new GradesDal();
            List<Grades> studentList = (from x in gdal.Grades
                                     where x.course_id == c.course_id
                                     select x).ToList<Grades>();

            GradesViewModel gvm = new GradesViewModel();
            gvm.grades = studentList;
            return View(gvm);

        }
        public ActionResult Update_cur(Curriculum cur)
        {

            int id = Convert.ToInt32(TempData["courseid"].ToString());
            string day = Request.Form["curriculum.day"].ToString();
            string hour = Request.Form["curriculum.hour"].ToString();
            string classroom = Request.Form["curriculum.classroom"].ToString();
            int duration = Convert.ToInt32(Request.Form["curriculum.duration"]);
            CurrDal curdal = new CurrDal();
            List<Curriculum> updateCurr = (from x in curdal.Curriculum
                                           where x.course_id == id
                                           select x).ToList<Curriculum>();
            foreach (Curriculum c in updateCurr) {
                if (c.day.Equals(day)) {
                    ModelState.AddModelError("", new Exception());
                    TempData["Message"] = "course already scheduled to this day";
                   
                }
                    
                }
            Curriculum c1 = new Curriculum();
            c1.course_id = id;
            c1.day = day;
            c1.hour = hour;
            c1.classroom = classroom;
            c1.duration = duration;

            if (ModelState.IsValid)
            {
                curdal.Curriculum.Add(c1);
                curdal.SaveChanges();
            }

            CurriculumViewModel cvm = new CurriculumViewModel();          
            List<Curriculum> objCurriculums = curdal.Curriculum.ToList<Curriculum>();
            cvm.curriculums = objCurriculums;
            cvm.curriculum = new Curriculum();
            cvm.course = new Courses();
            TempData["courseid"] = cur.course_id;
            return View("edit_curr", cvm);

        }
        public ActionResult edit_course(Courses course)
        {
            TempData["courseid"] = course.course_id;
            return View(course);

        }
        public ActionResult Update_course(Courses course)
        {
            int id = Convert.ToInt32(TempData["courseid"].ToString());
            int teacher_id = Convert.ToInt32(Request.Form["teacher_id"]);
            string moed_A_date = Request.Form["moed_A_date"].ToString();
            string moed_B_date = Request.Form["moed_B_date"].ToString();
            string moed_A_hour = Request.Form["moed_A_hour"].ToString();
            string moed_B_hour = Request.Form["moed_B_hour"].ToString();
            CoursesDal cdal = new CoursesDal();
            List<Courses> updateCourse = (from x in cdal.Courses
                                           where x.course_id == id
                                           select x).ToList<Courses>();

            updateCourse[0].teacher_id = teacher_id;
            updateCourse[0].moed_A_date = moed_A_date;
            updateCourse[0].moed_B_date = moed_B_date;
            updateCourse[0].moed_A_hour = moed_A_hour;
            updateCourse[0].moed_B_hour = moed_B_hour;
            cdal.SaveChanges();


            CurriculumViewModel cvm = new CurriculumViewModel();

            cvm.course = new Courses();
            CoursesDal cdal1 = new CoursesDal();
            List<Courses> objCourses = cdal.Courses.ToList<Courses>();
            cvm.courses = objCourses;
       
            return View("ChangeExam", cvm);
        }
        public ActionResult Update_grade(Grades g)
        {
            int courseid = Convert.ToInt32(TempData["courseid"].ToString());
            int studentid = Convert.ToInt32(TempData["studentid"].ToString());
            int moedA = Convert.ToInt32(Request.Form["moedA"]);
            int moedB = Convert.ToInt32(Request.Form["moedB"]);
           
            GradesDal gdal = new GradesDal();
            List<Grades> updateGrade = (from x in gdal.Grades
                                          where x.course_id == courseid && x.student_id == studentid
                                         select x).ToList<Grades>();

            updateGrade[0].moedA = moedA;
            updateGrade[0].moedB = moedB;
            
            gdal.SaveChanges();


            GradesViewModel gvm = new GradesViewModel();

            gvm.grades= new List<Grades>();
            
            List<Grades> objGrades = gdal.Grades.ToList<Grades>();
            gvm.grades= objGrades;


            return View("UpdateGrades", gvm);
        }

        public ActionResult delete_course(Courses c)
        {
            CoursesDal cdal = new CoursesDal();
            StudiesDal sdal = new StudiesDal();
            CurrDal curdal = new CurrDal();

            Studies study = (from x in sdal.Studies
                             where x.course_id == c.course_id
                             select x).FirstOrDefault();
            if (study!=null)
            {
                sdal.Studies.Remove(study);
                sdal.SaveChanges();
            }


            Curriculum cur = (from x in curdal.Curriculum
                              where x.course_id == c.course_id
                              select x).FirstOrDefault();
            if (cur != null)
            {
                curdal.Curriculum.Remove(cur);
                curdal.SaveChanges();
            }

            Courses course = (from x in cdal.Courses
                             where x.course_id == c.course_id
                             select x).FirstOrDefault();
            cdal.Courses.Remove(course);
            CurriculumViewModel cvm = new CurriculumViewModel();
            cdal.SaveChanges();
            List<Courses> objCourse = cdal.Courses.ToList<Courses>();
            cvm.courses = objCourse;

            return View("ChangeExam",cvm);
        }
              
        [HttpPost]
        public ActionResult AddCourse()
        {
            string courseName = Request.Form["course.course_name"].ToString();
            int teacher_id = Convert.ToInt32(Request.Form["course.teacher_id"]);
            string moed_A_date = Request.Form["course.moed_A_date"].ToString();
            string moed_B_date = Request.Form["course.moed_B_date"].ToString();
            string moed_A_hour = Request.Form["course.moed_A_hour"].ToString();
            string moed_B_hour = Request.Form["course.moed_B_hour"].ToString();
            CoursesDal cdal = new CoursesDal();


            TeachersDal tdal = new TeachersDal();
            Teachers t = (from x in tdal.Teachers
                          where x.teacher_id == teacher_id
                          select x).FirstOrDefault();
                CurriculumViewModel cvm1 = new CurriculumViewModel();
            if(t == null)
            {        
                ModelState.AddModelError("", new Exception());
                TempData["Message"] = "teacher does not exist";
            }

            Courses objCourse = new Courses();
            objCourse.course_name = courseName;
            objCourse.teacher_id = teacher_id;
            objCourse.moed_A_date = moed_A_date;
            objCourse.moed_B_date = moed_B_date;
            objCourse.moed_A_hour = moed_A_hour;
            objCourse.moed_B_hour = moed_B_hour;


            DateTime dateA = DateTime.Parse(moed_A_date);
            DateTime dateB = DateTime.Parse(moed_B_date);
            int result = DateTime.Compare(dateA, dateB);

            if (result >= 0)
            {              
               
                ModelState.AddModelError("", new Exception());
                TempData["Message"] = "moed A must be erlier than moed B ";
            }


                if (ModelState.IsValid)
            {
                cdal.Courses.Add(objCourse);
                cdal.SaveChanges();
            }


            CurriculumViewModel cvm = new CurriculumViewModel();

            cvm.course = new Courses();           
            List<Courses> objCourses = cdal.Courses.ToList<Courses>();
            cvm.courses = objCourses;
              
            return View("ChangeExam", cvm);
        }

        public ActionResult delete_curr(Curriculum c)
        {
            CurrDal curdal = new CurrDal();
           

            Curriculum cur = (from x in curdal.Curriculum
                              where x.id == c.id
                              select x).FirstOrDefault();
           
                curdal.Curriculum.Remove(cur);
                curdal.SaveChanges();
     
            CurriculumViewModel cvm = new CurriculumViewModel();
            List<Curriculum> objCurriculums = curdal.Curriculum.ToList<Curriculum>();
            cvm.curriculums = objCurriculums;
            cvm.curriculum = new Curriculum();
            cvm.course = new Courses();
            TempData["courseid"] = cur.course_id;

            return View("edit_curr", cvm);
        }

    }
  
    
}

