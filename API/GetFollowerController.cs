using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetFollowerController : ControllerBase
    {
        //Get followe info
        public string follower_details;
        [HttpGet("/details")]
        public async Task<string> Get_follower_detail(long user_id)
        {
            string method = "GET";
            var url = $"https://openapi.zalo.me/v2.0/oa/getprofile?data={JsonHelper.Serialize(new { user_id })}";
            string aToken = TokenHelper.GetToken();
            HttpStatusCode StatusCode;
            follower_details = HttpHelper.CallAuthJson(url, null, null, aToken,
                out StatusCode, method, 120000, "access_token");
            return follower_details;
        }
    }
}
