using BuyUsedCars.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyUsedCars.Models;

public class Car{
    [Key]
    public int Id { get; set; }
    public string Make { get; set; } = null!;
    public string Model { get; set; } = null!;
    [Column(TypeName = "money")]
    public decimal Price { get; set; }
    public int Km { get; set; }
    public int Year { get; set; }
    [StringLength(20)]
    public string CardDescription { get; set; } = null!;
    public string DetailedDescription { get; set; } = null!;
    public FuelCategory FuelCategory { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    // Navigation properties
    public IEnumerable<Image> ImagesList { get; set; } = null!;
}