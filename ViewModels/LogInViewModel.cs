using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuyUsedCars.ViewModels;

public class LogInViewModel
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    [DisplayName("Email Address")]
    public string EmailAddress { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}