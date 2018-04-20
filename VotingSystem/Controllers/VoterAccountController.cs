using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class VoterAccountController : Controller
    {
        SystemDb1 db = new SystemDb1();

        public ActionResult Index()
        {
            return RedirectToAction("Voterlogin");
        }

        public ActionResult VoterVerification()
        {
            var query = (from r in db.Institutions
                         select r).ToList();

            Voter MyVoter = new Voter();
            MyVoter.InstitutionList = new SelectList(query, "InstitutionName","InstitutionName");
            return View(MyVoter);
        }

        [HttpPost]
        public ActionResult VoterVerification(Voter details)
        {
            
            var query = (from r in db.Members
                         where r.MemberInstitution == details.VoterInstitution &&
                          r.MemberEmail == details.VoterEmail &&
                          r.MemberTelephone == details.VoterTelephone &&
                          r.MemberInstitutionNo == details.VoterInstitutionNo
                         select r).SingleOrDefault();

            if (query != null)
            {
                if (query.MemberStatus == "Verified")
                {
                    TempData["message"] = "Member Already Verified";
                    return RedirectToAction("VoterLogin");
                }

                else
                {
                    details.VoterStatus = "Pending";
                    details.VoterPassword = "default";
                    query.MemberStatus = "Verified";

                    db.Voters.Add(details);
                    db.SaveChanges();
                    TempData["message"] = "Verification Successful";
                    return RedirectToAction("VoterLogin");
                }
            }
            else
            {
                TempData["message"] = "Verification Not Successful! Contact your Institution";
                return RedirectToAction("VoterLogin");
            }

        }

        public ActionResult VoterLogin()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
                      
            return View();
        }

        [HttpPost]
        public ActionResult VoterLogin(Voter details)
        {
            try
            {
                var query = (from r in db.Voters
                             where r.VoterEmail == details.VoterEmail && r.VoterPassword == details.VoterPassword
                             select r).SingleOrDefault();

                if (query == null)
                {
                    ViewBag.Message = "Invalid email or Password!";
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    Session["InstitutionName"] = query.VoterInstitution.ToString();
                    Session["VoterRegNo"] = query.VoterInstitutionNo.ToString();
                    Session["email"] = query.VoterEmail.ToString();

                    return RedirectToAction("VoterLoggedIn");
                }

            }

            catch (Exception ex)
            {
                return View("Error");
            }
        }




        public ActionResult VoterLoggedIn()
        {

            if (Session["InstitutionName"] != null && Session["VoterRegNo"] != null)
            {
                //  return RedirectToAction("Index", "SuperAdminAccount", new { area = "" });
                return RedirectToAction("Index", "VoterPage");
            }

            else
            {
                return RedirectToAction("VoterLogin");
            }


        }

        public ActionResult VoterLogout()
        {
            Session["InstitutionName"] = null;
            Session["VoterRegNo"] = null;
            return RedirectToAction("VoterLogin");
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