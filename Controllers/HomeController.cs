using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuyUsedCars.Models;
using BuyUsedCars.Data;
using BuyUsedCars.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BuyUsedCars.Services;

namespace BuyUsedCars.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly BuyUsedCarsDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    public HomeController(BuyUsedCarsDbContext context, IPhotoService photoService, SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _photoService = photoService;
        _signInManager = signInManager;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    // Get all cars
    // Home page
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Join the Car and Image tables based on the CarId foreign key
        var cars = await _context.Cars.Join(_context.Images, c => c.Id, i => i.CarId, (c, i) => new { c, i }).ToListAsync();

        // Return only the first image for each car and ensure that there are no car duplicates
        var orderedDistinctCarImages = cars.DistinctBy(ci => ci.c.Id).OrderBy(ci => ci.i.Id);

        // Select the corresponding car details and image URL for each distinct car
        var carsWithDistinctImagesList = orderedDistinctCarImages.Select(ci => new CarsAndImagesViewModel
        {
            CarId = ci.c.Id,
            Make = ci.c.Make,
            Model = ci.c.Model,
            CardDescription = ci.c.CardDescription,
            ImageUrl = ci.i.ImageUrl
        }).ToList();

        return View(carsWithDistinctImagesList);
    }

    // Get car by id
    [HttpGet]
    [Route("/detail/{id}")]
    public async Task<IActionResult> Detail(int id)
    {
        // Selects all cars that match the id
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

        // Check if car does not exist
        if(car is null) return View("Error");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == car.UserId);

        // Selects all images that match the id
        var image = await _context.Images.Where(i => i.CarId == id).ToListAsync();

        // Creates new view model with selected properties and returns it to the View
        CarByIdViewModel carVM = new(){
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Price = car.Price,
            Km = car.Km,
            Year = car.Year,
            DetailedDescription = car.DetailedDescription,
            FuelCategory = car.FuelCategory,
            ImagesList = image,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber
        };

        return View(carVM);
    }

    // Send user to edit page and fetch the model
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        // Selects the car that matches the id
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

        // Check if car does not exist
        if(car is null) return View("Error");

        // Creates new view model with selected properties and returns it to the View
        EditCarViewModel carVM = new(){
            Id = id,
            Make = car.Make,
            Model = car.Model,
            Price = car.Price,
            Km = car.Km,
            Year = car.Year,
            DetailedDescription = car.DetailedDescription,
            FuelCategory = car.FuelCategory
        };

        return View(carVM);
    }

    [Authorize]
    [HttpGet]
    public IActionResult EditImages(EditCarViewModel carVM)
    {
        return RedirectToAction("EditImages", "Image", new {id = carVM.Id});
    }

    // Handle form sumbit
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditCarViewModel carVM)
    {
        // Gets user
        var user = await _userManager.GetUserAsync(HttpContext.User);
        // Checks if the model is valid
        if(!ModelState.IsValid){
            ModelState.AddModelError("", "Failed to edit car");
            return View("Edit", carVM);
        }

        // Initializes a new Car object using the properties of carVM
        Car car = new(){
            Id = id,
            Make = carVM.Make,
            Model = carVM.Model,
            Price = carVM.Price,
            Km = carVM.Km,
            Year = carVM.Year,
            DetailedDescription = carVM.DetailedDescription,
            CardDescription = carVM.CardDescription,
            FuelCategory = carVM.FuelCategory,
            UserId = user.Id
        };

        // Updates the db and closes the connection
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();

        // Redirects to homepage
        return RedirectToAction("Index");
    }

    // Sends user to the form which adds a car
    [HttpGet]
    public IActionResult AddCar()
    {
        var userId = _userManager.GetUserId(this.User);
        AddCarViewModel carVM = new(){UserId = userId};
        return View(carVM);
    }

    // Handles car adding
    [HttpPost]
    public async Task<IActionResult> AddCar(AddCarViewModel carVM)
    {
        if(!ModelState.IsValid){
            ModelState.AddModelError("", "Failed to edit car");
            return View("Error");
        }

        Car car = new(){
            Make = carVM.Make,
            Model = carVM.Model,
            Price = carVM.Price,
            Km = carVM.Km,
            Year = carVM.Year,
            CardDescription = carVM.CardDescription,
            DetailedDescription = carVM.DetailedDescription,
            FuelCategory = carVM.FuelCategory,
            UserId = carVM.UserId
        };

        _context.Add(car);
        await _context.SaveChangesAsync();
        var result = await _photoService.AddPhotoAsync(carVM.Image);

        Image image = new(){
            CarId = car.Id,
            ImageUrl = result.Url.ToString()
        };

        _context.Add(image);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // Sends to the remove car form
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> RemoveCar(int id)
    {
        Car? car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

        if(car is null) return View("Error");

        return View(car);
    }

    // Handle Car removal
    [Authorize]
    [HttpPost, ActionName("RemoveCar")]
    public async Task<IActionResult> Remove(int id)
    {
        Car? car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);

        if(car is null) return View("Error");

        _context.Remove(car);
        _context.SaveChanges();
        return RedirectToAction("Index");       
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
