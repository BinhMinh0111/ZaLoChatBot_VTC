using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Controllers.BLL.WebhookServices.Components;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Workflows
{
    public class EventListenController : Controller
    {
        public IUsersRepository usersRepository;
        public IPicturesRepository picturesRepository;
        public INoticesRepository noticesRepository;
        public IMessagesRepository messagesRepository;

        public EventListenController(IUsersRepository usersRepository, IPicturesRepository picturesRepository, 
            INoticesRepository noticesRepository, IMessagesRepository messagesRepository)
        {
            this.usersRepository = usersRepository;
            this.picturesRepository = picturesRepository;
            this.noticesRepository = noticesRepository;
            this.messagesRepository = messagesRepository;
        }

        public void Run(string json)
        {
            //Read and process events
            var eventHolder = ObjectsHelper.Events(json);
            TextServices textProcess = new TextServices();
            PictureServices pictureProcess = new PictureServices();
            OaServices oaProcess = new OaServices();
            OaFollowServices followProcess = new OaFollowServices(usersRepository);
            ReceivedServices received = new ReceivedServices();

            if (eventHolder.event_name == "user_send_text")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    textProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "user_send_image")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    pictureProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);

            }
            else if (eventHolder.event_name == "oa_send_text")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    oaProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "follow" || eventHolder.event_name == "unfollow")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    followProcess.Process(json, eventHolder.event_name);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "user_received_message")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    received.UserReceived(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "user_seen_message")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    received.UserSeen(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else
            {
                //ghi log even + timestamp
                LogWriter.LogWrite("New event: " + eventHolder.event_name);
            }

        }

    }
}
