using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace ZaloOA_v2.Controllers
{
    public class EventController : Controller
    {
        public void Run(string json)
        {
            //Read and process events
            var eventHolder = EventHelper.Events(json);
            if (eventHolder.event_name == "user_send_text" || eventHolder.event_name == "user_send_image")
            {
                if (eventHolder.event_name == "user_send_text")
                {
                   TextProcess(json);
                }
                else
                {
                   PictureProcess(json);
                }
            }
            else
            {
                //ghi log even + timestamp
                LogWriter log = new LogWriter("New event: " + eventHolder.event_name);
            }            
        }
        public void TextProcess(string json)
        {
            var textHolder = EventHelper.Text(json);
            long user_id = long.Parse(textHolder.id);
            if (UserExist(user_id))
            {
                AddText(json);
            }
            else
            {
                AddNewUser(user_id);
                AddText(json);
            }
        }
        public async Task PictureProcess(string json)
        {
            var picHolder = EventHelper.Picture(json);
            long user_id = long.Parse(picHolder.id);
            if (UserExist(user_id))
            {
                AddPicture(json);
            }
            else
            {
                AddNewUser(user_id);
                AddPicture(json);
            }
        }

        public Boolean UserExist(long user_id)
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
                LogWriter log = new LogWriter(ex.Message);
            }
            return false;
        }
        public void AddNewUser(long user_id)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var userholder = EventHelper.Users(user_id);
            using (context)
            {
                var zaloUser = new ZaloUser
                {
                    UserId = long.Parse(userholder.user_id),
                    UserIdByApp = long.Parse(userholder.user_id_by_app),
                    DisplayName = userholder.display_name,
                    UserGender = userholder.user_gender,
                    Avatar = userholder.avatar,
                    UserState = true
                };
                context.ZaloUsers.Add(zaloUser);
                context.SaveChanges();
            }
        }
        public void AddText(string json)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var textHolder = EventHelper.Text(json);
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
            Console.WriteLine("Move to mess process");
            try
            {
                var cancelToken = new CancellationTokenSource(20000).Token;
                Task.Run(async () =>
                {
                    ReplyController reply = new ReplyController();
                    reply.MessagesProcess(long.Parse(textHolder.id), textHolder.text);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            catch (Exception ex)
            {
                LogWriter logWriter = new LogWriter(ex.Message);
                Console.WriteLine("Too long!");
            }
        }
        public void AddPicture(string json)
        {
            db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();
            var pictureHolder = EventHelper.Picture(json);
            List<ZaloPicture> pictureList = new List<ZaloPicture>();
            foreach (string item in pictureHolder.url)
            {
                pictureList.Add(new ZaloPicture()
                { UserId = long.Parse(pictureHolder.id), PicUrl = item, Timestamp = long.Parse(pictureHolder.timeStamp) });
            }
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
    }
}
