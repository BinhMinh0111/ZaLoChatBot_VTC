using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using ZaloOA_v2.Models.DatabaseModels;
using ZaloOA_v2.API;

namespace ZaloOA_v2.Controllers
{
    public class ReplyController : Controller
    {
        public static void SendMessage (long userId,string text)
        {
            SendMessageController sendMessage = new SendMessageController();
            sendMessage.SendMessageToUser(userId, text);
        }
    }
}
