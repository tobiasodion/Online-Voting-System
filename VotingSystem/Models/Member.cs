using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class Member
    {
        [Key]
        public int MemberId { set; get; }

        [Required]
        public String MemberInstitution { set; get; }

        [Required]
        public String MemberFirstName { set; get; }

        [Required]
        public String MemberLastName { set; get; }

        [Required]
        public String MemberInstitutionNo { set; get; }

        [Required]
        public String MemberGender { set; get; }

        [Required]
        public String MemberTelephone { set; get; }

        [Required]
        public String MemberStatus { set; get; }
       
        [Required]
        [DataType(DataType.EmailAddress)]
        public String MemberEmail { set; get; }

    }
}