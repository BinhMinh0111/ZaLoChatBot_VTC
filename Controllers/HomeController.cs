using Microsoft.AspNetCore.Mvc;
using ZaloOA_v2.API;
using ZaloOA_v2.DAA;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ServiceModels;
using ZaloOA_v2.Repositories;
using ZaloOA_v2.Repositories.Interfaces;
using System.Text;
using ZaloOA_v2.Models.ViewModels;
using ZaloOA_v2.Models.Processes.Web;
using ZaloOA_v2.Controllers.BLL.WebServices;

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

        public IActionResult Index()
        {
            List<UsersViewModel> model = new List<UsersViewModel>();
            var users =  usersRepository.GetAllUsers();
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
        public string SendToUsers (bool checkAll, List<UsersViewModel> model, string text)
        {
            MessagesServices messagesServices = new MessagesServices(messagesRepository,usersRepository);
            NoticeServices noticeServices = new NoticeServices(noticesRepository);
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Bạn chưa nhap noi dung!");
                Console.WriteLine(text);
                return "Bạn chưa nhap noi dung!";
            }
            if(checkAll)
            {
                Console.WriteLine(checkAll +", "+ text);
                noticeServices.AddNewNotice(text);
                messagesServices.SendMessAllUsers(text);
                return checkAll + ", " + text;
            }
            else if (model.Count(x => x.isSelected) == 0)
            {
                Console.WriteLine(checkAll);
                return "Bạn chưa chọn người gửi!";
            }
            else
            {
                noticeServices.AddNewNotice(text);

                List<UsersViewModel> users = new List<UsersViewModel>();
                users = model;
                List<User> sendList = new List<User>();

                users.RemoveAll(user => user.isSelected == false);
                sendList = messagesServices.UserViewToUserModel(users);
                messagesServices.SendMessUsers(sendList, text);
                return "Sent!";
            }     
        }
    }
}
