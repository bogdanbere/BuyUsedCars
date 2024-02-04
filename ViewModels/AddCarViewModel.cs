using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BuyUsedCars.Models;
using BuyUsedCars.Models.Enum;

namespace BuyUsedCars.ViewModels;

public class AddCarViewModel
{
    public string UserId { get; set; } = null!;
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public int Km { get; set; }
    public int Year { get; set; }
    [DisplayName("Short Description")]
    public string CardDescription { get; set; } = null!;
    [DisplayName("Description")]
    public string DetailedDescription { get; set; } = null!;
    [DisplayName("Fuel")]
    public FuelCategory FuelCategory { get; set; }
    public IFormFile Image { get; set; } = null!;
}