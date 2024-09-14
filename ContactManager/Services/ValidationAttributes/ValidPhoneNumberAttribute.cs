using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ContactManager.Services.ValidationAttributes
{
    public class ValidPhoneNumberAttribute: ValidationAttribute
    {
        private const string PHONE_NUMBER_PATTERN = @"^\+?[1-9]\d{1,14}$";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Phone number is required");
            }
            var phoneNumber = value.ToString();

            if (!Regex.IsMatch(phoneNumber, PHONE_NUMBER_PATTERN))
            {
                return new ValidationResult("Invalid phone number format. Phone numbers must start with a + followed by up to 15 digits");
            }

            return ValidationResult.Success;
        }
    }
}
