using System.Data.SqlClient;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
using System.Net.Http.Headers;
using System.Linq;
using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.DAL.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ZaloOA_v2.Models.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly db_a8ebff_kenjenorContext context;

        public UsersRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }
        public async Task<UserDTO> GetUser(long userID)
        {
            UserDTO returnUser = new UserDTO();
            try
            {
                using (context)
                {
                    var users = await context.OaUsers.ToListAsync();
                    foreach (var user in users)
                    {
                        if (userID == user.UserId)
                        {
                            returnUser.UserId = user.UserId;
                            returnUser.IdByApp = user.IdByApp;
                            returnUser.DisplayName = user.DisplayName;
                            returnUser.Gender = user.Gender;
                            returnUser.UserState = user.UserState;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:GetUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return returnUser;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            List<UserDTO> returnList = new List<UserDTO>();
            await Task.Run(async () => 
            {
                var users = await context.OaUsers.Where(user => user.UserState == true).ToListAsync();
                foreach (var user in users)
                {
                    UserDTO _user = new UserDTO
                    {
                        UserId = user.UserId,
                        IdByApp = user.IdByApp,
                        DisplayName = user.DisplayName,
                        Gender = user.Gender,
                        UserState = user.UserState
                    };
                    returnList.Add(_user);
                }
            });           
            return returnList;
        }

        public async Task<int> UsersTotal()
        {
            int total = await context.OaUsers.Where(user => user.UserState == true).CountAsync();
            return total;
        }

        public List<UserDTO> GetPageUsers(int offset, int range)
        {
            List<UserDTO> userList = new List<UserDTO>();
                try
                {
                    var users = context.OaUsers.Skip(offset).Take(range);
                    foreach (var user in users)
                    {
                        UserDTO oaUser = new UserDTO
                        {
                            UserId = user.UserId,
                            IdByApp = user.IdByApp,
                            DisplayName = user.DisplayName,
                            Gender = user.Gender,
                            UserState = user.UserState
                        };
                        userList.Add(oaUser);
                    }
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:GetUserOffset \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            return userList;
        }

        public async Task Add(long userID)
        {
            var userholder = ObjectsHelper.Users(userID);
            try
            {
                var zaloUser = new OaUser
                {
                    UserId = long.Parse(userholder.user_id),
                    IdByApp = long.Parse(userholder.user_id_by_app),
                    DisplayName = userholder.display_name,
                    Gender = userholder.user_gender,
                    UserState = true
                };
                context.OaUsers.Add(zaloUser);
                await context.SaveChangesAsync();
            }
            catch (SqlException ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:PostUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public async Task Update(UserDTO changes)
        {
            UserDTO userChanges = new UserDTO();
            userChanges = changes;
            try
            {
                var users = context.OaUsers;
                await foreach (var user in users)
                {
                    if (userChanges.UserId == user.UserId)
                    {
                        OaUser oaUser = new OaUser
                        {
                            UserId = userChanges.UserId,
                            IdByApp = userChanges.IdByApp,
                            DisplayName = userChanges.DisplayName,
                            Gender = userChanges.Gender,
                            UserState = userChanges.UserState
                        };
                        context.OaUsers.Update(oaUser);
                        await context.SaveChangesAsync();
                        break;
                    }
                }
            }
            catch (SqlException ex)
            {
                string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public async Task Delete(long userID)
        {
            try
            {
                var users = context.OaUsers;
                await foreach (var user in users)
                {
                    if (userID == user.UserId)
                        user.UserState = false;
                }
            }
            catch (SqlException ex)
            {
                string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }

        public async Task Restore(long userID)
        {
            try
            {
                var users = context.OaUsers;
                await foreach (var user in users)
                {
                    if (userID == user.UserId)
                        user.UserState = true;
                }
            }
            catch (SqlException ex)
            {
                string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }
    }
}
