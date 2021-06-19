using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    public class TraineeInfoController : Controller
    {
        private ApplicationDbContext _context;
        public TraineeInfoController()
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
    }
}