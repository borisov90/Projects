
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace CRMServices.Helpers
{
    public class EventLog
    {
        public EventLog() { }

        /// Записва msg - стринг в log-файал
        /// </summary>
        /// <param name="msg">Съобщение</param>	
        public static void saveMessage(string msg)
        {
            saveMessage(msg, null, null);
        }
        //		/// <summary>
        //		///  Записва msg - стринг в log-файал
        //		///  Записва данни за потребителя 
        //		/// </summary>
        //		/// <param name="msg">Съобщение</param>
        //		/// <param name="user">Потребител, може да се вземе от сесията</param>
        //		public static void saveMassage(string msg, UserProp user)
        //		{	
        //			saveMassage(msg, user, null);									
        //		}
        /// <summary>
        /// Записва msg -  стринг в log-файал
        /// Записва Exception
        /// </summary>
        /// <param name="msg">Съобщение</param>
        /// <param name="e">Exception</param>
        public static void saveMassage(string msg, Exception e)
        {
            saveMessage(msg, null, e);
        }

        /// <summary>
        /// Записва msg - стринг в log-файал
        /// Записва данни за потребителя 
        /// Записва Exception
        /// </summary>
        /// <param name="msg">Съобщение</param>
        /// <param name="user">Потребител, може да се вземе от сесията</param>
        /// <param name="e">Exception</param>
        //public static void saveMassage(string msg, UserProp user, Exception e)
        public static void saveMessage(string msg, string userDetails, Exception e)
        {

            string messageItem = "";

            messageItem += "-----------Start Message---------------\n";
            messageItem += "Message time: " + System.DateTime.Now.ToString() + "\n";

            if (userDetails != null)
            {
                messageItem += "User Details: \n" + userDetails + "\n";
            }
            if (e != null)
            {
                messageItem += "Exception Message: " + e.Message + "\n";
                messageItem += "Exception Source: " + e.Source + "\n";
                messageItem += "Exception ToString: " + e.ToString() + "\n";
                messageItem += "Exception StackTrace: " + e.StackTrace + "\n";

            }

            messageItem += "Message: " + msg + "\n";
            messageItem += "-----------End Message-----------------\n";

            string logFolder = MainFunctions.GetValueFromWebConfig("LogFolder");

            DirectoryInfo dir = new DirectoryInfo(logFolder);
            if (!dir.Exists)
                dir.Create();

            FileStream logFile = new FileStream(logFolder + "\\log.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            

            //lock (logFile)
            //{
                using (logFile)
                {
                    StreamWriter writer = new StreamWriter(logFile);
                    writer.WriteLine(messageItem);
                    writer.Close();
                }
                logFile.Close();
            //}


        }

    }
}
