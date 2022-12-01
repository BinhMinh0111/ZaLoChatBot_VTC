using System.Collections.Generic;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAL.Repositories;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Components
{
    public class OaServices
    {
        private string filePath = Path.GetFullPath("Data\\Messages.txt");
        public Task Process(string json)
        {
            var textHolder = ObjectsHelper.OAText(json);
            long user_id = long.Parse(textHolder.id);
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                IsRequest(json);
                return Task.CompletedTask;
            }
            else
            {
                DbProcess.AddNewUser(user_id);
                IsRequest(json);
                return Task.CompletedTask;
            }
        }
        private Task IsRequest(string json)
        {
            var textHolder = ObjectsHelper.OAText(json);
            if (textHolder.text == "#upload" || textHolder.text == "Xin vui lòng gửi ảnh của bạn.")
            {
                KeyValuePair<string, string> requestedUser = new KeyValuePair<string, string>(textHolder.id, DateTime.Now.ToString());
                //Check in file if exist user then delete else write to file
                Dictionary<string, string> userList = DataHelper.GetUsersIds(filePath);
                if (userList.ContainsKey(requestedUser.Key) == true)
                {
                    try
                    {
                        userList.Remove(requestedUser.Key);
                        userList.TryAdd(requestedUser.Key, requestedUser.Value);
                        DataHelper.WriteUsers(filePath, userList);
                        return Task.CompletedTask;
                    }
                    catch (Exception ex)
                    {
                        string error = string.Format("Processes:OAProcess:IsRequest \n {0}", ex.Message);
                        LogWriter.LogWrite(error);
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    userList.TryAdd(requestedUser.Key, requestedUser.Value);
                    DataHelper.WriteUsers(filePath, userList);
                    return Task.CompletedTask;
                }
            }
            else
            {
                //LogWriter.LogWrite(string.Format("user ID: {0} \n Message: {1}", textHolder.id, textHolder.text));
                return Task.CompletedTask;
            }
        }
    }
}
