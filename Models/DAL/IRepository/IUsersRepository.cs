using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Models.DAL.IRepository
{
    public interface IUsersRepository
    {
        Task<UserDTO> GetUser(long userID);
        Task<List<UserDTO>> GetAllUsers();
        Task<int> UsersTotal();
        List<UserDTO> GetPageUsers(int offset, int range);
        Task Add(long userID);
        Task Update(UserDTO userChanges);
        Task Delete(long userID);
        Task Restore(long userID);
    }
}
