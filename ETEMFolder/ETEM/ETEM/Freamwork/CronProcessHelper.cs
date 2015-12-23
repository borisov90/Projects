using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using ETEMModel.Helpers;
using ETEMModel.Models;
using NLog;

namespace ETEM.Freamwork
{
    public class CronProcessHelper
    {
        private static Logger log = LogManager.GetLogger("CronProcessHelper");

        //AdminServiceReference.AdministrationClient AdminClientRef = new AdminServiceReference.AdministrationClient();
        public ETEMModel.Services.Admin.Administration AdminClientRef = new ETEMModel.Services.Admin.Administration();

        private Setting GetSettingByCode(ETEMEnums.AppSettings appSettings)
        {

            Setting settings = AdminClientRef.GetSettingByCode(appSettings.ToString());

            return settings;

        }

        public void StartThread()
        {
            Setting settings = GetSettingByCode(ETEMEnums.AppSettings.CronProcessStart);

            if (settings.SettingValue.ToLower().Equals("yes"))
            {
                Thread thread = new Thread(CronThread);
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void CronThread()
        {
            while (true)
            {
                Setting settings = GetSettingByCode(ETEMEnums.AppSettings.CronProcessStartPeriod);
                double cronProcessStartPeriod = Convert.ToDouble(settings.SettingValue);

                Thread.Sleep(TimeSpan.FromMinutes(cronProcessStartPeriod));
                log.Debug("CronThread at time:" + DateTime.Now.ToString());

                CronProcessHelper cronProcessHelper = new CronProcessHelper();
                cronProcessHelper.execute();

            }
        }

        public void execute()
        {

            List<string> listExecuteDate = new List<string>();


            CronProcessHistory lastCronProcessHistory = AdminClientRef.GetLastCronProcessHistory();


            DateTime startDate = DateTime.Now;
            if (lastCronProcessHistory != null && lastCronProcessHistory.ExecuteDateFormatted.HasValue)
            {
                startDate = lastCronProcessHistory.ExecuteDateFormatted.Value;
            }

            while (startDate.Date < DateTime.Now.Date)
            {
                startDate = startDate.AddDays(1);
                listExecuteDate.Add(startDate.ToString("dd.MM.yyyy"));
            }




            CallContext resultContext;
            foreach (string executeDate in listExecuteDate)
            {
                resultContext = CreateCronProcessHistory(executeDate);


                UpdateCronProcessHistoryResult(resultContext.EntityID);
            }


            log.Debug("CronProcessExecution finished at " + DateTime.Now.ToString());
        }

        private void UpdateCronProcessHistoryResult(string entityID)
        {
            CronProcessHistory cronProcess = AdminClientRef.GetCronProcessHistoryByID(entityID);

            CallContext resultContext = new CallContext();
            resultContext.CurrentConsumerID = "1";

            cronProcess.Successful = true;
            cronProcess.EndTime = DateTime.Now;
            cronProcess.Exception = "Успешно приключен";


            AdminClientRef.CronProcessHistorySave(cronProcess, resultContext);

        }

        private CallContext CreateCronProcessHistory(string executeDate)
        {
            CronProcessHistory cronProcess = new CronProcessHistory();

            CallContext resultContext = new CallContext();
            resultContext.CurrentConsumerID = "1";

            cronProcess.ExecuteDate = executeDate;
            cronProcess.RunTime = DateTime.Now;
            cronProcess.Successful = false;
            cronProcess.Exception = "Процеса е стартиран";

            return resultContext = AdminClientRef.CronProcessHistorySave(cronProcess, resultContext);
        }



    }




}


