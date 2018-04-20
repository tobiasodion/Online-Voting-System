using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class InstitutionPageController : Controller
    {
        SystemDb1 db = new SystemDb1();

        // GET: InstitutionPage
        public ActionResult Index()
        {
            try
            {
                ViewBag.Message = Session["InstitutionName"].ToString();
               
                return View();
            }

            catch(Exception ex)
            {
                return RedirectToAction("InstitutionLogin", "InstitutionAccount");
            }  
        }

        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(Administrator details)
        {
            details.AdministratorInstitution = Session["InstitutionName"].ToString();

            var query = (from r in db.Members
                         where r.MemberInstitution == details.AdministratorInstitution 
                         && r.MemberInstitutionNo == details.AdministratorInstitutionNo
                         && r.MemberTelephone == details.AdministratorTelephone
                         && r.MemberEmail == details.AdministratorEmail
                        select r).SingleOrDefault();

            if (query == null)
            {
                ViewBag.Message = "Administrator must be a member of Institution!";
                return View();
            }

            else
            {
                details.AdministratorFirstName = query.MemberFirstName;
                details.AdministratorLastName = query.MemberLastName;
                TempData["Admin"] = details;
                return RedirectToAction("RegisterObserver");
            }
        }

        public ActionResult RegisterObserver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterObserver(Observer details)
        {
            Administrator model = (Administrator)TempData["Admin"];
            details.ObserverInstitution = Session["InstitutionName"].ToString();

            model.AdministratorPassword = "Default";       //to be replaced by email sending block
            details.ObserverPassword = "Default";          //To be replaced by email sending block
           
            db.Administrators.Add(model);
            details.ObserverStatus = "Pending";

            db.Observers.Add(details);

            db.SaveChanges();

            ViewData["Success"] = "Election Started Successfully";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}