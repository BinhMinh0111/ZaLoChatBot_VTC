using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Workflow
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        [HttpPost]
        public Task SendMessageToUser(long user_id, string text)
        {
            try
            {
                string method = "POST";
                var url = "https://openapi.zalo.me/v2.0/oa/message";
                string aToken = DataHelper.GetToken();
                var data = new
                {
                    recipient = new Messages.Recipient
                    {
                        user_id = user_id
                    },
                    message = new Messages.Message
                    {
                        text = text
                    }
                };
                HttpStatusCode StatusCode;
                HttpHelper.CallAuthJson(url, null, data, aToken, out StatusCode, method, 120000, "access_token");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string error = string.Format("API:SendMessage:SendMessageToUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return Task.CompletedTask;
            }
        }

        //Get messages of specific user
        //ID cuar Binh Minh: 2560249990295819088;
        //ID của OA: 3365848085546568135
        public string follower_messages;
        [HttpGet("/messages/user")]
        public async Task<string> GetMessageFromUser(long user_id, int offset, int count)
        {
            string method = "GET";
            var url = $"https://openapi.zalo.me/v2.0/oa/conversation?data={JsonHelper.Serialize(new { user_id, offset, count })}";
            string aToken = DataHelper.GetToken();
            HttpStatusCode StatusCode;
            follower_messages = HttpHelper.CallAuthJson(url, null, null, aToken, out StatusCode, method, 120000, "access_token");
            return follower_messages;
        }

        [HttpGet("/conversation")]
        public async Task<string> GetConversation(int offset, int count)
        {
            string method = "GET";
            var url = $"https://openapi.zalo.me/v2.0/oa/listrecentchat?data={JsonHelper.Serialize(new { offset, count })}";
            string aToken = DataHelper.GetToken();
            HttpStatusCode StatusCode;
            follower_messages = HttpHelper.CallAuthJson(url, null, null, aToken, out StatusCode, method, 120000, "access_token");
            return follower_messages;
        }
    }
}
