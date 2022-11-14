using System;
using System.Data.SqlClient;
using ZaloOA_v2.Controllers.Services;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DatabaseModels;
using ZaloOA_v2.Models.Processes.Webhook;
using ZaloOA_v2.Processes;

namespace ZaloOA_v2.Models.Processes.Web
{
    public class MessagesProcess
    {
        public Task GetUsersPage (int offset,int range)
        {
            Procedures procedures = new Procedures();
            List<OaUser> userList = new List<OaUser>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            Task.Run(() =>
            {
                userList = procedures.GetUserOffset(offset, range);
                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
            return Task.CompletedTask;
        }
        public Task GetAllUsers()
        {
            Procedures procedures = new Procedures();
            List<OaUser> userList = new List<OaUser>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            Task.Run(() =>
            {
                userList = procedures.GetAllUsers();

                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
            return Task.CompletedTask;
        }
        public Task SendMessUsers(List<OaUser> userList, string message)
        {
            foreach (var user in userList)
            {
                var cancelToken = new CancellationTokenSource(1500).Token;
                Task.Run(() =>
                {
                    ReplyController.SendMessage(user.UserId, message);

                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }    
            return Task.CompletedTask;
        }
    }
}
