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
        public async Task<HttpResponseMessage> Listen()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            using (var reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            try
            {
                var cancelToken = new CancellationTokenSource(40000).Token;
                Task.Run(async () =>
                {
                    EventController eventController = new EventController();
                    eventController.Run(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter(ex.Message);
            }           
            return result;
        }
    }
}
