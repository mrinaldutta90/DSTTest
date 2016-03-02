using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pets.Models;

namespace Pets.Controllers
{
    public class HomeController : Controller
    {
         Person person = new Person();

        /// <summary>
        /// Calls the Index.cshtml view
        /// </summary> 
        public ActionResult Index()

        {
            return View(person.getPetsList());
        }
    }
}