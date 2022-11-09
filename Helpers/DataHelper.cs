using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using ZaloOA_v2.API;
using ZaloOA_v2.Models;
namespace ZaloOA_v2.Helpers
{
    public class DataHelper
    {
        public static string GetToken()
        {
            Token token = new Token();
            string filePath = Path.GetFullPath("Data\\Token.txt");
            string[] lines = GetAllTokens(filePath);

            //Check if access token is still valid
            if (IsRenewToken(lines))
            {
                //if valid return semaphore 1 and access token
                return lines[0];
            }
            else
            {
                //Call API to get new token
                GetTokenController getTokenController = new GetTokenController();
                token = JsonHelper.Deserialize<Token>(getTokenController.AuthToken(lines[1]));
                //Write new token and refresh token into file
                WriteToken(filePath, token.access_token, token.refresh_token);
                //Get token again and return value
                string[] lines1 = GetAllTokens(filePath);
                return lines1[0];
            }
        }
        //Logic if token expired yet
        private static bool IsRenewToken(string[] lines)
        {
            DateTime oldTime = DateTime.Parse(lines[2]);
            DateTime nowTime = DateTime.Now;
            if ((nowTime.Subtract(oldTime).TotalSeconds) > 90000)
                return false;
            return true;
        }
        //Get token and refresh code
        private static string[] GetAllTokens(string filePath)
        {
            var list = new List<string>();
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(filePath))
                {
                    // Read the stream as a string, and write the string to the file
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
            }
            catch (IOException e)
            {
                LogWriter.LogWrite(e.Message);
            }
            //List to array and trim blanks
            string[] _return = list.ToArray();
            _return = _return.Where(x => x != null).ToArray();
            return _return;
        }
        //Write token package into file
        public static void WriteToken(string filePath, string token, string refresh)
        {           
            string timeStamp = DateTime.Now.ToString();
            string[] lines = { token, refresh, timeStamp };
            using (StreamWriter streamWriter = new StreamWriter(path: filePath))
            {
                foreach (string line in lines)
                {
                    streamWriter.WriteLine(line);                
                }
            }
        }
        //Get IDs for Picture process
        public static Dictionary<string, string> GetUsersIds (string filePath)
        {
            var list = new Dictionary<string, string>();
            try
            {
                list = File.ReadAllLines(filePath).
                    Select(x => x.Split(',')).
                    ToDictionary(x => x[0], x => x[1]);
            }
            catch (Exception e)
            {
                LogWriter.LogWrite(e.Message);
            }
            return list;
        }
        //Update dictionary into Messages.txt
        public static void WriteUsers (string filePath, Dictionary<string, string> userList)
        {
            File.WriteAllLines(filePath,
                userList.Select(x => $"{x.Key},{x.Value}"));
        }
    }
}
