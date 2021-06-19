using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Ajax.Utilities;

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
        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize (Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string id, ResetPasswordViewModel model)
        {

            var provider = new DpapiDataProtectionProvider("TrainingManagementSystem");
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(
                provider.Create("Token"));

            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var token = await UserManager.GeneratePasswordResetTokenAsync(id);
            var result = await UserManager.ResetPasswordAsync(id, token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ShowTrainer", "Admin");
            }
            return View(result);
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
        public ActionResult ShowTrainee(string searchString)
        {
            var trainee = _context.Trainees
                .ToList();

            if ( !searchString.IsNullOrWhiteSpace())
            {
                trainee = trainee.Where(t => t.Email.ToLower().Contains(searchString) ||
                                             t.ProgrammingLanguage.Contains(searchString) ||
                                             t.TOEICScore.ToString().Contains(searchString))
                                            .ToList();
            }
            return View(trainee);
        }
        public ActionResult TrainerDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainer = _context.Trainers
                .SingleOrDefault(t => t.UserId == id);

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }

        public ActionResult StaffDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var staff = _context.Users
                .SingleOrDefault(t => t.Id == id);

            if (staff == null) return HttpNotFound();

            return View(staff);
        }

        public ActionResult TraineeDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainee = _context.Trainees
                .SingleOrDefault(t => t.UserId == id);

            if (trainee == null) return HttpNotFound();

            return View(trainee);
        }
    }
}