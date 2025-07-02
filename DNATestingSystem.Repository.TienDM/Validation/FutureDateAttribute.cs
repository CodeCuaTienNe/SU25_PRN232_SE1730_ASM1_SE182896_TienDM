using System.ComponentModel.DataAnnotations;

namespace DNATestingSystem.Repository.TienDM.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateOnly date)
            {
                return date >= DateOnly.FromDateTime(DateTime.Today);
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The {name} must be today or a future date.";
        }
    }
}
