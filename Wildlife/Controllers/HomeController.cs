using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wildlife.Data;
using Wildlife.Models;

namespace Wildlife.Controllers
{
    public class HomeController : Controller
    {
        public DriveContext db = new DriveContext();
        private ApplicationUserManager _userManager;

        // GET: Drives

        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "Driver";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Admin";
                }
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }

            var drives = await db.Drives.ToListAsync();
            List<DriveInfoViewModel> driveInfoViewModels = new List<DriveInfoViewModel>();
            foreach (var drive in drives)
            {
                if (drive.DriverId == User.Identity.GetUserId() || drive.DriverId == null || User.IsInRole("Admin"))
                {
                    var driveInfoViewModel = new DriveInfoViewModel
                    {
                        DriveId = drive.DriveId,
                        DriveName = drive.DriveName,
                        ExtraDetails = drive.ExtraDetails,
                        DriverId = drive.DriverId,
                        DriveDistance = (double)drive.DriveDistance * 0.000621371192,
                        DriveDuration = drive.DriveDuration / 60,

                        StartAddressLine1 = drive.StartLocation.AddressLine1,
                        StartAddressLine2 = drive.StartLocation.AddressLine2,
                        StartCity = drive.StartLocation.City,
                        StartCountryRegion = drive.StartLocation.CountryRegion,
                        StartPostalCode = drive.StartLocation.PostalCode,
                        StartStateProvince = drive.StartLocation.StateProvince,

                        EndAddressLine1 = drive.EndLocation.AddressLine1,
                        EndAddressLine2 = drive.EndLocation.AddressLine2,
                        EndCity = drive.EndLocation.City,
                        EndCountryRegion = drive.EndLocation.CountryRegion,
                        EndPostalCode = drive.EndLocation.PostalCode,
                        EndStateProvince = drive.EndLocation.StateProvince,

                    };
                    var user = await UserManager.FindByIdAsync(drive.DriverId);
                    if (user != null)
                    {
                        driveInfoViewModel.DriverId = user.UserName;
                    }
                    else
                    {
                        driveInfoViewModel.DriverId = null;
                    }
                    user = await UserManager.FindByNameAsync(User.Identity.Name);
                    if (user != null && user.PhoneNumber != null && user.Availabilities.Count() >= 1)
                    {
                        ViewBag.setup = true;
                    }
                    else
                    {
                        ViewBag.setup = false;
                    }
                    driveInfoViewModels.Add(driveInfoViewModel);
                }
            }
            driveInfoViewModels = driveInfoViewModels.OrderByDescending(d => d.DriverId).ToList();
            return View(driveInfoViewModels);
        }
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Count != 0 && s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Wheels for Wildlife";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public HomeController()
        {

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}