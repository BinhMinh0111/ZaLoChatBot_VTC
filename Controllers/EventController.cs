using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;

namespace ZaloOA_v2.Controllers
{
    public class EventController : Controller
    {
        public void run(string json)
        {
            //Read and process events
            var eventHolder = EventHelper.Events(json);
            if (eventHolder.event_name == "user_send_text" || eventHolder.event_name == "user_send_image")
            {
                db_a8ebff_kenjenorContext context = new db_a8ebff_kenjenorContext();

                if (eventHolder.event_name == "user_send_text")
                {
                    Console.WriteLine("event_name: " + eventHolder.event_name);
                    TextProcess(context, json);
                }
                else
                {
                    Console.WriteLine("event_name: " + eventHolder.event_name);
                    PictureProcess(context, json);
                }
            }
            else
            {
                //ghi log even + timestamp
                Console.WriteLine("Event unknow");
            }
        }
        public void TextProcess(db_a8ebff_kenjenorContext db, string json)
        {
            var textHolder = EventHelper.Text(json);
            long user_id = long.Parse(textHolder.id);
            Console.WriteLine("user_id: " + user_id);
            if (UserExist(user_id))
            {
                Console.WriteLine("Exist");
                AddText(json, db);
            }
            else
            {
                Console.WriteLine("Non exist");
                AddNewUser(user_id, db);
                AddText(json, db);
            }
        }
        public void PictureProcess(db_a8ebff_kenjenorContext db, string json)
        {
            var picHolder = EventHelper.Picture(json);
            long user_id = long.Parse(picHolder.id);
            Console.WriteLine("user_id: " + user_id);
            if (UserExist(user_id))
            {
                Console.WriteLine("Exist");
                AddPicture(json, db);
            }
            else
            {
                Console.WriteLine("Non exist");
                AddNewUser(user_id, db);
                AddPicture(json, db);
            }
        }

        public Boolean UserExist(long user_id)
        {
            //Check DB if user exist
            string conn = @"Data Source=SQL8003.site4now.net;Initial Catalog=db_a8ebff_kenjenor;User Id=db_a8ebff_kenjenor_admin;Password=Minh@258369";
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
                Console.WriteLine(ex);
            }
            return false;
        }
        public void AddNewUser(long user_id, db_a8ebff_kenjenorContext db)
        {
            var userholder = EventHelper.Users(user_id);
            using (db)
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
                Console.WriteLine("done parse");
                db.ZaloUsers.Add(zaloUser);
                Console.WriteLine("done add");
                db.SaveChanges();
                Console.WriteLine("done db");
            }
            Console.WriteLine("done user");
        }
        public void AddText(string json, db_a8ebff_kenjenorContext db)
        {
            Console.WriteLine("Add Text");
            var textHolder = EventHelper.Text(json);
            using (db)
            {
                var zaloFeedback = new ZaloFeedback
                {
                    UserId = long.Parse(textHolder.id),
                    Feedbacks = textHolder.text,
                    Timestamp = long.Parse(textHolder.timeStamp)
                };
                db.ZaloFeedbacks.Add(zaloFeedback);
                db.SaveChanges();
            }
            Console.WriteLine("done text");
        }
        public void AddPicture(string json, db_a8ebff_kenjenorContext db)
        {
            Console.WriteLine("Add Pics");
            var pictureHolder = EventHelper.Picture(json);
            List<ZaloPicture> pictureList = new List<ZaloPicture>();
            foreach (string item in pictureHolder.url)
            {
                pictureList.Add(new ZaloPicture()
                { UserId = long.Parse(pictureHolder.id), PicUrl = item, Timestamp = long.Parse(pictureHolder.timeStamp) });
            }
            Console.WriteLine("done pic list");
            using (db)
            {
                foreach (ZaloPicture picture in pictureList)
                {
                    var zaloPicture = new ZaloPicture
                    {
                        UserId = picture.UserId,
                        PicUrl = picture.PicUrl,
                        Timestamp = picture.Timestamp
                    };
                    Console.WriteLine("done parse");
                    db.ZaloPictures.Add(zaloPicture);
                    Console.WriteLine("done add " + picture.PicUrl);
                }
                db.SaveChanges();
                Console.WriteLine("done db");
            }
            Console.WriteLine("done pics");
        }
    }
}
