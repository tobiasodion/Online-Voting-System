using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class Observer
    {
        [Key]
        public int ObserverId { set; get; }

        public String ObserverInstitution { set; get; }

        [Required]
        [DisplayName("First Name")]
        public String ObserverFirstName { set; get; }

        [Required]
        [DisplayName("Last Name")]
        public String ObserverLastName { set; get; }

        [Required]
        [DisplayName("Mobile")]
        public String ObserverTelephone { set; get; }

        [Required]
        [DisplayName("e-mail")]
        [DataType(DataType.EmailAddress)]
        public String ObserverEmail { set; get; }

        
        [DataType(DataType.Password)]
        public String ObserverPassword { set; get; }

        
        public String ObserverStatus { set; get; }
    }
}