using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class AdminAccountController : Controller
    {
        SystemDb1 db = new SystemDb1();

        public ActionResult Index()
        {
            return RedirectToAction("Adminlogin");
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(Administrator details)
        {
            try
            {
                var query = (from r in db.Administrators
                             where r.AdministratorEmail == details.AdministratorEmail && r.AdministratorPassword == details.AdministratorPassword
                             select r).SingleOrDefault();

                if (query == null)
                {
                    ViewBag.Message = "Invalid email or Password!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    Session["InstitutionName"] = query.AdministratorInstitution.ToString();
                    Session["AdministratorRegNo"] = query.AdministratorInstitutionNo.ToString();

                    return RedirectToAction("AdminLoggedIn");
                }

            }

            catch (Exception ex)
            {
                return View("Error");
            }
        }




        public ActionResult AdminLoggedIn()
        {

            if (Session["InstitutionName"] != null && Session["AdministratorRegNo"] != null)
            {
                //  return RedirectToAction("Index", "SuperAdminAccount", new { area = "" });
                return RedirectToAction("Index", "AdminPage");
            }

            else
            {
                return RedirectToAction("AdminLogin");
            }


        }

        public ActionResult AdminLogout()
        {
            Session["InstitutionName"] = null;
            Session["AdministratorRegNo"] = null;
            return RedirectToAction("AdminLogin");
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