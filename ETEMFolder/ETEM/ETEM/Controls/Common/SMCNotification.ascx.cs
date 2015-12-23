using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models;
using ETEMModel.Helpers;
using System.Globalization;

using ETEM.Share;
using ETEMModel.Models.DataView.Admin;

namespace ETEM.Controls.Common
{
    public partial class SMCNotification : BaseUserControl
    {
        private Notification currentEntity;
        List<Person> personToNotice;
        Person personToSend;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //получател
            if (this.ucSendToPerson.SelectedValue == Constants.INVALID_ID_STRING && this.ddlGroup.SelectedValue == Constants.INVALID_ID_STRING && this.gvSendNoticeTo.Rows.Count == 0)
            {
                AddErrorMessage(this.lbResultContext, BaseHelper.GetCaptionString("Notification_SendTo_Mandatory"));
                return;
            }


            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new Notification();
            }
            else
            {
                this.currentEntity = this.ownerPage.CommonClientRef.GetNotificationByID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Notification_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    string falseResult = String.Format(BaseHelper.GetCaptionString("Entity_is_not_update"));

                    this.ownerPage.FormLoad();
                    return;
                }
            }

            DateTimeFormatInfo dtf = new DateTimeFormatInfo();
            dtf.DateSeparator = Constants.DATE_SEPARATOR;
            dtf.ShortDatePattern = Constants.SHORT_DATE_TIME_PATTERN;

            //11.4.2011 г. 16:27:43
            DateTime dtSendDate;
            if (!DateTime.TryParse(this.tbxSendDate.Text, dtf, DateTimeStyles.None, out dtSendDate))
            {
                dtSendDate = DateTime.Now.Date;
            }


            currentEntity.idSendFrom = Convert.ToInt32(this.ownerPage.UserProps.PersonID);
            currentEntity.About = this.tbxAbout.Text;
            currentEntity.Comment = this.tbxComment.Text;
            currentEntity.idStatus = Convert.ToInt32(this.ddlStatus.SelectedValue);
            currentEntity.SendDate = dtSendDate;
            currentEntity.LastUpdate = DateTime.Now;
            currentEntity.isReading = false;

            CallContext resultContext = new CallContext();
            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            if (!string.IsNullOrEmpty(this.ucSendToPerson.SelectedValue) && this.ucSendToPerson.SelectedValue != Constants.INVALID_ID_STRING)
            {
                currentEntity.idSendTo = Convert.ToInt32(this.ucSendToPerson.SelectedValue);
                resultContext = this.ownerPage.CommonClientRef.NotificationSave(currentEntity, resultContext);
            }


            if (this.gvSendNoticeTo.Rows.Count > 0)
            {
                HiddenField hdnRowDetailKey;

                int maxIDNotification = this.ownerPage.CommonClientRef.GetMaxNotificationID();

                foreach (GridViewRow row in this.gvSendNoticeTo.Rows)
                {
                    hdnRowDetailKey = row.FindControl("hdnRowDetailKey") as HiddenField;
                    this.currentEntity = new Notification();

                    currentEntity.idSendFrom = Convert.ToInt32(this.ownerPage.UserProps.PersonID);
                    currentEntity.About = this.tbxAbout.Text;
                    currentEntity.Comment = this.tbxComment.Text;
                    currentEntity.idStatus = Convert.ToInt32(this.ddlStatus.SelectedValue);
                    currentEntity.SendDate = dtSendDate;
                    currentEntity.LastUpdate = DateTime.Now;
                    currentEntity.isReading = false;
                    currentEntity.idSendTo = Convert.ToInt32(hdnRowDetailKey.Value);
                    currentEntity.LinkFile = maxIDNotification.ToString();

                    resultContext = new CallContext();
                    resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
                    resultContext = this.ownerPage.CommonClientRef.NotificationSave(currentEntity, resultContext);
                }
            }
            //съобщението се изпраща от студент
            else if (this.ddlGroup.SelectedValue != Constants.INVALID_ID_STRING && string.IsNullOrEmpty(this.hdnRowMasterKey.Value) &&
                 !this.ownerPage.CheckUserActionPermissionByIntCode(ETEMEnums.SecuritySettings.UsingFullFunctionalityNotif))
            {
                int maxIDNotification = this.ownerPage.CommonClientRef.GetMaxNotificationID();
                Group group = this.AdminClientRef.GetGroupByID(this.ddlGroup.SelectedValueINT);
                List<GroupPersonLinkDataView> groupPersonLink = this.AdminClientRef.GetGroupPersonLinkDataViewByGroupID(group.idGroup);

                //записваме 1 главно съобщение, което ще се показв като изходящо съобщение ако е изпратено до група, 
                //а не да се виждат всички изходящи съобщения до всеки член на групата
                this.currentEntity = new Notification();
                currentEntity.idSendFrom = Convert.ToInt32(this.ownerPage.UserProps.PersonID);
                //currentEntity.idSendTo          = Convert.ToInt32(this.ownerPage.UserProps.PersonID);
                currentEntity.About = this.tbxAbout.Text;
                currentEntity.Comment = this.tbxComment.Text;
                currentEntity.idStatus = Convert.ToInt32(this.ddlStatus.SelectedValue);
                currentEntity.SendDate = dtSendDate;
                currentEntity.LastUpdate = DateTime.Now;
                currentEntity.isReading = false;
                //изпратено до - записваме id на групата до която е изпратено съобщението
                currentEntity.idGroup = this.ddlGroup.SelectedValueINT;
                currentEntity.ParentID = Constants.INVALID_ID;
                currentEntity.LinkFile = maxIDNotification.ToString();

                resultContext = new CallContext();
                resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
                string parentID = this.ownerPage.CommonClientRef.NotificationSave(currentEntity, resultContext).EntityID;

                foreach (var item in groupPersonLink)
                {
                    this.currentEntity = new Notification();
                    currentEntity.idSendFrom = Convert.ToInt32(this.ownerPage.UserProps.PersonID);
                    currentEntity.About = this.tbxAbout.Text;
                    currentEntity.Comment = this.tbxComment.Text;
                    currentEntity.idStatus = Convert.ToInt32(this.ddlStatus.SelectedValue);
                    currentEntity.SendDate = dtSendDate;
                    currentEntity.LastUpdate = DateTime.Now;
                    currentEntity.isReading = false;
                    currentEntity.idSendTo = item.idPerson;
                    currentEntity.ParentID = Convert.ToInt32(parentID);
                    currentEntity.LinkFile = maxIDNotification.ToString();

                    resultContext = new CallContext();
                    resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
                    resultContext = this.ownerPage.CommonClientRef.NotificationSave(currentEntity, resultContext);
                }
            }


            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.CurrentEntityMasterID = resultContext.EntityID;

                UserControlLoad();

                this.lbResultContext.Text = BaseHelper.GetCaptionString("Message_Sent_Successfully");
            }
            else
            {
                this.lbResultContext.Text = BaseHelper.GetCaptionString("Message_Sent_NOTSuccessfully");
            }


            CheckIfResultIsSuccess(this.lbResultContext);

            //this.ownerPage.FormLoad();

            //this.pnlFormData.Visible = false;
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            loadInitControls();

            this.currentEntity = this.ownerPage.CommonClientRef.GetNotificationByID(this.CurrentEntityMasterID);

            if (currentEntity != null)
            {
                this.lbHeaderText.Text = "Преглед на съобщение";

                //съществуващо съобщение не може да се редактира
                this.btnSave.Enabled = false;
                this.btnAddNoticeTo.Enabled = false;
                this.btnDeleteNoticeTo.Enabled = false;
                this.fuNotificationFile.BtnUploadFileEnabled = true;

                this.ucSendFromPerson.SelectedValue = this.currentEntity.idSendFrom.ToString();
                this.ucSendFromPerson.Text = this.AdminClientRef.GetPersonByPersonID(this.currentEntity.idSendFrom.ToString()).FullName;

                if (this.currentEntity.ParentID == null)
                {
                    this.ucSendToPerson.SelectedValue = this.currentEntity.idSendTo.ToString();
                    this.ucSendToPerson.Text = this.AdminClientRef.GetPersonByPersonID(this.currentEntity.idSendTo.ToString()).FullName;
                }
                else if (this.currentEntity.ParentID == Constants.INVALID_ID)
                {
                    this.ddlGroup.SelectedValue = this.currentEntity.idGroup.ToString();
                    this.ucSendToPerson.SelectedValue = Constants.INVALID_ID_STRING;
                    this.ucSendToPerson.Text = string.Empty;
                }

                this.tbxSendDate.Text = this.currentEntity.SendDate.Value.ToString(Constants.SHORT_DATE_PATTERN);
                this.tbxAbout.Text = this.currentEntity.About;
                this.tbxComment.Text = this.currentEntity.Comment;
                this.ddlStatus.SelectedValue = this.currentEntity.idStatus.ToString();
                this.hdnRowMasterKey.Value = currentEntity.EntityID.ToString();

                //update статуса на съобщението и дата на последна промяна
                this.currentEntity.idStatus = Convert.ToInt32(this.ddlStatus.SelectedValue);
                this.currentEntity.LastUpdate = DateTime.Now;
                //маркираме съобщението като прочетено ако го отвори този, до когото е адресирано съобщението
                if (this.ownerPage.UserProps.PersonID == this.currentEntity.idSendTo.ToString())
                {
                    this.currentEntity.isReading = true;
                    this.fuNotificationFile.BtnUploadFileEnabled = false;
                }
                else
                {
                    this.fuNotificationFile.BtnUploadFileEnabled = true;
                }

                //Прикачени файлове
                //сетваме стойност на полето LinkFile когато изпращаме съобщение до група от хора
                if (currentEntity.LinkFile == null)
                {
                    this.fuNotificationFile.CustomFolder = this.CurrentEntityMasterID + "_" +
                                                           this.currentEntity.SendDate.Value.ToString(Constants.DATE_SHORT_PATTERN_FOR_FILE_SUFFIX) +
                                                           "_Notification";
                }
                else
                {
                    this.fuNotificationFile.CustomFolder = this.currentEntity.LinkFile + "_" +
                                                           this.currentEntity.SendDate.Value.ToString(Constants.DATE_SHORT_PATTERN_FOR_FILE_SUFFIX) +
                                                           "_Notification";
                }

                this.fuNotificationFile.UserControlLoad();

                CallContext resultContext = new CallContext();
                resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
                resultContext = this.CommonClientRef.NotificationSave(currentEntity, resultContext);

                

                this.lnkBtnPreviewDocument.Visible = true;

                

                this.lbResultContext.Attributes.Remove("class");

                this.ownerPage.FormLoad();
            }
            else
            {
                this.lbHeaderText.Text = "Изпращане на съобщение";

                SetEmptyValues();
            }
        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.ucSendToPerson.SelectedValue = Constants.INVALID_ID_STRING;
            this.ucSendToPerson.Text = string.Empty;

            this.tbxAbout.Text = string.Empty;
            this.tbxAbout.Attributes.Add("placeholder", BaseHelper.GetCaptionString("Placeholder_NotifAbout"));

            this.tbxComment.Text = string.Empty;
            this.tbxComment.Attributes.Add("placeholder", BaseHelper.GetCaptionString("Placeholder_NotifComment"));

            //дата и час на изпращане
            this.tbxSendDate.Text = DateTime.Now.ToString(Constants.SHORT_DATE_PATTERN);
            //статус изпратено
            this.ddlStatus.SelectedValue = this.AdminClientRef.GetKeyValueIdByIntCode("NotificationStatus", "Submitted").ToString();

            this.hdnRowMasterKey.Value = string.Empty;
            this.btnSave.Enabled = true;
            this.btnAddNoticeTo.Enabled = true;
            this.btnDeleteNoticeTo.Enabled = true;
            //'Изпратено от' не може да се редактира
            this.ucSendFromPerson.SelectedValue = this.ownerPage.UserProps.PersonID;
            this.ucSendFromPerson.Text = this.ownerPage.UserProps.PersonNamePlusTitle;
            this.ucSendFromPerson.ReadOnly = true;

            this.lnkBtnPreviewDocument.Text = string.Empty;
            this.lnkBtnPreviewDocument.Visible = false;

            this.gvSendNoticeTo.DataSource = null;
            this.gvSendNoticeTo.DataBind();

            //this.fuNotificationFile.BtnUploadFileEnabled    = false;
            this.fuNotificationFile.ClearGrid();

            ///
            //Прикачени файлове
            //сетваме стойност на полето LinkFile когато изпращаме съобщение до група от хора
            //if (currentEntity.LinkFile == null)
            //{
            //    this.fuNotificationFile.CustomFolder = this.CurrentEntityMasterID + "_" +
            //                                           this.currentEntity.SendDate.Value.ToString(Constants.DATE_SHORT_PATTERN_FOR_FILE_SUFFIX) +
            //                                           "_Notification";
            //}
            //else
            //{
            //    this.fuNotificationFile.CustomFolder = this.currentEntity.LinkFile + "_" +
            //                                           this.currentEntity.SendDate.Value.ToString(Constants.DATE_SHORT_PATTERN_FOR_FILE_SUFFIX) +
            //                                           "_Notification";
            //}

            //this.fuNotificationFile.UserControlLoad();

            ///
        }

        private void loadInitControls()
        {
            ClearResultContext(lbResultContext);

            this.ddlStatus.UserControlLoad();

            this.pnlFormData.Visible = true;

            //дата на изпращане не може да се редактира
            this.tbxSendDate.Enabled = false;
            //статуса не може да се редактира
            this.ddlStatus.DropDownListCTRL.Enabled = false;


            if (this.ownerPage.CheckUserActionPermissionByIntCode(ETEMEnums.SecuritySettings.UsingFullFunctionalityNotif))
            {
                this.ucSendToPerson.Enabled = true;
                this.divBtnAddDelete.Visible = true;
                this.divGvSendNoticeTo.Visible = true;
                this.ddlGroup.AdditionalParam = string.Empty;
            }
            else
            {
                this.ucSendToPerson.Enabled = false;
                this.divBtnAddDelete.Visible = false;
                this.divGvSendNoticeTo.Visible = false;
                this.ddlGroup.AdditionalParam = "RestrictedVisibility";
            }

            //групи
            this.ddlGroup.UserControlLoad();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlFormData.Visible = false;
        }

        protected void lnkBtnPreviewDocument_Click(object sender, EventArgs e)
        {
            string linkedDocType = this.hdnLinkedDocument.Value.Split(',')[0];
            string idDocument = this.hdnLinkedDocument.Value.Split(',')[1];

            btnCancelParentPanel_OnClick(ImageButton3, e);

          
            

        }

        protected void btnAddNoticeTo_Click(object sender, EventArgs e)
        {
            if (this.ddlGroup.SelectedValue != Constants.INVALID_ID_STRING && string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                //List<OrderUserLink> list = new List<OrderUserLink>();
                CallContext resultContext = new CallContext();

                //дублиране на лицата за известяване                
                personToNotice = new List<Person>();

                //Добавяне на списък потребители от група
                if (!string.IsNullOrEmpty(this.ddlGroup.SelectedValue) && this.ddlGroup.SelectedValue != Constants.INVALID_ID_STRING)
                {
                    Group group = this.AdminClientRef.GetGroupByID(this.ddlGroup.SelectedValueINT);
                    List<GroupPersonLinkDataView> groupPersonLink = this.AdminClientRef.GetGroupPersonLinkDataViewByGroupID(group.idGroup);
                    int? idUser;

                    foreach (var item in groupPersonLink)
                    {
                        idUser = this.ownerPage.AdminClientRef.GetUserByPersonID(item.idPerson).idUser;

                        if (personToNotice.Where(d => idUser.HasValue && d.idPerson == item.idPerson).Count() == 0)
                        {
                            personToNotice.Add(new Person
                            {
                                idPerson = item.idPerson,
                                Title = item.Title,
                                FirstName = item.FirstName,
                                LastName = item.LastName
                            }
                            );
                        }
                    }
                }

                HiddenField hdnRowDetailKey;
                CheckBox chbxDeleteNotificationTo;

                //добавяме и редовете от грида, ако има такива
                foreach (GridViewRow row in this.gvSendNoticeTo.Rows)
                {
                    hdnRowDetailKey = row.FindControl("hdnRowDetailKey") as HiddenField;
                    chbxDeleteNotificationTo = row.FindControl("chbxDeleteNotificationTo") as CheckBox;

                    if (!chbxDeleteNotificationTo.Checked)
                    {
                        personToSend = this.AdminClientRef.GetPersonByPersonID(hdnRowDetailKey.Value);

                        if (personToNotice != null && personToNotice.Where(z => z.idPerson == personToSend.idPerson).Count() == 0)
                        {
                            personToNotice.Add(new Person
                            {
                                idPerson = personToSend.idPerson,
                                Title = personToSend.Title,
                                FirstName = personToSend.FirstName,
                                LastName = personToSend.LastName
                            }
                            );
                        }
                    }
                }

                this.gvSendNoticeTo.DataSource = personToNotice;
                this.gvSendNoticeTo.DataBind();
            }

        }

        protected void btnDeleteNoticeTo_Click(object sender, EventArgs e)
        {
            personToNotice = new List<Person>();
            HiddenField hdnRowDetailKey;
            CheckBox chbxDeleteNotificationTo;

            foreach (GridViewRow row in this.gvSendNoticeTo.Rows)
            {
                hdnRowDetailKey = row.FindControl("hdnRowDetailKey") as HiddenField;
                chbxDeleteNotificationTo = row.FindControl("chbxDeleteNotificationTo") as CheckBox;

                if (!chbxDeleteNotificationTo.Checked)
                {
                    personToSend = this.AdminClientRef.GetPersonByPersonID(hdnRowDetailKey.Value);

                    if (personToNotice != null)
                    {
                        personToNotice.Add(new Person
                        {
                            idPerson = personToSend.idPerson,
                            Title = personToSend.Title,
                            FirstName = personToSend.FirstName,
                            LastName = personToSend.LastName
                        }
                        );
                    }
                }
            }


            this.gvSendNoticeTo.DataSource = personToNotice;
            this.gvSendNoticeTo.DataBind();
        }

    }
}