using LMS.Attributes;
using System;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace LMS.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public string EventDescription { get; set; } = string.Empty;


        // AssignmentId default is -1. This is used to tell if it is a class event.

        public int AssignmentId { get; set; } = -1;


        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public virtual User User { get; set; }
        public virtual Class Class { get; set; }
    }
}
