using System.Threading.Tasks.Sources;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.DAA
{
    public class NoticesRepository : INoticesRepository
    {
        private readonly db_a8ebff_kenjenorContext context;

        public NoticesRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }

        public NoticeDTO GetNotice(long NoticeId)
        {
            NoticeDTO returnNotice = new NoticeDTO();
            try
            {
                using (context)
                {
                    var notices = context.OaNotices;
                    foreach (var notice in notices)
                    {
                        if (NoticeId == notice.NoticeId)
                        {
                            returnNotice.NoticeId = notice.NoticeId;
                            returnNotice.NoticeDate = notice.NoticeDate;
                            returnNotice.NumNotice = notice.NumNotice;
                            returnNotice.ContentUrl = notice.ContentUrl;
                        }    
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:GetNotice \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return returnNotice;
        }

        public List<NoticeDTO> GetAllNotices()
        {
            List<NoticeDTO> returnNotice = new List<NoticeDTO> ();
            try
            {
                using (context)
                {
                    var notices = context.OaNotices;
                    foreach (var item in notices)
                    {
                        NoticeDTO notice = new NoticeDTO
                        {
                            NoticeId = item.NoticeId,
                            NoticeDate = item.NoticeDate,
                            NumNotice = item.NumNotice,
                            ContentUrl = item.ContentUrl,
                        };
                        returnNotice.Add(notice);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:GetNotice \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return returnNotice;
        }

        public List<NoticeDTO> GetPageNotices(int offset, int range, string? condition)
        {
            throw new NotImplementedException();
        }

        public async Task Add(NoticeDTO notice)
        {
            try
            {
                //await using (context)
                //{
                    var OANotice = new OaNotice
                    {
                        NoticeId = notice.NoticeId,
                        NoticeDate = notice.NoticeDate,
                        NumNotice = notice.NumNotice,
                        ContentUrl = notice.ContentUrl,
                    };
                    context.OaNotices.Add(OANotice);
                    context.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:PostUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public Task Update(NoticeDTO noticeChanges)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long NoticeId)
        {
            throw new NotImplementedException();
        }
    }
}
