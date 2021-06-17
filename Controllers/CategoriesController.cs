using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.ViewModels;
using System.Data.Entity;

namespace TrainingManagementSystem.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }
        [Authorize(Roles = "admin, staff")]
        // GET: Categories

        public ActionResult Index(string searchString)
        {
            var category = _context.Categories
                .ToList();
            if (!searchString.IsNullOrWhiteSpace())
            {
                category = category.Where(c => c.Name.ToLower().Contains(searchString)).ToList();
            }
            return View(category);
        }
        [HttpGet]
        [Authorize(Roles = "admin, staff")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin, staff")]
        public ActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var category = _context.Categories
                .SingleOrDefault(c => c.Id == id);

            if (category == null) return HttpNotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var categoryInDb = _context.Categories
                .SingleOrDefault(c => c.Id == id);
            if (categoryInDb == null) return HttpNotFound();

            var viewModel = new CategoriesViewModel()
            {
                Category = categoryInDb
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CategoriesViewModel()
                {
                    Category = category
                };
                return View(viewModel);
            }

            var categoryInDb = _context.Categories
                .SingleOrDefault(c => c.Id == category.Id);

            if (categoryInDb == null) return HttpNotFound();

            categoryInDb.Name = category.Name;
            categoryInDb.Description = category.Description;

            _context.SaveChanges();

            return RedirectToAction("Index");

        }
    }
}