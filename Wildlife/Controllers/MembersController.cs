using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wildlife.Models;

namespace Wildlife.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MembersHomePage()
        {
            ViewBag.Message = "Welcome, Driver!";

            return View();
        }
    }
}