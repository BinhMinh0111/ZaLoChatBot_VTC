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
                LogWriter.LogWrite(ex.Message);
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
                    var users = context.ZaloUsers;
                    foreach (var user in users)
                    {
                        if (userId == user.UserId)
                            displayName = user.DisplayName;
                    }
                }
                catch (SqlException ex)
                {
                    LogWriter.LogWrite(ex.Message);
                }
            }
            return displayName;
        }
    }
}
