using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using TrainingManagementSystem.ViewModels;
using System.Net;

namespace TrainingManagementSystem.Controllers
{
    [Authorize(Roles = "trainer")]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index(string searchString)
        {
            var userId = User.Identity.GetUserId();

            var course = _context.CourseTrainers
                .Where(t => t.TrainerId.Equals(userId))
                .Include(c => c.Course.Category)
                .ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                course = course.Where(c => c.Course.Name.ToLower().Contains(searchString)).ToList();
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

        [HttpGet]
        public ActionResult Report()
        {
            var viewModel = new List<StatisticalReportViewModel>();

            var userId = User.Identity.GetUserId();

            var coursesInDb = _context.CourseTrainers
                .Include(c => c.Course.Category)
                .Where(c => c.TrainerId.Equals(userId))
                .ToList();

            var coursesGroupByName = coursesInDb.GroupBy(c => c.Course.Category.Name).ToList();

            foreach (var categoryGroup in coursesGroupByName)
            {
                var categoryQuantity = categoryGroup.Select(c => c.Course.Category).Count();
                viewModel.Add(new StatisticalReportViewModel()
                {
                    CategoryName = categoryGroup.Key,
                    CategoryQuantity = categoryQuantity
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var trainer = _context.Trainers.SingleOrDefault(t => t.UserId.Equals(userId));

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }

        [HttpPost]
        public ActionResult Edit(Trainer trainer)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.UserId == trainer.UserId);
            var userInDb = _context.Users.SingleOrDefault(t => t.Id == trainer.UserId);

            if (trainerInDb == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            trainerInDb.Name = trainer.Name;
            trainerInDb.Type = trainer.Type;
            trainerInDb.Education = trainer.Education;
            trainerInDb.WorkPlace = trainer.WorkPlace;
            trainerInDb.Phone = trainer.Phone;
            trainerInDb.Email = trainer.Email;
            userInDb.Email = trainer.Email;
            userInDb.UserName = trainer.Email;

            _context.SaveChanges();

            return RedirectToAction("Edit");
        }
    }
}