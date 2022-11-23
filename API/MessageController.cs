using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ZaloOA_v2.Controllers.BLL.WebhookServices.Workflow;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.API
{
    public class MessageController : Controller
    {
        public Task SendMessage(long userId, string text)
        {
            Console.WriteLine(userId + " Sending ");
            SendMessageController sendMessage = new SendMessageController();
            sendMessage.SendMessageToUser(userId, text);
            Console.WriteLine(userId + " Sent!");
            return Task.CompletedTask;
        }
    }
}
