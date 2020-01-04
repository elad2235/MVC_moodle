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

                try
                {
                    dal.Users.Add(objUsers); // in memory adding
                    dal.SaveChanges();
                    asvm.user = new Users();
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}", e);
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
                ModelState.AddModelError("", new Exception());
                TempData["Message"] = "student or course does not exist";
                return View("AssignStudent");
            }


            List<int> attending = (from x in stdal.Studies
                                   where x.student_id.Equals(student_id)
                                   select x.course_id).ToList<int>();


            //notice that this stupid mvc dosent work well with class property get methods
            CurrDal currDal = new CurrDal();
            int current_course_id = objCourses[0].course_id;
            List<Curriculum> current_course = (from x in currDal.Curriculum
                                               where x.course_id.Equals(current_course_id)
                                               select x).ToList();

            Boolean flag = false;
            foreach (int att in attending)
            {
                List<Curriculum> tmp = (from x in currDal.Curriculum
                                        where x.course_id.Equals(att)
                                        select x).ToList();


                if (tmp[0].day.Equals(current_course[0].day) && tmp[0].hour.Equals(current_course[0].hour))
                {
                    flag = true;
                }
                if (tmp.Count > 1 && current_course.Count > 1)
                {
                    if (tmp[1].day.Equals(current_course[1].day) && tmp[1].hour.Equals(current_course[1].hour)) flag = true;
                }
            }


            if (flag)
            {

                return View("AssignStudent");
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
            GradesViewModel gvm = new GradesViewModel();

            gvm.grade= new Grades();
            GradesDal gdal = new GradesDal();
            List<Grades> objGrades = gdal.Grades.ToList<Grades>();
            gvm.grades= objGrades;
            return View(gvm);
            
        }
       
            public ActionResult UpdateGradesAction()
        {
            GradesViewModel gvm = new GradesViewModel();
            gvm.grade = new Grades();
            GradesDal gdal = new GradesDal();

            int courseid = Convert.ToInt32(Request.Form["grade.course_id"]);
            int studentid = Convert.ToInt32(Request.Form["grade.student_id"]);
            int moedAgrade = Convert.ToInt32(Request.Form["grade.moedA"]);
            int moedBgrade = Convert.ToInt32(Request.Form["grade.moedB"]);

            

            return View(gvm);

        }
        public ActionResult edit_curr(Curriculum cur)
        {
            TempData["courseid"] = cur.course_id;
            return View(cur);

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
        
        
        public ActionResult Update_cur(Curriculum cur)
        {

            int id = Convert.ToInt32(TempData["courseid"].ToString());
            string day = Request.Form["day"].ToString();
            string hour = Request.Form["hour"].ToString();
            string classroom = Request.Form["classroom"].ToString();
            int duration = Convert.ToInt32(Request.Form["duration"]);
            CurrDal curdal = new CurrDal();
            List<Curriculum> updateCurr = (from x in curdal.Curriculum
                                           where x.course_id == id
                                           select x).ToList<Curriculum>();

            updateCurr[0].day = day;
            updateCurr[0].hour = hour;
            updateCurr[0].classroom = classroom;
            updateCurr[0].duration = duration;
            curdal.SaveChanges();


            CurriculumViewModel cvm = new CurriculumViewModel();

           
            CurrDal curdal2 = new CurrDal();
            List<Curriculum> objCurriculums = curdal2.Curriculum.ToList<Curriculum>();
            cvm.curriculums = objCurriculums;

            return View("ChangeSchedule1",cvm);

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
    }
  
    
}

