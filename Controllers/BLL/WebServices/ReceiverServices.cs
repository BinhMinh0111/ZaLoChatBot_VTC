using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Controllers.BLL.WebServices
{
    public class ReceiverServices
    {
        private IMessagesRepository messagesRepository;

        public ReceiverServices(IMessagesRepository messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }
        public async Task UserReceived(string json)
        {
            var objectHolder = ObjectsHelper.UserReceive(json);
            string msgId = objectHolder.id;
            MessageDTO message = await messagesRepository.GetMessage(msgId);
            message.State = 2;
            bool updateStatus = await messagesRepository.Update(message);
            if (!updateStatus)
                LogWriter.LogWrite("WebServices/ReceiverServices/UserReceived: Failed");
        }
        public async Task UserSeen(string json)
        {
            var objectHolder = ObjectsHelper.UserReceive(json);
            string msgId = objectHolder.id;
            MessageDTO message = await messagesRepository.GetMessage(msgId);
            message.State = 3;
            bool updateStatus = await messagesRepository.Update(message);
            if (!updateStatus)
                LogWriter.LogWrite("WebServices/ReceiverServices/UserSeen: Failed");
        }
    }
}
