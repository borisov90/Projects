using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEM.Admin;
using ETEM.Controls.Common;

namespace ETEM.Controls.Admin
{
    public partial class KeyType : BaseUserControl
    {
        private ETEMModel.Models.KeyType currentEntity;
        private ETEMModel.Models.KeyValue currentKeyValue;
        public SMCDropDownList currentCallerDropDownList { get; set; }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            this.pnlKeyType.Visible = true;

            ClearResultContext(lbResultContext);

            this.currentEntity = this.ownerPage.AdminClientRef.GetKeyTypeByKeyTypeID(CurrentEntityMasterID);

            if (currentEntity != null)
            {
                this.tbxName.Text = currentEntity.Name;
                this.tbxKeyTypeIntCode.Text = currentEntity.KeyTypeIntCode;
                this.tbxDescription.Text = currentEntity.Description;
                this.cbxIsSystem.Checked = currentEntity.IsSystemBool;
                this.hdnRowMasterKey.Value = currentEntity.idKeyType.ToString();

                LoadKeyValues(currentEntity.idKeyType.ToString());
            }
            else
            {
                this.tbxName.Text = string.Empty;
                this.tbxKeyTypeIntCode.Text = string.Empty;
                this.tbxDescription.Text = string.Empty;
                this.cbxIsSystem.Checked = false;
                this.hdnRowMasterKey.Value = string.Empty;
            }

            if (currentCallerDropDownList != null && currentCallerDropDownList.UniqueID != null)
            {
                this.hdnCurrentCallerDropDownListUniqueID.Value = currentCallerDropDownList.UniqueID;
                this.TabContainer.ActiveTab = this.tabValues;
            }


        }

        private void LoadKeyValues(string idKeyType)
        {
            this.gvKeyValues.DataSource = this.ownerPage.AdminClientRef.GetAllKeyValueByKeyTypeID(idKeyType,
                this.GridViewSortExpression, this.GridViewSortDirection);
            if (NewPageIndex.HasValue)
            {
                this.gvKeyValues.PageIndex = NewPageIndex.Value;
            }
            this.gvKeyValues.DataBind();



        }



        protected void lnkBtnServerEditSlave_Click(object sender, EventArgs e)
        {

            this.pnlKeyValue.Visible = true;
            this.lbResultContext.Text = "";
            LinkButton lnkBtnServerEditSlave = sender as LinkButton;

            if (lnkBtnServerEditSlave == null)
            {
                this.ownerPage.ShowMSG("lnkBtnServerEditSlave is null");
                return;
            }
            string idRowSlaveKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEditSlave.CommandArgument)["idRowSlaveKey"].ToString();

            this.currentKeyValue = this.ownerPage.AdminClientRef.GetKeyValueByKeyValueID(idRowSlaveKey);

            if (currentKeyValue != null)
            {
                this.tbxKeyValueNameBG.Text = currentKeyValue.Name;
                this.tbxKeyValueNameEN.Text = currentKeyValue.NameEN;
                this.tbxKeyValueIntCode.Text = currentKeyValue.KeyValueIntCode;
                this.tbxKeyValueDescription.Text = currentKeyValue.Description;
                this.tbxKeyValueOrder.Text = (currentKeyValue.V_Order.HasValue ? currentKeyValue.V_Order.Value.ToString() : string.Empty);
                this.tbxDefaultValue1.Text = currentKeyValue.DefaultValue1;
                this.tbxDefaultValue2.Text = currentKeyValue.DefaultValue2;
                this.tbxDefaultValue3.Text = currentKeyValue.DefaultValue3;
                this.tbxDefaultValue4.Text = currentKeyValue.DefaultValue4;
                this.tbxDefaultValue5.Text = currentKeyValue.DefaultValue5;
                this.tbxDefaultValue6.Text = currentKeyValue.DefaultValue6;
                //this.tbxCodeAdminUNI.Text = currentKeyValue.CodeAdminUNI;

            }

            this.hdnRowSlaveKey.Value = this.currentKeyValue.idKeyValue.ToString();


        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            this.pnlKeyType.Visible = true;

            this.lbResultContext.Text = "";
            LinkButton lnkBtnServerEdit = sender as LinkButton;

            if (lnkBtnServerEdit == null)
            {
                this.ownerPage.ShowMSG("lnkBtnServerEdit is null");
                return;
            }
            string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();
            this.currentEntity = this.ownerPage.AdminClientRef.GetKeyTypeByKeyTypeID(idRowMasterKey);

            if (currentEntity != null)
            {
                this.tbxName.Text = currentEntity.Name;
                this.tbxKeyTypeIntCode.Text = currentEntity.KeyTypeIntCode;
                this.tbxDescription.Text = currentEntity.Description;
                this.cbxIsSystem.Checked = currentEntity.IsSystemBool;
            }

            this.hdnRowMasterKey.Value = currentEntity.idKeyType.ToString();

            LoadKeyValues(currentEntity.idKeyType.ToString());


        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            pnlKeyType.Visible = true;

            if (string.IsNullOrEmpty(this.hdnRowMasterKey.Value))
            {
                this.currentEntity = new ETEMModel.Models.KeyType();
            }
            else
            {
                this.currentEntity = this.ownerPage.AdminClientRef.GetKeyTypeByKeyTypeID(this.hdnRowMasterKey.Value);

                if (this.currentEntity == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_KeyType_Not_Found_By_ID"), this.hdnRowMasterKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }



            currentEntity.Name = this.tbxName.Text;
            currentEntity.KeyTypeIntCode = this.tbxKeyTypeIntCode.Text;
            currentEntity.Description = this.tbxDescription.Text;
            currentEntity.IsSystemBool = this.cbxIsSystem.Checked;

            CallContext resultContext = new CallContext();

            resultContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            resultContext = this.ownerPage.AdminClientRef.KeyTypeSave(currentEntity, resultContext);

            this.lbResultContext.Text = resultContext.Message;
            this.hdnRowMasterKey.Value = resultContext.EntityID;
            this.ownerPage.FormLoad();
            this.ownerPage.ReloadKeyTypeInApplication();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlKeyType.Visible = false;

            this.hdnRowMasterKey.Value = "";
            this.lbResultContext.Text = "";
            this.lbResultKeyValueContext.Text = "";
        }


        protected void btnCancelKeyValue_Click(object sender, EventArgs e)
        {
            this.pnlKeyValue.Visible = false;
            this.pnlKeyType.Visible = true;

            this.lbResultContext.Text = "";
            this.lbResultKeyValueContext.Text = "";

        }

        protected void btnSaveKeyValue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.hdnRowSlaveKey.Value))
            {
                this.currentKeyValue = new ETEMModel.Models.KeyValue();
            }
            else
            {
                this.currentKeyValue = this.ownerPage.AdminClientRef.GetKeyValueByKeyValueID(this.hdnRowSlaveKey.Value);

                if (this.currentKeyValue == null)
                {
                    this.lbResultContext.Text = String.Format(BaseHelper.GetCaptionString("Entity_KeyValue_Not_Found_By_ID"), this.hdnRowSlaveKey.Value);
                    this.ownerPage.FormLoad();
                    return;
                }

            }


            currentKeyValue.Name = this.tbxKeyValueNameBG.Text.Trim();
            currentKeyValue.NameEN = this.tbxKeyValueNameEN.Text.Trim();
            currentKeyValue.KeyValueIntCode = this.tbxKeyValueIntCode.Text.Trim();
            currentKeyValue.Description = this.tbxKeyValueDescription.Text;
            currentKeyValue.V_Order = BaseHelper.ConvertToInt(this.tbxKeyValueOrder.Text.Trim());

            currentKeyValue.DefaultValue1 = this.tbxDefaultValue1.Text.Trim();
            currentKeyValue.DefaultValue2 = this.tbxDefaultValue2.Text.Trim();
            currentKeyValue.DefaultValue3 = this.tbxDefaultValue3.Text.Trim();
            currentKeyValue.DefaultValue4 = this.tbxDefaultValue4.Text.Trim();
            currentKeyValue.DefaultValue5 = this.tbxDefaultValue5.Text.Trim();
            currentKeyValue.DefaultValue6 = this.tbxDefaultValue6.Text.Trim();
            //currentKeyValue.CodeAdminUNI = this.tbxCodeAdminUNI.Text.Trim();


            currentKeyValue.idKeyType = Int32.Parse(this.hdnRowMasterKey.Value);

            ETEMModel.Helpers.CallContext resultContext = this.ownerPage.AdminClientRef.KeyValueSave(currentKeyValue);

            this.lbResultKeyValueContext.Text = resultContext.Message;
            this.hdnRowSlaveKey.Value = resultContext.EntityID;


            LoadKeyValues(currentKeyValue.idKeyType.ToString());




            currentCallerDropDownList = this.ownerPage.FindControl(this.hdnCurrentCallerDropDownListUniqueID.Value) as SMCDropDownList;

            if (this.currentCallerDropDownList != null)
            {
                currentCallerDropDownList.UserControlLoad();
                currentCallerDropDownList.SelectedValue = resultContext.EntityID;
            }

            this.ownerPage.ReloadKeyValueInApplication();


        }

        protected void bntNewKeyValue_Click(object sender, EventArgs e)
        {
            this.pnlKeyValue.Visible = true;
            this.pnlKeyType.Visible = false;

            this.lbResultContext.Text = string.Empty;

            this.tbxKeyValueNameBG.Text = string.Empty;
            this.tbxKeyValueNameEN.Text = string.Empty;
            this.tbxKeyValueIntCode.Text = string.Empty;
            this.tbxKeyValueDescription.Text = string.Empty;
            this.tbxKeyValueOrder.Text = string.Empty;
            this.tbxDefaultValue1.Text = string.Empty;
            this.tbxDefaultValue2.Text = string.Empty;
            this.tbxDefaultValue3.Text = string.Empty;
            this.tbxDefaultValue4.Text = string.Empty;
            this.tbxDefaultValue5.Text = string.Empty;
            this.tbxDefaultValue6.Text = string.Empty;
            //this.tbxCodeAdminUNI.Text = string.Empty;

            this.hdnRowSlaveKey.Value = string.Empty;

        }

        protected void gvKeyValues_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            LoadKeyValues(this.hdnRowMasterKey.Value);
        }

        protected void gvKeyValues_OnSorting(object sender, GridViewSortEventArgs e)
        {

            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            LoadKeyValues(this.hdnRowMasterKey.Value);
        }
    }
}