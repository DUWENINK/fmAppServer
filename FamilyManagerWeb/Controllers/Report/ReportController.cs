using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyManagerWeb.Models;

namespace FamilyManagerWeb.Controllers
{
    public class ReportController : Controller
    {
        private FamilyCaiWuDBEntities db = new FamilyCaiWuDBEntities();

        public ActionResult test() 
        {
            return View();
        }

        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}