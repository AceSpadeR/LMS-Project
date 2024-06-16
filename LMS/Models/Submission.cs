using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Submission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }
        [ForeignKey("AssignmentId")]
        public virtual Assignment Assignment { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual User Student { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TurnInTime { get; set; }

        public int? Points { get; set; }

        public string? SubmissionText { get; set; }

        public string? SubmissionFileName { get; set; }

        public string? SubmissionFilePath { get; set; }
    }
}
