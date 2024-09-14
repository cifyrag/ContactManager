using System.ComponentModel.DataAnnotations;

namespace ContactManager.Services.ValidationAttributes
{
    public class ValidSalaryAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if ( !decimal.TryParse(value.ToString(), out decimal salary))
            {
                return new ValidationResult("Invalid salary format.");
            }

            if (salary <= 0)
            {
                return new ValidationResult($"Salary must be at least {1:C}.");
            }

            return ValidationResult.Success;
        }
    }

}
