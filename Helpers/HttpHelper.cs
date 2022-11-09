using System.Net;

namespace ZaloOA_v2.Helpers
{
    public class HttpHelper
    {
        public static string CallAuthJson(string url, string contentType, object data, string auth,
            out HttpStatusCode statusCode, string method = "POST",
            int timeout = 120000, string authHeader = "")
        {
            string responseData = string.Empty;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //Set content type
                if (!string.IsNullOrEmpty(contentType))
                { httpWebRequest.ContentType = "application/json"; }
                else
                { httpWebRequest.ContentType = contentType; }
                httpWebRequest.Method = method;
                //theo mac dinh
                if (!string.IsNullOrEmpty(auth) && string.IsNullOrEmpty(authHeader))
                    httpWebRequest.Headers.Add("Authorization", auth);
                //custom theo doi tac
                if (!string.IsNullOrEmpty(auth) && !string.IsNullOrEmpty(authHeader))
                    httpWebRequest.Headers.Add(authHeader, auth);
                httpWebRequest.Timeout = timeout;
                //httpWebRequest.UserAgent = "Vtctelecom api";

                if (method == "POST" && data != null)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(JsonHelper.Serialize(data));
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusCode = httpResponse.StatusCode;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseData = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                LogWriter.LogWrite(e.Response.ToString());
                statusCode = HttpStatusCode.NotFound;
                using (WebResponse response = e.Response)
                {
                    if (response != null)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        statusCode = httpResponse.StatusCode;
                        using (var resdata = new StreamReader(response.GetResponseStream()))
                        {
                            responseData = resdata.ReadToEnd();
                        }
                    }
                }
                LogWriter.LogWrite(responseData);
            }
            catch (Exception ex)
            {
                string error = string.Format("HttpHelper \n {0}", ex.Message);
                LogWriter.LogWrite(error);
                statusCode = HttpStatusCode.NotFound;
            }

            return responseData;
        }
    }
}
