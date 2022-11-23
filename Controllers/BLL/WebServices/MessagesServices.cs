using System;
using System.Data.SqlClient;
using System.Security.Policy;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Controllers.BLL.WebhookServices;
using ZaloOA_v2.Processes;
using ZaloOA_v2.DAO;
using ZaloOA_v2.API;
using ZaloOA_v2.Models.ServiceModels;
using ZaloOA_v2.Repositories.Interfaces;
using ZaloOA_v2.Models.ViewModels;
using System.Collections.Generic;
using ZaloOA_v2.DAA;

namespace ZaloOA_v2.Models.Processes.Web
{
    public class MessagesServices
    {
        private IMessagesRepository _messagesRepository;
        private IUsersRepository usersRepository;
        public MessagesServices(IMessagesRepository messagesRepository, IUsersRepository usersRepository)
        {
            _messagesRepository = messagesRepository;
            this.usersRepository = usersRepository;
        }

        public Task GetUsersPage (int offset,int range)
        {
            Procedures procedures = new Procedures();
            List<Message> userList = new List<Message>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            Task.Run(() =>
            {
                userList = _messagesRepository.GetPageMessages(offset, range,null);
                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
            return Task.CompletedTask;
        }
        public Task GetAllMessage()
        {
            Procedures procedures = new Procedures();
            List<Message> userList = new List<Message>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            Task.Run(() =>
            {
                userList = _messagesRepository.GetAllMessages();

                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
            return Task.CompletedTask;
        }
        public List<User> UserViewToUserModel(List<UsersViewModel> input)
        {
            List<UsersViewModel> holder = new List<UsersViewModel>();
            holder = input;
            List<User> userList =new List<User>();
            foreach(UsersViewModel item in holder)
            {
                User user = new User
                {
                    UserId = item.UserId,
                    DisplayName = item.DisplayName,
                    Gender = item.Gender
                };
                Console.WriteLine(user.DisplayName);
                userList.Add(user);
            }
            return userList;
        }
        public Task SendMessUsers(List<User> userList, string message)
        {
            foreach (var user in userList)
            {
                var cancelToken = new CancellationTokenSource(2000).Token;
                Task.Run(() =>
                {
                    MessageController messageController = new MessageController();
                    messageController.SendMessage(user.UserId, message);

                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }    
            return Task.CompletedTask;
        }
        public Task SendMessAllUsers( string message)
        {
            Console.WriteLine("Start");
            var users = usersRepository.GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine(user.DisplayName);
                var cancelToken = new CancellationTokenSource(3000).Token;
                Task.Run(() =>
                {
                    MessageController messageController = new MessageController();
                    messageController.SendMessage(user.UserId, message);

                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            return Task.CompletedTask;
        }
        public Task SaveMessage (Message message)
        {
            try
            {
                var cancelToken = new CancellationTokenSource(3000).Token;
                Task.Run(() =>
                {
                    _messagesRepository.Add(message);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                string error = string.Format("Processes:Web:MessagesProcess:SaveMessage \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
