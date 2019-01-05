using ImageChecker.Extensions;
using ImageChecker.Services;
using Models.Request;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Threading;

namespace ImageChecker
{
    public class ImageServices : IImageServices
    {
        public bool ImageCheck(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentNullException(nameof(imagePath), "Value cannot be null.");
            }

            using (var countdownEvent = new CountdownEvent(1))
            {
                var callBackResult = new ConcurrentQueue<Tuple<string, bool>>();
                Action<string, bool> callback = (url, valid) =>
                {
                    callBackResult.Enqueue(Tuple.Create(url, valid));
                    countdownEvent.Signal();
                };

                ValidateUrlAsync(imagePath, callback);

                countdownEvent.Wait();

                return callBackResult.FirstOrDefault().Item2;
            }
        }

        public List<Tuple<string, bool>> ImageCheck(List<string> imagePaths, out List<string> notFoundedUrls)
        {
            if (!imagePaths.Any())
            {
                throw new ArgumentNullException(nameof(imagePaths), "Value cannot be null.");
            }

            using (var countdownEvent = new CountdownEvent(imagePaths.Count))
            {
                var callBackResult = new ConcurrentQueue<Tuple<string, bool>>();
                Action<string, bool> callback = (url, valid) =>
                {
                    callBackResult.Enqueue(Tuple.Create(url, valid));
                    countdownEvent.Signal();
                };

                foreach (var url in imagePaths)
                {
                    ValidateUrlAsync(url, callback);
                }

                countdownEvent.Wait();

                var concurrentNotFoundedUrls = callBackResult.Where(p => p.Item2 == false).ToList();
                notFoundedUrls = (from n in concurrentNotFoundedUrls select n.Item1).ToList();
                return callBackResult.ToList();
            }
        }

        public int CompareWith(ImageCompareRequestModel request)
        {
            var sourceBitmap = FolderExtensions.GetBitmap(request.Source.FileName, request.Source.NetworkName, request.Source.NetworkCredential);
            var targetBitmap = FolderExtensions.GetBitmap(request.Target.FileName, request.Target.NetworkName, request.Target.NetworkCredential);

            return Comparisons(sourceBitmap, targetBitmap);
        }

        public int CompareWith(Bitmap source, Bitmap target)
        {
            return Comparisons(source, target);
        }

        #region Private Methods

        private void ValidateUrlAsync(string imagePath, Action<string, bool> callback)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(imagePath);

            try
            {
                webRequest.BeginGetResponse(asyncResult =>
                {
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)webRequest.EndGetResponse(asyncResult);
                        callback(imagePath, response.StatusCode == HttpStatusCode.OK);
                    }
                    catch
                    {
                        callback(imagePath, false);
                    }
                    finally
                    {
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                }, null);
            }
            catch
            {
                callback(imagePath, false);
            }
        }

        private unsafe int Comparisons(Bitmap source, Bitmap target)
        {
            int percentageByDifference = 0;

            if (source.Width != target.Width)
                return percentageByDifference;

            if (source.Height != target.Height)
                return percentageByDifference;

            var sourceLockBits = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
            var targetLockBits = target.LockBits(new Rectangle(0, 0, target.Width, target.Height), ImageLockMode.ReadOnly, target.PixelFormat);

            float difference = 0;

            try
            {
                byte sourceBitsPerPixel = (byte)Image.GetPixelFormatSize(sourceLockBits.PixelFormat);
                byte targetBitsPerPixel = (byte)Image.GetPixelFormatSize(targetLockBits.PixelFormat);

                byte* sourcePointer = (byte*)sourceLockBits.Scan0.ToPointer();
                byte* targetPointer = (byte*)targetLockBits.Scan0.ToPointer();

                for (var i = 0; i < sourceLockBits.Height; ++i)
                {
                    for (var j = 0; j < sourceLockBits.Width; ++j)
                    {
                        byte* sourceData = sourcePointer + (i * sourceLockBits.Stride) + (j * (sourceBitsPerPixel / 8));
                        byte* targetData = targetPointer + (i * targetLockBits.Stride) + (j * (targetBitsPerPixel / 8));

                        var sourceR = sourceData[0];
                        var sourceG = sourceData[1];
                        var sourceB = sourceData[2];
                        var targetR = targetData[0];
                        var targetG = targetData[1];
                        var targetB = targetData[2];

                        difference += (float)Math.Abs(sourceR - targetR) / 255;
                        difference += (float)Math.Abs(sourceG - targetG) / 255;
                        difference += (float)Math.Abs(sourceB - targetB) / 255;
                    }
                }
            }
            catch (Exception ex)
            {
                percentageByDifference = 0;
            }
            finally
            {
                source.UnlockBits(sourceLockBits);
                target.UnlockBits(targetLockBits);
            }

            return percentageByDifference;
        }

        #endregion
    }
}
