using System.ComponentModel.DataAnnotations;

namespace BuyUsedCars.ViewModels;

public class AddImageViewModel
{
    public string Id { get; set; }
    public IFormFile Image { get; set; } = null!;
    public int CarId { get; set; }
}