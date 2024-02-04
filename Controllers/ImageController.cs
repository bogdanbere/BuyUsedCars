using Microsoft.AspNetCore.Mvc;
using BuyUsedCars.Data;
using BuyUsedCars.Models;
using BuyUsedCars.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BuyUsedCars.Services;

namespace BuyUsedCars.Controllers;

[Authorize]
public class ImageController : Controller
{
    private readonly BuyUsedCarsDbContext _context;
    private readonly IPhotoService _photoService;
    public ImageController(BuyUsedCarsDbContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    // Send user to remove Images or add Image form and fetch all iamges by car id
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> EditImages(int id)
    {
        // Get all images by car id
        IEnumerable<Image>? imagesList = await _context.Images.Where(i => i.CarId == id).ToListAsync();
        var car = await _context.Cars.Where(c => c.Id == id).Select(c => new { Make = c.Make, Model = c.Model}).SingleOrDefaultAsync();
        EditImagesViewModel imagesVM = new();

        if(car is not null){
            imagesVM.CarId = id;
            imagesVM.ImagesList = imagesList;
            imagesVM.Make = car.Make;
            imagesVM.Model = car.Model;
        }
        else{
            imagesVM = null!;
        }

        // The id is the car id from Image model
        // Initializes imagesVM object with the images list and the id

        return View(imagesVM);
    }

    // Redirect to delete confirmation
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> RemoveImage(int id)
    {
        Image image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);

        if(image is null) return View("Error");

        return View(image);

    }

    // Delete image by id
    [Authorize]
    [HttpPost, ActionName("RemoveImage")]
    public async Task<IActionResult> Remove(int id)
    {
        // Gets the image to be deleted
        Image image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);

        if(image is null) return View("Error");

        // Removes image from cloudinary and from db
        await _photoService.DeletePhotoAsync(image.ImageUrl);
        _context.Remove(image);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    // Redirect to Create form and fill the CarId with the id of the car you want toa dd iamge to
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> AddImage(int id)
    {
        AddImageViewModel imageVM = new(){CarId = id};
        return View(imageVM);
    }

    // Form to handle adding of images
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddImage(int id, AddImageViewModel imageVM)
    {
        if(!ModelState.IsValid){
            ModelState.AddModelError("", "Failed to add image");
            return View("Error");
        }

               var result = await _photoService.AddPhotoAsync(imageVM.Image);

        Image image = new(){
            ImageUrl = result.Url.ToString(),
            CarId = id
        };

        _context.Add(image);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }
}