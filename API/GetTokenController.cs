using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;

namespace ZaloOA_v2.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTokenController : ControllerBase
    {
        [HttpPost]
        public string AuthToken(string refresh)
        {
            var client = new RestClient("https://oauth.zaloapp.com/v4/oa/access_token");
            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("secret_key", "5GCe4GWCBvLcd76B1ojL");
            request.AddParameter("refresh_token", refresh);
            request.AddParameter("app_id", "4517810202964705506");
            request.AddParameter("grant_type", "refresh_token");
            RestResponse response = client.Execute(request);          
            return response.Content;
        }
    }
}
