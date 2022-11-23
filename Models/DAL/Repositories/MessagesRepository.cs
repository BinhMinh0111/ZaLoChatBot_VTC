using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
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
        public Message GetMessage(string MessageId)
        {
            Message returnMessage = new Message();
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

        public List<Message> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public List<Message> GetPageMessages(int offset, int range, string? conditions)
        {
            throw new NotImplementedException();
        }

        public bool Add(Message message)
        {
            try
            {
                using (context)
                {
                    var OaMessage = new OaMessage
                    {
                        MessageId = message.MessageId,
                        NoticeId = message.NoticeId,
                        UserId = message.UserId, 
                    };
                    context.OaMessages.Add(OaMessage);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = string.Format("Repositories:GUID:PostMessages \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return false;
            }
        }

        public bool Update(Message userChanges)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string MessageId)
        {
            throw new NotImplementedException();
        }
    }
}
