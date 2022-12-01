using Microsoft.AspNetCore.Mvc;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ViewModels;
using ZaloOA_v2.Controllers.BLL.WebServices;
using ZaloOA_v2.Models.DAL.IRepository;

namespace ZaloOA_v2.Controllers
{
    public class HomeController : Controller
    {
        private IUsersRepository usersRepository;
        private IMessagesRepository messagesRepository;
        private INoticesRepository noticesRepository;
        public HomeController(IUsersRepository usersRepository,IMessagesRepository messagesRepository, INoticesRepository noticesRepository)
        {
            this.usersRepository = usersRepository;
            this.messagesRepository = messagesRepository;
            this.noticesRepository = noticesRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<UsersViewModel> model = new List<UsersViewModel>();
            var users = await usersRepository.GetAllUsers();
            foreach(var user in users)
            {
                UsersViewModel usersView = new UsersViewModel
                {
                    UserId = user.UserId,
                    DisplayName = user.DisplayName,
                    Gender = user.Gender,
                    isSelected = false
                };
                model.Add(usersView);
            }    
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SendToUsers(bool checkAll, List<UsersViewModel> model, string text)
        {
            MessagesServices messagesServices = new MessagesServices(messagesRepository,usersRepository);
            NoticeServices noticeServices = new NoticeServices(noticesRepository);
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {                
                return Content("Bạn chưa nhap noi dung!");
            }
            if(checkAll)
            {
                int totalUsers = await usersRepository.UsersTotal();

                Int32 noticeId = await noticeServices.AddNewNotice(text, totalUsers);
                await messagesServices.SendMessAllUsers(text, noticeId);                  
                return Content("Sent all!");
            }
            else if (model.Count(x => x.isSelected) == 0)
            {
                return Content("You haven't choose receivers!");
            }
            else
            {
                List<UsersViewModel> users = new List<UsersViewModel>();
                users = model;
                List<UserDTO> sendList = new List<UserDTO>();

                users.RemoveAll(user => user.isSelected == false);
                sendList = messagesServices.UserViewToUserModel(users);

                Int32 noticeId = await noticeServices.AddNewNotice(text, sendList.Count());
                await messagesServices.SendMessUsers(sendList, text, noticeId);
                return Content("Sent by list!");
            }     
        }
        [HttpPost]
        public async Task NoticeReport()
        {

        }
    }
}
