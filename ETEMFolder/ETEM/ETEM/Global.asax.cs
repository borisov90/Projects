using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ETEM.Freamwork;
using System.Threading;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Helpers.Admin;
using ETEMModel.Helpers.Settings;

namespace ETEM
{
    public class Global : System.Web.HttpApplication
    {
        // public static ILog log = LogManager.GetLogger(typeof(Global));

        void Application_Start(object sender, EventArgs e)
        {


            //log4net.Config.DOMConfigurator.Configure();
            // GeneralPage.Log("ETEM Application Start at " + DateTime.Now);


            BaseSettingHelper bsh = new BaseSettingHelper();
            bsh.MergeSettingsAll();


            Dictionary<string, HttpSessionState> sessionData = new Dictionary<string, HttpSessionState>();
            Application[Constants.APPLICATION_ALL_SESSIONS] = sessionData;

            try
            {
                CronProcessHelper cronProcess = new CronProcessHelper();
                cronProcess.StartThread();
            }
            catch (Exception ex)
            {
                BaseHelper.Log(ex.ToString());
            }

        }






        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            Dictionary<string, HttpSessionState> sessionData =
                (Dictionary<string, HttpSessionState>)Application[Constants.APPLICATION_ALL_SESSIONS];

            if (sessionData.Keys.Contains(HttpContext.Current.Session.SessionID))
            {
                sessionData.Remove(HttpContext.Current.Session.SessionID);
                sessionData.Add(HttpContext.Current.Session.SessionID, HttpContext.Current.Session);
            }
            else
            {
                sessionData.Add(HttpContext.Current.Session.SessionID, HttpContext.Current.Session);
            }

            GeneralPage.LogDebug("Session_Start at " + DateTime.Now + " SessionID = " + HttpContext.Current.Session.SessionID);


            Application[Constants.APPLICATION_ALL_SESSIONS] = sessionData;

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

            //GeneralPage.LogDebug("Session_End at" + DateTime.Now + " SessionID = " + HttpContext.Current.Session.SessionID);

            // Dictionary<string, HttpSessionState> sessionData =
            //     (Dictionary<string, HttpSessionState>)Application[Constants.APPLICATION_ALL_SESSIONS];
            // if (HttpContext.Current != null)
            // {
            //     sessionData.Remove(HttpContext.Current.Session.SessionID);
            // }
            // Application[Constants.APPLICATION_ALL_SESSIONS] = sessionData;

        }

    }
}
