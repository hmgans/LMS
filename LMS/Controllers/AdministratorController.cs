using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace LMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department(string subject)
        {
            ViewData["subject"] = subject;
            return View();
        }

        public IActionResult Course(string subject, string num)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of all the courses in the given department.
        /// Each object in the array should have the following fields:
        /// "number" - The course number (as in 5530)
        /// "name" - The course name (as in "Database Systems")
        /// </summary>
        /// <param name="subject">The department subject abbreviation (as in "CS")</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetCourses(string subject)
        {
                var query = from p in db.Courses
                            join g in db.Department on p.DId equals g.DId
                            where g.Subject.Equals(subject)
                            select new
                            {
                                number = p.Number,
                                name = p.Name
                            };
                return Json(query.ToArray());
        }





        /// <summary>
        /// Returns a JSON array of all the professors working in a given department.
        /// Each object in the array should have the following fields:
        /// "lname" - The professor's last name
        /// "fname" - The professor's first name
        /// "uid" - The professor's uid
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetProfessors(string subject)
        {
                var query = from p in db.Professor
                            join g in db.Department on p.DId equals g.DId
                            where g.Subject.Equals(subject)
                            where g.Subject.Equals(subject)
                            select new
                            {
                                lname = p.LastName,
                                fname = p.FirstName,
                                uid = p.UId

                            };
                return Json(query.ToArray());
        }



        /// <summary>
        /// Creates a course.
        /// A course is uniquely identified by its number + the subject to which it belongs
        /// </summary>
        /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
        /// <param name="number">The course number</param>
        /// <param name="name">The course name</param>
        /// <returns>A JSON object containing {success = true/false},
        /// false if the Course already exists.</returns>
        public IActionResult CreateCourse(string subject, int number, string name)
        {
                var query = (from p in db.Department
                             where p.Subject.Equals(subject)
                             select p.DId).Distinct();
            
                if (query.Count() == 1)
                {
                    Courses course = new Courses();
                    course.DId = query.ToArray()[0];
                    course.Number = number;
                    course.Name = name;

                    db.Courses.Add(course);
                    int success = db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = true });
        }



        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
        {
                var query = from p in db.Department
                             join x in db.Courses on p.DId equals x.DId where p.Subject.Equals(subject) && x.Number.Equals(number)
                            select x.CId;

                if (query.Count() == 1)
                {
                    Classes newClass = new Classes();
                    newClass.CId = query.ToArray()[0];
                    newClass.SemesterSeason = season;
                    newClass.SemesterYear = (uint)year;
                    newClass.ProfId = instructor;
                    newClass.Location = location;
                    newClass.Start = start;
                    newClass.End = end;

                    db.Classes.Add(newClass);
                    int success = db.SaveChanges();

                    return Json(new { success = true });
                }
                return Json(new { success = false });
        }


        /*******End code to modify********/

    }
}