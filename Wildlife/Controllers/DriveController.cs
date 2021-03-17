﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wildlife.Data;
using Wildlife.Models;
using System.IO;
//using System.Device.Location;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Device.Location;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wildlife.Controllers
{
    public class DriveController : Controller
    {
        private DriveContext db = new DriveContext();
        private ApplicationUserManager _userManager;

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
            foreach ( var drive in drives) {
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
                driveInfoViewModels.Add(driveInfoViewModel);
            }
            return View(driveInfoViewModels);
        }

        // GET: Drives/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drive drive = await db.Drives.FindAsync(id);
            if (drive == null)
            {
                return HttpNotFound();
            }
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

            return View(driveInfoViewModel);
        }

        // GET: Drives/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();

        }

        // POST: Drives/Create
        // To protect from overposting  attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DriveInfoViewModel driveInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                if (driveInfoViewModel.DriverId != null)
                {
                    var user = await UserManager.FindByNameAsync(driveInfoViewModel.DriverId);
                    if (user != null)
                    {
                        driveInfoViewModel.DriverId = user.Id;
                    }
                    else
                    {
                        driveInfoViewModel.DriverId = null;
                    }
                }
                CivicAddress startLocation = new CivicAddress(driveInfoViewModel.StartAddressLine1,
                    driveInfoViewModel.StartAddressLine2,
                    null, driveInfoViewModel.StartCity,
                    driveInfoViewModel.StartCountryRegion,
                    null, driveInfoViewModel.StartPostalCode,
                    driveInfoViewModel.StartStateProvince);
                CivicAddress endLocation = new CivicAddress(driveInfoViewModel.EndAddressLine1,
                    driveInfoViewModel.EndAddressLine2,
                    null, driveInfoViewModel.EndCity,
                    driveInfoViewModel.EndCountryRegion,
                    null, driveInfoViewModel.EndPostalCode,
                    driveInfoViewModel.EndStateProvince);
                Drive drive = new Drive(driveInfoViewModel.DriveName, driveInfoViewModel.ExtraDetails,startLocation, endLocation, driveInfoViewModel.DriverId);
                db.Drives.Add(drive);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(driveInfoViewModel);
        }


        // TODO: check if addresses changed, and if so recalc drive dur and dist
        // GET: Drives/Edit/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drive drive = await db.Drives.FindAsync(id);
            if (drive == null)
            {
                return HttpNotFound();
            }
            var driveInfoViewModel = new DriveInfoViewModel
            {
                DriveId = drive.DriveId,
                DriveName = drive.DriveName,
                ExtraDetails = drive.ExtraDetails,
                DriverId = drive.DriverId,

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

            return View(driveInfoViewModel);
        }

        // POST: Drives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DriveInfoViewModel driveInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(driveInfoViewModel.DriverId);
                if (user != null)
                {
                    driveInfoViewModel.DriverId = user.UserName;
                }
                else
                {
                    driveInfoViewModel.DriverId = null;
                }
                if (user != null)
                {
                    driveInfoViewModel.DriverId = user.Id;
                }
                else
                {
                    driveInfoViewModel.DriverId = null;
                }
                CivicAddress startLocation = new CivicAddress(driveInfoViewModel.StartAddressLine1,
                    driveInfoViewModel.StartAddressLine2,
                    null, driveInfoViewModel.StartCity,
                    driveInfoViewModel.StartCountryRegion,
                    null, driveInfoViewModel.StartPostalCode,
                    driveInfoViewModel.StartStateProvince);
                CivicAddress endLocation = new CivicAddress(driveInfoViewModel.EndAddressLine1,
                    driveInfoViewModel.EndAddressLine2,
                    null, driveInfoViewModel.EndCity,
                    driveInfoViewModel.EndCountryRegion,
                    null, driveInfoViewModel.EndPostalCode,
                    driveInfoViewModel.EndStateProvince);
                var drive = await db.Drives.FindAsync(driveInfoViewModel.DriveId);
                drive.DriveName = driveInfoViewModel.DriveName;
                drive.DriverId = driveInfoViewModel.DriverId;
                drive.StartLocation = startLocation;
                drive.EndLocation = endLocation;
                drive.ExtraDetails = drive.ExtraDetails;

                db.Entry(drive).State = EntityState.Modified;
                _ = await db.SaveChangesAsync();
                return RedirectToAction("Index", "Drive", new { Message = "Updated!" });
            }
            return View(driveInfoViewModel);
        }

        // GET: Drives/Delete/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drive drive = await db.Drives.FindAsync(id);
            if (drive == null)
            {
                return HttpNotFound();
            }
            return View(drive);
        }

        // POST: Drives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Drive drive = await db.Drives.FindAsync(id);
            db.Drives.Remove(drive);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public DriveController()
        {
        }

        public DriveController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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
