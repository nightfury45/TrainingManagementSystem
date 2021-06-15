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
            if (!ModelState .IsValid)
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
    }
}