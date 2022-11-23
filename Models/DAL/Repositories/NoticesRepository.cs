using ZaloOA_v2.Helpers;
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

        public Notice GetNotice(long NoticeId)
        {
            Notice returnNotice = new Notice();
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

        public List<Notice> GetAllNotices()
        {
            List<Notice> returnNotice = new List<Notice> ();
            try
            {
                using (context)
                {
                    var notices = context.OaNotices;
                    foreach (var item in notices)
                    {
                        Notice notice = new Notice
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

        public List<Notice> GetPageNotices(int offset, int range, string? condition)
        {
            throw new NotImplementedException();
        }

        public bool Add(Notice notice)
        {
            try
            {
                using (context)
                {
                    var OANotice = new OaNotice
                    {
                        NoticeId = notice.NoticeId,
                        NoticeDate = notice.NoticeDate,
                        ContentUrl = notice.ContentUrl,
                    };
                    context.OaNotices.Add(OANotice);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:PostUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return false;
            }
        }

        public bool Update(Notice noticeChanges)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long NoticeId)
        {
            throw new NotImplementedException();
        }
    }
}
