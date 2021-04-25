using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
  public class CommonController : Controller
  {

    /*******Begin code to modify********/

    protected Team89LMSContext db;

    public CommonController()
    {
      db = new Team89LMSContext();
    }

    /*
     * WARNING: This is the quick and easy way to make the controller
     *          use a different LibraryContext - good enough for our purposes.
     *          The "right" way is through Dependency Injection via the constructor 
     *          (look this up if interested).
    */

    public void UseLMSContext(Team89LMSContext ctx)
    {
      db = ctx;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }
    


    /// <summary>
    /// Retrieve a JSON array of all departments from the database.
    /// Each object in the array should have a field called "name" and "subject",
    /// where "name" is the department name and "subject" is the subject abbreviation.
    /// </summary>
    /// <returns>The JSON array</returns>
    public IActionResult GetDepartments()
    {
        var query = from d in db.Department
                    select new
                    {
                        name = d.Name,
                        subject = d.Subject
                    };

        return Json(query.ToArray());
    }



    /// <summary>
    /// Returns a JSON array representing the course catalog.
    /// Each object in the array should have the following fields:
    /// "subject": The subject abbreviation, (e.g. "CS")
    /// "dname": The department name, as in "Computer Science"
    /// "courses": An array of JSON objects representing the courses in the department.
    ///            Each field in this inner-array should have the following fields:
    ///            "number": The course number (e.g. 5530)
    ///            "cname": The course name (e.g. "Database Systems")
    /// </summary>
    /// <returns>The JSON array</returns>
    public IActionResult GetCatalog()
    {
            var query = from d in db.Department
                        select new
                        {
                            subject = d.Subject,
                            dname = d.Name,
                            courses = from course in d.Courses select new
                            {
                                number = course.Number,
                                cname = course.Name
                            }
                        };

            return Json(query.ToArray());
    }

    /// <summary>
    /// Returns a JSON array of all class offerings of a specific course.
    /// Each object in the array should have the following fields:
    /// "season": the season part of the semester, such as "Fall"
    /// "year": the year part of the semester
    /// "location": the location of the class
    /// "start": the start time in format "hh:mm:ss"
    /// "end": the end time in format "hh:mm:ss"
    /// "fname": the first name of the professor
    /// "lname": the last name of the professor
    /// </summary>
    /// <param name="subject">The subject abbreviation, as in "CS"</param>
    /// <param name="number">The course number, as in 5530</param>
    /// <returns>The JSON array</returns>
    public IActionResult GetClassOfferings(string subject, int number)
    {
            var query = from d in db.Department
                        where d.Subject.Equals(subject)
                        join c in db.Courses on d.DId equals c.DId
                        where c.Number.Equals(number)
                        join cl in db.Classes on c.CId equals cl.CId
                        join p in db.Professor on cl.ProfId equals p.UId
                        select new
                        {
                            season = cl.SemesterSeason,
                            year = cl.SemesterYear,
                            location = cl.Location,
                            start = cl.Start,
                            end = cl.End,
                            fname = p.FirstName,
                            lname = p.LastName
                        };

            return Json(query.ToArray());
    }

    /// <summary>
    /// This method does NOT return JSON. It returns plain text (containing html).
    /// Use "return Content(...)" to return plain text.
    /// Returns the contents of an assignment.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment in the category</param>
    /// <returns>The assignment contents</returns>
    public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
    {
            var query = from d in db.Department
                        join c in db.Courses on d.DId equals c.DId
                        join cl in db.Classes on c.CId equals cl.CId
                        join ac in db.AssignmentCategory on cl.ClassId equals ac.ClassId
                        join a in db.Assignments on ac.AcId equals a.AcId into result
                        from r in result.DefaultIfEmpty()
                        where r.Name.Equals(asgname) && d.Subject.Equals(subject) && c.Number.Equals(num)
                        && cl.SemesterSeason.Equals(season) && cl.SemesterYear == year && ac.Name.Equals(category)
                        select r.Contents;

            return Content(query.ToArray()[0]);
    }


    /// <summary>
    /// This method does NOT return JSON. It returns plain text (containing html).
    /// Use "return Content(...)" to return plain text.
    /// Returns the contents of an assignment submission.
    /// Returns the empty string ("") if there is no submission.
    /// </summary>
    /// <param name="subject">The course subject abbreviation</param>
    /// <param name="num">The course number</param>
    /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
    /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
    /// <param name="category">The name of the assignment category in the class</param>
    /// <param name="asgname">The name of the assignment in the category</param>
    /// <param name="uid">The uid of the student who submitted it</param>
    /// <returns>The submission text</returns>
    public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
    {

            var query = from d in db.Department
                        join c in db.Courses on d.DId equals c.DId
                        join cl in db.Classes on c.CId equals cl.CId
                        join ac in db.AssignmentCategory on cl.ClassId equals ac.ClassId
                        join a in db.Assignments on ac.AcId equals a.AcId
                        join s in db.Submissions on a.AssId equals s.AssId into result
                        from r in result.DefaultIfEmpty()
                        where r.UId.Equals(uid) && a.Name.Equals(asgname) && ac.Name.Equals(category)
                        && cl.SemesterSeason.Equals(season) && cl.SemesterYear == year && c.Number.Equals(num)
                        && d.Subject.Equals(subject)
                        select new
                        {
                           contents = r.Contents
                        };

            if (query.Count() == 1)
            {
                return Content(query.ToArray()[0].contents);
            }
            return Content("");
    }


    /// <summary>
    /// Gets information about a user as a single JSON object.
    /// The object should have the following fields:
    /// "fname": the user's first name
    /// "lname": the user's last name
    /// "uid": the user's uid
    /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
    ///               If the user is a Professor, this is the department they work in.
    ///               If the user is a Student, this is the department they major in.    
    ///               If the user is an Administrator, this field is not present in the returned JSON
    /// </summary>
    /// <param name="uid">The ID of the user</param>
    /// <returns>
    /// The user JSON object 
    /// or an object containing {success: false} if the user doesn't exist
    /// </returns>
    public IActionResult GetUser(string uid)
    {
            var query1 = from s in db.Student
                         where s.UId.Equals(uid)
                         join d in db.Department on s.DId equals d.DId
                         select new
                         {
                             fname = s.FirstName,
                             lname = s.LastName,
                             uid = s.UId,
                             department = d.Name
                         };

            if (query1.Any()) // if query1 is successful
            {
                return Json(query1.ToArray()[0]);
            }

            var query2 = from p in db.Professor
                         where p.UId.Equals(uid)
                         join d in db.Department on p.DId equals d.DId
                         select new
                         {
                             fname = p.FirstName,
                             lname = p.LastName,
                             uid = p.UId,
                             department = d.Name
                         };

            if (query2.Any()) // if query2 is successful
            {
                return Json(query2.ToArray()[0]);
            }

            var query3 = from a in db.Administrator
                         where a.UId.Equals(uid)
                         select new
                         {
                             fname = a.FirstName,
                             lname = a.LastName,
                             uid = a.UId,
                         };

            if (query3.Any()) // if query3 is successful
            {
                return Json(query3.ToArray()[0]);
            }


            // user doesn't exist
            return Json(new { success = false } );
    }


    /*******End code to modify********/

  }
}