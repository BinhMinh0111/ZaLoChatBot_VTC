using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.Processes
{
    public class PictureProcess
    {
        public void Process(string json)
        {
            var picHolder = DataHelper.UserPicture(json);
            long user_id = long.Parse(picHolder.id);
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                DbProcess.AddPicture(json);
            }
            else
            {
                DbProcess.AddNewUser(user_id);
                DbProcess.AddPicture(json);
            }
        }
    }
}
