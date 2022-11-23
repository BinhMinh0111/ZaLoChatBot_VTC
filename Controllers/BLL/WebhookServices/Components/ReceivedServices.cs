using ZaloOA_v2.Helpers;
namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Components
{
    public class ReceivedServices
    {
        public void UserReceived(string json)
        {
            var objectHolder = ObjectsHelper.UserReceive(json);

        }
        public void UserSeen(string json)
        {
            var objectHolder = ObjectsHelper.UserReceive(json);
        }
    }
}
