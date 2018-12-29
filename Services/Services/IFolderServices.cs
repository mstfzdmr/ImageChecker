using ImageChecker.Models.Request;
using ImageChecker.Models.Response;
using System.Threading.Tasks;

namespace ImageChecker.Services
{
    public interface IFolderServices
    {
        MovePhotoResponseModel MovePhoto(MovePhotoRequestModel request);

        Task<MovePhotoResponseModel> MovePhotoAsync(MovePhotoRequestModel request);

        DeleteDirectoryResponseModel DeleteDirectory(DeleteDirectoryRequestModel request);
        
        Task<DeleteDirectoryResponseModel> DeleteDirectoryAsync(DeleteDirectoryRequestModel request);
    }
}
