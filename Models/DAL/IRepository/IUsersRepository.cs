using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.ServiceModels;

namespace ZaloOA_v2.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        UserDTO GetUser(long userID);
        List<UserDTO> GetAllUsers();
        int UsersTotal();
        List<UserDTO> GetPageUsers(int offset , int range);
        bool Add(long userID);
        bool Update(OaUser userChanges);
        bool Delete (long userID);
        bool Restore(long userID);
    }
}
