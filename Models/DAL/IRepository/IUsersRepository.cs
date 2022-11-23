using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Models.ServiceModels;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        User GetUser(long userID);
        List<User> GetAllUsers();
        List<User> GetPageUsers(int offset , int range);
        bool Add(long userID);
        bool Update(OaUser userChanges);
        bool Delete (long userID);
        bool Restore(long userID);
    }
}
