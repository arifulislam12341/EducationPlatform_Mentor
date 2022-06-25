﻿using EducationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EducationPlatform.Controllers
{
    public class M_MentorController : Controller
    {
        // GET: M_Mentor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult M_MentorInformation()
        {
            var db = new EducationPlatformEntities();
            if (Session["MentorEmail"] != null)
            {
                var email = Session["MentorEmail"].ToString();
              
               // var mentor = db.Mentors.Where(d => d.Email == email).FirstOrDefault();
               var mentor= (from p in db.Mentors where p.Email == email select p).FirstOrDefault();
                return View(mentor);
            }
          //  else
        //    {
        //        return RedirectToAction("Index");
         //   }

            return View();
        }

        public ActionResult M_MentorConfirmDelete(int id)
        {
            Session["MentorId"] = id;
            return View();
        }
       
        public ActionResult M_MentorDelete()
        {
            var mentorid = Session["MentorId"].ToString();
            var id =int.Parse(mentorid);
            var db = new EducationPlatformEntities();
            var mentor=(from p in db.Mentors where p.Id == id select p).SingleOrDefault(); 
            db.Mentors.Remove(mentor);
            db.SaveChanges();
            return RedirectToAction("M_MentorLogIN");
        }
        [HttpGet]
        public ActionResult M_MentorUpdate(int id)
        {
            var db = new EducationPlatformEntities();
            var mentor = (from p in db.Mentors where p.Id == id select p).SingleOrDefault();
            return View(mentor);
        }
        [HttpPost]
        public ActionResult M_MentorUpdate(Mentor obj)
        {
           
                var db = new EducationPlatformEntities();
                var mentor = (from p in db.Mentors where p.Id == obj.Id select p).SingleOrDefault();
            mentor.Id = obj.Id; 
            mentor.Name = obj.Name;
            mentor.Address = obj.Address;   
            mentor.Email = obj.Email;
            mentor.Phone=obj.Phone;
            mentor.Password=obj.Password;
            mentor.Gender = obj.Gender;
            
              //  db.Entry(mentor).CurrentValues.SetValues(obj);
                db.SaveChanges();
                return RedirectToAction("M_MentorInformation");
           
        }
        [HttpGet]
        public ActionResult M_MentorLogIN()
        {
            return View();
        }
        [HttpPost]
       
        public ActionResult M_MentorLogIN(Mentor m)
        {
            var db = new EducationPlatformEntities();
            var mentor = db.Mentors.Where(d => d.Email == m.Email && d.Password == m.Password && d.IsValid == "Yes").FirstOrDefault();

        //    var mentor =(from p in db.Mentors where p.Email==m.Email && p.Password==m.Password && p.IsValid=="Yes" select p).FirstOrDefault();
            if (mentor != null )
            {
                Session["Type"] = 2;
                Session["MentorEmail"] = m.Email.ToString();
                FormsAuthentication.SetAuthCookie(m.Email, true);
                //FormsAuthentication.SetAuthCookie(a.Fname, true);
                return RedirectToAction("M_MentorInformation");
            }
            else
            {
                ViewBag.log = "Username or password is incorrect!";
            }

            return View();
        }

        public ActionResult M_MentorCourseDetails()
        { 

            var db=new EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId=(from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            var course = (from c in db.Courses where c.MentorId == mentorId select c).ToList();
            return View(course);
        }


        public ActionResult M_MentorStudentId(int id)
        {
            Session["COURSEID"] = id;//this session id used to review student M_MentorStudentReview
            var db = new EducationPlatformEntities();
            var studentId = (from p in db.ValidStudents where p.CourseId == id select p).ToList();




            return View(studentId);
        }
        public ActionResult M_MentorStudentList(int id)
        {
           
            var db =new EducationPlatformEntities();
           var studentId = (from p in db.Students where p.Id == id select p ).ToList();


          
           
           return View(studentId);
        }
        [HttpGet]
        public ActionResult M_MentorCounsiling(int id)
        {
            Session["COURSEID"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult M_MentorCounsiling(Counseling obj)
        {
            var courseid = Session["COURSEID"].ToString();
           

            var db=new EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            var counsiling = new Counseling()
            {
                MentorId = mentorId,
                CourseId = int.Parse(courseid), 
                MeetLink = obj.MeetLink,
                Details=obj.Details,    
                Date=obj.Date,
            };
            db.Counselings.Add(counsiling); 
            db.SaveChanges();
           // db.Counselings.Add(obj);
            return RedirectToAction("M_MentorCourseDetails");

        }


        public ActionResult M_MentorNotice(int id)
        {
            Session["COURSEID"] = id;
            return View();

        }

        [HttpPost]
        public ActionResult M_MentorNotice(Notice obj)
        {
            var courseid = Session["COURSEID"].ToString();
            var db = new EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            var mentorname=(from i in db.Mentors where i.Id == mentorId select i.Name).FirstOrDefault();    
            var notice = new Notice()
            {
                CourseId = int.Parse(courseid),
                AnnouncedBy = mentorname,
                AnnouncerId =mentorId,
                Details=obj.Details,
                Date=obj.Date,
            };
            db.Notices.Add(notice);
            db.SaveChanges();
            return RedirectToAction("M_MentorCourseDetails");

        }

        public ActionResult M_MentorRatings(int id)
        {
           
            var db = new EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
           

            var viewrate=(from i in db.Ratings where i.CourseId==id && i.MentorId==mentorId select i).ToList();
            return View(viewrate);
        }
        [HttpGet]
        public ActionResult M_MentorAssignment(int id)
        {
            Session["COURSEID"] = id;

            return View();  
        }
        [HttpPost]
        public ActionResult M_MentorAssignment(Assignment obj)
        {
            var courseid = Session["COURSEID"].ToString();
           // TempData["id"]=courseid;
           var db = new  EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            //  TempData["id"]=mentorId;
            var assignment = new Assignment()
            {
                MentorId = mentorId,
                CourseId=int.Parse(courseid),
                Question=obj.Question,
                Date=obj.Date,
            };
           // TempData["x"] = obj.Question;
           // TempData["y"] = obj.Date;
           db.Assignments.Add(assignment);
            db.SaveChanges();
            return RedirectToAction("M_MentorCourseDetails");

        }
        [HttpGet]
        public ActionResult M_MentorStudentReview(int id)//this student id came from M_MentorStudentId
        {
            Session["StudentId"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult M_MentorStudentReview(Reviewstudent obj)
        {
            var studentid = Session["StudentId"].ToString();
            var courseid = Session["COURSEID"].ToString();
            var db=new EducationPlatformEntities();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();

            var review = new Reviewstudent()
            {
                MentorId=mentorId,
                CourseId=int.Parse(courseid),
                StudentId=int.Parse(studentid),
                FeedBack=obj.FeedBack,
                Date=obj.Date,
            };
            db.Reviewstudents.Add(review);
            db.SaveChanges();
            return RedirectToAction("M_MentorCourseDetails");
        }
        [HttpGet]
        public ActionResult M_MentorStudentCertificate(int id)
        {
            Session["studentId"] = id;
            return View();
        }
        [HttpPost]
        public ActionResult M_MentorStudentCertificate(Certificate obj)
        {
            var studentId = Session["studentId"].ToString();
            var coursid = Session["COURSEID"].ToString();
           var db = new EducationPlatformEntities();    
            var email = Session["MentorEmail"].ToString();
            var recomanderId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            var recommanderName = (from i in db.Mentors where i.Email == email select i.Name).FirstOrDefault();

            var certifacte = new Certificate()
            {

                CourseId = int.Parse(coursid),
                RecommderId = recomanderId,
                RecomendBy = recommanderName,
                Status = "Pending",
                Comments = obj.Comments,
                Date = obj.Date,
                ApplierId = int.Parse(studentId),
            };
            db.Certificates.Add(certifacte);
            db.SaveChanges();

            //TempData["x"] = studentId;
             //TempData["y"] = coursid;
            return RedirectToAction("M_MentorCourseDetails");
        }
        [HttpGet]
        public ActionResult M_MentorStudentResult(int id)
        {
            Session["studentId"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult M_MentorStudentResult(Result obj)
        {

          //  var studentId= Session["studentId"].ToString();
            //   var courseId = Session["courseId"].ToString();
            //   var id = int.Parse(courseId);
            var db = new EducationPlatformEntities();
          //  var coursename = (from p in db.Courses where p.Id == id select p.Name).FirstOrDefault();
            var email = Session["MentorEmail"].ToString();
            var mentorId = (from i in db.Mentors where i.Email == email select i.Id).FirstOrDefault();
            
            var courseId=(from i in db.Assignments where i.MentorId == mentorId select i.CourseId).FirstOrDefault();
            
            var courseName = (from i in db.Courses where i.Id == courseId select i.Name).FirstOrDefault();
            var studentId = (from i in db.Certificates where i.CourseId == courseId select i.ApplierId).FirstOrDefault();
            var assignmentId = (from i in db.Assignments where i.MentorId == mentorId select i.Id).FirstOrDefault();

            var result = new Result()
            {
              CourseId = courseId,
              CourseName = courseName,
              StudentId=studentId,
               MentorId=mentorId,
               AssignmentId=assignmentId,
               Mark=obj.Mark,
               Date=obj.Date,   
               BackResult=obj.BackResult,
               Comment=obj.Comment,
            };
            db.Results.Add(result);
            db.SaveChanges();   
            return RedirectToAction("M_MentorCourseDetails");

        }
        public ActionResult M_MentorAssignmentList(int id)
        {
            var db= new EducationPlatformEntities();
            var assignment=(from p in db.Assignments where p.CourseId==id select p).ToList();
            return View(assignment);

        }
       
        public ActionResult M_MentorViewAnswer(int id)
        {

            var db = new EducationPlatformEntities();
            var answer = (from p in db.AnswerScripts where p.StudentId == id select p).FirstOrDefault();
            return View(answer);
        }

       public ActionResult M_MentorTrackStudent(int id)
        {
            var db = new EducationPlatformEntities();
            var answer = (from p in db.AnswerScripts where p.AssignmentId == id select p).ToList();
            return View(answer);
        }







        public ActionResult M_MentorLogOut()
        {
            Session.Clear();
            return RedirectToAction("M_MentorLogIN");
        }


        public ActionResult M_MentorAllStudentResults(int id)
        {
           // Session["CourseId"] = id;
           // var courseid = Session["CourseId"].ToString();
           // var Id=int.Parse(courseid);
            var db = new EducationPlatformEntities();

            var allstudentresults = (from p in db.Results where p.CourseId == id select p).ToList();

            return View(allstudentresults);
        }

        public ActionResult M_MentorReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from p in db.Results where p.Id == id select p).FirstOrDefault();
        
                results.BackResult = "Yes";
         
            db.SaveChanges();
            return RedirectToAction("M_MentorCourseDetails");
        }
        public ActionResult M_MentorNotReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from p in db.Results where p.Id == id select p).FirstOrDefault();

            results.BackResult = null;
          
            db.SaveChanges();
            return RedirectToAction("M_MentorCourseDetails");
        }


       public ActionResult M_MentorSeeNotice(int id)
        {
            var db = new EducationPlatformEntities();

            var institutionname = (from p in db.Mentors where p.Id == id select p.Institution).FirstOrDefault();
            var notice =(from p in db.Notices where p.AnnouncedBy==institutionname select p).ToList();

            return View(notice);
        }


    }
}
