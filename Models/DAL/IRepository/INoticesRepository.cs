using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface INoticesRepository
    {
        Notice GetNotice(long NoticeId);
        List<Notice> GetAllNotices();
        List<Notice> GetPageNotices(int offset, int range, string? condition);
        bool Add(Notice notice);
        bool Update(Notice noticeChanges);
        bool Delete(long NoticeId);
    }
}
