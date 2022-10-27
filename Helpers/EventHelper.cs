﻿using Newtonsoft.Json;
using ZaloOA_v2.API;

namespace ZaloOA_v2.Helpers
{
    public class EventHelper
    {
        //dynamic object hanlde for Events
        public static (string? event_name, string? timeStamp) Events(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var eventName = dynamicObject.event_name;
            var timeStamp = dynamicObject.timeStamp;

            return (eventName, timeStamp);
        }
        //dynamic object hanlde Messages
        public static (string? id, string? text, string? timeStamp) Text(string jsonString)
        {
            var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

            var user_id = dynamicObject.sender.id;
            var text = dynamicObject.message.text;
            var timeStamp = dynamicObject.timestamp;
            return (user_id, text, timeStamp);
        }
        //dynamic object handle Pictures
        public static (string? id, List<string>? url, string? timeStamp) Picture(string jsonString)
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
        public static (string? user_id, string? user_id_by_app, string? display_name, int? user_gender, string? avatar) Users(long id)
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
            int user_gender = dynamicObject.data.user_gender;
            var avatar = dynamicObject.data.avatar;
            return (user_id, user_id_app, display_name, user_gender, avatar);
        }
    }
}
