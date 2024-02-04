using System.ComponentModel.DataAnnotations;

namespace BuyUsedCars.Helpers;

public class Validation
{
    public static ValidationResult ValidateYear(decimal value, ValidationContext context){
        bool isValid = true;
        int year = DateTime.Today.Year;

        if (value < 1920 && value > year){
            isValid = false;
        }

        if(isValid){
            return ValidationResult.Success!;
        }
        else{
            return new ValidationResult(
                string.Format("The filed {0} must be greater than 1920 and lower than {1}.", context.MemberName, year),
                new List<string>() {context.MemberName!});
        }
    }
}