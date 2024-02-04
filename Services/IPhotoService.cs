using CloudinaryDotNet.Actions;

namespace BuyUsedCars.Services;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
    Task<DeletionResult> DeletePhotoAsync(string photoUrl);
}