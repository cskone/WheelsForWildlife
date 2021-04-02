using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Wildlife.Models;

namespace Wildlife.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public class UserUpdateViewModel
        {
            [Required]
            [MaxLength(50)]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(255)]
            public string Email { get; set; }
        }

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAvailabilities()
        {
            // possible issue if user isnt logged in somehow? but i think only possible via direct url so doesnt matter
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            return Json(user.Availabilities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveFromAvailabilities(int slotId)
        {
            // possible issue if user isnt logged in somehow? but i think only possible via direct url so doesnt matter
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.Availabilities.Remove(user.Availabilities.First(s => s.SlotId == slotId));
            IdentityResult res = UserManager.Update(user);
            if (res.Succeeded)
            {
                ViewBag.Title = "Success";
            }
            else
            {
                ViewBag.Title = "Failed";
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> EditAvailabilities(int slotId, string start, string end)
        {
            // possible issue if user isnt logged in somehow? but i think only possible via direct url so doesnt matter
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Availability ava = user.Availabilities.First(s => s.SlotId == slotId);
            ava.Update(Double.Parse(start), Double.Parse(end));
            IdentityResult res = UserManager.Update(user);
            if (res.Succeeded)
            {
                ViewBag.Title = "Success";
            }
            else
            {
                ViewBag.Title = "Failed";
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> AddToAvailabilities(string UserId, string day, string start, string end)
        {
            // possible issue if user isnt logged in somehow? but i think only possible via direct url so doesnt matter
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            user.Availabilities.Add(new Availability(UserId, (DayOfWeek)int.Parse(day), Double.Parse(start), Double.Parse(end)));
            IdentityResult res = UserManager.Update(user);
            if (res.Succeeded)
            {
                ViewBag.Title = "Success";
            }
            else
            {
                ViewBag.Title = "Failed";
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        //https://stackoverflow.com/questions/33984530/how-to-update-identitys-manage-controller
        [HttpGet]
        public async Task<ActionResult> Edit(ManageMessageId? message = null)
        {
            // possible issue if user isnt logged in somehow? but i think only possible via direct url so doesnt matter
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            // move entity fields to viewmodel from constructor, automapper, etc.        
            var model = new EditUserInfoViewModel
            {
                OldUserName = user.UserName,
                OldEmail = user.Email,
                OldPhoneNumber = user.PhoneNumber,
                OldVehicleMake = user.VehicleMake,
                OldVehicleModel = user.VehicleModel,
                // OldDriverLocation = user.DriverLocation

                NewUserName = user.UserName,
                NewEmail = user.Email,
                NewPhoneNumber = user.PhoneNumber,
                NewVehicleMake = user.VehicleMake,
                NewVehicleModel = user.VehicleModel,
                // NewDriverLocation = user.DriverLocation
                NewAddressLine1 = user.DriverLocation.AddressLine1,
                NewAddressLine2 = user.DriverLocation.AddressLine2,
                NewCity = user.DriverLocation.City,
                NewStateProvince = user.DriverLocation.StateProvince,
                NewPostalCode = user.DriverLocation.PostalCode,
                Availabilities = user.Availabilities.ToList(),
            };
            //model.Availabilities.Add(new Availability(1, "54ab16bb-e8b9-4575-b88b-cf71b910cd6c", DayOfWeek.Monday, 10.50, 12.50));
            ViewBag.MessageId = message;
            ViewBag.UserId = User.Identity.GetUserId();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserInfoViewModel userEditUserInfoViewModel)
        {
            if (!ModelState.IsValid) return View(userEditUserInfoViewModel);

            // test random stuff
            //Random generator = new Random();
            //Int32.Parse(generator.Next(0, 1000000).ToString("D6"));

            var user = await UserManager.FindByNameAsync(userEditUserInfoViewModel.OldUserName);
            // Mapper.Map(userUpdateViewModel, user);  // move viewmodel to entity model
            // instead of automapper, you can do this:
            user.UserName = userEditUserInfoViewModel.NewUserName;
            user.Email = userEditUserInfoViewModel.NewEmail;
            user.PhoneNumber = userEditUserInfoViewModel.NewPhoneNumber;
            user.VehicleMake = userEditUserInfoViewModel.NewVehicleMake;
            user.VehicleModel = userEditUserInfoViewModel.NewVehicleModel;

            user.DriverLocation.AddressLine1 = userEditUserInfoViewModel.NewAddressLine1;
            user.DriverLocation.AddressLine2 = userEditUserInfoViewModel.NewAddressLine2;
            user.DriverLocation.City = userEditUserInfoViewModel.NewCity;
            user.DriverLocation.StateProvince = userEditUserInfoViewModel.NewStateProvince;
            user.DriverLocation.PostalCode = userEditUserInfoViewModel.NewPostalCode;
            user.Availabilities = userEditUserInfoViewModel.Availabilities;
            //user.Availabilities.Add(new Availability(1, "54ab16bb-e8b9-4575-b88b-cf71b910cd6c", DayOfWeek.Monday, 10.50, 12.50));

            /*
            user.DriverLocation.AddressLine1 = userEditUserInfoViewModel.NewAddressLine1;
            user.DriverLocation.AddressLine2 = userEditUserInfoViewModel.NewAddressLine2;
            user.DriverLocation.City = userEditUserInfoViewModel.NewCity;
            user.DriverLocation.StateProvince = userEditUserInfoViewModel.NewStateProvince;
            user.DriverLocation.PostalCode = userEditUserInfoViewModel.NewPostalCode;
            */
            UserManager.Update(user);

            // resigns in for identity refresh 

            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Manage", new { Message = "Updated!" });
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                User = UserManager.FindByNameAsync(User.Identity.Name).Result
        };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        [CustomAuthorize(Roles = "Admin")]
        public ActionResult UserManagement(string searchString)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;
            }
            else
            {
                ViewBag.Name = "Not Logged In";
            }
            var users = from u in db.Users select u;
            List<UserInfoViewModel> userInfoViewModels = new List<UserInfoViewModel>();

            if (!String.IsNullOrEmpty(searchString))
            {
                foreach (var user in users.Where(u => u.UserName.Contains(searchString) || u.PhoneNumber.Contains(searchString)))
                {
                    var userInfoViewModel = new UserInfoViewModel
                    {
                        User = user
                    };
                    userInfoViewModels.Add(userInfoViewModel);
                }
            }
            else
            {
                foreach (var user in users)
                {
                    var userInfoViewModel = new UserInfoViewModel
                    {
                        User = user
                    };
                    userInfoViewModels.Add(userInfoViewModel);
                }
            }
            return View(userInfoViewModels);
        }

        // GET: manage/Details
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var userInfoViewModel = new UserInfoViewModel
            {
                User = user
            };

            return View(userInfoViewModel);
        }

        [CustomAuthorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> EditAdmin(string id)
        {
            ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name");
            var user = await UserManager.FindByIdAsync(id);
            // move entity fields to viewmodel from constructor, automapper, etc.
            List<string> roles = new List<string>();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            foreach (var x in user.Roles)
            {
                roles.Add(roleManager.FindByIdAsync(x.RoleId).Result.Name);
            }
            if (roles.Count >= 1)
            {
                ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name", roles[0]);
            }
            else
            {
                roles.Add("");
            }

            var model = new EditAdminUserInfoViewModel
            {
                OldUserRole = roles[0],
                OldUserName = user.UserName,
                OldEmail = user.Email,

                NewUserRole = roles[0],
                NewUserName = user.UserName,
                NewEmail = user.Email,
            };
            //ViewBag.MessageId = message;
            return View(model);
        }
        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> EditAdmin(EditAdminUserInfoViewModel userEditAdminUserInfoViewModel)
        {
            if (!ModelState.IsValid) return View(userEditAdminUserInfoViewModel);

            // test random stuff
            //Random generator = new Random();
            //Int32.Parse(generator.Next(0, 1000000).ToString("D6"));

            var user = await UserManager.FindByNameAsync(userEditAdminUserInfoViewModel.OldUserName);
            user.UserName = userEditAdminUserInfoViewModel.NewUserName;
            user.Email = userEditAdminUserInfoViewModel.NewEmail;
            if (userEditAdminUserInfoViewModel.OldUserRole != null)
            {
                UserManager.RemoveFromRole(user.Id, userEditAdminUserInfoViewModel.OldUserRole);
            }
            UserManager.AddToRole(user.Id, userEditAdminUserInfoViewModel.NewUserRole);
            UserManager.Update(user);

            // resigns in for identity refresh 

            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("UserManagement", "Manage", new { Message = "Updated!" });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}