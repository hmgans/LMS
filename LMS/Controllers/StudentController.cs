using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
  [Authorize(Roles = "Student")]
  public class StudentController : CommonController
  {

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Catalog()
    {
      return View();
    }

    public IActionResult Class(string subject, string num, string season, string year)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      return View();
    }

    public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
    {
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      ViewData["season"] = season;
      ViewData["year"] = year;
      ViewData["cat"] = cat;
      ViewData["aname"] = aname;
      return View();
    }


    public IActionResult ClassListings(string subject, string num)
    {
      System.Diagnostics.Debug.WriteLine(subject + num);
      ViewData["subject"] = subject;
      ViewData["num"] = num;
      return View();
    }


    /*******Begin code to modify********/

    /// <summary>
    /// Returns a JSON array of the classes the given student is enrolled in.
    /// Each object in the array should have the following fields:
    /// "subject" - The subject abbreviation of the class (such as "CS")
    /// "number" - The course number (such as 5530)
    /// "name" - The course name
    /// "season" - The season part of the semester
    /// "year" - The year part of the semester
    /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
    /// </summary>
    /// <param name="uid">The uid of the student</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetMyClasses(string uid)
    {
                //Does this work?
                var query = from p in db.Student
                            where p.UId.Equals(uid)
                            join x in db.Enrolled on p.UId equals x.UId
                            join y in db.Classes on x.ClassId equals y.ClassId
                            join z in db.Courses on y.CId equals z.CId
                            join k in db.Department on z.DId equals k.DId
                            select new
                            {
                                subject = k.Subject,
                                number = z.Number,
                                name = z.Name,
                                season = y.SemesterSeason,
                                year = y.SemesterYear,
                                grade = x.Grade
                            };


                return Json(query.ToArray());
    }

    /// <summary>
    /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
    /// Each object in the array should have the following fields:
    /// "aname" - The assignment name
    /// "cname" - The category name that the assignment belongs to
    /// "due" - The due Date/Time
    /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="uid"></param>
    /// <returns>The JSON array</returns>
    public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
    {
            var query = from p in db.Department
                        join g in db.Courses on p.DId equals g.DId
                        join h in db.Classes on g.CId equals h.CId
                        join z in db.AssignmentCategory on h.ClassId equals z.ClassId
                        join o in db.Assignments on z.AcId equals o.AcId
                        where p.Subject.Equals(subject) && g.Number.Equals(num) && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                        select new
                        {
                            Assignment = o,
                            AssignmentCat = z
                        };



            var query2 = from q in query
                         join s in db.Submissions on
                         new
                         {
                             A = q.Assignment.AssId,
                             B = uid
                         }
                         equals new
                         {
                             A = s.AssId,
                             B = s.UId

                         }
                         into p
                         from l in p.DefaultIfEmpty() select new
                         {
                             aname = q.Assignment.Name,
                             cname = q.AssignmentCat.Name,
                             due = q.Assignment.DueDate,
                             score = l.Score
                         };


      return Json(query2.ToArray());
    }



    /// <summary>
    /// Adds a submission to the given assignment for the given student
    /// The submission should use the current time as its DateTime
    /// You can get the current time with DateTime.Now
    /// The score of the submission should start as 0 until a Professor grades it
    /// If a Student submits to an assignment again, it should replace the submission contents
    /// and the submission time (the score should remain the same).
	/// Does *not* automatically reject late submissions.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The new assignment name</param>
    /// <param name="uid">The student submitting the assignment</param>
    /// <param name="contents">The text contents of the student's submission</param>
    /// <returns>A JSON object containing {success = true/false}.</returns>
    public IActionResult SubmitAssignmentText(string subject, int num, string season, int year, 
      string category, string asgname, string uid, string contents)
    {
            var query = (from p in db.Department
                        join g in db.Courses on p.DId equals g.DId
                        join h in db.Classes on g.CId equals h.CId
                        join x in db.Enrolled on h.ClassId equals x.ClassId
                        join w in db.Student on x.UId equals w.UId
                        join z in db.AssignmentCategory on h.ClassId equals z.ClassId
                        join o in db.Assignments on z.AcId equals o.AcId
                        join e in db.Submissions on o.AssId equals e.AssId
                        where p.Subject.Equals(subject) && g.Number.Equals(num) && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                        && e.UId.Equals(uid) && z.Name.Equals(category) && o.Name.Equals(asgname)
                        select e).Distinct();

            //Update Submission
            if(query.Count() == 1)
            {
                query.ToArray()[0].Contents = contents;
                query.ToArray()[0].Time = DateTime.Now;
                query.ToArray()[0].Score = 0;
                db.SaveChanges();
                return Json(new { success = true });
            }
            else // Create Submission if it doesnt exist 
            {
                var query2 = (from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join x in db.Enrolled on h.ClassId equals x.ClassId
                            join w in db.Student on x.UId equals w.UId
                            join z in db.AssignmentCategory on h.ClassId equals z.ClassId
                            join o in db.Assignments on z.AcId equals o.AcId
                            where o.Name.Equals(asgname) && p.Subject.Equals(subject) && g.Number.Equals(num)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                            && z.Name.Equals(category)
                            select o).Distinct();

                if (query2.Count() == 1)
                {
                    Submissions sub = new Submissions();
                    sub.UId = uid;
                    sub.AssId = query2.ToArray()[0].AssId;
                    sub.Time = DateTime.Now;
                    sub.Contents = contents;
                    sub.Score = 0;
                    db.Submissions.Add(sub);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
    }

    
    /// <summary>
    /// Enrolls a student in a class.
    /// </summary>
    /// <param name="subject">The department subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester</param>
    /// <param name="year">The year part of the semester</param>
    /// <param name="uid">The uid of the student</param>
    /// <returns>A JSON object containing {success = {true/false},
	/// false if the student is already enrolled in the Class.</returns>
    public IActionResult Enroll(string subject, int num, string season, int year, string uid)
    {
            var query = from p in db.Department
                        join g in db.Courses on p.DId equals g.DId
                        join h in db.Classes on g.CId equals h.CId
                        join x in db.Enrolled on h.ClassId equals x.ClassId where x.UId.Equals(uid)
                        where p.Subject.Equals(subject) && g.Number.Equals(num) && h.SemesterSeason.Equals(season) &&
                        h.SemesterYear == year
                        select x;

            if(query.Count() == 1)// If Student already exists return false
            {
                return Json(new { success = false });
            }
            else
            {
               var query2 = from p in db.Department
                        join g in db.Courses on p.DId equals g.DId
                        join h in db.Classes on g.CId equals h.CId
                        where h.SemesterSeason.Equals(season) && h.SemesterYear == year
                        && p.Subject.Equals(subject) && g.Number.Equals(num)
                        select h;

                if(query2.Count() == 1)
                {
                    Enrolled enroll = new Enrolled();
                    enroll.ClassId = query2.ToArray()[0].ClassId;
                    enroll.UId = uid;
                    enroll.Grade = "--";
                    db.Enrolled.Add(enroll);
                    db.SaveChanges();
                    return Json(new { success = true });

                }
            }

      return Json(new { success = false });
    }



    /// <summary>
    /// Calculates a student's GPA
    /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
    /// Assume all classes are 4 credit hours.
    /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
    /// If a student does not have any grades, they have a GPA of 0.0.
    /// Otherwise, the point-value of a letter grade is determined by the table on this page:
    /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
    /// </summary>
    /// <param name="uid">The uid of the student</param>
    /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
    public IActionResult GetGPA(string uid)
    {
            var query = from p in db.Enrolled
                        where p.UId.Equals(uid)
                        select p;

            //Get all Enrolled Classes
            double GPATotal = 0.0;
            int TotalClasses = 0;
            foreach(Enrolled cls in query)
            {
                TotalClasses++;
                switch (cls.Grade)
                {
                    case "A":
                        GPATotal += 4.0;
                        break;
                    case "A-":
                        GPATotal += 3.7;
                        break;
                    case "B+":
                        GPATotal += 3.3;
                        break;
                    case "B":
                        GPATotal += 3.0;
                        break;
                    case "B-":
                        GPATotal += 2.7;
                        break;
                    case "C+":
                        GPATotal += 2.3;
                        break;
                    case "C":
                        GPATotal += 2.0;
                        break;
                    case "C-":
                        GPATotal += 1.7;
                        break;
                    case "D+":
                        GPATotal += 1.3;
                        break;
                    case "D":
                        GPATotal += 1.0;
                        break;
                    case "D-":
                        GPATotal += 0.7;
                        break;
                    case "--":
                        TotalClasses--;
                        break;
                    default:
                        break;
                }

            }

            if(TotalClasses == 0)
            {
                return Json(new { gpa = 0.0 });
            }

            double result = GPATotal / TotalClasses;

            return Json(new { gpa = result });
    }

    /*******End code to modify********/

  }
}