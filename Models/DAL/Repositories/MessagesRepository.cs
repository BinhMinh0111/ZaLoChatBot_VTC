using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.ServiceModels;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.DAO
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly db_a8ebff_kenjenorContext context;

        public MessagesRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }
        public MessageDTO GetMessage(string MessageId)
        {
            MessageDTO returnMessage = new MessageDTO();
            try
            {
                using (context)
                {
                    var messages = context.OaMessages;
                    foreach (var message in messages)
                    {
                        if (MessageId == message.MessageId)
                        {
                            returnMessage.MessageId = message.MessageId;
                            returnMessage.NoticeId = message.NoticeId;
                            returnMessage.UserId = message.UserId;
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("Repositories:GUID:GetMessages \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return returnMessage;
        }

        public List<MessageDTO> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public List<MessageDTO> GetPageMessages(int offset, int range, string? conditions)
        {
            throw new NotImplementedException();
        }

        public async Task Add(MessageDTO message)
        {
            try
            {
                //await using (context)
                //{
                    var OaMessage = new OaMessage
                    {
                        MessageId = message.MessageId,
                        NoticeId = message.NoticeId,
                        UserId = message.UserId, 
                        State = message.State,
                        Status = message.Status
                    };
                    context.OaMessages.Add(OaMessage);
                    context.SaveChanges();
                    Console.WriteLine("Saved message");
                //}
            }
            catch (Exception ex)
            {
                string error = string.Format("Model:Repositories:MessagesRepository:Add \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public Task Update(MessageDTO userChanges)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string MessageId)
        {
            throw new NotImplementedException();
        }
    }
}
