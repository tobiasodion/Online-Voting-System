using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class Ballot
    {
        [Key]
        public int BallotId { get; set; }

        public String VoterId { get; set; }

        public String BallotPosition { get; set; }

        public int VoterCandidateChoice { get; set; }
    }
}