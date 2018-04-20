using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingSystem.Models;

namespace VotingSystem.Controllers
{
    public class PollingBoothController : Controller
    {
        SystemDb1 db = new SystemDb1();
        // GET: PollingBooth
        public ActionResult Index()
        {
            if (Session["VotingDetails"] != null)
            {
                return RedirectToAction("ViewCandidateVote");
            }

            else
            {
                return RedirectToAction("Login","VoterLogin");
            }
        }

        public ActionResult ViewCandidateVote()
        {
            var test = Session["InstitutionName"].ToString();

            var query = (from r in db.Positions
                         where r.PositionInstitution == test
                         select r).ToList();

            Session["PostionCount"] = query.Count();

            return View(query);
        }


        public PartialViewResult VoteForm(String id = null)
        {
            var test = Session["InstitutionName"].ToString();

            var model = (from r in db.Candidates
                         where r.CandidateInstitution == test && r.CandidatePosition == id && r.CandidateStatus == "Approved"
                         select r).ToList();

            return PartialView("VoteFormPartial", model);
        }

        [HttpPost]
        public PartialViewResult CountVote(int id = 0)
        {
            var test2 = Session["VoterRegNo"].ToString();

            int positionCount = (Int32)Session["PostionCount"];

                //check the position the candidate is contesting
                var query = (from r in db.Candidates
                             where r.CandidateId == id
                             select r).SingleOrDefault();

                //Saving to temporary Store

                Ballot myBallot = new Ballot();

                myBallot.VoterId = test2;
                myBallot.VoterCandidateChoice = id;
                myBallot.BallotPosition = query.CandidatePosition;

                //Check if voter is making changes or fresh vote
                var query2 = (from r in db.Ballots
                              where r.VoterId == test2 && r.BallotPosition == query.CandidatePosition
                              select r).SingleOrDefault();

                //If Fresh vote
                if (query2 == null)
                {
                    db.Ballots.Add(myBallot);
                    db.SaveChanges();

                //check if voter is done voting
                var check = (from r in db.Ballots
                             where r.VoterId == test2
                             select r).ToList();

                //Voter has voted for all Positions
                if (check.Count() == positionCount)
                {
                    ViewBag.Message3 = "Click Submit to Complete Voting";
                    return PartialView("CountVotePartial1");
                }

                //Voter has not voted for all postions
                else
                {
                    ViewBag.Message2 = "Click Next Tab to Vote next Candidate";
                    return PartialView("CountVotePartial");
                }
            }

                //If making changes
                else
                {
                    db.Ballots.Remove(query2);
                    db.Ballots.Add(myBallot);
                    db.SaveChanges();

                //check if voter is done voting
                var check = (from r in db.Ballots
                             where r.VoterId == test2
                             select r).ToList();

                //Voter has voted for all Positions
                if (check.Count() == positionCount)
                {
                    ViewBag.Message3 = "Click Submit to Complete Voting Or Postion Tab to change Choice";
                    return PartialView("CountVotePartial1");
                }

                //Voter has not voted for all postions
                else
                {
                    ViewBag.Message2 = "Click Next Tab to Vote next Candidate";
                    return PartialView("CountVotePartial");
                }
            }         
      }

        
        public ActionResult Vote()
        {
            var test2 = Session["VoterRegNo"].ToString();

            var query = (from r in db.Ballots
                        where r.VoterId == test2
                        select r).ToList();

            foreach (var item in query)
            {
                var model = (from r in db.Candidates
                             where r.CandidateId == item.VoterCandidateChoice
                             select r).Single();

                model.CandidateVoteCount++;

               // db.SaveChanges();
            }

            var query2 = (from r in db.Voters
                          where r.VoterInstitutionNo == test2
                          select r).Single();

            query2.VoterStatus = "Voted";
            db.SaveChanges();

            TempData["Vote Success"] = "Voting Successful! ";
            return RedirectToAction("Index","VoterPage");
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