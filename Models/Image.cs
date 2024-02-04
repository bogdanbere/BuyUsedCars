namespace BuyUsedCars.Models;

public class Image{
    //[Key]
    public int Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    // [ForeignKey("Car")]
    public int CarId { get; set; }
    
    // Navigation propertiess
    public Car Car { get; set; } = null!;
}