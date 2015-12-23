using ETEM.CostCalculation;
using ETEM.Freamwork;
using ETEMModel.Helpers;
using ETEMModel.Models;
using ETEMModel.Helpers.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ETEM.Controls.CostCalculation
{
    
    public partial class OfferMainData : BaseUserControl
    {
        private ETEMModel.Models.Offer currentEntity;
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetHdnField(string value)
        {
            this.hdnRowMasterKey.Value = value;
        }

        public override void UserControlLoad()
        {
            if (this.ownerPage == null)
            {
                throw new UMSException("Current Page is null or is not inheritor of BasicPage.");
            }

            this.currentEntity = this.ownerPage.CostCalculationRef.GetOfferByID(this.CurrentEntityMasterID);



            this.InquiryData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.InquiryData.SetHdnField(this.CurrentEntityMasterID);
            this.InquiryData.UserControlLoad();

            this.TabContainer.ActiveTab = this.tabInquiryData;

            this.DiesData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.DiesData.SetHdnField(this.CurrentEntityMasterID);
            this.DiesData.UserControlLoad();


            this.BilletScrap.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.BilletScrap.SetHdnField(this.CurrentEntityMasterID);
            this.BilletScrap.UserControlLoad();

            this.ProducitivityData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.ProducitivityData.SetHdnField(this.CurrentEntityMasterID);
            this.ProducitivityData.UserControlLoad();

            this.ExpensesByCostCentersData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.ExpensesByCostCentersData.SetHdnField(this.CurrentEntityMasterID);
            this.ExpensesByCostCentersData.UserControlLoad();

            this.ExpensesByCostCentersDataTon.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.ExpensesByCostCentersDataTon.SetHdnField(this.CurrentEntityMasterID);
            this.ExpensesByCostCentersDataTon.UserControlLoad();

            this.ucProductCostData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.ucProductCostData.SetHdnField(this.CurrentEntityMasterID);
            this.ucProductCostData.UserControlLoad();

            this.OfferOverviewData.CurrentEntityMasterID = this.CurrentEntityMasterID;
            this.OfferOverviewData.SetHdnField(this.CurrentEntityMasterID);
            this.OfferOverviewData.UserControlLoad();

            if (this.currentEntity != null)
            {
                this.lbOfferData.Text =     " - Inquiry No " + this.currentEntity.InquiryNumber + "/ " + 
                                            this.currentEntity.OfferDate.ToString("dd.MM.yyyy") + ", " + 
                                            this.currentEntity.Customer;


                this.ucAttachments.CustomFolder = this.CurrentEntityMasterID + "_" + this.currentEntity.InquiryNumber;
                this.ucAttachments.UserControlLoad();
            }
            else
            {
                this.lbOfferData.Text = string.Empty;
                this.ucAttachments.ClearGrid();
            }

            this.pnlFormData.Visible = true;
        }

        protected void btnSaveTabs_Click(object sender, EventArgs e)
        {
            //if (!this.ownerPage.CheckUserActionPermission(UMSEnums.SecuritySettings.PersonSave, false))
            //{
            //    return;
            //}


            List<Tuple<CallContext, string>> listCallContext = new List<Tuple<CallContext, string>>();
            List<string> listErrors = new List<string>();


          

            listCallContext.Add(this.InquiryData.UserControlSave());

            
         


            if (listCallContext.First().Item1.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                this.DiesData.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.DiesData.SetHdnField(this.InquiryData.CurrentEntityMasterID );
                this.DiesData.UserControlLoad();


                this.BilletScrap.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.BilletScrap.SetHdnField(this.InquiryData.CurrentEntityMasterID );
                this.BilletScrap.UserControlLoad();

                 
                this.ProducitivityData.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.ProducitivityData.SetHdnField(this.InquiryData.CurrentEntityMasterID );
                listCallContext.Add(this.ProducitivityData.UserControlSave());
                this.ProducitivityData.UserControlLoad();

                this.ExpensesByCostCentersData.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.ExpensesByCostCentersData.SetHdnField(this.InquiryData.CurrentEntityMasterID) ;
                this.ExpensesByCostCentersData.UserControlLoad();

                this.ExpensesByCostCentersDataTon.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.ExpensesByCostCentersDataTon.SetHdnField(this.InquiryData.CurrentEntityMasterID );
                this.ExpensesByCostCentersDataTon.UserControlLoad();

                this.ucProductCostData.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.ucProductCostData.SetHdnField(this.InquiryData.CurrentEntityMasterID);
                this.ucProductCostData.UserControlLoad();


                this.OfferOverviewData.CurrentEntityMasterID = this.InquiryData.CurrentEntityMasterID;
                this.OfferOverviewData.SetHdnField( this.InquiryData.CurrentEntityMasterID);
                this.OfferOverviewData.UserControlLoad();


              // listCallContext.Add(this.ProducitivityData.UserControlSave());


                ///this.ownerPage.CostCalculationRef.CreateExpenseGroupForOffer(Int32.Parse( this.CurrentEntityMasterID ));
            }




            foreach (var item in listCallContext)
            {
                if (item.Item1.ResultCode == ETEMEnums.ResultEnum.Error)
                {
                    string[] currentItemErrors = item.Item1.Message.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < currentItemErrors.Length; i++)
                    {
                        currentItemErrors[i] = currentItemErrors[i] +  (item.Item2.IsNotNullOrEmpty()?"(" + item.Item2 + ")":string.Empty);
                    }
                    listErrors.AddRange(currentItemErrors);
                }
            }

            if (listErrors.Count > 0)
            {
                foreach (var error in listErrors)
                {
                    var listItem = new ListItem(error);
                    listItem.Attributes.Add("class", "lbResultSaveError");
                    this.blEroorsSave.Items.Add(listItem);
                }

                this.pnlErrors.Visible = true;
            }
            

           if (this.ownerPage is OffersList)
            {
                (this.ownerPage as OffersList).LoadOfferList();
            }


        }

        protected void imgBtnPersonImage_Click(object sender, ImageClickEventArgs e)
        {
            this.ownerPage.OpenPageInNewWindow(ETEM.Share.UploadFile.formResource);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAllLabels();
            this.pnlFormData.Visible = false;
        }

        private void ClearAllLabels()
        {
           // this.ucPersonalData.ClearResultContext();

        }

        protected void btnCancelErorrs_Click(object sender, EventArgs e)
        {
            this.blEroorsSave.Items.Clear();
            this.pnlErrors.Visible = false;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            this.ownerPage.ShowMSG("OPS");
        }


    }
}