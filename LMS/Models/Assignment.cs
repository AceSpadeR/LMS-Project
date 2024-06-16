using LMS.Attributes;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LMS.Models
{
    public class Assignment
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "Max Points")]
        [Range(1, int.MaxValue)]
        public int MaxPoints { get; set; }

        [Required]
        [Display(Name = "Submission")]
        public string SubmissionType { get; set; } = string.Empty;

        [Required]
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual required Class Classes { get; set; }

    }
}