using Serilog;
using System.Reflection;

namespace ZaloOA_v2.Helpers
{
    public class LogWriter
    {
        private string m_exePath = string.Empty;
        //private string _exePath = string.Empty;
        public LogWriter(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //_exePath = Path.GetFullPath("Logs\\log-"+DateTime.Now.ToString("yyyyMMdd")+".txt");
            m_exePath = ConfigHelper.Logtring("LogFilePath");
            Console.WriteLine(m_exePath);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText(m_exePath))
                {
                    Log(ex.Message, w);
                }
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                using (StreamWriter w = File.AppendText(m_exePath))
                {
                    Log(ex.Message, w);
                }
            }
        }
    }
}
