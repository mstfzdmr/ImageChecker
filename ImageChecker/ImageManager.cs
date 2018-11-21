using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace ImageChecker
{
    public class ImageManager : IImageManager
    {
        public bool ImageCheck(string imagePath)
        {
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
                notFoundedUrls = new List<string>();
                return null;
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
    }
}
