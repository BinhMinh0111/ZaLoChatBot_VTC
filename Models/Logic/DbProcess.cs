using Microsoft.Data.SqlClient;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DatabaseModels;

namespace ZaloOA_v2.Models.Logic
{
    public class DbProcess
    {
        public static void AddNewUser(long user_id)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var userholder = DataHelper.Users(user_id);
            try
            {
                using (context)
                {
                    var zaloUser = new ZaloUser
                    {
                        UserId = long.Parse(userholder.user_id),
                        UserIdByApp = long.Parse(userholder.user_id_by_app),
                        DisplayName = userholder.display_name,
                        UserGender = userholder.user_gender,
                        UserState = true
                    };
                    context.ZaloUsers.Add(zaloUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message);
            }
        }
        public static void AddText(string json)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var textHolder = DataHelper.UserText(json);
            try 
            {
                using (context)
                {
                    var zaloFeedback = new ZaloFeedback
                    {
                        UserId = long.Parse(textHolder.id),
                        Feedbacks = textHolder.text,
                        Timestamp = long.Parse(textHolder.timeStamp)
                    };
                    context.ZaloFeedbacks.Add(zaloFeedback);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) 
            {
                LogWriter.LogWrite(ex.Message);
            }            
        }
        public static void AddPicture(string json)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var pictureHolder = DataHelper.UserPicture(json);
            List<ZaloPicture> pictureList = new List<ZaloPicture>();
            foreach (string item in pictureHolder.url)
            {
                pictureList.Add(new ZaloPicture()
                { UserId = long.Parse(pictureHolder.id), PicUrl = item, Timestamp = long.Parse(pictureHolder.timeStamp) });
            }
            try
            {
                using (context)
                {
                    foreach (ZaloPicture picture in pictureList)
                    {
                        var zaloPicture = new ZaloPicture
                        {
                            UserId = picture.UserId,
                            PicUrl = picture.PicUrl,
                            Timestamp = picture.Timestamp
                        };
                        context.ZaloPictures.Add(zaloPicture);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message);
            }
        }
    }
}
