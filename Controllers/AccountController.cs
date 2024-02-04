using BuyUsedCars.Data;
using BuyUsedCars.Models;
using BuyUsedCars.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BuyUsedCars.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly BuyUsedCarsDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(BuyUsedCarsDbContext context, SignInManager<User> signInManager, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    // Send user to login form
    [HttpGet]
    public IActionResult LogIn()
    {
        var loginVM = new LogInViewModel();
        return View(loginVM);
    }

    // Handle login
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel logInVM)
    {
        if (!ModelState.IsValid) return View(logInVM);

        var user = await _userManager.FindByEmailAsync(logInVM.EmailAddress);

        if(user is not null)
        {
            // User exists, check password
            var passwordChecker = await _userManager.CheckPasswordAsync(user, logInVM.Password);

            if(passwordChecker)
            {
                // Password is correct
                var result = await _signInManager.PasswordSignInAsync(user, logInVM.Password, false, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
               
            }
            // Password incorrect
            @TempData["Error"] = "Wrong username or password. Try again.";
            return View(logInVM);
        }
        // User does not exist
        @TempData["Error"] = "Wrong username or password. Try again.";
        return View(logInVM);
    }

    // Send user to register form
    [HttpGet]
    public IActionResult Register()
    {
        var registerVM = new RegisterViewModel();
        return View(registerVM);
    }

    // Handle register form
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM)
    {
        if(!ModelState.IsValid) return View(registerVM);

        // Checks if the email is already in use
        var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
        if(user is not null)
        {
            TempData["Error"] = "This email is already in use";
            return View(registerVM);
        }

        // If email is not in use, creates new user
        var newUser = new User()
        {
            Email = registerVM.EmailAddress,
            UserName = registerVM.UserName,
            PhoneNumber = registerVM.PhoneNumber,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };
        var newUserResult = await _userManager.CreateAsync(newUser, registerVM.Password);

        if(newUserResult.Succeeded) await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    public async Task <IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // View all the user posted cars
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        // Get User Id as string
        var userId = _userManager.GetUserId(this.User);

        // Return only the cars which belong to the user
        var userCars = _context.Cars.Where(c => c.UserId == userId);

        // Join the user cars and Image tables based on the CarId foreign key
        var cars = await userCars.Join(_context.Images, c => c.Id, i => i.CarId, (c, i) => new { c, i }).ToListAsync();

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

        var user = await _userManager.GetUserAsync(HttpContext.User);

        ViewData["User"] = user.UserName;

        return View(carsWithDistinctImagesList);
    }

    
    // Sends the user to the edit profile form
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        // Returns the current user
        var user = await _userManager.GetUserAsync(HttpContext.User);

        // Checks if user exists
        if(user is null) {
            @TempData["Error"] = "User does not exist";
            return View("EditProfile");
        }
        EditProfileViewModel profileVM = new(){
            Id = user.Id,
            PhoneNumber = user.PhoneNumber
        };
        return View(profileVM);
    }

    // Handles edit profile requests
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileViewModel profileVM)
    {
        // Checks if user exists
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if(user is null) return View("Error");

        // Checks if model is valid
        if(!ModelState.IsValid) {
            ModelState.AddModelError("", "Failed to edit profile");
            return View("EditProfile", profileVM);
        }
        
            user.Id = profileVM.Id;
            user.PhoneNumber = profileVM.PhoneNumber;

        await _userManager.UpdateAsync(user);
        _context.SaveChanges();
        return RedirectToAction("Dashboard");
    }

    // Sends user to edit password form
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        ChangePasswordViewModel passVM = new();
        return View(passVM);
    }

    // Handles password edit forms
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel passVM)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        // Returns to the same view to show errors
        if(!ModelState.IsValid) return View(passVM);

        // Changes password if user is found
        await _userManager.ChangePasswordAsync(user, passVM.OldPassword, passVM.NewPassword);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }
}