using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ServiceModels;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        Message GetMessage(string MessageId);
        List<Message> GetAllMessages();
        List<Message> GetPageMessages(int offset, int range, string? conditions);
        bool Add(Message message);
        bool Update(Message userChanges);
        bool Delete(string MessageId);
    }
}
