using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEM.Share;
using ETEM.Admin;

namespace ETEM.Controls.Admin
{
    public partial class GroupData : BaseUserControl
    {
        private Group currentEntity;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            SetEmptyValues();
            ClearResultContext(this.lbResultContext);

            this.currentEntity = this.ownerPage.AdminClientRef.GetGroupByID(Convert.ToInt32(this.CurrentEntityMasterID));

            if (this.currentEntity != null)
            {
                this.hdnRowMasterKey.Value = this.CurrentEntityMasterID;
                this.tbxGroupName.Text = this.currentEntity.GroupName.ToString();

                this.acAddPersonForGroup.SelectedValue = Constants.INVALID_ID_STRING;
                this.acAddPersonForGroup.Text = string.Empty;
                this.chbxSharedAccess.Checked = Convert.ToBoolean(this.currentEntity.SharedAccess);

                this.btnAddPerson.Enabled = true;
                this.btnDeletePerson.Enabled = true;

                //Известие до
                this.gvGroupPerson.DataSource = this.ownerPage.AdminClientRef.GetGroupPersonLinkDataViewByGroupID(Int32.Parse(this.CurrentEntityMasterID));
                this.gvGroupPerson.DataBind();
            }
            else
            {
                SetEmptyValues();
            }

            this.pnlGroupData.Visible = true;
            this.pnlGroupData.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupSave, false))
            {
                return;
            }

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value) || this.hdnRowMasterKey.Value == Constants.INVALID_ID_STRING)
            {
                this.currentEntity = new Group();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetGroupByID(Convert.ToInt32(this.hdnRowMasterKey.Value));

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_Group_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    string falseResult = String.Format(BaseHelper.GetCaptionString("Entity_is_not_update"));

                    this.ownerPage.FormLoad();
                    return;
                }
            }

            currentEntity.GroupName = this.tbxGroupName.Text;
            currentEntity.SharedAccess = this.chbxSharedAccess.Checked;

            CallContext resultContext = new CallContext();
            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
            resultContext = this.ownerPage.AdminClientRef.GroupSave(currentEntity, resultContext);

            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.CurrentEntityMasterID = resultContext.EntityID;

                UserControlLoad();

                RefreshParent();
            }

            CheckIfResultIsSuccess(this.lbResultContext);
            lbResultContext.Text = resultContext.Message; ;
        }

        private void SetEmptyValues()
        {
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;

            this.hdnRowMasterKey.Value = string.Empty;
            this.tbxGroupName.Text = string.Empty;
            this.btnAddPerson.Enabled = false;
            this.btnDeletePerson.Enabled = false;
            this.acAddPersonForGroup.SelectedValue = Constants.INVALID_ID_STRING;
            this.acAddPersonForGroup.Text = string.Empty;

            this.gvGroupPerson.DataSource = null;
            this.gvGroupPerson.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlGroupData.Visible = false;
        }

        private void RefreshParent()
        {
            if (Parent != null)
            {
                if (Parent.Page != null && (Parent.Page as GroupList) != null)
                {
                    (Parent.Page as GroupList).FormLoad();
                }
            }
        }

        protected void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupPersonAddDelete, false))
            {
                return;
            }

            if (this.acAddPersonForGroup.SelectedValueINT != Constants.INVALID_ID && this.acAddPersonForGroup.SelectedValueINT != null && !string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                GroupPersonLink groupPersonLink = new GroupPersonLink();
                CallContext resultContext = new CallContext();

                if (this.CurrentEntityMasterID == Constants.INVALID_ID_STRING || string.IsNullOrEmpty(CurrentEntityMasterID))
                {
                    this.CurrentEntityMasterID = this.hdnRowMasterKey.Value;
                }

                //дублиране на лицата за известяване
                bool isUniquePerson = this.AdminClientRef.IsUniqueRecordGroupPersonLink(Int32.Parse(this.CurrentEntityMasterID), this.acAddPersonForGroup.SelectedValueINT.Value);

                if (!isUniquePerson)
                {
                    resultContext.ResultCode = ETEMEnums.ResultEnum.Error;
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Not_Unique_Records"), this.CurrentEntityMasterID);
                    return;
                }

                groupPersonLink.idGroup = Int32.Parse(this.CurrentEntityMasterID);
                groupPersonLink.idPerson = this.acAddPersonForGroup.SelectedValueINT.Value;

                resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
                resultContext = this.AdminClientRef.GroupPersonLinkSave(groupPersonLink, resultContext);

                this.acAddPersonForGroup.SelectedValue = Constants.INVALID_ID_STRING;
                this.acAddPersonForGroup.Text = string.Empty;

                this.gvGroupPerson.DataSource = this.ownerPage.AdminClientRef.GetGroupPersonLinkDataViewByGroupID(Int32.Parse(this.CurrentEntityMasterID));
                this.gvGroupPerson.DataBind();
            }
        }

        protected void btnDeletePerson_Click(object sender, EventArgs e)
        {

            if (!this.ownerPage.CheckUserActionPermission(ETEMEnums.SecuritySettings.GroupPersonAddDelete, false))
            {
                return;
            }

            List<GroupPersonLink> list = new List<GroupPersonLink>();

            foreach (GridViewRow row in this.gvGroupPerson.Rows)
            {
                HiddenField hdnRowDetailKey = row.FindControl("hdnRowDetailKey") as HiddenField;
                CheckBox chbxDeleteGroupPersonLink = row.FindControl("chbxDeleteGroupPersonLink") as CheckBox;

                if (chbxDeleteGroupPersonLink.Checked)
                {
                    list.Add(new GroupPersonLink()
                    {
                        idGroupPersonLink = Int32.Parse(hdnRowDetailKey.Value)

                    }
                    );
                }
            }


            if (list.Count > 0)
            {
                CallContext resultContext = new CallContext();
                resultContext = this.AdminClientRef.GroupPersonLinkDelete(list, resultContext);
            }

            this.gvGroupPerson.DataSource = this.ownerPage.AdminClientRef.GetGroupPersonLinkDataViewByGroupID(Int32.Parse(this.hdnRowMasterKey.Value));
            this.gvGroupPerson.DataBind();
        }

    }
}