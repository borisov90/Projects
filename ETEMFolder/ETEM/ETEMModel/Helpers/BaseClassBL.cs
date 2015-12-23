using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web;
using System.Xml;
using ETEMModel.Models;
using ETEMModel.Helpers.Admin;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using System.Linq.Expressions;
using System.Web.UI.HtmlControls;
using ETEMModel.Helpers.Common;
using ETEMModel.Helpers.Interfaces;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using NLog;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.Compator;
using ETEMModel.Models.LogModel;

using System.Reflection;
//using log4net;


namespace ETEMModel.Helpers
{
    public abstract class BaseClassBL<T> where T : Identifiable
    {
        private T savedEntity;
        protected ETEMDataModelEntities dbContext;
        private UMS_LOGEntities dbContextLog;
        protected string EntitySetName { get; set; }

        protected RequestMeasure RequestMeasure { get; set; }

        #region NLog
        private static Logger logger = LogManager.GetLogger("BaseClassBL");

        public static void LogDebug(string message)
        {

            logger.Debug(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
        }

        public static void LogError(string message)
        {

            logger.Error(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
        }

        public static void LogTrace(string message)
        {

            logger.Trace(DateTime.Now.ToString() + "\t" + "\tMessage: " + message);
        }
        #endregion


        public BaseClassBL()
        {
            dbContext = new ETEMDataModelEntities();


            this.RequestMeasure = new RequestMeasure("BaseClassBL");
        }

        internal abstract void EntityToEntity(T sourceEntity, T targetEntity);
        internal abstract T GetEntityById(int idEntity);



        public static IEnumerable<T> Sort(IEnumerable<T> source, string sortBy, string sortDirection, bool? alphaNumComparer = false)
        {
            if (sortBy == Constants.INVALID_ID_STRING || sortBy == string.Empty || sortBy == null || sortDirection == null)
            {
                return source;
            }

            var param = Expression.Parameter(typeof(T), "item");

            var sortExpression = Expression.Lambda<Func<T, object>>
                (Expression.Convert(Expression.Property(param, sortBy), typeof(object)), param);

            switch (sortDirection.ToUpper())
            {
                case "ASC":
                    if (alphaNumComparer == true)
                    {
                        source = source.AsQueryable<T>().OrderBy<T, object>(sortExpression, new AlphanumComparatorFast<object>());
                    }
                    else
                    {
                        source = source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
                    }



                    return source;
                default:
                    if (alphaNumComparer == true)
                    {
                        source = source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression, new AlphanumComparatorFast<object>());
                    }
                    else
                    {
                        source = source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
                    }

                    return source;
            }
        }

        internal List<E> GetAllEntities<E>() where E : class
        {
            List<E> list = new List<E>();
            list = this.dbContext.CreateObjectSet<E>().ToList<E>();
            return list;
        }

        protected bool HasUserActionPermission(T entity, CallContext outputContext, CallContext inputContext)
        {
            bool res = false;
            ///TODO: Проверка за правата
            if (res)
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                outputContext.Message = BaseHelper.GetCaptionString("Permission_Action_Denied");
                outputContext.EntityID = entity.EntityID.ToString();
            }

            return true;
        }



        /// <summary>
        /// Saves list of Entities
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="entities"></param>
        /// <param name="inputContext"></param>
        /// <param name="setNull"> this param is flag for refrection save, and it allows to set null or string empry as value of some prop</param>
        /// <param name="propertiesToChange">if this is != null, it means that only selected protoperties of the Entity will be saved</param>
        /// <returns></returns>

        public virtual CallContext EntitySave<E>(List<T> entities, CallContext inputContext, List<string> propertiesToChange = null, bool? allowNullAsPropvalue = null) where E : Identifiable
        {

            CallContext outputContext = new CallContext();
            outputContext.CurrentConsumerID = inputContext.CurrentConsumerID;

            try
            {
                SetCallContextDataFromSession(outputContext);

                //outputContext.CurrentConsumerNames = props.PersonTwoNamePlusTitle;
                //outputContext.CurrentConsumerSessionId = props.SessionID;
                //outputContext.CurrentConsumerID = props.IdUser;
                //
            }
            catch (Exception ex)
            {
                outputContext.CurrentConsumerID = inputContext.CurrentConsumerID;
                BaseHelper.Log("Грешка създаване на обекта сесия в модела на приложението!");
                BaseHelper.Log(ex.Message);
            }




            outputContext.PersonType = inputContext.PersonType;
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            outputContext.securitySettings = inputContext.securitySettings;
            outputContext.CurrentYear = inputContext.CurrentYear;
            outputContext.CurrentPeriod = inputContext.CurrentPeriod;


            List<Tuple<EventLog, T>> itemsToLog = new List<Tuple<EventLog, T>>();

            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                if (!HasUserActionPermission(entity, outputContext, inputContext))
                {
                    return outputContext;
                }

                if (entity.ValidateEntity(outputContext).Count > 0)
                {
                    return outputContext;
                }

                if (entity != null && entity.EntityID != Constants.INVALID_ID_ZERO && entity.EntityID != Constants.INVALID_ID)
                {
                    savedEntity = GetEntityById(entity.EntityID);

                    if (savedEntity != null)
                    {
                        outputContext.ActionName = Constants.ACTION_UPDATE;
                        if (propertiesToChange == null)
                        {
                            EntityToEntity(entity, savedEntity);
                        }
                        else
                        {
                            BaseGenericHelper<T>.EntityToEntityByReflection(entity, savedEntity, propertiesToChange, allowNullAsPropvalue);
                        }

                        if (savedEntity is IModifiable)
                        {
                            ((IModifiable)savedEntity).SetModificationData(outputContext);
                        }

                        outputContext.EntityID = savedEntity.EntityID.ToString();

                        outputContext.Message = BaseHelper.GetCaptionString("Entity_is_not_update");
                        outputContext.ResultCode = ETEMEnums.ResultEnum.Error;

                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            BaseHelper.Log("Грешка в BaseClassBL.EntitySave() - Update");
                            BaseHelper.Log(ex.Message);
                            BaseHelper.Log(ex.StackTrace);
                        }

                        outputContext.listKvEntityID.Add(new KeyValuePair<string, int>(EntitySetName, entity.EntityID));
                        outputContext.Message = BaseHelper.GetCaptionString("Entity_is_update_successful");
                        outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

                        EventLog simpleLog = MakeEventLog(outputContext, savedEntity);
                        itemsToLog.Add(new Tuple<EventLog, T>(simpleLog, savedEntity));
                    }
                    else
                    {
                        outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        outputContext.Message = String.Format(BaseHelper.GetCaptionString("Entity_Not_Found_By_ID"), Constants.INVALID_ID);
                    }
                }
                else
                {
                    outputContext.Message = BaseHelper.GetCaptionString("Entity_is_not_created");
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    outputContext.EntityID = Constants.INVALID_ID_STRING;

                    if (string.IsNullOrEmpty(this.EntitySetName))
                    {
                        outputContext.Message = BaseHelper.GetCaptionString("EntitySetName_is_not_set");
                    }

                    if (entity is IModifiable)
                    {
                        ((IModifiable)entity).SetCreationData(outputContext);
                    }

                    dbContext.AddObject(this.EntitySetName, entity);

                    try
                    {
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        BaseHelper.Log("Грешка в BaseClassBL.EntitySave() - Create");
                        BaseHelper.Log(ex.Message);
                        BaseHelper.Log(ex.StackTrace);
                    }

                    outputContext.ActionName = Constants.ACTION_INSERT;
                    outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    outputContext.EntityID = entity.EntityID.ToString();
                    outputContext.listKvEntityID.Add(new KeyValuePair<string, int>(EntitySetName, entity.EntityID));
                    outputContext.Message = BaseHelper.GetCaptionString("Entity_is_created_successful");

                    EventLog simpleLog = MakeEventLog(outputContext, entity);

                    itemsToLog.Add(new Tuple<EventLog, T>(simpleLog, entity));
                }
            }


            var application = System.Web.HttpContext.Current.Application;
            Setting setting = new Setting();

            Dictionary<string, Setting> dictionarySetting = new Dictionary<string, Setting>();

            if (application != null && application[Constants.APPLICATION_SETTING_LIST] != null)
            {
                dictionarySetting = application[Constants.APPLICATION_SETTING_LIST] as Dictionary<string, Setting>;

                if (dictionarySetting == null)
                {
                    setting = new Setting()
                    {
                        SettingName = "Създаване на подробен лог в базата данни(YES|NO)",
                        SettingDescription = "Създаване на подробен лог в базата данни(YES|NO)",
                        SettingIntCode = ETEMEnums.AppSettings.MakeLogInDB.ToString(),
                        SettingValue = "YES",
                        SettingDefaultValue = "YES",
                        SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
                    };

                    dictionarySetting.Add(ETEMEnums.AppSettings.MakeLogInDB.ToString(), setting);
                }
            }
            else
            {
                setting = new Setting()
                {
                    SettingName = "Създаване на подробен лог в базата данни(YES|NO)",
                    SettingDescription = "Създаване на подробен лог в базата данни(YES|NO)",
                    SettingIntCode = ETEMEnums.AppSettings.MakeLogInDB.ToString(),
                    SettingValue = "YES",
                    SettingDefaultValue = "YES",
                    SettingClass = ETEMEnums.AppSettingsClass.String.ToString()
                };
                dictionarySetting.Add(ETEMEnums.AppSettings.MakeLogInDB.ToString(), setting);
            }

            setting = dictionarySetting.Where(k => k.Key == ETEMEnums.AppSettings.MakeLogInDB.ToString()).FirstOrDefault().Value;

            if (setting.SettingValue == "YES")
            {
                if (itemsToLog.Count > 0)
                {
                    Thread t = new Thread(() => MakeFullDbLog(itemsToLog));
                    t.Start();
                }
            }

            return outputContext;
        }

        private static void SetCallContextDataFromSession(CallContext outputContext)
        {
            var session = System.Web.HttpContext.Current.Session;
            var props = session[Constants.SESSION_USER_PROPERTIES];//as UserProps;
            
            Type type = props.GetType();
            PropertyInfo name = type.GetProperty("PersonTwoNamePlusTitle");
            if (name != null)
            {
                outputContext.CurrentConsumerNames = (string)name.GetValue(props, null);
            }

            PropertyInfo sessionId = type.GetProperty("SessionID");
            if (sessionId != null)
            {
                outputContext.CurrentConsumerSessionId = (string)sessionId.GetValue(props, null);
            }

            PropertyInfo idUser = type.GetProperty("IdUser");
            if (idUser != null)
            {
                outputContext.CurrentConsumerID = (string)idUser.GetValue(props, null);
            }

            PropertyInfo page = type.GetProperty("Page");
            if (page != null)
            {
                outputContext.Page = (string)page.GetValue(props, null);
            }

            PropertyInfo securitySetting = type.GetProperty("SecuritySetting");
            if (securitySetting != null)
            {
                outputContext.SecuritySetting = (string)securitySetting.GetValue(props, null);
            }

            PropertyInfo securitySettingBG = type.GetProperty("SecuritySettingBG");
            if (securitySettingBG != null)
            {
                outputContext.SecuritySettingBG = (string)securitySettingBG.GetValue(props, null);
            }
        }

        public virtual CallContext EntitySave<E>(T entity, CallContext inputContext, List<string> propertiesToChange = null, bool? allowNullAsPropvalue = null) where E : Identifiable
        {
            CallContext outputContext = EntitySave<E>(new List<T>() { entity }, inputContext, propertiesToChange);
            return outputContext;
        }

        public virtual CallContext EntityDelete<E>(List<T> entities, CallContext inputContext)
        {
            CallContext outputContext = new CallContext();

            outputContext.CurrentConsumerID = inputContext.CurrentConsumerID;
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            // outputContext.securitySettings = inputContext.securitySettings;
            outputContext.ActionName = Constants.ACTION_DELETE;

            try
            {
                SetCallContextDataFromSession(outputContext);

                //outputContext.CurrentConsumerNames = props.PersonTwoNamePlusTitle;
                //outputContext.CurrentConsumerSessionId = props.SessionID;
                //outputContext.CurrentConsumerID = props.IdUser;
                //
            }
            catch (Exception ex)
            {
                outputContext.CurrentConsumerID = inputContext.CurrentConsumerID;
                BaseHelper.Log("Грешка създаване на обекта сесия в модела на приложението!");
                BaseHelper.Log(ex.Message);
            }

            List<Tuple<EventLog, T>> itemsToLog = new List<Tuple<EventLog, T>>();

            if (entities.Count > 0)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    if (!HasUserActionPermission(entity, outputContext, inputContext))
                    {
                        return outputContext;
                    }

                    if (entity != null && entity.EntityID != Constants.INVALID_ID_ZERO && entity.EntityID != Constants.INVALID_ID)
                    {
                        savedEntity = GetEntityById(entity.EntityID);

                        if (savedEntity != null)
                        {
                            outputContext.EntityID = savedEntity.EntityID.ToString();

                            outputContext.Message = BaseHelper.GetCaptionString("Entity_is_not_deleted");
                            outputContext.ResultCode = ETEMEnums.ResultEnum.Error;


                            dbContext.DeleteObject(savedEntity);

                            dbContext.SaveChanges();

                            outputContext.listKvEntityID.Add(new KeyValuePair<string, int>(EntitySetName, entity.EntityID));
                            outputContext.Message = BaseHelper.GetCaptionString("Entity_is_deleted_successful");
                            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;

                            EventLog simpleLog = MakeEventLog(outputContext, savedEntity);
                            itemsToLog.Add(new Tuple<EventLog, T>(simpleLog, entity));
                        }
                        else
                        {
                            outputContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                            outputContext.Message = String.Format(BaseHelper.GetCaptionString("Entity_Not_Found_By_ID"), Constants.INVALID_ID);
                        }
                    }
                    else
                    {
                        outputContext.Message = BaseHelper.GetCaptionString("Entity_is_not_created");
                        outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                        outputContext.EntityID = Constants.INVALID_ID_STRING;
                    }
                }


                if (itemsToLog.Count > 0)
                {
                    Thread t = new Thread(() => MakeFullDbLog(itemsToLog));
                    t.Start();
                }

                return outputContext;
            }
            else
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                outputContext.Message = BaseHelper.GetCaptionString("The_list_of_entities_was_empty");
                return outputContext;
            }
        }

        public virtual CallContext EntityDelete<E>(T entity, CallContext inputContext)
        {
            CallContext outputContext = EntityDelete<E>(new List<T>() { entity }, inputContext);
            return outputContext;
        }

        public void SetEventLogInfoFull(EventFullLog fullEvent, EventLog simpleEvent)
        {
            fullEvent.idUser = simpleEvent.idUser;
            fullEvent.EventDate = simpleEvent.EventDate;
            fullEvent.EventActionType = simpleEvent.EventAction;
            fullEvent.EntityActionID = simpleEvent.EntityID;
            fullEvent.PersonSessionID = simpleEvent.SessionId;
            fullEvent.EntityActionName = simpleEvent.EntityName;
            fullEvent.PersonMakingAction = simpleEvent.PersonName;
            fullEvent.EventMessage = simpleEvent.EventMessage;
            fullEvent.Page = simpleEvent.Page;
            fullEvent.SecuritySetting = simpleEvent.SecuritySetting;



        }

        public virtual EventLog GetEventLogInfo(CallContext outputContext, T savedEntity)
        {
            EventLog result = new EventLog();

            int currentConsumerID = Constants.INVALID_ID;
            Person currentPerson = null;

            if (!string.IsNullOrEmpty(outputContext.CurrentConsumerSessionId) && !string.IsNullOrEmpty(outputContext.CurrentConsumerNames))
            {
                result.idUser = int.Parse(outputContext.CurrentConsumerID);
                result.SessionId = outputContext.CurrentConsumerSessionId;
                result.PersonName = outputContext.CurrentConsumerNames;
                result.Page = outputContext.Page;
                result.EventMessage = outputContext.SecuritySettingBG;
                result.SecuritySetting = outputContext.SecuritySetting;
            }
            else
            {


                if (Int32.TryParse(outputContext.CurrentConsumerID, out currentConsumerID))
                {
                    currentPerson = new UserBL().GetCurrentPersonByUserId(Int32.Parse(outputContext.CurrentConsumerID));
                }
                else
                {
                    currentConsumerID = Constants.INVALID_ID;
                }

                result.PersonName = currentPerson != null ? currentPerson.FullName : " неопределен ";
                result.idUser = currentConsumerID;
            }
            try
            {

                result.EventDate = DateTime.Now;
                //  result.EventAction = outputContext.securitySettings.ToString();
                result.EventAction = outputContext.ActionName;
                result.EntityID = savedEntity.EntityID.ToString();
                result.EntityName = savedEntity.GetType().FullName;



                if (string.IsNullOrEmpty(result.EventMessage) && outputContext.securitySettings.ToString().Contains("Save"))
                {
                    result.EventMessage = "Запис на данни";
                }
            }
            catch (Exception e)
            {
                BaseHelper.Log("Грешка при създаване на EventLog за entity " + this.savedEntity.GetType().FullName);
                BaseHelper.Log(e.Message);



                BaseHelper.LogToMail("Грешка при създаване на EventLog за entity " + this.savedEntity.GetType().FullName);
                BaseHelper.LogToMail(e.Message);
            }

            return result;

        }

        protected EventLog MakeEventLog(CallContext outputContext, T savedEntity)
        {
            EventLog eventLog = null;
            try
            {
                if (outputContext.ResultCode == ETEMEnums.ResultEnum.Success ||
                    outputContext.ResultCode == ETEMEnums.ResultEnum.Warning)
                {
                    eventLog = GetEventLogInfo(outputContext, savedEntity);
                    this.dbContext.AddToEventLogs(eventLog);
                    this.dbContext.SaveChanges();

                }
            }
            catch (ETEMModelException e)
            {
                BaseHelper.Log("Грешка при създаване на EventLog на entity " + savedEntity.GetType().FullName);
                BaseHelper.Log(e.Message);
            }


            return eventLog;
        }

        #region MakeFullDbLog



        public static void SetAllProperties(XmlDocument doc, T savedEntity)
        {
            Type savedEntityType = savedEntity.GetType();

            var savedEntityProperties = savedEntityType.GetProperties();

            for (int i = 0; i < savedEntityProperties.Length; i++)
            {
                var propSourse = savedEntityProperties[i];
                if (propSourse.CanRead && propSourse.CanWrite)
                {
                    var valueEntity = savedEntityType.GetProperty(propSourse.Name);
                    var value = "";
                    try
                    {
                        value = valueEntity.GetValue(savedEntity, null) != null ? valueEntity.GetValue(savedEntity, null).ToString() : null;
                    }
                    catch (Exception ex)
                    {
                        //sometimes throws exception becouse it's the prop has complex getter like:
                        //public string CourseText { get; set; }
                        // public int CourseTextInt { get { return int.Parse(this.CourseText); } set {this.CourseTextInt=value ;} }
                        continue;
                    }

                    string propertyName = valueEntity.Name.Trim(' ');
                    if (value != null && value.ToString() != "" && !string.IsNullOrEmpty(propertyName)

                        && !value.ToString().Contains("EntityCollection")
                        && !value.ToString().Contains("EntityReference"))
                    {
                        XMLLogHelper.AppendValueToMainNode(doc, propertyName, value.ToString());
                    }

                }
            }

        }



        private void MakeFullDbLog(List<Tuple<EventLog, T>> itemsToLog)
        {
            //to make full db log the Entity needs to be ILoggable
            try
            {


                using (dbContextLog = new UMS_LOGEntities())
                {
                    for (int i = 0; i < itemsToLog.Count; i++)
                    {


                        EventFullLog eventFullLog = new EventFullLog();
                        SetEventLogInfoFull(eventFullLog, itemsToLog[i].Item1);


                        XmlDocument doc = XMLLogHelper.CreateNewXMLDoc(eventFullLog.EntityActionName);
                        SetAllProperties(doc, itemsToLog[i].Item2);
                        eventFullLog.FullLogPropertiesXML = XMLLogHelper.SringValueOf(doc);


                        //make async log in a different database by the simple log and the entity that is being saved
                        if (typeof(ICustomLoggable).IsAssignableFrom(typeof(T)))
                        {
                            ((ICustomLoggable)itemsToLog[i].Item2).CreateMeaningfulEntityLogXML(eventFullLog);
                        }


                        dbContextLog.AddToEventFullLogs(eventFullLog);
                        dbContextLog.SaveChanges();
                    }

                }

            }
            catch (Exception e)
            {

                BaseHelper.Log("Грешка при детайлно създаване на EventLog на entity " + savedEntity.GetType().FullName);
                BaseHelper.Log(e.Message);
            }
        }

        //private void MakeFullDbLog(EventLog eventSimpleLog, T savedEntity)
        //{
        //    //to make full db log the Entity needs to be ILoggable
        //    try
        //    {


        //            using (dbContextLog = new UMS_LOGEntities())
        //            {
        //                EventFullLog eventFullLog = new EventFullLog();
        //                SetEventLogInfoFull(eventFullLog, eventSimpleLog);


        //                XmlDocument doc = XMLLogHelper.CreateNewXMLDoc(eventFullLog.EntityActionName);
        //                SetAllProperties(doc, savedEntity);
        //                eventFullLog.FullLogPropertiesXML = XMLLogHelper.SringValueOf(doc);


        //                //make async log in a different database by the simple log and the entity that is being saved
        //                if (typeof(ICustomLoggable).IsAssignableFrom(typeof(T)))
        //                {
        //                    ((ICustomLoggable)savedEntity).CreateMeaningfulEntityLogXML(eventFullLog);
        //                }


        //                dbContextLog.AddToEventFullLogs(eventFullLog);
        //                dbContextLog.SaveChanges();
        //            }

        //    }
        //    catch (Exception e)
        //    {

        //        BaseHelper.Log("Грешка при детайлно създаване на EventLog на entity " + savedEntity.GetType().FullName);
        //        BaseHelper.Log(e.Message);
        //    }
        //}



        #endregion


        #region Disciplines by period


     


        internal SoursePeriodsData CalculateSourseActualData(int idAcademiYear, int idPeriod)
        {
            SoursePeriodsData data = new SoursePeriodsData();
            data.AcademicYear = dbContext.KeyValues.FirstOrDefault(kv => kv.idKeyValue == idAcademiYear).Name;
            data.Period = int.Parse(dbContext.KeyValues.FirstOrDefault(kv => kv.idKeyValue == idPeriod).DefaultValue1);
            string[] protocolPeriodValues = data.AcademicYear.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
            data.StartYear = int.Parse(protocolPeriodValues[0]);
            data.EndYear = int.Parse(protocolPeriodValues[1]);
            data.CurrentYear = data.Period == 1 ? data.StartYear : data.EndYear;

            return data;
        }

       






      

        #endregion

        protected Control FindControlById(Control holder, string idControl)
        {
            if (holder.ID == idControl)
            {
                return holder;
            }
            if (holder.HasControls())
            {
                Control temp;
                foreach (Control subcontrol in holder.Controls)
                {
                    temp = FindControlById(subcontrol, idControl);
                    if (temp != null)
                    {
                        return temp;
                    }
                }
            }
            return null;
        }

        public List<int> TakeGridSelectedCellsMasterRowKeys(GridView grid, string checkBoxIdName, string hdnRowMasterKeyName)
        {
            List<int> listUpdatedDetails = new List<int>();


            for (int i = 0; i < grid.Rows.Count; i++)
            {
                GridViewRow row = grid.Rows[i];
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cbxGroupedDiscplines = FindControlById(row, checkBoxIdName) as CheckBox;
                    if (cbxGroupedDiscplines.Checked)
                    {
                        HiddenField hdnRowMasterKey = FindControlById(row, hdnRowMasterKeyName) as HiddenField;
                        if (hdnRowMasterKey != null)
                        {

                            listUpdatedDetails.Add(int.Parse(hdnRowMasterKey.Value));
                        }
                    }

                }
            }

            return listUpdatedDetails;
        }


    }
}