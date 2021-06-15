using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrainingManagementSystem.Models
{
    public class Trainer
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string WorkPlace { get; set; }
        public int Phone { get; set; }
        public string Education { get; set; }
    }
}