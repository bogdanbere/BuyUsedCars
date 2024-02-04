using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BuyUsedCars.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress]
    [DisplayName("Email Address")]
    public string EmailAddress { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password do not match.")]
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;
    [Required(ErrorMessage = "User Name is required.")]
    [DisplayName("User Name")]
    public string UserName { get; set; } = null!;
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^07\d{8}$", ErrorMessage = "Enter a valid phone number. Only Romanian phone numbers allowed.")]
    [DisplayName("Phone Number")]
    public string PhoneNumber { get; set; } = null!;
}