using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotingSystem.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateId { set; get; }

        [Required]
        [DisplayName("Institution")]
        public String CandidateInstitution { set; get; }

        [Required]
        [DisplayName("ID Number")]
        public String CandidateInstitutionNo { set; get; }

        [Required]
        [DisplayName("First Name")]
        public String CandidateFirstName { set; get; }

        [Required]
        [DisplayName("Last Name")]
        public String CandidateLastName { set; get; }

        [Required]
        [DisplayName("Photo")]
        public byte[] CandidatePhoto { set; get; }

        [Required]
        [DisplayName("Slogan")]
        public String CandidateManifesto { set; get; }

        [Required]
        public String CandidateStatus { set; get; }

        [Required]
        [DisplayName("Position")]
        public String CandidatePosition { set; get; }

        [DisplayName("Votes")]
        public int CandidateVoteCount { set; get; }

        [NotMapped]
        public SelectList PositionList { get; set; }

    }
}