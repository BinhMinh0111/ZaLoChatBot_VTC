using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Models.DAL.IRepository
{
    public interface IMessagesRepository
    {
        Task<MessageDTO> GetMessage(string MessageId);
        List<MessageDTO> GetAllMessages();
        List<MessageDTO> GetPageMessages(int offset, int range, string? conditions);
        Task Add(MessageDTO message);
        Task<bool> Update(MessageDTO userChanges);
        Task Delete(string MessageId);
    }
}
