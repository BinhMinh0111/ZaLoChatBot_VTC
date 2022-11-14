using ZaloOA_v2.Helpers;
using ZaloOA_v2.Processes;

namespace ZaloOA_v2.Models.Processes.Webhook
{
    public class OaFollowProcess
    {
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
                    procedures.UpdateUserState(user_id, true);
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
                    procedures.UpdateUserState(user_id, false);
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
