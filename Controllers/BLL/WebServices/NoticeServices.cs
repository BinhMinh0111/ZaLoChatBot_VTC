using System;
using System.Collections.Generic;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ViewModels;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.Controllers.BLL.WebServices
{
    public class NoticeServices
    {
        private INoticesRepository noticesRepository;

        public NoticeServices(INoticesRepository noticesRepository)
        {
            this.noticesRepository = noticesRepository;
        }

        public async Task<Int32> AddNewNotice(string text, int num)
        {
            Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            DateTime noticeTime = DateTime.Now;
            int noticeNum = num;
            string content = text;

            var cancelToken = new CancellationTokenSource(3000).Token;
            await Task.Run(() => 
            {
                NoticeDTO notice = new NoticeDTO
                {
                    NoticeId = unixTimestamp,
                    NoticeDate = noticeTime,
                    NumNotice = noticeNum,
                    ContentUrl = content
                };
                noticesRepository.Add(notice);
                cancelToken.ThrowIfCancellationRequested();
            }, cancelToken);

            return unixTimestamp;
        }
        public List<NoticeViewModel> NoticesByYear(int year)
        {
            List<NoticeViewModel> returnList = new List<NoticeViewModel>();
            var notices = noticesRepository.GetAllNotices();
            foreach(var item in notices)
            {
                DateTime? noticesDate = item.NoticeDate; 
                if(noticesDate.HasValue)
                {
                    DateTime _noticesDate = (DateTime)noticesDate;
                    if(_noticesDate.Year == year)
                    {
                        NoticeViewModel notice = new NoticeViewModel
                        {
                            NoticeId = item.NoticeId,
                            NoticeDate = item.NoticeDate,
                            NumNotice = item.NumNotice,
                            ContentUrl = item.ContentUrl
                        };
                        returnList.Add(notice);
                    }    
                }    
            }    
            return returnList;
        }
        public List<NoticeViewModel> NoticesByMonth (int month)
        {
            List<NoticeViewModel> returnList = new List<NoticeViewModel>();
            var notices = noticesRepository.GetAllNotices();
            foreach (var item in notices)
            {
                DateTime? noticesDate = item.NoticeDate;
                if (noticesDate.HasValue)
                {
                    DateTime _noticesDate = (DateTime)noticesDate;
                    if (_noticesDate.Month == month)
                    {
                        NoticeViewModel notice = new NoticeViewModel
                        {
                            NoticeId = item.NoticeId,
                            NoticeDate = item.NoticeDate,
                            NumNotice = item.NumNotice,
                            ContentUrl = item.ContentUrl
                        };
                        returnList.Add(notice);
                    }
                }
            }
            return returnList;
        }
        public List<NoticeViewModel> NoticesByDate(string date)
        {
            List<NoticeViewModel> returnList = new List<NoticeViewModel>();
            var notices = noticesRepository.GetAllNotices();
            foreach (var item in notices)
            {
                DateTime? noticesDate = item.NoticeDate;
                if (noticesDate.HasValue)
                {
                    DateTime _noticesDate = (DateTime)noticesDate;
                    if (_noticesDate.Date.ToString() == date)
                    {
                        NoticeViewModel notice = new NoticeViewModel
                        {
                            NoticeId = item.NoticeId,
                            NoticeDate = item.NoticeDate,
                            NumNotice = item.NumNotice,
                            ContentUrl = item.ContentUrl
                        };
                        returnList.Add(notice);
                    }
                }
            }
            return returnList;
        }
    }
}
