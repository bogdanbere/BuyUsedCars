using BuyUsedCars.Models;

namespace BuyUsedCars.ViewModels;

public class CarsAndImagesViewModel
{
    public int CarId { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string CardDescription { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}