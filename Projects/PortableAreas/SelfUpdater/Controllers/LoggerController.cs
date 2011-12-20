using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGet;

namespace SelfUpdater.Controllers
{
    public class LoggerController : ILogger

        

    {

        private Dictionary<String, String> list;

        public LoggerController() {
            list = new Dictionary<string, string>();
        }

        public void Log(MessageLevel level, string message, params object[] args) {

            string msg = String.Empty;//level.ToString() + ": ";
            if (args != null) {
                for (int i = 0; i < args.Length; i++) {
                    message = message.Replace("{" + i + "}", args[i].ToString());
                }
            }
            msg += message;




            // Lo escribo en un archivo para ver que anda

            try {
                var dir = HttpContext.Current.Request.MapPath("~/rb_logs") + "\\Nuget.txt";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dir, true)) {
                    file.WriteLine(DateTime.Now.ToString());
                    file.WriteLine(msg);
                }
            }
            catch(Exception){}

            if (!list.ContainsKey(msg)) {
                list.Add(msg, msg);
            


            // Creating message

                if (msg.Contains("Starting") && !list.ContainsKey("install")) {
                list.Add("install", msg);
            }
            if(message.Contains("Downloading")){
                int index = message.IndexOf("...");

                if (!list.ContainsKey("download")) {
                    string mssg = message.Substring(0, index + 3);
                    list.Add("download", mssg);
                    list.Add("percent", message.Substring(index+2,message.Length - (index + 2)));

                }
                else {
                    list.Remove("percent");
                    list.Add("percent", message.Substring(index + 2, message.Length - (index + 2)));
                }
            }

            var mensaje = String.Empty;
            if (list.ContainsKey("install"))
                mensaje = "<li>" + list["install"] + "</li>";
            if (list.ContainsKey("download") && list.ContainsKey("percent")) {
                mensaje += "<li>" + list["download"]  + list["percent"] + "%" + "</li>";
                if (list["percent"].Contains("100"))
                    mensaje += "<li> Installing package ... </li>";
            }

            

            HttpContext.Current.Application["NugetLogger"] = mensaje;
            }
            
            
        }

        public string getLogs() {

            var msgs = String.Empty;            
            return msgs;
        }
    }
}
