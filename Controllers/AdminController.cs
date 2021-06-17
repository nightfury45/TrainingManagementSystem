using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Controllers
{
    [Authorize(Roles = "admin, staff")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public AdminController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult ShowTrainer()
        {
            var users = _context.Users.ToList();
            var trainers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("trainer"))
                {
                    trainers.Add(user);
                }
            }
            return View(trainers);
        }
        [HttpGet]
        public ActionResult ShowStaff()
        {
            var users = _context.Users.ToList();
            var staffs = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("staff"))
                {
                    staffs.Add(user);
                }
            }
            return View(staffs);
        }
        [HttpGet]
        public ActionResult ShowTrainee()
        {
            var users = _context.Users.ToList();
            var trainees = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (_userManager.GetRoles(user.Id)[0].Equals("trainee"))
                {
                    trainees.Add(user);
                }
            }
            return View(trainees);
        }
        public ActionResult TrainerDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainer = _context.Trainers
                .SingleOrDefault(t => t.UserId == id);

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }
    }
}