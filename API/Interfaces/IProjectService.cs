using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IProjectService
    {
        Task<UploadResult> AddProjectAsync(IFormFile file);
        Task<DeletionResult> DeleteProjectAsync(string publicId);
    }
}