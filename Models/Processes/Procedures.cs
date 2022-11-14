using System.Data;
using System.Data.SqlClient;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DatabaseModels;

namespace ZaloOA_v2.Processes
{
    public class Procedures
    {
        public bool UserExist(long user_id)
        {
            //Check DB if user exist
            string conn = ConfigHelper.ConnString("DefaultConnection");
            SqlConnection sqlCon = null;
            if (sqlCon == null)
                sqlCon = new SqlConnection(conn);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_exist_user";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = sqlCon;
                cmd.Parameters.Add("@user_id", SqlDbType.BigInt).Value = user_id;
                cmd.Parameters.Add("@is_exist", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int isExist = Convert.ToInt32(cmd.Parameters["@is_exist"].Value);
                Console.WriteLine(isExist);
                if (isExist == 1)
                    return true;
                return false;
            }
            catch (SqlException ex)
            {
                string error = string.Format("Processes:Procedures:UserExist \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
            return false;
        }
        public string GetDisplayName(long userId)
        {
            string displayName = string.Empty;
            using (var context = new db_a8ebff_kenjenorContext())
            {
                try
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        if (userId == user.UserId)
                            displayName = user.DisplayName;
                    }
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:GetDisplayName \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            }
            return displayName;
        }
        public bool GetUserState (long userId)
        {
            bool returnValue = false;
            using(var context = new db_a8ebff_kenjenorContext())
            {
                try 
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        if (userId == user.UserId)
                            returnValue = (bool)user.UserState;
                    }
                }
                catch(SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:GetUserState \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            }    
            return returnValue;
        }
        public void UpdateUserState(long userId, bool state)
        {
            using (var context = new db_a8ebff_kenjenorContext())
            {
                try
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        if (userId == user.UserId)
                            user.UserState = state;
                    }
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:UpdateUserState \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            }
        }
        public List<OaUser> GetUserOffset (int offset, int range)
        {
            List<OaUser> userList = new List<OaUser>();
            using (var context = new db_a8ebff_kenjenorContext())
            {
                try
                {
                    var users = context.OaUsers.Skip(offset).Take(range);
                    foreach (var user in users)
                    {
                        OaUser oaUser = new OaUser
                        {
                            UserId = user.UserId,
                            UserIdByApp = user.UserIdByApp,
                            DisplayName = user.DisplayName,
                            UserGender = user.UserGender,
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

        public List<OaUser> GetAllUsers ()
        {
            List<OaUser> userList = new List<OaUser>();
            using (var context = new db_a8ebff_kenjenorContext())
            {
                try
                {
                    var users = context.OaUsers;
                    foreach (var user in users)
                    {
                        OaUser oaUser = new OaUser
                        {
                            UserId = user.UserId,
                            UserIdByApp = user.UserIdByApp,
                            DisplayName = user.DisplayName,
                            UserGender = user.UserGender,
                            UserState = user.UserState
                        };
                        userList.Add(oaUser);
                    }
                }
                catch (SqlException ex)
                {
                    string error = string.Format("Processes:Procedures:GetAllUsers \n {0}", ex.Message);
                    LogWriter.LogWrite(error);
                }
            }
            return userList;
        }
    }
}
