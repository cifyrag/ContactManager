using System.ComponentModel.DataAnnotations;

namespace ContactManager.Services.ValidationAttributes
{
    public class ValidDateOfBirthAttribute:ValidationAttribute
    {
        private readonly int _minimumAge;

        public ValidDateOfBirthAttribute(int minimumAge = 0)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateOfBirth;

            if (DateTime.TryParse(value.ToString(), out dateOfBirth))
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;

                if (dateOfBirth > today)
                {
                    return new ValidationResult("Date of Birth cannot be in the future");
                }

                if (dateOfBirth > today.AddYears(-age))
                {
                    age--;
                }

                if (age < _minimumAge)
                {
                    return new ValidationResult($"You must be at least {_minimumAge} years old");
                }
            }
            else
            {
                return new ValidationResult("Invalid Date of Birth format");
            }

            return ValidationResult.Success;
        }
    }
}
