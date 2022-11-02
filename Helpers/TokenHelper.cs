using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using ZaloOA_v2.API;
namespace ZaloOA_v2.Helpers
{
    public class TokenHelper
    {
        public class Token
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string expires_in { get; set; }
        }

        public static string GetToken()
        {
            Token token = new Token();
            string filePath = Path.GetFullPath("Data\\Token.txt");
            string[] lines = GetAll(filePath);

            //Check if access token is still valid
            if (IsRenew(lines))
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
                Write(filePath, token.access_token, token.refresh_token);
                //Get token again and return value
                string[] lines1 = GetAll(filePath);
                return lines1[0];
            }
        }
        //Logic if token expired yet
        private static bool IsRenew(string[] lines)
        {
            DateTime oldTime = DateTime.Parse(lines[2]);
            DateTime nowTime = DateTime.Now;
            if ((nowTime.Subtract(oldTime).TotalSeconds) > 90000)
                return false;
            return true;
        }
        //Get token and refresh code
        private static string[] GetAll(string filePath)
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
                        Log.Information("token: "+line);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
            //List to array and trim blanks
            string[] _return = list.ToArray();
            _return = _return.Where(x => x != null).ToArray();
            return _return;
        }
        //Write token package into file
        public static void Write(string filePath, string token, string refresh)
        {           
            string timeStamp = DateTime.Now.ToString();
            string[] lines = { token, refresh, timeStamp };
            using (StreamWriter streamWriter = new StreamWriter(path: filePath))
            {
                foreach (string line in lines)
                {
                    streamWriter.WriteLine(line);
                    LogWriter log = new LogWriter(line);                    
                }
            }
        }
    }
}
