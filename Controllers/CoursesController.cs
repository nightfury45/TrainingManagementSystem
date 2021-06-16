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
        public ActionResult Index(string searchString)
        {
            var courses = _context.Courses
                .Include(c => c.Category)
                .ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                courses = courses.Where(c => c.Description.Contains(searchString)).ToList();
            }
            return View(courses);
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
                .SingleOrDefault(t => t.Id == id);

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
    }
}