using System.Data.SqlClient;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DTO;
using ZaloOA_v2.Repositories.Interfaces;
using ZaloOA_v2.Models.ServiceModels;

namespace ZaloOA_v2.DAA
{
    public class UsersRepository : IUsersRepository
    {
        private readonly db_a8ebff_kenjenorContext context;

        public UsersRepository(db_a8ebff_kenjenorContext context)
        {
            this.context = context;
        }
        public User GetUser(long userID)
        {
            User returnUser = new User();
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

        public List<User> GetAllUsers()
        {           
            var users = context.OaUsers.ToList();
            List<User> returnList = new List<User>();
            foreach(var user in users)
            {
                User _user = new User
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

        public List<User> GetPageUsers(int offset, int range)
        {
            List<User> userList = new List<User>();
            using (context)
            {
                try
                {
                    var users = context.OaUsers.Skip(offset).Take(range);
                    foreach (var user in users)
                    {
                        User oaUser = new User
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
