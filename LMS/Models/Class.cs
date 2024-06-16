using LMS.Attributes;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LMS.Models
{
    public class Class
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course")]
        public string CourseNumber { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Credit Hrs")]
        [Range(1, 8)]
        public int CreditHours { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Semester Start Date")]
        public DateTime StartDate { get; set; } = new DateTime(2024, 1, 1); // Replace with your semester start date

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Semester End Date")]
        public DateTime EndDate { get; set; } = new DateTime(2024, 5, 31); // Replace with your semester end date
    

        [Required]
        [Display(Name = "Meeting Days")]
        public List<DayOfWeek> MeetingDays { get; set; } = new List<DayOfWeek>();

        [Required]
        public int ProfId {  get; set; }

    }
}