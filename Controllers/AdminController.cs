using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.ViewModels;

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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
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
                return RedirectToAction("Index", "Admin");
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

        public ActionResult StaffDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var staff = _context.Users
                .SingleOrDefault(t => t.Id == id);

            if (staff == null) return HttpNotFound();

            return View(staff);
        }

        public ActionResult TrainerDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainer = _context.Trainers
                .SingleOrDefault(t => t.UserId == id);

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }

        public ActionResult TraineeDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainee = _context.Trainees
                .SingleOrDefault(t => t.UserId == id);

            if (trainee == null) return HttpNotFound();

            return View(trainee);
        }


        [HttpGet]
        public ActionResult TrainerEdit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainerInDb = _context.Trainers
                .SingleOrDefault(t => t.UserId == id);

            if (trainerInDb == null) return HttpNotFound();

            var viewModel = new TrainerViewModel()
            {
                Trainer = trainerInDb,

            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult StaffEdit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var staff = _context.Users
                .SingleOrDefault(s => s.Id == id);
            if (staff == null) return HttpNotFound();

            var viewModel = new StaffViewModel()
            {
                User = staff,

            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult StaffEdit(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new StaffViewModel()
                {
                    User = user,
                };
                return View(viewModel);
            }

            var staff = _context.Users
                .SingleOrDefault(s => s.Id == user.Id);

            if (staff == null) return HttpNotFound();

            staff.Email = user.Email;
            _context.SaveChanges();

            return RedirectToAction("index");

        }

        [HttpPost]
        public ActionResult TrainerEdit(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TrainerViewModel()
                {
                    Trainer = trainer,
                };
                return View(viewModel);
            }

            var trainerInDb = _context.Trainers
                .SingleOrDefault(t => t.UserId == trainer.UserId);

            if (trainerInDb == null) return HttpNotFound();

            trainerInDb.Name = trainer.Name;
            trainerInDb.Type = trainer.Type;
            trainerInDb.Email = trainer.Email;
            trainerInDb.WorkPlace = trainer.WorkPlace;
            trainerInDb.Phone = trainer.Phone;
            trainerInDb.Education = trainer.Education;
            _context.SaveChanges();

            return RedirectToAction("index");

        }

        public ActionResult StaffDelete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var del = _context.Users
                .SingleOrDefault(d => d.Id == id);

            if (del == null) return HttpNotFound();

            _context.Users.Remove(del);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult TrainerDelete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var del = _context.Trainers
                .SingleOrDefault(d => d.UserId == id);
            var del2 = _context.Users
                .SingleOrDefault(d => d.Id == id);

            if (del == null) return HttpNotFound();

            _context.Trainers.Remove(del);
            _context.Users.Remove(del2);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        

    }
}