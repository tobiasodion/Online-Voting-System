using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class VoterPageController : Controller
    {
        SystemDb1 db = new SystemDb1();
        // GET: Voter
        public ActionResult Index()
        {
            if (TempData.ContainsKey("FraudAlert"))
            {
                ViewBag.Message = TempData["FraudAlert"].ToString();   
            }

            if (TempData.ContainsKey("Vote Success"))
            {
                ViewBag.Message = TempData["Vote Success"].ToString();
            }

            if (TempData.ContainsKey("AlreadyVoted"))
            {
                ViewBag.Message = TempData["AlreadyVoted"].ToString();
            }    
                return View();
        }

        public ActionResult CandidacyRequest()
        {
           var test = Session["InstitutionName"].ToString();

            var query = (from r in db.Positions
                         where r.PositionInstitution == test
                         select r).ToList();

            Candidate MyPosition = new Candidate();
            MyPosition.PositionList = new SelectList(query, "PositionName", "PositionName");

            return View(MyPosition);
        }

        [HttpPost]
        public ActionResult CandidacyRequest(Candidate details, HttpPostedFileBase file)
        {
            var test = Session["InstitutionName"].ToString();
            var InstitutionNo = Session["VoterRegNo"].ToString();

            var query = (from r in db.Voters
                         where r.VoterInstitution == test && r.VoterInstitutionNo == InstitutionNo
                         select r).SingleOrDefault();

            var check = (from r in db.Candidates
                         where r.CandidateInstitutionNo == InstitutionNo
                         select r).SingleOrDefault();

            if (check == null) {

                if (file != null && file.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        details.CandidatePhoto = reader.ReadBytes(file.ContentLength);
                    }

                    details.CandidateInstitution = query.VoterInstitution;
                    details.CandidateInstitutionNo = query.VoterInstitutionNo;
                    details.CandidateLastName = query.VoterLastName;
                    details.CandidateFirstName = query.VoterFirstName;
                    details.CandidateStatus = "Pending";


                    db.Candidates.Add(details);

                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = "Application Successful";
                  
                }
                return RedirectToAction("Index");
            }

            else
            {
                ViewBag.Message = "Already Applied for a post";
                return RedirectToAction("Index");
            }    
            
        }

        public ActionResult ViewCandidate()
        {      
            var test = Session["InstitutionName"].ToString();

            var query = (from r in db.Positions
                         where r.PositionInstitution == test
                         select r).ToList();

            return View(query);
        }

        public PartialViewResult DisplayCandidate(String id = null )
        {
            var test = Session["InstitutionName"].ToString();

            var query = (from r in db.Candidates
                         where r.CandidateInstitution == test && r.CandidatePosition == id && r.CandidateStatus == "Approved"
                         select r).ToList();

            if (query.Count == 0)
            {
                ViewBag.Message = "No Candidates for this Position!";
            }
           
            return PartialView("DisplayCandidatePartial", query);
        }


        public ActionResult CastVote()
        {
            var InstitutionNo = Session["VoterRegNo"].ToString();

            //Check If Voter has Voted
            var query = (from r in db.Voters
                         where r.VoterInstitutionNo == InstitutionNo
                         select r).Single();

            if (query.VoterStatus == "Pending")
            {
                //Send OTP to voters phone Number
                //Display a form for User to Enter the OTP 
                return View();
            }

            else
            {
                TempData["AlreadyVoted"] = "Voter has Already Voted!";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult CastVote(Voter details)
        {
            //Send OTP to voters phone Number
            //Display a form for User to Enter the OTP 

            var InstitutionNo = Session["VoterRegNo"].ToString();

            var query = (from r in db.Voters
                         where r.VoterInstitutionNo == InstitutionNo
                         select r).Single();

            if (details.VoterTelephone == query.VoterTelephone )
            {
                Session["VotingDetails"] = details.VoterTelephone;
                return RedirectToAction("Index", "PollingBooth");
            }

            else
            {
                TempData["FraudAlert"] = "Wrong OTP!";
                return RedirectToAction("Index");
            }
           
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