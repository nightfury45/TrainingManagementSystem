﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using TrainingManagementSystem.ViewModels;

namespace TrainingManagementSystem.Controllers
{
    [Authorize]
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
    }
}