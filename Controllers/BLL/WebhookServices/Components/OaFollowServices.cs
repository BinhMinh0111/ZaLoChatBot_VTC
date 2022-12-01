using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAL.IRepository;
using ZaloOA_v2.Models.DAL.Repositories;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Controllers.BLL.WebhookServices.Components
{
    public class OaFollowServices
    {
        private IUsersRepository usersRepository;
        public OaFollowServices(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }
        public async Task UserFollow(string json)
        {
            Procedures procedures = new Procedures();
            var holder = ObjectsHelper.UserFollow(json);
            if (!procedures.UserExist(Int32.Parse(holder.id)))
            {
                await usersRepository.Add(Int32.Parse(holder.id));
            }
            else
            {
                UserDTO user = await usersRepository.GetUser(Int32.Parse(holder.id));
                user.UserState = true;
                await usersRepository.Update(user);
            }
        }

        public async Task UserUnfollow(string json)
        {
            Procedures procedures = new Procedures();
            var holder = ObjectsHelper.UserFollow(json);
            if (!procedures.UserExist(Int32.Parse(holder.id)))
            {
                await usersRepository.Add(Int32.Parse(holder.id));
            }
            else
            {
                UserDTO user = await usersRepository.GetUser(Int32.Parse(holder.id));
                user.UserState = false;
                await usersRepository.Update(user);
            }
        }
    }
}
