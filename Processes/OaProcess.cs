using System.Collections.Generic;
using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.Processes
{
    public class OaProcess
    {
        private string filePath = Path.GetFullPath("Data\\Messages.txt");
        public void Process(string json)
        {
            var textHolder = DataHelper.OAText(json);
            long user_id = long.Parse(textHolder.id);
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                IsRequest(json);
            }
            else
            {
                DbProcess.AddNewUser(user_id);
                IsRequest(json);
            }
        }
        private void IsRequest(string json)
        {
            var textHolder = DataHelper.OAText(json);
            if (textHolder.text == "#upload")
            {
                KeyValuePair<string, string> requestedUser = new KeyValuePair<string, string>(textHolder.id, DateTime.Now.ToString());
                //Check in file if exist user then delete else write to file
                Dictionary<string, string> userList = GetIds(filePath);
                if(userList.ContainsKey(requestedUser.Key)== true)
                {
                    userList.Remove(requestedUser.Key);
                    userList.TryAdd(requestedUser.Key, requestedUser.Value);
                }
                else 
                {
                    userList.TryAdd(requestedUser.Key, requestedUser.Value);
                }
            }
            else
            {
                LogWriter.LogWrite(string.Format("user ID: {0} \n Message: {1}", textHolder.id, textHolder.text));
            }
        }
        private static Dictionary<string,string> GetIds(string filePath)
        {
            var list = new Dictionary<string,string>();
            try
            {
                list = File.ReadAllLines(filePath).
                    Select(x => x.Split('=')).
                    ToDictionary(x => x[0], x => x[1]);
            }
            catch (Exception e)
            {
                LogWriter.LogWrite(e.Message);
            }
            return list;
        }
    }
}
