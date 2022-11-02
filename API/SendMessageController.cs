using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;

namespace ZaloOA_v2.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        [HttpPost]
        public async Task SendMessageToUser(long user_id, string text)
        {
            try 
            {
                string method = "POST";
                var url = "https://openapi.zalo.me/v2.0/oa/message";
                string aToken = TokenHelper.GetToken();
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
            }
            catch (Exception ex)
            {
                LogWriter log = new LogWriter(ex.Message);
            }           
        }
    }
}
