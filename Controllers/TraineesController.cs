using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    public class TraineesController : Controller
    {
        private ApplicationDbContext _context;
        public TraineesController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var trainee = _context.Trainees
                .SingleOrDefault(t => t.UserId == id);

            if (trainee == null) return HttpNotFound();

            return View(trainee);
        }
        public ActionResult ShowAssignCourse(string searchString)
        {
            var userId = User.Identity.GetUserId();

            var course = _context.CourseTrainees
                .Where(t => t.TraineeId.Equals(userId))
                .Include(c => c.Course.Category)
                .ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                course = course.Where(c => c.Course.Name.ToLower().Contains(searchString)).ToList();
            }
            return View(course);
        }
        public ActionResult ShowAllCourse(string searchString)
        {
            var course = _context.Courses
                .Include(c => c.Category)
                .ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                course = course.Where(c => c.Name.ToLower().Contains(searchString)).ToList();
            }
            return View(course);
        }
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var course = _context.Courses
                .Include(c => c.Category)
                .SingleOrDefault(c => c.Id == id);

            if (course == null) return HttpNotFound();

            return View(course);
        }
    }
}