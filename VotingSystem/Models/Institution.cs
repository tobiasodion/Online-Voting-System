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
    public class Institution
    {
        [Key]
        public int InstitutionId { set; get; }

        [DisplayName("Name")]
        public String InstitutionName { set; get; }

        [Required]
        [DisplayName("Mobile")]
        public String InstitutionTelephone { set; get; }

        [Required]
        [DisplayName("e-mail")]
        [DataType(DataType.EmailAddress)]
        public String InstitutionEmail { set; get; }

        [Required]
        [DisplayName("Institution Code")]
        public String InstitutionRegNo { set; get; }

        
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public String InstitutionPassword { set; get; }
        
        public string InstitutionMemberList { get; set; }

        public string InstitutionAdminLog { get; set; }

        public string ElectionReport { get; set; }

       
    }
}