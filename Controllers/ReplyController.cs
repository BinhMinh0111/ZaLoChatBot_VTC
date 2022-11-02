using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using ZaloOA_v2.API;

namespace ZaloOA_v2.Controllers
{
    public class ReplyController : Controller
    {
        public void MessagesProcess (long userId,string text)
        {
            string displayName = string.Empty;
            using (var context = new db_a8ebff_kenjenorContext())
            {
                Console.WriteLine("Start query");
                try
                {
                    var users = context.ZaloUsers;
                    foreach( var user in users)
                    {
                        if (userId == user.UserId)
                            displayName = user.DisplayName;
                    }    
                    Console.WriteLine("Done query");
                    Console.WriteLine(displayName);
                    text = "Hello " + displayName;
                    SendMessageController sendMessage = new SendMessageController();
                    sendMessage.SendMessageToUser(userId, text);
                    Console.WriteLine("Sent");
                }
                catch (SqlException ex)
                {
                    LogWriter log = new LogWriter(ex.Message);
                    Console.WriteLine("Can't send");
                }
            }
        }
    }
}
