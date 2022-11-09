using System;
using System.Net;
using System.Security.Policy;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.Processes
{
    public class PictureProcess
    {
        private string filePath = Path.GetFullPath("Data\\Messages.txt");
        public Task Process(string json)
        {
            var picHolder = ObjectsHelper.UserPicture(json);
            long user_id = long.Parse(picHolder.id);
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
            DateTime requestedTime = DateTime.Now;
            //Check in file if exist user then delete else write to file
            Dictionary<string, string> userList = DataHelper.GetUsersIds(filePath);

            if (userList.ContainsKey(requestedUser) == true)
            {
                string oldTimeString = userList[requestedUser];
                DateTime oldTime = DateTime.Parse(oldTimeString);
                TimeSpan timeSpan = requestedTime.Subtract(oldTime);
                var totalMinutes = timeSpan.TotalMinutes;

                if (totalMinutes > 10)
                {
                    userList.Remove(requestedUser);
                    return Task.CompletedTask;
                }
                else
                {                   
                    string url = picHolder.url.First();
                    string picPath = (string.Format("PicturesFiles\\{0}\\{1}\\", requestedUser,DateTime.Now.ToString("dd_MM_yyyy")));
                    try 
                    {
                        var cancelToken = new CancellationTokenSource(10000).Token;
                        Task.Run(() =>
                        {
                            //Download picture and return file path
                            string downloadPath = DownloadPicture(picPath, url);

                            //Save file path if success, pic url if failed
                            savePicPath(json, downloadPath);
                            cancelToken.ThrowIfCancellationRequested();
                        }, cancelToken);                 
                        return Task.CompletedTask;
                    }
                    catch(Exception ex)
                    {
                        LogWriter.LogWrite(ex.Message);
                        return Task.CompletedTask;
                    }
                    return Task.CompletedTask;
                }
            }
            else
            {
                //userList.TryAdd(requestedUser, requestedTime.ToString());
                //DataHelper.WriteUsers(filePath, userList);
                LogWriter.LogWrite(string.Format("User ID: {0} \n Time: {1}", requestedUser, requestedTime.ToString()));
                return Task.CompletedTask;
            }
        }
        private string DownloadPicture (string picPath, string url)
        {
            Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            //string time = DateTime.Now.ToString("ss_mm_HH_dd_MM_yyyy");           
            string filename = unixTimestamp + "_" + TicksHelper.EncodeTransmissionTimestamp(DateTime.Now) + ".jpg";
            try
            {
                using (WebClient myWebClient = new WebClient())
                {
                    // Download the Web resource and save it into the filesystem folder.
                    byte[] data = myWebClient.DownloadData(url);

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
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }
        private Task savePicPath (string json,string path)
        {
            if(path == "Failed")
            {
                var cancelToken = new CancellationTokenSource(4000).Token;
                Task.Run(() =>
                {
                    DbProcess.AddPicture(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                return Task.CompletedTask;
            }
            else
            {
                var cancelToken = new CancellationTokenSource(4000).Token;
                Task.Run(() =>
                {
                    DbProcess.AddPath(json,path);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                return Task.CompletedTask;
            }    
            return Task.CompletedTask;
        }
    }
}
