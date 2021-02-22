using System;
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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Ajax.Utilities;

namespace Wildlife.Controllers
{
    public class DriveController : Controller
    {
        private DriveContext db = new DriveContext();
        private ApplicationUserManager _userManager;

        // GET: Drive
        public async Task<ActionResult> Index()
        {
            return View(await db.Drives.ToListAsync());
        }

        // GET: Drive/Details/5
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
            return View(drive);
        }

        // GET: Drive/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Drive/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DriveId,DriveName,StartLocation,EndLocation,ExtraDetails,DriverId")] Drive drive)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(drive.DriverId);
                if (user != null)
                {
                    drive.DriverId = user.Id;
                }
                else
                {
                    drive.DriverId = null;
                }
                db.Drives.Add(drive);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(drive);
        }

        // GET: Drive/Edit/5
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
            // in case of manual db input shenanigans (definitely didn't cause that myself)
            var user = await UserManager.FindByIdAsync(drive.DriverId);
            if (user != null) { drive.DriverId = user.UserName; }
            return View(drive);
        }

        // POST: Drive/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DriveId,DriveName,StartLocation,EndLocation,ExtraDetails,DriverId")] Drive drive)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(drive.DriverId);
                if (user != null)
                {
                    drive.DriverId = user.Id;
                }
                else
                {
                    drive.DriverId = null;
                }
                db.Entry(drive).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(drive);
        }

        // GET: Drive/Delete/5
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

        // POST: Drive/Delete/5
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
