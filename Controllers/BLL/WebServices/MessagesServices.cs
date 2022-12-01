using System;
using System.Data.SqlClient;
using System.Security.Policy;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Controllers.BLL.WebhookServices;
using ZaloOA_v2.Models.ViewModels;
using System.Collections.Generic;
using ZaloOA_v2.Controllers.BLL.WebhookServices.Workflows;
using System.Security.Cryptography;
using ZaloOA_v2.Models.DAL.Repositories;
using ZaloOA_v2.Models.DAL.IRepository;

namespace ZaloOA_v2.Controllers.BLL.WebServices
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

        public async Task GetUsersPage(int offset, int range)
        {
            Procedures procedures = new Procedures();
            List<MessageDTO> userList = new List<MessageDTO>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            await Task.Run(() =>
            {
                userList = _messagesRepository.GetPageMessages(offset, range, null);
                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
        }
        public async Task GetAllMessage()
        {
            Procedures procedures = new Procedures();
            List<MessageDTO> userList = new List<MessageDTO>();

            var cancelToken = new CancellationTokenSource(3000).Token;
            await Task.Run(() =>
            {
                userList = _messagesRepository.GetAllMessages();

                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);
        }
        public List<UserDTO> UserViewToUserModel(List<UsersViewModel> input)
        {
            List<UsersViewModel> holder = new List<UsersViewModel>();
            holder = input;
            List<UserDTO> userList = new List<UserDTO>();
            foreach (UsersViewModel item in holder)
            {
                UserDTO user = new UserDTO
                {
                    UserId = item.UserId,
                    DisplayName = item.DisplayName,
                    Gender = item.Gender
                };
                userList.Add(user);
            }
            return userList;
        }
        public async Task SendMessUsers(List<UserDTO> userList, string message, int notId)
        {
            int noticeId = notId;
            Console.WriteLine("Start");
            foreach (var user in userList)
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                await Task.Run(async () =>
                {
                    string[] response = new string[3];
                    MessageController messageController = new MessageController();

                    response = await messageController.SendMessage(user.UserId, message);
                    //[0] error, [1] message, [2] messageId
                    Console.WriteLine(response[2]);
                    if (int.Parse(response[0]) == 0)
                    {
                        await SaveMessage(response[2], user.UserId, noticeId, 1, true);
                    }
                    else
                    {
                        //Nếu không gửi thành công thì gắn timestamp làm Id, và lưu mã lỗi
                        int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        await SaveMessage(unixTimestamp.ToString() + "_" + response[0], user.UserId, noticeId, 1, true);
                    };

                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
        }
        public async Task SendMessAllUsers(string message, int notId)
        {
            int noticeId = notId;
            var users = await usersRepository.GetAllUsers();
            foreach (var user in users)
            {
                Console.WriteLine(user.DisplayName);
                var cancelToken = new CancellationTokenSource(10000).Token;
                await Task.Run(async () =>
                {
                    string[] response = new string[3];
                    MessageController messageController = new MessageController();

                    response = await messageController.SendMessage(user.UserId, message);
                    //[0] error, [1] message, [2] messageId
                    if (int.Parse(response[0]) == 0)
                    {
                        await SaveMessage(response[2], user.UserId, noticeId, 1, true);
                    }
                    else
                    {
                        //Nếu không gửi thành công thì gắn timestamp làm Id, và lưu mã lỗi
                        int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        await SaveMessage(unixTimestamp.ToString() + "_" + response[0], user.UserId, noticeId, 1, false);
                        LogWriter.LogWrite("Failed sending to user: " + user.UserId + " at: " + unixTimestamp.ToString());
                    };

                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
        }

        //Message State: 0 = unsent, 1 = sent, 2 = received, 3 = seen
        public async Task SaveMessage(string? messId, long uId, int notId, short state, bool status = true)
        {
            string? messageId = messId;
            long userId = uId;
            int noticeId = notId;
            short messageState = state;
            bool messageStatus = status;

            MessageDTO message = new MessageDTO
            {
                MessageId = messageId,
                UserId = userId,
                NoticeId = noticeId,
                State = messageState,
                Status = messageStatus
            };

            try
            {
                var cancelToken = new CancellationTokenSource(3000).Token;
                await Task.Run(async () =>
                {
                    await _messagesRepository.Add(message);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (Exception ex)
            {
                string error = string.Format("Processes:Web:MessagesProcess:SaveMessage \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }
    }
}
