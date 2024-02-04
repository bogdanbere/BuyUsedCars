using BuyUsedCars.Models;

namespace BuyUsedCars.ViewModels;

public class EditImagesViewModel
{
    public int CarId { get; set; }
    public IEnumerable<Image> ImagesList { get; set; } = null!;
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
}