using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Common;
using ETEMModel.Models;
using ETEMModel.Helpers;
using ETEMModel.Helpers.Admin;

using ETEMModel.Models.Partial;
using ETEMModel.Helpers.Share;


namespace ETEMModel.Services.Common
{

    public class Common : ICommon
    {

        #region ICommon Members

        public List<NotificationDataView> GetNotificationDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new NotificationBL().GetNotificationDataView(searchCriteria, sortExpression, sortDirection);
        }

        public Notification GetNotificationByID(string _entityID)
        {
            return new NotificationBL().GetEntityById(Int32.Parse(_entityID));
        }

        public int GetNotificationCountByPersonID(string idPerson)
        {
            return new NotificationBL().GetNotificationCountByPersonID(Int32.Parse(idPerson));
        }

        public CallContext NotificationSave(Notification entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.NotificationSave;
            CallContext resContext = new NotificationBL().EntitySave<Notification>(entity, resultContext);

            return resContext;
        }

        public int GetMaxNotificationID()
        {
            return new NotificationBL().GetMaxNotificationID();
        }

        public CallContext UploadedFileSave(UploadedFile entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.UploadedFileSave;
            CallContext resContext = new UploadedFileBL().EntitySave<UploadedFile>(entity, resultContext);

            return resContext;
        }

        public List<UploadedFile> GetUploadedFile(string idPerson)
        {
            return new UploadedFileBL().GetUploadedFile(Int32.Parse(idPerson));
        }

        public UploadedFile GetUploadFileByID(string _entityID)
        {
            return new UploadedFileBL().GetEntityById(Int32.Parse(_entityID));
        }

        /// <summary>
        /// Метода изпраща съобщение 
        /// </summary>
        /// <param name="idSendFromPerson">Изпратено от</param>
        /// <param name="idSendToPerson">Изпратено до</param>
        /// <param name="idStatus">set- ва се на статус "Изпратено" ( this.AdminClientRef.GetKeyValueIdByIntCode("NotificationStatus", "Submitted") )</param>
        /// <param name="about">Относно</param>
        /// <param name="comment">Коментар</param>
        /// <returns>Връща ID на новото съобщение</returns>
        public int SendNotification(int idSendFromPerson, int idSendToPerson, int idStatus, string about, string comment)
        {
            int result = Constants.INVALID_ID;

            Notification saveEntity = new Notification();

            saveEntity.About = about;
            saveEntity.Comment = comment;
            saveEntity.idSendFrom = idSendFromPerson;
            saveEntity.idSendTo = idSendToPerson;
            saveEntity.idStatus = idStatus;
            saveEntity.LastUpdate = DateTime.Now;
            saveEntity.SendDate = DateTime.Now;

            CallContext resultContext = new CallContext();
            resultContext = NotificationSave(saveEntity, resultContext);

            return result;
        }

        #endregion

        

      

      


      
       





       


        

      

        public List<AttachmentDataView> GetAccountingAttachmentList(ICollection<AbstractSearch> searchCriteria, string GridViewSortExpression, string GridViewSortDirection)
        {
            List<AttachmentDataView> list = new List<AttachmentDataView>();

            list = new AttachmentBL().GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);
            return list;
        }

        public Attachment GetAttachmentID(string idEntity)
        {
            return new AttachmentBL().GetEntityById(Int32.Parse(idEntity));
        }

        public CallContext AttachmentSave(Attachment entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.AttachmentSave;
            CallContext resContext = new AttachmentBL().EntitySave<Attachment>(entity, resultContext);

            return resContext;
        }
    }
}
