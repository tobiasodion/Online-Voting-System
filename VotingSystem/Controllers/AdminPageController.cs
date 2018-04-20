using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class AdminPageController : Controller
    {
        SystemDb1 db = new SystemDb1();

        // GET: InstitutionPage
        public ActionResult Index()
        {
            try
            {
                ViewBag.Message = Session["InstitutionName"].ToString();
                if (ViewData["message"] != null)
                {
                    ViewBag.message2 = ViewData["message"].ToString();
                }

                var test = Session["InstitutionName"].ToString();

                var model = (from r in db.Elections
                             where r.ElectionInstitution == test
                             select r).ToList();
                
                if (model.Count == 0)
                {
                    return RedirectToAction("RegisterElection");
                }

                else
                {
                    return View(model);
                }
                
            }

            catch (Exception ex)
            {
                return RedirectToAction("AdminLogin", "AdminAccount");
            }
        }

        public ActionResult RegisterElection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterElection(Election details)
        {
       try {
              
                    details.ElectionInstitution = Session["InstitutionName"].ToString();
                    db.Elections.Add(details);
                    db.SaveChanges();

                    return RedirectToAction("Index");     
                }

        catch(Exception ex)
            {
                throw;
            }
            
           
        }

        [HttpGet]
        public ActionResult EditTime(int id)
        {
            var model = db.Elections.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditTime(Election details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(details);
        }

        public ActionResult ManagePosition()
        {
            var test = Session["InstitutionName"].ToString();

            var query = (from r in db.Elections
                         where r.ElectionInstitution == test
                         select r).SingleOrDefault();

            DateTime PresentDate = new DateTime();
            PresentDate = DateTime.Now;

            var compare = DateTime.Compare(PresentDate, query.ElectionStartTime);

            if (compare <= 0)
            {
                var model = (from r in db.Positions
                             where r.PositionInstitution == test
                             select r).ToList();

                if (model.Count == 0)
                {
                    ViewBag.Message = "No Position Available!";
                }
                return View(model);
            }

             else
            {
                ViewBag.Message = "Voting Already in Progress";
                ModelState.Clear();
                return RedirectToAction("Index");
            }

        }

        public ActionResult RemovePosition(int id)
        {
            var query = (from r in db.Positions
                         where r.PositionId == id
                         select r).SingleOrDefault();

            db.Positions.Remove(query);
            db.SaveChanges();

            return RedirectToAction("ManagePosition");
        }

        public ActionResult RegisterPosition()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterPosition(Position details) 
        {
           
            details.PositionInstitution = Session["InstitutionName"].ToString();
           
                db.Positions.Add(details);
                db.SaveChanges();
                ViewBag.Message = "Position Successfully Created";
                return RedirectToAction("ManagePosition");
        }

        public ActionResult ManageCandidate()
        {
            var test = Session["InstitutionName"].ToString();

            var model = (from r in db.Candidates
                         where r.CandidateStatus == "Approved" && r.CandidateInstitution == test
                         select r).ToList();

            if (model.Count == 0)
            {
                ViewBag.Message = "No Candidate Available!";
            }

            return View(model);
        }

        public ActionResult RemoveCandidate(int id)
        {
            var test = Session["InstitutionName"].ToString();

            var model = (from r in db.Candidates
                         where r.CandidateId == id
                         select r).SingleOrDefault();

            db.Candidates.Remove(model);
            db.SaveChanges();
            return RedirectToAction("ManageCandidate");
        }

        public ActionResult PendingRequest()
        {
            var test = Session["InstitutionName"].ToString();

            var model = (from r in db.Candidates
                         where r.CandidateStatus == "Pending"
                         select r).ToList();

            if (model.Count == 0)
            {
                ViewBag.Message = "No Pending Request!";
            }
            return View(model);
        }

        public ActionResult ApprovedDetails(int id)
        {
            var model = (from r in db.Candidates
                         where r.CandidateId == id && r.CandidateStatus == "Approved"
                         select r).SingleOrDefault();

            return View(model);
        }

        public ActionResult PendingDetails(int id)
        {
            var model = (from r in db.Candidates
                         where r.CandidateId == id && r.CandidateStatus == "Pending"
                         select r).SingleOrDefault();
            return View(model);
        }

        public ActionResult ApproveCandidate(int id)
        {
            var test = Session["InstitutionName"].ToString();

            var model = (from r in db.Candidates
                         where r.CandidateId == id
                         select r).SingleOrDefault();

            model.CandidateStatus = "Approved";
            model.CandidateVoteCount = 0;
            db.SaveChanges();
            return RedirectToAction("PendingRequest");
        }

       

        public ActionResult ViewReport()
        {
            return View();
        }

        public ActionResult ViewResult()
        {
            return View();
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