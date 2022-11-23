using System;
using System.Net;
using System.Security.Policy;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.BussinessProcesses.DatabaseProcesses;
using ZaloOA_v2.Processes;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Components
{
    public class PictureServices
    {
        private string filePath = Path.GetFullPath("Data\\Messages.txt");

        public Task Process(string json)
        {
            var picHolder = ObjectsHelper.UserPicture(json);
            long user_id = long.Parse(picHolder.id);
            //Check if user already exist in database
            //If exist then process request
            //If not just save user to DB
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                IsRequest(json);
                return Task.CompletedTask;
            }
            else
            {
                DbProcess.AddNewUser(user_id);
                return Task.CompletedTask;
            }
        }
        private Task IsRequest(string json)
        {
            var picHolder = ObjectsHelper.UserPicture(json);
            string requestedUser = picHolder.id;
            string timeStamp = picHolder.timeStamp;
            DateTime requestedTime = DateTime.Now;
            //Check in file if exist user then delete else write to file
            Dictionary<string, string> userList = DataHelper.GetUsersIds(filePath);
            //Get list of noted request in messages.txt and check for exist user
            if (userList.ContainsKey(requestedUser) == true)
            {
                string oldTimeString = userList[requestedUser];
                DateTime oldTime = DateTime.Parse(oldTimeString);
                TimeSpan timeSpan = requestedTime.Subtract(oldTime);
                var totalMinutes = timeSpan.TotalMinutes;
                //If picture was sent longer than 10m delete the user in message.txt to wait for new request
                if (totalMinutes > 10)
                {
                    userList.Remove(requestedUser);
                    return Task.CompletedTask;
                }
                //else download picture and 
                else
                {
                    string url = picHolder.url.First();
                    string picPath = string.Format("Data\\PicturesFiles\\{0}\\{1}\\", requestedUser, DateTime.Now.ToString("dd_MM_yyyy"));
                    try
                    {
                        var cancelToken = new CancellationTokenSource(10000).Token;
                        Task.Run(() =>
                        {
                            //Download picture and return file path
                            string downloadPath = DownloadPicture(picPath, url, timeStamp);

                            //Save file path if success download picture,
                            //Save pic's url if failed to download
                            savePicPath(json, downloadPath);
                            cancelToken.ThrowIfCancellationRequested();
                        }, cancelToken);
                        return Task.CompletedTask;
                    }
                    catch (Exception ex)
                    {
                        string error = string.Format("Processes:PictureProcess:IsRequest \n {0}", ex.Message);
                        LogWriter.LogWrite(error);
                        return Task.CompletedTask;
                    }
                    return Task.CompletedTask;
                }
            }
            //If not exist in messages.txt file means user did not sent request so just log user messages
            else
            {
                LogWriter.LogWrite(string.Format("User ID: {0} \n Time: {1}", requestedUser, requestedTime.ToString()));
                return Task.CompletedTask;
            }
        }
        private string DownloadPicture(string picPath, string url, string timeStamp)
        {
            //Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            //string time = DateTime.Now.ToString("ss_mm_HH_dd_MM_yyyy");
            //TicksHelper.EncodeTransmissionTimestamp(DateTime.Now)
            string filename = timeStamp + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + ".jpg";
            try
            {
                using (WebClient myWebClient = new WebClient())
                {
                    // Download the image and save it into the PictureFiles folder.
                    byte[] data = myWebClient.DownloadData(url);
                    //If directory not exist, create new
                    if (!Directory.Exists(picPath))
                    {
                        Directory.CreateDirectory(picPath);
                    }
                    File.WriteAllBytes(Path.Combine(picPath, filename), data);
                }
                Console.WriteLine("Done Save");
                string returnPath = Path.Combine(picPath, filename);
                return returnPath;
            }
            catch (IOException ex)
            {
                string error = string.Format("Processes:OAProcess:DownloadPicture \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return "Failed";
            }
        }
        private Task savePicPath(string json, string path)
        {
            if (path == "Failed")
            {
                var cancelToken = new CancellationTokenSource(4000).Token;
                Task.Run(() =>
                {
                    DbProcess.AddPictureUrl(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                return Task.CompletedTask;
            }
            else
            {
                var cancelToken = new CancellationTokenSource(4000).Token;
                Task.Run(() =>
                {
                    DbProcess.AddPath(json, path);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
