using Newtonsoft.Json;
using Serilog;
using System.Dynamic;
using ZaloOA_v2.API;

namespace ZaloOA_v2.Helpers
{
    public class ObjectsHelper
    {
        //dynamic object hanlde for Events
        public static (string? event_name, string? timeStamp) Events(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var eventName = dynamicObject.event_name;
            var timeStamp = dynamicObject.timeStamp;

            return (eventName, timeStamp);
        }
        //dynamic object hanlde User Messages
        public static (string? id, string? text, string? timeStamp, string msg_id) UserText(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.sender.id;
            var text = dynamicObject.message.text;
            var timeStamp = dynamicObject.timestamp;
            var msgId = dynamicObject.message.msg_id;
            return (user_id, text, timeStamp, msgId);
        }
        //dynamic object hanlde OA Messages
        public static (string? id, string? text, string? timeStamp, string msg_id) OAText(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.recipient.id;
            var text = dynamicObject.message.text;
            var timeStamp = dynamicObject.timestamp;
            var msgId = dynamicObject.message.msg_id;
            return (user_id, text, timeStamp, msgId);
        }
        //dynamic object handle Pictures
        public static (string? id, List<string>? url, string? timeStamp) UserPicture(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.sender.id;
            //Get pic url
            List<string> pic_url = new List<string>();
            var Packages = dynamicObject.message.attachments;
            foreach (var package in Packages)
            {
                string urlString = Convert.ToString(package.payload.url);
                pic_url.Add(urlString);
            }
            var timeStamp = dynamicObject.timestamp;
            return (user_id, pic_url, timeStamp);
        }

        //dynamic object hanlde User
        public static (string? user_id, string? user_id_by_app, string? display_name, short? user_gender) Users(long id)
        {
            //Get NewUser info
            GetFollowerController getfollower = new GetFollowerController();
            Task<string> json = getfollower.Get_follower_detail(id);
            string newUser = json.Result;
            //Deserialize User info
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(newUser)!;
            var user_id = dynamicObject.data.user_id;
            var user_id_app = dynamicObject.data.user_id_by_app;
            var display_name = dynamicObject.data.display_name;
            short user_gender = dynamicObject.data.user_gender;
            return (user_id, user_id_app, display_name, user_gender);
        }

        //dynamic object hanlde user follow OA
        public static (string? id, string? source) UserFollow(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.follower.id;
            var followSource = dynamicObject.source;
            return (user_id, followSource);
        }
        //dynamic object hanlde user recive/seen OA message
        public static (string? id, string event_name) UserReceive(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.recipient.id;
            var eventName = dynamicObject.event_name;
            return (user_id, eventName);
        }
    }
}
