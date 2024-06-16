using System.ComponentModel.DataAnnotations;

namespace LMS.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthDate.Year;

                // Adjust age if the birthday hasn't occurred yet this year
                if (birthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                if (age < _minimumAge)
                {
                    return new ValidationResult($"Must be at least {_minimumAge} years old.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
