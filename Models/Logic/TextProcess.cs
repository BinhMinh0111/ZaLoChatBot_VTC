using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.Models.Logic
{
    public class TextProcess
    {
        public void Process(string json)
        {
            var textHolder = DataHelper.UserText(json);
            long user_id = long.Parse(textHolder.id);
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                Console.WriteLine();
                //DbProcess.AddText(json);
            }
            else
            {
                Console.WriteLine();
                //DbProcess.AddNewUser(user_id);
                //DbProcess.AddText(json);
            }
        }
        public void IsRequest(string json)
        {
            var textHolder = DataHelper.UserText(json);
            if(textHolder.text == "#upload")
            {
                KeyValuePair<string,string> requestUser = new KeyValuePair<string,string>(textHolder.id,DateTime.Now.ToString());
                //Check in file if exist user then delete else write to file
            }   
            else
            {
                LogWriter.LogWrite(string.Format("user ID: {0} \n Message: {1}", textHolder.id, textHolder.text));
            }    
        }
        public static void Reply(long userId, string text)
        {
            try
            {
                var cancelToken = new CancellationTokenSource(3000).Token;
                Task.Run(async () =>
                {
                    ReplyController.SendMessage(userId, text);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message);
            }
        }
    }
}
