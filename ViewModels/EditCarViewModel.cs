using System.ComponentModel.DataAnnotations;
using BuyUsedCars.Helpers;
using BuyUsedCars.Models;
using BuyUsedCars.Models.Enum;

namespace BuyUsedCars.ViewModels;

public class EditCarViewModel
{
    public int Id { get; set; }
    [Required]
    public string Make { get; set; } = null!;
    [Required]
    public string Model { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Km { get; set; }
    [Required]
    [CustomValidation(typeof(Validation), "ValidateYear")]
    public int Year { get; set; }
    public string DetailedDescription { get; set; } = null!;
    [StringLength(20)]
    public string CardDescription { get; set; } = null!;
    [Required]
    public FuelCategory FuelCategory { get; set; }
}