using System.Data.SqlClient;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Repositories.Interfaces;
using ZaloOA_v2.Models.ServiceModels;
using System.Net.Http.Headers;
using System.Linq;
using ZaloOA_v2.Models.DAO;

namespace ZaloOA_v2.DAA
{
    public class UsersRepository : IUsersRepository
    {
        private readonly db_a8ebff_kenjenorContext context;

        public UsersRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }
        public UserDTO GetUser(long userID)
        {
            UserDTO returnUser = new UserDTO();
            try
            {
                using (context)
                {
                    var users = context.OaUsers;
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

        public List<UserDTO> GetAllUsers()
        {           
            var users = context.OaUsers.Where(user => user.UserState == true).ToList();
            List<UserDTO> returnList = new List<UserDTO>();
            foreach(var user in users)
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
            return returnList;
        }
        
        public int UsersTotal()
        {
            int total = context.OaUsers.Where(user => user.UserState == true).Count();
            return total;
        }

        public List<UserDTO> GetPageUsers(int offset, int range)
        {
            List<UserDTO> userList = new List<UserDTO>();
            using (context)
            {
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
            }
            return userList;
        }

        public bool Add(long userID)
        {
            var userholder = ObjectsHelper.Users(userID);
            try
            {
                using (context)
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
                    context.SaveChanges();
                }
                return true;
            }
            catch (SqlException ex)
            {
                string error = string.Format("BussinessProcesses:DatabaseProcess:DAO:PostUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                return false;
            }
        }

        public bool Update(OaUser userChanges)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long userID)
        {
            using (context)
            {
                try
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        if (userID == user.UserId)
                            user.UserState = false;
                    }
                    return true;
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                    return false;
                }
            }
        }

        public bool Restore(long userID)
        {
            using (context)
            {
                try
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        if (userID == user.UserId)
                            user.UserState = true;
                    }
                    return true;
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                    return false;
                }
            }
        }
    }
}
