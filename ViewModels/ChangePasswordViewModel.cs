using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BuyUsedCars.Models;

namespace BuyUsedCars.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [DisplayName("Old Password")]
    public string OldPassword { get; set; } = null!;
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    [DisplayName("New Password")]
    public string NewPassword { get; set; } = null!;
}