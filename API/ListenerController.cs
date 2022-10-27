using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;

namespace ZaloOA_v2.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        public string json = string.Empty;

        [HttpPost("/listen")]
        public async Task Listen()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            Return200(result);

            using (var reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            Console.WriteLine(json);
            EventController eventController = new EventController();
            eventController.run(json);
        }
        [HttpPost]
        public async Task<HttpResponseMessage> Return200(HttpResponseMessage result)
        {
            return result;
        }
    }
}
