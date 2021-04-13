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
using System.IO;
//using System.Device.Location;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Device.Location;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Text;
using Newtonsoft.Json.Linq;
using Twilio.Clients;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Owin.Security;


namespace Wildlife.Controllers
{
    public class DriveController : Controller
    {
        private DriveContext db = new DriveContext();
        private ApplicationUserManager _userManager;
        private ApplicationDbContext udb = new ApplicationDbContext();

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

        [CustomAuthorize(Roles = "Admin")]
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
        [HttpGet]
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
                DriveDone = drive.DriveDone,
                OptedInDrivers = new SelectList(drive.OptedInDrivers, "DriverId", "DriverId"),
                SelectedDriver = null,

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
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (User.IsInRole("Driver") && user.DriverLocation.AddressLine1 != null && user.Availabilities.Count() >= 1){
                ViewBag.ShowTimes = true;
                Tuple<int, int> driveDetails = CalcDriveDistDurFromUser(user, drive);
                int userDistance = driveDetails.Item2;
                int userDuration = driveDetails.Item1;
                driveInfoViewModel.UserDistance = userDistance * 0.000621371192;
                driveInfoViewModel.UserDuration = userDuration / 60;
            }
            else
            {

            }

            user = await UserManager.FindByIdAsync(drive.DriverId);
            if (user != null)
            {
                driveInfoViewModel.DriverId = user.UserName;
            }
            else
            {
                driveInfoViewModel.DriverId = null;
            }

            // num active drives
            ViewBag.numDrives = db.Drives.ToList().Where(d => d.DriveDone == false && d.DriverId == User.Identity.GetUserId()).Count();
            if (drive.OptedInDriversToList().Count() != 0 && drive.OptedInDriversToList().Contains(User.Identity.GetUserName()))
            {
                ViewBag.alreadyOptedIn = true;
            }
            else
            {
                ViewBag.alreadyOptedIn = false;
            }
            return View(driveInfoViewModel);
        }

        // need to make this database relation to user and drive id so it doenst recalc fourteen thousand times
        public Tuple<int, int> CalcDriveDistDurFromUser(ApplicationUser usr, Drive drive)
        {
            try
            {
                int alongroaddis = Convert.ToInt32(ConfigurationManager.AppSettings["alongroad"].ToString());
                string keyString = ConfigurationManager.AppSettings["keyString"].ToString(); // passing API key
                string clientID = ConfigurationManager.AppSettings["clientID"].ToString();
                string driveStartLoc = drive.StartLocation.AddressLine1 + ","
                    + drive.StartLocation.City + ","
                    + drive.StartLocation.StateProvince + ","
                    + drive.StartLocation.PostalCode;

                string UserLoc = usr.DriverLocation.AddressLine1 + ","
                + usr.DriverLocation.City + ","
                + usr.DriverLocation.StateProvince + ","
                + usr.DriverLocation.PostalCode;

                string ApiURL = "https://maps.googleapis.com/maps/api/distancematrix/json?";
                // &departure_time=now for traffic (maybe can be used now with the improved algo)
                string p = "units=imperial=now&origins=" + UserLoc + "&destinations=" + driveStartLoc + "&mode=Driving";
                string urlRequest = ApiURL + p;
                urlRequest += "&key=" + keyString;
                //if (keyString.ToString() != "")
                //{
                //    urlRequest += "&client=" + clientID;
                //    urlRequest = Sign(urlRequest, keyString); // request with api key and client id
                //}
                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                //string resp = "{\n   \"destination_addresses\" : [ \"1 Aloha Tower Dr, Honolulu, HI 96813, USA\" ],\n   \"origin_addresses\" : [ \"830 Lokahi St, Honolulu, HI 96826, USA\" ],\n   \"rows\" : [\n      {\n         \"elements\" : [\n            {\n               \"distance\" : {\n                  \"text\" : \"3.0 mi\",\n                  \"value\" : 4856\n               },\n               \"duration\" : {\n                  \"text\" : \"12 mins\",\n                  \"value\" : 711\n               },\n               \"duration_in_traffic\" : {\n                  \"text\" : \"12 mins\",\n                  \"value\" : 704\n               },\n               \"status\" : \"OK\"\n            }\n         ]\n      }\n   ],\n   \"status\" : \"OK\"\n}\n";


                JObject values = JObject.Parse(resp);
                if (!((string)values["rows"][0]["elements"][0]["status"] == "ZERO_RESULTS" || (string)values["rows"][0]["elements"][0]["status"] == "NOT_FOUND"))
                {
                    var duration = (string)values["rows"][0]["elements"][0]["duration"]["value"];
                    var distance = (string)values["rows"][0]["elements"][0]["distance"]["value"];
                    return Tuple.Create(Int32.Parse(duration), Int32.Parse(distance));
                    //return Tuple.Create(values[], values[1]);
                }
                return Tuple.Create(-1, -1);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Details(int id, DriveInfoViewModel driveInfoViewModel)
        {
            var drive = db.Drives.ToList().Find(d => d.DriveId == id);
            if (User.IsInRole("Driver"))
            {
                if (drive.DriverId == null)
                {
                    drive.OptedInDrivers.Add(new DriverIdString(User.Identity.GetUserName()));
                }
                //var drive = db.Drives.ToList().Find(d => d.DriveId == id);
                //if (drive.DriverId == null)
                //{
                //    // num active drives
                //    if (db.Drives.ToList().Where(d => d.DriveDone == false && d.DriverId == User.Identity.GetUserId()).Count() > 0)
                //    {
                //        ViewBag.emsg = "u have an active drive";
                //        return View();
                //    }
                //    drive.DriverId = User.Identity.GetUserId();
                //    db.Entry(drive).State = EntityState.Modified;
                //    _ = await db.SaveChangesAsync();
                //}
                else if (drive.DriverId == User.Identity.GetUserId())
                {
                    drive.DriveDone = true;
                }
            }
            else
            {
               if (driveInfoViewModel.SelectedDriver == null)
               {
                    return View(driveInfoViewModel);
               }
                else
                {
                    drive.DriverId = UserManager.FindByName(driveInfoViewModel.SelectedDriver).Id;
                }
            }
            db.Entry(drive).State = EntityState.Modified;
            _ = await db.SaveChangesAsync();
            return RedirectToAction("Details");
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

                var users = from u in udb.Users select u;
                var usersToNotify = new List<ApplicationUser>();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(udb));
                // for each user, go through availabilities where the day is the same, the start time is before now, the endtime is after now + drive time
                foreach (var z in users) {
                    foreach (var x in z.Availabilities.Where(a => a.Dayoftheweek == DateTime.Now.DayOfWeek && a.StartTime <= DateTime.Now.Hour && a.EndTime >= (double)(DateTime.Now.Hour + (driveInfoViewModel.DriveDuration / 60))))
                    {
                        string role = "";
                        foreach (var y in z.Roles)
                        {
                            role = roleManager.FindByIdAsync(y.RoleId).Result.Name;
                        }
                        if (role != "Inactive")
                        {
                            usersToNotify.Add(z);
                        }
                    }
                }

                await db.SaveChangesAsync();
                List<Drive> drives = await db.Drives.ToListAsync();

                usersToNotify = usersToNotify.Distinct().ToList();
                await SendEmailAsync(usersToNotify, drives.Last());

                return RedirectToAction("Index");
            }

            return View(driveInfoViewModel);
        }

        // half broken i dunno, also kinda expensive, and needs to verify phone number to be countrycode + phonenumber
        public void SendTextMsg(List<ApplicationUser> users)
        {
            string AccountSid = "ACc2b05b3b84b6739a3a2ef068c21352e1";
            string AuthToken = "bc1428ec7c9aba9ff97476ff9a15d270";

            TwilioClient.Init(AccountSid, AuthToken);

            foreach (var x in users)
            {
                var message = MessageResource.Create(
                body: "Hi there!",
                from: new Twilio.Types.PhoneNumber("+17164188033"),
                to: new Twilio.Types.PhoneNumber(x.PhoneNumber)
                );

                Console.WriteLine(message.Sid);
            }

        }

        public async Task SendEmailAsync(List<ApplicationUser> users, Drive drive)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            // need to verify this email like noreply@wildlifecenter.org or whatever
            var from = new EmailAddress("itshawk@gnode.org", "Wheels for Wildlife");
            //var plainTextContent = drive.DriveName + " is available now!";
            //var htmlContent = "<a href=https://localhost:44361/Drive/Details/" + drive.DriveId + ">" +
            //    "<strong>" + drive.DriveName + " is available now! Click Here To Go To The Drive!</a></strong>";

            //var templateId = { "Sender_Name" :  }
            foreach (var x in users) {
                var to = new EmailAddress(x.Email, "NameGoesHere");
                var msg = MailHelper.CreateSingleTemplateEmail(from, to, "d-2e5fce8502b04f44bd0860a0b31050fc", new NotifEmail()
                {
                    Sender_Name = "Wheels For Wildlife",
                    Sender_Address = "53 Lighthouse Rd Box 551752",
                    Sender_City = "Kapaau",
                    Sender_State = "HI",
                    Sender_Zip = "96755",
                    Text = drive.DriveName + " is available now!",
                    Url = "https://localhost:44361/Drive/Details/" + drive.DriveId,
                    driveName = drive.DriveName
                });
                var response = await client.SendEmailAsync(msg);
            }
        }

        private class NotifEmail
        {
            [JsonProperty("Sender_Name")]
            public string Sender_Name { get; set; }

            [JsonProperty("Sender_Address")]
            public string Sender_Address { get; set; }

            [JsonProperty("Sender_City")]
            public string Sender_City { get; set; }

            [JsonProperty("Sender_State")]
            public string Sender_State { get; set; }


            [JsonProperty("Sender_Zip")]
            public string Sender_Zip { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }


            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("driveName")]
            public string driveName { get; set; }
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
        [CustomAuthorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DriveInfoViewModel driveInfoViewModel)
        {
            if (ModelState.IsValid)
            {
                if (driveInfoViewModel.DriverId != null)
                {
                    var user = await UserManager.FindByNameAsync(driveInfoViewModel.DriverId);
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
                drive.ExtraDetails = driveInfoViewModel.ExtraDetails;

                db.Entry(drive).State = EntityState.Modified;
                _ = await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { Message = "Updated!" });
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
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Drive drive = await db.Drives.FindAsync(id);
            //IList<DriverIdString> tmplist = drive.OptedInDrivers.;
            while (drive.OptedInDrivers.Count > 0) {
                db.OptedInDrivers.Remove(drive.OptedInDrivers.First());
            }
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
