using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using ZaloOA_v2.Controllers.BLL.WebhookServices.Workflows;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Workflow
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        public IUsersRepository usersRepository;
        public IPicturesRepository picturesRepository;
        public INoticesRepository noticesRepository;
        public IMessagesRepository messagesRepository;

        public string json = string.Empty;

        public ListenerController(IUsersRepository usersRepository, IPicturesRepository picturesRepository, 
            INoticesRepository noticesRepository, IMessagesRepository messagesRepository)
        {
            this.usersRepository = usersRepository;
            this.picturesRepository = picturesRepository;
            this.noticesRepository = noticesRepository;
            this.messagesRepository = messagesRepository;
        }

        [HttpPost("/listen")]
        public async Task<HttpResponseMessage> Listen()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            using (var reader = new StreamReader(Request.Body))
            {
                json = await reader.ReadToEndAsync();
            }
            LogWriter.LogWrite(json);
            try
            {
                var cancelToken = new CancellationTokenSource(20000).Token;
                Task.Run(() =>
                {
                    EventListenController eventController = 
                    new EventListenController(usersRepository, picturesRepository, noticesRepository, messagesRepository);
                    eventController.Run(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (Exception ex)
            {
                string error = string.Format("API:ListenerController:Listen \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return result;
        }
    }
}
