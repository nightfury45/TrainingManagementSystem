using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;
using TrainingManagementSystem.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TrainingManagementSystem.Controllers
{

    [Authorize]
    public class CoursesController : Controller
    {
        private ApplicationDbContext _context;
        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Courses
        [Authorize(Roles = "admin, staff")]
        public ActionResult Index (string searchString)
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
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new CourseCategoriesViewModel()
            {
                Categories = _context.Categories.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModel()
                {
                    Course = course,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }
            var newCourse = new Course()
            {
                CategoryId = course.CategoryId,
                Name = course.Name,
                Description = course.Description
            };

            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            return RedirectToAction("Index");
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

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var course = _context.Courses
                .SingleOrDefault(c => c.Id == id);

            if (course == null) return HttpNotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var coursesInDb = _context.Courses
                .SingleOrDefault(c => c.Id == id);
            if (coursesInDb == null) return HttpNotFound();

            var viewModel = new CourseCategoriesViewModel()
            {
                Course = coursesInDb,
                Categories = _context.Categories.ToList()

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Course course)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModel()
                {
                    Course = course,
                    Categories = _context.Categories.ToList()
                };
                return View(viewModel);
            }

            var courseInDb = _context.Courses
                .SingleOrDefault(c => c.Id == course.Id);

            if (courseInDb == null) return HttpNotFound();

            courseInDb.Name = course.Name;
            courseInDb.Description = course.Description;
            courseInDb.CategoryId = course.CategoryId;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Report()
        {
            var viewModel = new List<StatisticalReportViewModel>();

            var userId = User.Identity.GetUserId();

            var coursesInDb = _context.Courses
                .Include(c => c.Category)
                .ToList();

            var coursesGroupByName = coursesInDb.GroupBy(t => t.Category.Name).ToList();

            foreach (var categoryGroup in coursesGroupByName)
            {
                var categoryQuantity = categoryGroup.Select(c => c.Category).Count();
                viewModel.Add(new StatisticalReportViewModel()
                {
                    CategoryName = categoryGroup.Key,
                    CategoryQuantity = categoryQuantity
                });
            }

            return View(viewModel);
        }
        [HttpGet]
        public ActionResult ViewTrainees(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var trainees = _context.CourseTrainees
                .Include(c => c.Trainee)
                .Where(c => c.CourseId == id)
                .Select(c => c.Trainee);
            ViewBag.CourseId = id;

            return View(trainees);
        }

        [HttpGet]
        public ActionResult ViewTrainers(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var trainers = _context.CourseTrainers
                .Include(c => c.Trainer)
                .Where(c => c.CourseId == id)
                .Select(c => c.Trainer);
            ViewBag.CourseId = id;

            return View(trainers);
        }


        [HttpGet]
        public ActionResult AddTrainees(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                return HttpNotFound();

            var traineesInDb = _context.Trainees.ToList();

            var traineesInCourse = _context.CourseTrainees
                    .Include(c => c.Trainee)
                    .Where(c => c.CourseId == id)
                    .Select(c => c.Trainee)
                    .ToList();

            var traineesToAdd = new List<Trainee>();

            foreach (var trainee in traineesInDb)
            {
                if (!traineesInCourse.Contains(trainee))
                    traineesToAdd.Add(trainee);
            }

            var viewModel = new CourseTraineesViewModel
            {
                CourseId = (int)id,
                Trainees = traineesToAdd
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainees(CourseTrainee model)
        {
            var courseTrainee = new CourseTrainee
            {
                CourseId = model.CourseId,
                TraineeId = model.TraineeId
            };

            _context.CourseTrainees.Add(courseTrainee);
            _context.SaveChanges();

            return RedirectToAction("ViewTrainees", new { id = model.CourseId });
        }

        [HttpGet]
        public ActionResult AddTrainers(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            if (_context.Courses.SingleOrDefault(t => t.Id == id) == null)
                return HttpNotFound();

            var trainersInDb = _context.Trainers.ToList();

            var trainersInCourse = _context.CourseTrainers
                    .Include(c => c.Trainer)
                    .Where(c => c.CourseId == id)
                    .Select(c => c.Trainer)
                    .ToList();

            var trainersToAdd = new List<Trainer>();

            foreach (var trainer in trainersInDb)
            {
                if (!trainersInCourse.Contains(trainer))
                    trainersToAdd.Add(trainer);
            }

            var viewModel = new CourseTrainersViewModel
            {
                CourseId = (int)id,
                Trainers = trainersToAdd
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainers(CourseTrainer model)
        {
            var courseTrainer = new CourseTrainer
            {
                CourseId = model.CourseId,
                TrainerId = model.TrainerId
            };

            _context.CourseTrainers.Add(courseTrainer);
            _context.SaveChanges();

            return RedirectToAction("ViewTrainers", new { id = model.CourseId });
        }

        [HttpGet]
        [Authorize(Roles = "staff")]
        public ActionResult RemoveTrainee(int id, string userId)
        {
            var courseTraineeToRemove = _context.CourseTrainees
                .SingleOrDefault(c => c.CourseId == id && c.TraineeId == userId);

            if (courseTraineeToRemove == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            _context.CourseTrainees.Remove(courseTraineeToRemove);
            _context.SaveChanges();
            return RedirectToAction("ViewTrainees", new { id = id });
        }

        [HttpGet]
        [Authorize(Roles = "staff")]
        public ActionResult RemoveTrainer(int id, string userId)
        {
            var courseTrainerToRemove = _context.CourseTrainers
                .SingleOrDefault(c => c.CourseId == id && c.TrainerId == userId);

            if (courseTrainerToRemove == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            _context.CourseTrainers.Remove(courseTrainerToRemove);
            _context.SaveChanges();
            return RedirectToAction("ViewTrainers", new { id = id });
        }



    }
}