using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class Election
    {
        [Key]
        public int ElectionId { set; get; }


        public String ElectionInstitution { set; get; }

        [Required]
        [DisplayName("Name")]
        public String ElectionName { set; get; }

        [Required]
        [DisplayName("Start Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ElectionStartTime { set; get; }

        [Required]
        [DisplayName("End Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ElectionEndTime { set; get; }
    }
}