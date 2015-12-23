using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.Partial;
using ETEMModel.Models;

namespace ETEM.Share
{
    public partial class AttachmentList : BasicPage
    {
        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_REPORTS,
            PageFullName = Constants.UMS_SHARE_ATTACHMENTLIST,
            PagePath = "../Share/AttachmentList.aspx"
        };

        #region Overridden Form Main Data Methods
        public override string CurrentPagePath()
        {
            return formResource.PagePath;
        }
        public override string CurrentPageFullName()
        {
            return formResource.PageFullName;
        }
        public override FormResources CurrentFormResources()
        {
            return formResource;
        }
        public override string CurrentModule()
        {
            return formResource.Module;
        }
        public override void SetPageCaptions()
        {

        }
        #endregion

        public int AttachmentTypeKeyTypeID
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.hdnAttachmentTypeKeyTypeID.Value))
                {
                    return Int32.Parse(this.hdnAttachmentTypeKeyTypeID.Value);
                }
                else
                {
                    return Constants.INVALID_ID;
                }
            }
            set { this.hdnAttachmentTypeKeyTypeID.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormLoad();
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.pnlFilterData.Visible = true;
        }

        public override void FormLoad()
        {
            base.FormLoad();

            if (!this.FormContext.QueryString.Contains("ModuleSysName"))
            {
                this.ShowMSG("Подадени са грешни параметри на страницата");
                return;
            }
            if (!this.FormContext.QueryString.Contains("AttachmentDocumentType"))
            {
                this.ShowMSG("Подадени са грешни параметри на страницата");
                return;
            }

            string ModuleSysName = this.FormContext.QueryString["ModuleSysName"].ToString();
            string AttachmentDocumentType = this.FormContext.QueryString["AttachmentDocumentType"].ToString();

            if (ModuleSysName == Constants.MODULE_FINANCE &&
                AttachmentDocumentType == ETEMEnums.FinanceReportTypeEnum.FinanceReport.ToString())
            {
                CheckUserActionPermission(ETEMEnums.SecuritySettings.AttachmentShowList, true);
            }

            string DocKeyTypeIntCode = string.Empty;
            if (this.FormContext.QueryString.Contains("DocKeyTypeIntCode"))
            {
                DocKeyTypeIntCode = this.FormContext.QueryString["DocKeyTypeIntCode"].ToString();
            }

            if (DocKeyTypeIntCode == string.Empty)
            {
                DocKeyTypeIntCode = "AttachmentType";
                this.ddlAttachmentType.KeyTypeIntCode = DocKeyTypeIntCode;
            }
            else
            {
                this.ddlAttachmentType.KeyTypeIntCode = DocKeyTypeIntCode;
            }
            this.ddlAttachmentType.UserControlLoad();

            this.Attachment.ModuleSysName = ModuleSysName;
            this.Attachment.AttachmentDocumentType = AttachmentDocumentType;
            this.Attachment.DocKeyTypeIntCode = DocKeyTypeIntCode;

            if (!IsPostBack)
            {
                KeyType ktAttachmentType = this.AdminClientRef.GetKeyTypeByIntCode(DocKeyTypeIntCode);

                if (ktAttachmentType != null)
                {
                    this.AttachmentTypeKeyTypeID = ktAttachmentType.idKeyType;
                }
            }

            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);

            if (string.IsNullOrEmpty(this.GridViewSortExpression) || this.GridViewSortExpression == Constants.INVALID_ID_STRING)
            {
                this.GridViewSortExpression = "AttachmentDate";
            }

            this.gvAttachment.DataSource = CommonClientRef.GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);
            if (NewPageIndex.HasValue)
            {
                this.gvAttachment.PageIndex = NewPageIndex.Value;
            }
            this.gvAttachment.DataBind();
        }

        public void LoadSearchResult(List<AttachmentDataView> listAttachment)
        {
            this.gvAttachment.DataSource = listAttachment;
            BindDataWithPaging();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);
            List<AttachmentDataView> list = CommonClientRef.GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);

            LoadSearchResult(list);
            this.pnlFilterData.Visible = false;
        }

        private void BindDataWithPaging()
        {
            if (NewPageIndex.HasValue)
            {
                this.gvAttachment.PageIndex = NewPageIndex.Value;
            }
            this.gvAttachment.DataBind();
        }

        public override void AlphabetClick(string alpha)
        {
            if (alpha == Constants.ALL_CHARACTERS)
            {
                List<AbstractSearch> searchCriteria = new List<AbstractSearch>();

                List<AttachmentDataView> list = CommonClientRef.GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                ClearFilterForm();
                LoadSearchResult(list);
            }
            else
            {
                ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
                AddCustomSearchCriterias(searchCriteria);
                searchCriteria.Add(new TextSearch
                {
                    Comparator = TextComparators.StartsWith,
                    Property = "FirstName",
                    SearchTerm = alpha
                });

                List<AttachmentDataView> list = CommonClientRef.GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);

                LoadSearchResult(list);
            }
        }

        public void LoadAttachmentList()
        {
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();
            AddCustomSearchCriterias(searchCriteria);
            List<AttachmentDataView> list = CommonClientRef.GetAccountingAttachmentList(searchCriteria, GridViewSortExpression, GridViewSortDirection);
            LoadSearchResult(list);
        }

        private void ClearFilterForm()
        {
            this.tbxDescription.Text = "";
            this.tbxFullName.Text = "";
            this.ddlAttachmentType.SelectedValue = Constants.INVALID_ID_STRING;
        }

        private void AddCustomSearchCriterias(ICollection<AbstractSearch> searchCriteria)
        {
            string ModuleSysName = this.FormContext.QueryString["ModuleSysName"].ToString();

            Module module = this.AdminClientRef.GetModuleBySysName(ModuleSysName);

            string DocKeyTypeIntCode = string.Empty;
            if (this.FormContext.QueryString.Contains("DocKeyTypeIntCode"))
            {
                DocKeyTypeIntCode = this.FormContext.QueryString["DocKeyTypeIntCode"].ToString();
            }

            if (DocKeyTypeIntCode == string.Empty)
            {
                DocKeyTypeIntCode = "AttachmentType";
            }

            searchCriteria.Add(
                 new NumericSearch
                 {
                     Comparator = NumericComparators.Equal,
                     Property = "idModule",
                     SearchTerm = module.EntityID
                 });

            if (!string.IsNullOrEmpty(this.tbxFullName.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "FirstName",
                        SearchTerm = this.tbxFullName.Text
                    });
            }

            if (!string.IsNullOrEmpty(this.tbxDescription.Text))
            {
                searchCriteria.Add(
                    new TextSearch
                    {
                        Comparator = TextComparators.Contains,
                        Property = "Description",
                        SearchTerm = this.tbxDescription.Text
                    });
            }

            if (this.ddlAttachmentType.SelectedValueINT != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idAttachmentType",
                       SearchTerm = ddlAttachmentType.SelectedValueINT
                   });
            }

            if (this.AttachmentTypeKeyTypeID != Constants.INVALID_ID)
            {
                searchCriteria.Add(
                   new NumericSearch
                   {
                       Comparator = NumericComparators.Equal,
                       Property = "idAttachmentTypeKeyType",
                       SearchTerm = this.AttachmentTypeKeyTypeID
                   });
            }
        }

        protected void lnkBtnServerEdit_Click(object sender, EventArgs e)
        {
            string moduleSysName = this.FormContext.QueryString["ModuleSysName"].ToString();
            string attachmentDocumentType = this.FormContext.QueryString["AttachmentDocumentType"].ToString();

            if (moduleSysName == Constants.MODULE_FINANCE &&
                attachmentDocumentType == ETEMEnums.FinanceReportTypeEnum.FinanceReport.ToString())
            {
                CheckUserActionPermission(ETEMEnums.SecuritySettings.AttachmentPreview, true);
            }

            LinkButton lnkBtnServerEdit = sender as LinkButton;

            if (lnkBtnServerEdit == null)
            {
                ShowMSG("lnkBtnServerEdit is null");
                return;
            }
            string idRowMasterKey = BaseHelper.ParseStringByAmpersand(lnkBtnServerEdit.CommandArgument)["idRowMasterKey"].ToString();

            this.Attachment.CurrentEntityMasterID = idRowMasterKey;
            this.Attachment.SetHdnField(idRowMasterKey);
            this.Attachment.UserControlLoad();

            this.Attachment.Visible = true;
        }

        protected void bntNew_Click(object sender, EventArgs e)
        {

            string moduleSysName = this.FormContext.QueryString["ModuleSysName"].ToString();
            string attachmentDocumentType = this.FormContext.QueryString["AttachmentDocumentType"].ToString();

            if (moduleSysName == Constants.MODULE_FINANCE &&
                attachmentDocumentType == ETEMEnums.FinanceReportTypeEnum.FinanceReport.ToString())
            {
                CheckUserActionPermission(ETEMEnums.SecuritySettings.AttachmentSave, true);
            }



            this.Attachment.CurrentEntityMasterID = Constants.INVALID_ID_STRING;
            this.Attachment.SetHdnField(Constants.INVALID_ID_STRING);

            //this.PersonMainData.ClearForm();
            this.Attachment.UserControlLoad();
            this.Attachment.Visible = true;
        }

        protected void gvAttachment_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void gvAttachment_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearFilterForm();

            LoadAttachmentList();
        }



    }
}