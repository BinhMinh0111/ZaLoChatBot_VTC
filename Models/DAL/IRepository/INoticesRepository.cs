using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface INoticesRepository
    {
        NoticeDTO GetNotice(long NoticeId);
        List<NoticeDTO> GetAllNotices();
        List<NoticeDTO> GetPageNotices(int offset, int range, string? condition);
        Task Add(NoticeDTO notice);
        Task Update(NoticeDTO noticeChanges);
        Task Delete(long NoticeId);
    }
}
