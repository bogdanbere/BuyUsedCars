using BuyUsedCars.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BuyUsedCars.Data;

public class BuyUsedCarsDbContext : IdentityDbContext<User>
{
    public BuyUsedCarsDbContext(DbContextOptions<BuyUsedCarsDbContext> options) : base(options){}

    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}