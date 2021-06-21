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
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin, staff")]
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

        [Authorize(Roles = "staff")]
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

        [Authorize (Roles = "admin")]
        public ActionResult StaffDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var staff = _context.Users
                .SingleOrDefault(t => t.Id == id);

            if (staff == null) return HttpNotFound();

            return View(staff);
        }

        [Authorize (Roles = "admin, staff")]
        public ActionResult TrainerDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainer = _context.Trainers
                .SingleOrDefault(t => t.UserId == id);

            if (trainer == null) return HttpNotFound();

            return View(trainer);
        }

        [Authorize (Roles = "staff")]
        public ActionResult TraineeDetails(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var trainee = _context.Trainees
                .SingleOrDefault(t => t.UserId == id);

            if (trainee == null) return HttpNotFound();

            return View(trainee);
        }

        [Authorize (Roles = "admin")]
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

        [Authorize (Roles = "admin")]
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
            staff.UserName = user.Email;
            _context.SaveChanges();

            return RedirectToAction("ShowStaff");
        }

        [Authorize(Roles = "admin, staff")]
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

        [Authorize(Roles = "admin, staff")]
        [HttpPost]
        public ActionResult TrainerEdit(Trainer trainer, ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TrainerViewModel()
                {
                    Trainer = trainer,
                };
                return View(viewModel);
            }

            var trainerUser = _context.Users
                .SingleOrDefault(s => s.Id == user.Id);
            var trainerInDb = _context.Trainers
                .SingleOrDefault(t => t.UserId == trainer.UserId);

            if (trainerInDb == null) return HttpNotFound();
            trainerInDb.Name = trainer.Name;
            trainerInDb.Type = trainer.Type;
            trainerInDb.Email = trainer.Email;
            trainerUser.UserName = trainer.Email;
            trainerUser.Email = trainer.Email;
            trainerInDb.WorkPlace = trainer.WorkPlace;
            trainerInDb.Phone = trainer.Phone;
            trainerInDb.Education = trainer.Education;
            _context.SaveChanges();

            return RedirectToAction("ShowTrainer");
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        public ActionResult TraineeEdit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var traineeInDb = _context.Trainees
                .SingleOrDefault(t => t.UserId == id);

            if (traineeInDb == null) return HttpNotFound();

            var viewModel = new TraineeViewModel()
            {
                Trainee = traineeInDb,
            };

            return View(viewModel);
        }

        [Authorize(Roles = "staff")]
        [HttpPost]
        public ActionResult TraineeEdit(Trainee trainee, ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TraineeViewModel()
                {
                    Trainee = trainee,
                };
                return View(viewModel);
            }

            var traineeUser = _context.Users
                .SingleOrDefault(s => s.Id == user.Id);
            var traineeInDb = _context.Trainees
                .SingleOrDefault(t => t.UserId == trainee.UserId);

            if (traineeInDb == null) return HttpNotFound();

            traineeInDb.Name = trainee.Name;
            traineeInDb.Email = trainee.Email;
            traineeUser.UserName = trainee.Email;
            traineeUser.Email = trainee.Email;
            traineeInDb.Age = trainee.Age;
            traineeInDb.DoB = trainee.DoB;
            traineeInDb.Education = trainee.Education;
            traineeInDb.ProgrammingLanguage = trainee.ProgrammingLanguage;
            traineeInDb.TOEICScore = trainee.TOEICScore;
            traineeInDb.ExperienceDetail = trainee.ExperienceDetail;
            traineeInDb.Department = trainee.Department;
            traineeInDb.Address = trainee.Address;
            _context.SaveChanges();

            return RedirectToAction("ShowTrainee");

        }

        [Authorize(Roles = "admin")]
        public ActionResult StaffDelete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var del = _context.Users
                .SingleOrDefault(d => d.Id == id);

            if (del == null) return HttpNotFound();

            _context.Users.Remove(del);
            _context.SaveChanges();

            return RedirectToAction("ShowStaff");
        }

        [Authorize(Roles = "admin")]
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

            return RedirectToAction("ShowTrainer");
        }

        [Authorize(Roles = "admin")]
        public ActionResult TraineeDelete(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var del = _context.Trainees
                .SingleOrDefault(d => d.UserId == id);
            var del2 = _context.Users
                .SingleOrDefault(d => d.Id == id);

            if (del == null) return HttpNotFound();

            _context.Trainees.Remove(del);
            _context.Users.Remove(del2);
            _context.SaveChanges();

            return RedirectToAction("ShowTrainee");
        }

    }
}