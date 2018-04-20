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
    public class Voter
    {
        [Key]
        public int VoterId { set; get; }

        [DisplayName("Institution")]
        public String VoterInstitution { set; get; }

        [Required]
        [DisplayName("First Name")]
        public String VoterFirstName { set; get; }

        [Required]
        [DisplayName("Last Name")]
        public String VoterLastName { set; get; }

        [DisplayName("ID Number")]
        public String VoterInstitutionNo { set; get; }

        [Required]
        [DisplayName("Mobile")]
        public String VoterTelephone { set; get; }

        [Required]
        [DisplayName("e-mail")]
        [DataType(DataType.EmailAddress)]
        public String VoterEmail { set; get; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public String VoterPassword { set; get; }

        [Required]
        public String VoterStatus { set; get; }

        [NotMapped]
        public SelectList InstitutionList { get; set; }
    }
}