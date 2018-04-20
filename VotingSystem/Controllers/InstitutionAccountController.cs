using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class InstitutionAccountController : Controller
    {
        SystemDb1 db = new SystemDb1();

        public ActionResult Index()
        {      
            return RedirectToAction("Institutionlogin");
        }

        public ActionResult InstitutionLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InstitutionLogin(Institution details)
        {
            try
            {
                var query = (from r in db.Institutions
                             where r.InstitutionEmail == details.InstitutionEmail && (r.InstitutionPassword == details.InstitutionPassword)
                             select r).SingleOrDefault();

                if (query == null)
                {
                    ViewBag.Message = "Invalid email or Password!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                        Session["InstitutionName"] = query.InstitutionName.ToString();
                        Session["InstitutionRegNo"] = query.InstitutionRegNo.ToString();
                        
                        return RedirectToAction("InstitutionLoggedIn");
                }

            }

            catch (Exception ex)
            {
                return View("Error");
            }
        }


        public ActionResult AdminLoggedIn()
        {
            return View();
        }

        public ActionResult InstitutionLoggedIn()
        {

            if (Session["InstitutionName"] != null && Session["InstitutionRegNo"] != null)
            {
                //  return RedirectToAction("Index", "SuperAdminAccount", new { area = "" });
                return RedirectToAction("Index", "InstitutionPage");
            }

            else
            {
                return RedirectToAction("InstitutionLogin");
            }
               

        }

        public ActionResult InstitutionLogout()
        {
            Session["InstitutionName"] = null;
            Session["InstitutionRegNo"] = null;
            return RedirectToAction("InstitutionLogin");
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