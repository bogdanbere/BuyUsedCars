using BuyUsedCars.Models;
using BuyUsedCars.Models.Enum;

namespace BuyUsedCars.ViewModels;

public class CarByIdViewModel
{
    public int Id { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public decimal Price { get; set; }
    public int Km { get; set; }
    public int Year { get; set; }
    public string DetailedDescription { get; set; } = null!;
    public FuelCategory FuelCategory { get; set; }
    public IEnumerable<Image> ImagesList { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}