using System.Data.SqlClient;
using System.Threading.Channels;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.DTO;
using static ZaloOA_v2.Models.Messages;

namespace ZaloOA_v2.Models.DAL.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private db_a8ebff_kenjenorContext context;

        public MessagesRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }
        public async Task<MessageDTO> GetMessage(string messageId)
        {
            //db_a8ebff_kenjenorContext _context = new db_a8ebff_kenjenorContext();
            MessageDTO returnMessage = new MessageDTO();
                try
                {
                    //using (_context)
                    //{
                        var messages = await context.OaMessages.Where(message => message.MessageId.Equals(messageId)).FirstAsync();
                        returnMessage.MessageId = messages.MessageId;
                        returnMessage.NoticeId = messages.NoticeId;
                        returnMessage.UserId = messages.UserId;
                    //}
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Repositories:MessagesRepositoriy:GetMessage \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            return returnMessage;
        }

        public List<MessageDTO> GetAllMessages()
        {
            List<MessageDTO> returnMessage = new List<MessageDTO>();
            try
            {
                    var messages = context.OaMessages.Where(message => message.Status == true).ToList();
                    foreach (var message in messages)
                    {
                        MessageDTO mess = new MessageDTO
                        {
                            MessageId = message.MessageId,
                            NoticeId = message.NoticeId,
                            UserId = message.UserId,
                            State = message.State,
                            Status = message.Status
                        };
                        returnMessage.Add(mess);
                    }
            }
            catch (SqlException ex)
            {
                string error = string.Format("Repositories:MessagesRepositoriy:GetAllMessages \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return returnMessage;
        }

        public List<MessageDTO> GetPageMessages(int offset, int range, string? conditions)
        {
            throw new NotImplementedException();
        }

        public async Task Add(MessageDTO message)
        {
            try
            {
                var cancelToken = new CancellationTokenSource(2000).Token;
                var oaMessage = new OaMessage
                {
                    MessageId = message.MessageId,
                    NoticeId = message.NoticeId,
                    UserId = message.UserId,
                    State = message.State,
                    Status = message.Status
                };
                await Task.Run(async () =>
                {                    
                    context.OaMessages.Add(oaMessage);
                    await context.SaveChangesAsync();
                    Console.WriteLine("Saved message");
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (SqlException ex)
            {
                string error = string.Format("Model:Repositories:MessagesRepository:Add \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public async Task<bool> Update(MessageDTO changes)
        {
            MessageDTO userChanges = new MessageDTO();
            userChanges = changes;
            try
            {
                //var messages = context.OaMessages.Where(message => message.MessageId == userChanges.MessageId).FirstOrDefault();
                var cancelToken = new CancellationTokenSource(2000).Token;
                await Task.Run(() =>
                {
                    OaMessage mess = new OaMessage
                    {
                        MessageId = userChanges.MessageId,
                        NoticeId = userChanges.NoticeId,
                        UserId = userChanges.UserId,
                        State = userChanges.State,
                        Status = userChanges.Status
                    };
                    context.OaMessages.Update(mess);
                    context.SaveChangesAsync();
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (SqlException ex)
            {
                string error = string.Format("Repositories:MessageRepository:Update \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return false;
            }
            return true;
        }

        public Task Delete(string MessageId)
        {
            throw new NotImplementedException();
        }
    }
}
