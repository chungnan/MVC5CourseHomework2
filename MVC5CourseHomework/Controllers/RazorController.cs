using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5CourseHomework.Controllers
{
    public class RazorController : Controller
    {
        public ActionResult Calendar()
        {
            return View();
        }
        public ActionResult Chart()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult FileManager()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult Gallery()
        {
            return View();
        }

        public ActionResult Icon()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Messages()
        {
            return View();
        }

        public ActionResult Submenu()
        {
            return View("Dashboard");
        }

        public ActionResult Submenu2()
        {
            return View("Dashboard");
        }

        public ActionResult Submenu3()
        {
            return View("Dashboard");
        }

        public ActionResult Table()
        {
            return View();
        }

        public ActionResult Tasks()
        {
            return View();
        }

        public ActionResult Typography()
        {
            return View();
        }

        public ActionResult Ui()
        {
            return View();
        }

        public ActionResult Widgets()
        {
            return View();
        }

    }
}