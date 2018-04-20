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
    public class Position
    {
        [Key]
        public int PositionId { set; get; }

        [Required]
        [DisplayName("Institution")]
        public String PositionInstitution { set; get; }

        [Required]
        [DisplayName("Name")]
        public String PositionName { set; get; }

        [Required]
        [DisplayName("Gender")]
        [Range(1, int.MaxValue, ErrorMessage = "Select the Gender")]
        public Gender PositionGender { set; get; }

        
    }

    public enum Gender
    {
        Male = 1,
        Female = 2,
    }
}