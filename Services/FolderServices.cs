using ImageChecker.Helper;
using ImageChecker.Models.Request;
using ImageChecker.Models.Response;
using ImageChecker.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ImageChecker
{
    public class FolderServices : IFolderServices
    {
        public MovePhotoResponseModel MovePhoto(MovePhotoRequestModel request)
        {
            if (string.IsNullOrEmpty(request.WriteFilePath))
            {
                throw new ArgumentNullException(nameof(request.WriteFilePath), "Value cannot be null.");
            }

            try
            {
                using (new NetworkConnection(request.ReadNetworkName, request.NetworkCredential))
                {
                    using (new NetworkConnection(request.WriteNetworkName, request.NetworkCredential))
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(request.WriteFilePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(request.WriteFilePath));
                            File.Copy(request.SourceFileName, request.DestFileName);
                        }
                        else
                        {
                            if (File.Exists(request.DestFileName))
                            {
                                File.Delete(request.DestFileName);
                            }

                            File.Copy(request.SourceFileName, request.DestFileName);
                        }
                    }
                }

                return new MovePhotoResponseModel
                {
                    IsSuccess = true
                };
            }
            catch (Exception exception)
            {
                return new MovePhotoResponseModel
                {
                    Message = exception.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<MovePhotoResponseModel> MovePhotoAsync(MovePhotoRequestModel request)
        {
            throw new NotImplementedException();
        }

        public DeleteDirectoryResponseModel DeleteDirectory(DeleteDirectoryRequestModel request)
        {
            DeleteDirectoryResponseModel response = new DeleteDirectoryResponseModel()
            {
                IsSuccess = true
            };

            if (!Directory.Exists(request.TargetDirectory))
            {
                return response;
            }

            try
            {
                var files = Directory.GetFiles(request.TargetDirectory);
                var dirs = Directory.GetDirectories(request.TargetDirectory);

                foreach (var file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (var dir in dirs)
                {
                    this.DeleteDirectory(new DeleteDirectoryRequestModel { TargetDirectory = dir });
                }

                Directory.Delete(request.TargetDirectory, false);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<DeleteDirectoryResponseModel> DeleteDirectoryAsync(DeleteDirectoryRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}
