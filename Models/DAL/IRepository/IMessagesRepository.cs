using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ServiceModels;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IMessagesRepository
    {
        MessageDTO GetMessage(string MessageId);
        List<MessageDTO> GetAllMessages();
        List<MessageDTO> GetPageMessages(int offset, int range, string? conditions);
        Task Add(MessageDTO message);
        Task Update(MessageDTO userChanges);
        Task Delete(string MessageId);
    }
}
