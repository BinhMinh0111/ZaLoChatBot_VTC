using ZaloOA_v2.Helpers;

namespace ZaloOA_v2.Models.Logic
{
    public class OaProcess
    {
        public void Process(string json)
        {
            var textHolder = DataHelper.OAText(json);
            long user_id = long.Parse(textHolder.id);
            Procedures exist = new Procedures();
            if (exist.UserExist(user_id))
            {
                Console.WriteLine();                
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
