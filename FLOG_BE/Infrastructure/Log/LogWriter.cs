using Infrastructure.Log;
using Infrastructure.Log.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Infrastructure.Log
{
    public class LogWriter : ILogWriter
    {
        private readonly IConfiguration _config;
        private string m_exePath = string.Empty;
        public LogWriter(IConfiguration config)
        {
            _config = config;
        }
        public void Write(LoggerType type, string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var dir = Path.Combine($"{_config["Path:Logger"]}{Path.DirectorySeparatorChar}");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string filename = Path.Combine($"{dir}{System.DateTime.Now.ToString("ddMMyyy_hhmm")}_log_{type.GetEnumDescription()}.txt");
            if (!File.Exists(filename))
            {
                var fileStream = File.Create(filename);
                fileStream.Close();
            }

            try
            {
                using (StreamWriter w = File.AppendText(filename))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ type.GetEnumDescription() } Exception on Logger : { ex.Message }");
                Console.WriteLine($"      for Logger : { logMessage }");
            }
        }

        void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
