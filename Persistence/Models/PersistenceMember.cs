using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Persistence.Models
{
    public class PersistenceMember
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(30 ,ErrorMessage = "Max characters are 30.")]
        [MinLength(3, ErrorMessage = "Min Characters are 3.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(30, ErrorMessage = "Max characters are 30.")]
        public string Username{get;set;}
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; }
        public float Hours { get; set; }
        public string Status { get; set; } = "inactive";
        public string Role { get; set; } = "worker";
        public string Password { get; set; }
        public string ResetToken { get; set; } = null;
    }
}