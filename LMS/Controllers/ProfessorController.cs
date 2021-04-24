using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
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

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
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

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            using (Team89LMSContext db = new Team89LMSContext())
            {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join x in db.Enrolled on h.ClassId equals x.ClassId
                            join w in db.Student on x.UId equals w.UId
                            where h.SemesterSeason.Equals(season) && h.SemesterYear == year && g.Number.Equals(num) && p.Subject.Equals(subject)
                            select new
                            {
                                fname = w.FirstName,
                                lname = w.LastName,
                                uid = w.UId,
                                dob = w.Dob,
                                grade = x.Grade
                            };
                return Json(query.ToArray());

            }
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            if (category != null)
            {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            join w in db.Assignments on s.AcId equals w.AcId
                            where p.Subject.Equals(subject) && g.Number.Equals(num)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year && s.Name.Equals(category)
                            select new
                            {
                                aname = w.Name,
                                cname = s.Name,
                                due = w.DueDate,
                                submissions = (from q in db.Submissions where q.AssId.Equals(w.AssId) select q).Count()
                            };
                return Json(query.ToArray());
            }
            else
            {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            join w in db.Assignments on s.AcId equals w.AcId
                            where p.Subject.Equals(subject) && g.Number.Equals(num)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                            select new
                            {
                                aname = w.Name,
                                cname = s.Name,
                                due = w.DueDate,
                                submissions = (from q in db.Submissions where q.AssId.Equals(w.AssId) select q).Count()
                            };
                return Json(query.ToArray());
            }
        }



        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            where p.Subject.Equals(subject) && g.Number.Equals(num) && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                            select new
                            {
                                name = s.Name,
                                weight = s.Weight
                            };

                return Json(query.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false},
        ///	false if an assignment category with the same name already exists in the same class.</returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var query1 = from p in db.Department
                        join g in db.Courses on p.DId equals g.DId
                        join h in db.Classes on g.CId equals h.CId
                        join w in db.AssignmentCategory on h.ClassId equals w.ClassId
                        where h.SemesterSeason.Equals(season) && h.SemesterYear == year && g.Number.Equals(num)
                        && p.Subject.Equals(subject) && w.Name.Equals(category)
                        select h.ClassId;
            if( query1.Count() == 1)
            {
                return Json(new { success = false });
            }

            var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            where h.SemesterSeason.Equals(season) && h.SemesterYear == year && g.Number.Equals(num)
                            && p.Subject.Equals(subject)
                            select h.ClassId;

            if (query.Count() == 1)
            {
                AssignmentCategory assignCat = new AssignmentCategory();
                assignCat.ClassId = query.ToArray()[0]; // TODO: This is throwing an exception because query is null for some reason
                assignCat.Name = category;
                assignCat.Weight = (uint?)catweight;

                db.AssignmentCategory.Add(assignCat);
                int success = db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false,
        /// false if an assignment with the same name already exists in the same assignment category.</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            where s.Name.Equals(category) && p.Subject.Equals(subject) && g.Number.Equals(num)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                            select new
                            {
                                ACID = s.AcId,
                                ClassID = s.ClassId
                            };

                    Assignments newAssign = new Assignments();
                    newAssign.AcId = query.ToArray()[0].ACID;
                    newAssign.Name = asgname;
                    newAssign.DueDate = asgdue;
                    newAssign.Contents = asgcontents;
                    newAssign.Points = (uint?)asgpoints;
                    db.Assignments.Add(newAssign);
                    db.SaveChanges();

            // Time to recalculate Grades

            var AllStudents = from p in db.Enrolled
                              where p.ClassId.Equals(query.ToArray()[0].ClassID)
                              select p;
            foreach(Enrolled student in AllStudents)
            {

            }

                    return Json(new { success = true });
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            join w in db.Assignments on s.AcId equals w.AcId
                            join x in db.Submissions on w.AssId equals x.AssId
                            join y in db.Student on x.UId equals y.UId
                            where w.Name.Equals(asgname) && s.Name.Equals(category)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year
                            && g.Number.Equals(num) && p.Subject.Equals(subject)
                            select new
                            {
                                fname = y.FirstName,
                                lname = y.LastName,
                                uid = y.UId,
                                time = x.Time,
                                score = x.Score
                            };

                return Json(query.ToArray());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
                var query = from p in db.Department
                            join g in db.Courses on p.DId equals g.DId
                            join h in db.Classes on g.CId equals h.CId
                            join s in db.AssignmentCategory on h.ClassId equals s.ClassId
                            join w in db.Assignments on s.AcId equals w.AcId
                            join x in db.Submissions on w.AssId equals x.AssId 
                            where x.UId.Equals(uid) && w.Name.Equals(asgname) && s.Name.Equals(category)
                            && h.SemesterSeason.Equals(season) && h.SemesterYear == year 
                            && g.Number.Equals(num) && p.Subject.Equals(subject)
                            select x;
                // At this point we have the Submissions for a specific student 

                query.ToArray()[0].Score = (uint?)score;
                db.SaveChanges();

                CalculateGrade(subject, num, season, year, uid, category);

                return Json(new { success = true });
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
                var query = from p in db.Professor
                            join x in db.Classes on p.UId equals x.ProfId
                            join w in db.Courses on x.CId equals w.CId
                            join y in db.Department on w.DId equals y.DId
                            where p.UId.Equals(uid)
                            select new
                            {
                                subject = y.Subject,
                                number = w.Number,
                                name = w.Name,
                                season = x.SemesterSeason,
                                year = x.SemesterYear
                            };

                return Json(query.ToArray());
        }


        /*******End code to modify********/

        #region Helpers

        /// <summary>
        /// Calculates the grade for a an assignment
        /// </summary>
        /// <returns>A new uID that's 1 larger than the current highest uID</returns>
        private int CalculateGrade(string subject, int num, string season, int year, string uid, string category)
        {
            var query = from d in db.Department
                        join c in db.Courses on d.DId equals c.DId
                        join cl in db.Classes on c.CId equals cl.CId
                        join ac in db.AssignmentCategory on cl.ClassId equals ac.ClassId
                        join a in db.Assignments on ac.AcId equals a.AcId
                        join s in db.Submissions on a.AssId equals s.AssId
                        join e in db.Enrolled on s.UId equals e.UId
                        where s.UId.Equals(uid) && ac.Name.Equals(category)
                        && cl.SemesterSeason.Equals(season) && cl.SemesterYear == year
                        && c.Number.Equals(num) && d.Subject.Equals(subject)
                        select new
                        {
                            uid = s.UId,
                            score = s.Score,
                            points = a.Points
                        };

            int pointTotal = 0;
            int stuPointTotal = 0;
            foreach (var q in query)
            {
                pointTotal += (int)q.points;
                stuPointTotal += (int)q.score;
            }

            int classGrade = stuPointTotal / pointTotal;

            // update grade saved in db
            //query.ToArray()[0].Score = (uint?)score;
            //db.SaveChanges();

            return 0;
        }

        #endregion
    }
}