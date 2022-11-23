using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.BussinessProcesses.DatabaseProcesses;
using ZaloOA_v2.Processes;
using ZaloOA_v2.Repositories.Interfaces;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Components
{
    public class OaFollowServices
    {
        private IUsersRepository usersRepository;
        public OaFollowServices(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }
        public Task Process(string json, string eventName)
        {
            var followHolder = ObjectsHelper.UserFollow(json);
            long user_id = long.Parse(followHolder.id);
            Procedures exist = new Procedures();
            if (!exist.UserExist(user_id) && eventName == "follow")
            {
                DbProcess.AddNewUser(user_id);
                return Task.CompletedTask;
            }

            if (eventName == "follow")
            {
                if (!isFollowing(user_id))
                {
                    Procedures procedures = new Procedures();
                    usersRepository.Restore(user_id);
                    return Task.CompletedTask;
                }
                else
                {
                    string error = string.Format("Processes:OaFollowProcess:Process \n User already exist & state already following");
                    LogWriter.LogWrite(error);
                    return Task.CompletedTask;
                }
            }
            else //unfollow
            {
                if (isFollowing(user_id))
                {
                    Procedures procedures = new Procedures();
                    usersRepository.Delete(user_id);
                    return Task.CompletedTask;
                }
                else
                {
                    string error = string.Format("Processes:OaFollowProcess:Process \n User already exist & state already unfollowing");
                    LogWriter.LogWrite(error);
                    return Task.CompletedTask;
                }
            }
        }
        private bool isFollowing(long user_id)
        {
            Procedures procedures = new Procedures();
            bool returnValue = procedures.GetUserState(user_id);
            return returnValue;
        }
    }
}
