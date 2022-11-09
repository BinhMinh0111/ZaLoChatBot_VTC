﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;
using ZaloOA_v2.Helpers;
using ZaloOA_v2.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;
using ZaloOA_v2.Models.DatabaseModels;
using ZaloOA_v2.Processes;

namespace ZaloOA_v2.Controllers
{
    public class EventController : Controller
    {
        public void Run(string json)
        {
            //Read and process events
            var eventHolder = ObjectsHelper.Events(json);
            TextProcess textProcess = new TextProcess();
            PictureProcess pictureProcess = new PictureProcess();
            OaProcess oaProcess = new OaProcess();

            if (eventHolder.event_name == "user_send_text")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    textProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else if (eventHolder.event_name == "user_send_image")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    pictureProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
                
            }
            else if (eventHolder.event_name == "oa_send_text")
            {
                var cancelToken = new CancellationTokenSource(10000).Token;
                Task.Run(() =>
                {
                    oaProcess.Process(json);
                    cancelToken.ThrowIfCancellationRequested();
                }, cancelToken);
            }
            else
            {
                //ghi log even + timestamp
                LogWriter.LogWrite("New event: " + eventHolder.event_name);
            }

        }
       
    }
}
