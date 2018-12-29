using ImageChecker.Models.Request;
using ImageChecker.Models.Response;

namespace ImageChecker.Services
{
    public interface IFolderServices
    {
        MovePhotoResponseModel MovePhoto(MovePhotoRequestModel request);
        DeleteDirectoryResponseModel DeleteDirectory(DeleteDirectoryRequestModel request);
    }
}
