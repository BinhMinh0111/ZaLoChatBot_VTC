using Microsoft.Data.SqlClient;
using ZaloOA_v2.Controllers;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models.DAO;
using ZaloOA_v2.Models.DTO;

namespace ZaloOA_v2.Models.DAL.Repositories
{
    public class DbProcess
    {
        public static void AddNewUser(long user_id)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var userholder = ObjectsHelper.Users(user_id);
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
            }
            catch (Exception ex)
            {
                string error = string.Format("Processes:DbProcess:AddNewUser \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }
        public static void AddPictureUrl(string json)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var pictureHolder = ObjectsHelper.UserPicture(json);
            try
            {
                using (context)
                {
                    var zaloPicture = new OaPicture
                    {
                        UserId = long.Parse(pictureHolder.id),
                        PicUrl = pictureHolder.url.First(),
                        PicTime = DateTime.Now
                    };
                    context.OaPictures.Add(zaloPicture);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("Processes:DbProcess:AddPicture \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }
        public static void AddPath(string json, string path)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var pictureHolder = ObjectsHelper.UserPicture(json);
            try
            {
                using (context)
                {
                    var zaloPicture = new OaPicture
                    {
                        UserId = long.Parse(pictureHolder.id),
                        PicUrl = path,
                        PicTime = DateTime.Now
                    };
                    context.OaPictures.Add(zaloPicture);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("Processes:DbProcess:AddPath \n {0}", ex.Message);
                LogWriter.LogWrite(error);
            }
        }
    }
}
