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
using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Controllers.BLL.WebServices;

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

        public async void Run(string json)
        {
            //Read and process events
            var eventHolder = ObjectsHelper.Events(json);
            TextServices textProcess = new TextServices();
            PictureServices pictureProcess = new PictureServices();
            OaServices oaProcess = new OaServices();
            OaFollowServices followProcess = new OaFollowServices(usersRepository);
            ReceiverServices received = new ReceiverServices(messagesRepository);

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
            else if (eventHolder.event_name == "follow")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(async() =>
                {
                    await followProcess.UserFollow(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "unfollow")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(async() =>
                {
                    await followProcess.UserUnfollow(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }    
            else if (eventHolder.event_name == "user_received_message")
            {
                //Thread.Sleep(5000);
                var cancelToken = new CancellationTokenSource(10000).Token;
                await Task.Run(async() =>
                {                   
                    await received.UserReceived(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "user_seen_message")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                await Task.Run(async() =>
                {
                    //await Task.Delay(3000);
                    await received.UserSeen(json);
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
