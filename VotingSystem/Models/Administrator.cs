using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class Administrator
    {
        [Key]
        public int AdministratorId { set; get; }

        [DisplayName("Institution")]
        public String AdministratorInstitution { set; get; }

        [Required]
        [DisplayName("First Name")]
        public String AdministratorFirstName { set; get; }

        [Required]
        [DisplayName("Last Name")]
        public String AdministratorLastName { set; get; }

        [Required]
        [DisplayName("ID Number")]
        public String AdministratorInstitutionNo { set; get; }

        [Required]
        [DisplayName("Mobile")]
        public String AdministratorTelephone { set; get; }

        [Required]
        [DisplayName("e-mail")]
        [DataType(DataType.EmailAddress)]
        public String AdministratorEmail { set; get; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public String AdministratorPassword { set; get; }

    }
}