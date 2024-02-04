using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BuyUsedCars.Models;

namespace BuyUsedCars.ViewModels;

public class EditProfileViewModel
{
    public string Id { get; set; } = null!;
    [RegularExpression(@"^07\d{8}$", ErrorMessage = "Enter a valid phone number. Only Romanian phone numbers allowed.")]
    public string PhoneNumber { get; set; } = null!;
}