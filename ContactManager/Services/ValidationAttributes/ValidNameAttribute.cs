using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ContactManager.Services.ValidationAttributes
{
    public class ValidNameAttribute: ValidationAttribute
    {
        private const string NamePattern = @"^[a-zA-Z\s\-]+$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Name is required");
            }
            var name = value.ToString();

            if (!Regex.IsMatch(name, NamePattern))
            {
                return new ValidationResult("Name can only contain letters, spaces and hyphens");
            }

            if (name.Length < 2 || name.Length > 50)
            {
                return new ValidationResult("Name must be between 2 and 50 characters");
            }

            return ValidationResult.Success;
        }
    }
}
