using Microsoft.AspNetCore.Identity;

namespace BuyUsedCars.Models;

public class User : IdentityUser
{
    // Navigation Property
    public ICollection<Car> Cars { get; set; } = null!;
}