using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using ZaloOA_v2.API;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Workflows
{
    public class MessageController : Controller
    {
        private string[] returnArr = new string[3];
        public async Task<string[]> SendMessage(long userId, string text)
        {
            return await Task.Run(async () =>
            {
                SendMessageController sendMessage = new SendMessageController();
                string response = await sendMessage.SendMessageToUser(userId, text);

                var holder = ObjectsHelper.MessageResponse(response);
                returnArr[0] = holder.error.ToString();
                returnArr[1] = holder.message.ToString();
                returnArr[2] = holder.message_id.ToString();

                return returnArr;
            });

        }
    }
}
