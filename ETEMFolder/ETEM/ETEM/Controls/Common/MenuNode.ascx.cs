using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers;
using ETEM.Freamwork;
using System.Xml;
using ETEMModel.Models;

namespace ETEM.Controls.Common
{
    public partial class MenuNode : BaseUserControl
    {
        List<MenuNodeDataView> nodeList;



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public override void UserControlLoad()
        {
            if (string.IsNullOrEmpty(hdnDllValue.Value))
            {
                if (!IsPostBack)
                {

                    this.ddlRole.DataSource = this.ownerPage.AdminClientRef.GetAllRoles("Name", "ASC");
                    this.ddlRole.DataBind();
                }
            }
            else
            {
                if (!IsPostBack)
                {
                    this.ddlRole.DataSource = this.ownerPage.AdminClientRef.GetAllRoles("Name", "ASC");
                    this.ddlRole.DataBind();
                    ddlRole.Items.FindByValue(hdnDllValue.Value).Selected = true;
                }
            }

            nodeList = this.ownerPage.AdminClientRef.GetAllMenuNode(this.ownerPage.CallContext);
            List<MenuNodeDataView> nodeListRootItems = nodeList.Where(m => m.parentNode == 0).ToList();
            this.gvMainManu.DataSource = nodeListRootItems;
            this.gvMainManu.DataBind();

            string chechBoxName = "cbxRoot";
            CheckIfCheckBoxIsChecked(chechBoxName, nodeListRootItems, this.gvMainManu);

        }

        private void CheckIfCheckBoxIsChecked(string checkBoxName, List<MenuNodeDataView> nodeList, GridView grid)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                int nodeId = nodeList[i].idNode;
                int roleId = Constants.INVALID_ID;
                if (string.IsNullOrEmpty(hdnDllValue.Value))
                {
                    roleId = int.Parse(this.ddlRole.SelectedValue.ToString());
                }
                else
                {
                    roleId = int.Parse(this.hdnDllValue.Value);
                }

                bool isChecked = this.ownerPage.AdminClientRef.GetAllMenuNode(nodeId, roleId);
                GridViewRow rowRoot = grid.Rows[i];
                CheckBox cbxRoot = rowRoot.Cells[0].FindControl(checkBoxName) as CheckBox;
                if (cbxRoot != null && isChecked)
                {
                    cbxRoot.Checked = true;
                }


            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> listRootMenuChecked = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> listNewNodeNames = new List<KeyValuePair<string, string>>();
            foreach (GridViewRow rowRoot in this.gvMainManu.Rows)
            {
                if (rowRoot.RowType == DataControlRowType.DataRow)
                {
                    TextBox txbNewNodeName = rowRoot.Cells[0].FindControl("txtMainMenuName") as TextBox;
                    HiddenField hdnMainNodeID = rowRoot.Cells[0].FindControl("hdnRootNoteID") as HiddenField;
                    GridView gvSubMenu = rowRoot.Cells[0].FindControl("gvSubMenu") as GridView;
                    CheckBox cbxRoot = rowRoot.Cells[0].FindControl("cbxRoot") as CheckBox;
                    listNewNodeNames.Add(new KeyValuePair<string, string>(hdnMainNodeID.Value, txbNewNodeName.Text));
                    if (hdnMainNodeID != null && gvSubMenu != null && cbxRoot != null)
                    {
                        if (cbxRoot.Checked)
                        {
                            listRootMenuChecked.Add(new KeyValuePair<string, string>(this.ddlRole.SelectedValue, hdnMainNodeID.Value));
                        }

                        foreach (GridViewRow rowSubMenu in gvSubMenu.Rows)
                        {
                            if (rowRoot.RowType == DataControlRowType.DataRow)
                            {
                                TextBox txbNewSubNodeName = rowSubMenu.Cells[0].FindControl("txbSubMenu") as TextBox;
                                HiddenField hdnNoteSubMenuID = rowSubMenu.Cells[0].FindControl("hdnNoteSubMenuID") as HiddenField;
                                CheckBox cbxSubMenu = rowSubMenu.Cells[0].FindControl("cbxSubMenu") as CheckBox;
                                listNewNodeNames.Add(new KeyValuePair<string, string>(hdnNoteSubMenuID.Value, txbNewSubNodeName.Text));
                                GridView gvSubMenuLink = rowSubMenu.Cells[0].FindControl("gvSubMenuLink") as GridView;
                                if (hdnNoteSubMenuID != null && gvSubMenuLink != null && cbxSubMenu != null)
                                {
                                    if (cbxSubMenu.Checked)
                                    {
                                        listRootMenuChecked.Add(new KeyValuePair<string, string>
                                            (this.ddlRole.SelectedValue, hdnNoteSubMenuID.Value));
                                    }

                                    foreach (GridViewRow rowDoubleSubMenu in gvSubMenuLink.Rows)
                                    {
                                        TextBox txbNewLastNodeName = rowDoubleSubMenu.Cells[0].FindControl("txbLastMenuNode") as TextBox;
                                        HiddenField hdnNoteDoubleSubMenuID = rowDoubleSubMenu.Cells[0].FindControl("hdnNoteDoubleSubMenuID") as HiddenField;
                                        CheckBox cbxDoubleSubMenu = rowDoubleSubMenu.Cells[0].FindControl("cbxDoubleSubMenu") as CheckBox;
                                        listNewNodeNames.Add(new KeyValuePair<string, string>(hdnNoteDoubleSubMenuID.Value, txbNewLastNodeName.Text));
                                        if (hdnNoteDoubleSubMenuID != null && cbxDoubleSubMenu != null)
                                        {
                                            if (cbxDoubleSubMenu.Checked)
                                            {
                                                listRootMenuChecked.Add(new KeyValuePair<string, string>
                                                             (this.ddlRole.SelectedValue, hdnNoteDoubleSubMenuID.Value));
                                            }
                                        }

                                    }
                                }

                            }

                        }
                    }
                }
            }

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;

            this.ownerPage.CallContext = this.ownerPage.AdminClientRef.RolesMenuSave(listNewNodeNames, listRootMenuChecked, this.ownerPage.CallContext);


        }


        protected void gvMainManu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvSubMenuGrid = e.Row.FindControl("gvSubMenu") as GridView;

                if (gvSubMenuGrid != null)
                {
                    HiddenField hdnNoteID = e.Row.Cells[0].FindControl("hdnRootNoteID") as HiddenField;

                    if (hdnNoteID != null)
                    {
                        List<MenuNodeDataView> subMenuNodes = nodeList.Where(p => p.parentNode == Int32.Parse(hdnNoteID.Value)).ToList();
                        string chechBoxName = "cbxSubMenu";
                        gvSubMenuGrid.DataSource = subMenuNodes;
                        gvSubMenuGrid.DataBind();

                        CheckIfCheckBoxIsChecked(chechBoxName, subMenuNodes, gvSubMenuGrid);

                    }
                }
            }
        }

        protected void gvSubMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvSubMenuLink = e.Row.FindControl("gvSubMenuLink") as GridView;

                if (gvSubMenuLink != null)
                {
                    HiddenField hdnNoteSubMenuID = e.Row.Cells[0].FindControl("hdnNoteSubMenuID") as HiddenField;

                    if (hdnNoteSubMenuID != null)
                    {
                        string chechBoxName = "cbxDoubleSubMenu";
                        List<MenuNodeDataView> doubleSubMenuNodes = nodeList
                            .Where(p => p.parentNode == Int32.Parse(hdnNoteSubMenuID.Value)).ToList();
                        gvSubMenuLink.DataSource = doubleSubMenuNodes;
                        gvSubMenuLink.DataBind();
                        CheckIfCheckBoxIsChecked(chechBoxName, doubleSubMenuNodes, gvSubMenuLink);
                    }
                }
            }
        }

        public void ddlRole_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var dll = sender as DropDownList;
            this.hdnDllValue.Value = dll.SelectedValue;
            UserControlLoad();
        }

        public void btnCancelAddMenuItem_Click(Object sender, EventArgs e)
        {
            this.pnlAddNewMenuItem.Visible = false;
        }

        public void btnAddMenuItem_Click(Object sender, EventArgs e)
        {
            var newMenuNode = new ETEMModel.Models.MenuNode();
            var newUrl = new ETEMModel.Models.NavURL();
            Button btnSender = sender as Button;
            if (btnSender != null && btnSender.Text == BaseHelper.GetCaptionString("Add_Btn"))
            {

                int selectedMainMenuItemValue = int.Parse(this.ddlMainMenuItems.SelectedValue.ToString());

                if (selectedMainMenuItemValue != Constants.INVALID_ID_ZERO)
                {
                    int selectedSubMenuItemValue = int.Parse(this.ddlSubMenuItems.SelectedValue.ToString());
                    if (selectedSubMenuItemValue != Constants.INVALID_ID_ZERO)
                    {
                        newMenuNode.parentNode = selectedSubMenuItemValue;
                        newMenuNode.name = this.tbxNewMenuItemName.Text;
                        newMenuNode.type = "link";
                    }
                    else
                    {
                        newMenuNode.parentNode = selectedMainMenuItemValue;
                        newMenuNode.name = this.tbxNewMenuItemName.Text;
                        newMenuNode.type = "parent";
                    }
                }
                else
                {
                    newMenuNode.parentNode = Constants.INVALID_ID_ZERO;
                    newMenuNode.name = this.tbxNewMenuItemName.Text;
                    newMenuNode.type = "root";


                }


            }
            else if (btnSender != null)
            {
                var edittingIndexValue = this.hdnEditingIndexValue.Value;
                newMenuNode = new ETEMModel.Models.MenuNode();
                newMenuNode.idNode = int.Parse(edittingIndexValue);
                newMenuNode.name = this.tbxNewMenuItemName.Text;
                newMenuNode.parentNode = int.Parse(this.ddlCurrentNodePlace.SelectedValue);
                newMenuNode.type = this.hdnEditingNodeType.Value;
                newUrl.idNavURL = int.Parse(this.hdnEditingNodeUrlIndex.Value);

            }

            newUrl.URL = this.txvNavUrl.Text;
            newUrl.code = "1";
            newUrl.QueryParams = this.tbxQueryParams.Text.Trim();

            this.ownerPage.CallContext.CurrentConsumerID = this.ownerPage.UserProps.IdUser;
            this.ownerPage.CallContext = this.ownerPage.AdminClientRef.MenuNodeSave(newUrl, newMenuNode, this.ownerPage.CallContext);
            CheckIfResultIsSuccess();
            AddResultMessage(this.ownerPage.CallContext);
            this.ownerPage.ReloadMenuNodeDataViewApplication();
        }

        private void AddResultMessage(CallContext callContext)
        {
            this.lbResultContext.Text = callContext.Message;
        }

        protected void lnkbtnEdit_OnClick(object sender, EventArgs e)
        {
            LinkButton btnEdit = sender as LinkButton;
            if (btnEdit != null)
            {
                var nodeId = btnEdit.CommandArgument;
                HiddenControlsLoad(nodeId);
                this.pnlAddNewMenuItem.Visible = true;

            }
        }

        //protected void btnHideNestedGrids_OnClick(object sender, EventArgs e)
        //{
        //    LinkButton btnEdit = sender as LinkButton;
        //    GridView gvSubMenu = this.gvMainManu.Rows[0].Cells[0].FindControl("gvSubMenu") as GridView;
        //    if (gvSubMenu.Visible==true)
        //    {
        //        gvSubMenu.Visible = false;
        //    }
        //    else
        //    {
        //        gvSubMenu.Visible = true;
        //    }
        //}


        private void CheckIfResultIsSuccess()
        {
            if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-success");
            }
            else
            {
                this.lbResultContext.Attributes.Add("class", "alert alert-error");
            }
        }
        public void btnShowPlNewItem_Click(Object sender, EventArgs e)
        {
            HiddenControlsLoad();
            SetInitialValues();
        }

        private void SetInitialValues()
        {
            this.ddlCurrentNodePlace.Visible = false;
            this.ddlMainMenuItems.Visible = true;
            this.pnlAddNewMenuItem.Visible = true;
            this.lbMainNodesDll.Visible = true;
            this.lbSubMenuItems.Visible = true;
            this.txvNavUrl.Text = string.Empty;
            this.tbxNewMenuItemName.Text = string.Empty;
            this.tbxQueryParams.Text = string.Empty;
            this.lbNodePlace.Visible = false;
            this.btnAddMenuItem.Text = BaseHelper.GetCaptionString("Add_Btn");
            this.PopUpHeadline.InnerText = BaseHelper.GetCaptionString("Adding_Nodes");
            this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("Add_New_Item_In_Base_Menu");
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;
        }

        private void HiddenControlsLoad(string nodeId = null)
        {

            List<MenuNodeDataView> allnodes = this.ownerPage.AdminClientRef.GetAllMenuNode(this.ownerPage.CallContext);
            if (nodeId == null)
            {
                List<MenuNodeDataView> nodeListRootItems = allnodes.Where(m => m.parentNode == 0).ToList();
                AddDefaultEmtryValue(nodeListRootItems, BaseHelper.GetCaptionString("UI_Please_Select"));
                this.ddlMainMenuItems.DataSource = nodeListRootItems;
                this.ddlMainMenuItems.DataBind();
            }
            else
            {
                List<MenuNodeDataView> listRootsAndParentsNodes = allnodes.Where(n => (n.type == "root") || n.type == "parent").ToList();
                List<MenuNodeDataView> rootNodes = allnodes.Where(n => (n.type == "root")).ToList();
                List<MenuNodeDataView> parentNodes = allnodes.Where(n => n.type == "parent").ToList();
                MenuNodeDataView edittedNode = allnodes.FirstOrDefault(n => n.idNode == int.Parse(nodeId));
                NavURL navUrl = this.ownerPage.AdminClientRef.GetUrlNavById(edittedNode.idNavURL);

                SetEddingNodeValues(nodeId, edittedNode, navUrl);

                bool isSelectedNodeInRoots = rootNodes.Any(n => n.idNode == int.Parse(nodeId));
                if (isSelectedNodeInRoots)
                {
                    this.hdnEditingNodeType.Value = "root";
                    this.hdnEditingNodeParent.Value = Constants.INVALID_ID_ZERO_STRING;
                    this.lbNodePlace.Visible = true;
                    this.lbNodePlace.Text = BaseHelper.GetCaptionString("Can_Only_Change_Name_And_URL");

                }
                else
                {
                    bool isEditedInParentNodes = parentNodes.Any(n => n.idNode == int.Parse(nodeId));
                    this.hdnEditingNodeParent.Value = edittedNode.parentNode.ToString();
                    if (isEditedInParentNodes)
                    {
                        this.hdnEditingNodeType.Value = "parent";

                        AddDefaultEmtryValue(rootNodes, BaseHelper.GetCaptionString("Make_Node_Root"));
                        BindDdl(rootNodes, ddlCurrentNodePlace, BaseHelper.GetCaptionString("Can_Change_Position_In_Root_Menu")
                            , lbNodePlace, edittedNode.parentNode.ToString());
                    }
                    else
                    {
                        this.hdnEditingNodeType.Value = "link";

                        AddDefaultEmtryValue(listRootsAndParentsNodes, BaseHelper.GetCaptionString("Make_Node_Root"));
                        BindDdl(listRootsAndParentsNodes, ddlCurrentNodePlace, BaseHelper.GetCaptionString("Can_Change_Position_In_Root_And_Parent_Menu")
                            , lbNodePlace, edittedNode.parentNode.ToString());
                    }
                }

            }


        }

        private void SetEddingNodeValues(string nodeId, MenuNodeDataView edittedNode, NavURL navUrl)
        {
            this.hdnEditingIndexValue.Value = nodeId;
            this.hdnEditingNodeUrlIndex.Value = edittedNode.idNavURL.ToString();

            this.btnAddMenuItem.Text = BaseHelper.GetCaptionString("Save_Btn");
            this.txvNavUrl.Text = navUrl.URL;
            this.tbxNewMenuItemName.Text = edittedNode.name;
            this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("New_Node_Name");
            this.tbxQueryParams.Text = navUrl.QueryParams;
            this.ddlMainMenuItems.Visible = false;
            this.ddlSubMenuItems.Visible = false;
            lbMainNodesDll.Visible = false;
            lbSubMenuItems.Visible = false;
            this.PopUpHeadline.InnerText = BaseHelper.GetCaptionString("Editing_Nodes");
            this.lbResultContext.Attributes.Remove("class");
            this.lbResultContext.Text = string.Empty;
        }

        private void FindSelectedMainItem(int? id, List<MenuNodeDataView> allNodes)
        {
            MenuNodeDataView parentNode = allNodes.FirstOrDefault(n => n.idNode == id);
            int? mainNodeId = parentNode.parentNode;
            this.ddlMainMenuItems.Items.FindByValue(mainNodeId.ToString()).Selected = true;

        }


        public void btnComfirmDelete_Click(Object sender, EventArgs e)
        {
            var nodeId = int.Parse(this.hdnEditingIndexValue.Value);
            List<MenuNodeDataView> allNodes = this.ownerPage.AdminClientRef.GetAllMenuNode(this.ownerPage.CallContext);
            bool hasParentNodes = allNodes.Any(n => n.parentNode == nodeId);
            if (hasParentNodes)
            {
                this.lbDelComfirmationResult.Text = BaseHelper.GetCaptionString("Node_has_parent_nodes_deletion_canceled");
                this.lbDelComfirmationResult.Attributes.Add("class", "alert alert-error");
            }
            else
            {
                this.ownerPage.CallContext = this.ownerPage.AdminClientRef.RemoveMenuNode(nodeId, this.ownerPage.CallContext);
                if (this.ownerPage.CallContext.ResultCode == ETEMEnums.ResultEnum.Success)
                {
                    this.pnlDeleteComfirmation.Visible = false;
                }
                else
                {
                    this.lbDelComfirmationResult.Text = this.ownerPage.CallContext.Message;
                    this.lbDelComfirmationResult.Attributes.Add("class", "alert alert-error");
                }
            }

        }

        public void btnCancelDelete_Click(Object sender, EventArgs e)
        {
            this.pnlDeleteComfirmation.Visible = false;
        }


        protected void lnkbtnDelete_OnClick(object sender, EventArgs e)
        {
            this.lbDelComfirmationResult.Text = string.Empty;
            this.lbDelComfirmationResult.Attributes.Remove("class");
            LinkButton btnDelete = sender as LinkButton;
            if (btnDelete != null)
            {
                var nodeId = btnDelete.CommandArgument;
                this.hdnEditingIndexValue.Value = nodeId;
                this.pnlDeleteComfirmation.Visible = true;

            }
        }


        private static void AddDefaultEmtryValue(List<MenuNodeDataView> nodeListRootItems, string name)
        {
            nodeListRootItems.Insert(Constants.INVALID_ID_ZERO, new MenuNodeDataView
            {
                idNode = Constants.INVALID_ID_ZERO,
                name = name

            });
        }



        public void ddlMainMenuItems_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            List<MenuNodeDataView> allnodes = this.ownerPage.AdminClientRef.GetAllMenuNode(this.ownerPage.CallContext);

            var ddlMainNode = sender as DropDownList;
            int selectedMainNodeValue = int.Parse(ddlMainNode.SelectedValue.ToString());
            if (selectedMainNodeValue != Constants.INVALID_ID_ZERO)
            {
                List<MenuNodeDataView> nodeSubRootItems = allnodes.Where(m => m.parentNode == selectedMainNodeValue).ToList();
                this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("Add_New_Item_In_Main_Menu");
                AddDefaultEmtryValue(nodeSubRootItems, BaseHelper.GetCaptionString("UI_Please_Select"));
                BindDdl(nodeSubRootItems, this.ddlSubMenuItems);
            }
            else
            {
                this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("Add_New_Item_In_Base_Menu");
                this.ddlSubMenuItems.Visible = false;
            }

        }

        private void BindDdl(List<MenuNodeDataView> nodeItems, DropDownList dll)
        {
            dll.DataSource = nodeItems;
            dll.DataBind();
            dll.Visible = true;
        }

        private void BindDdl(List<MenuNodeDataView> nodeItems, DropDownList dll, string lbText, Label lb, string parentId)
        {
            dll.DataSource = nodeItems;
            dll.DataBind();
            dll.Visible = true;
            dll.Items.FindByValue(parentId).Selected = true;
            lb.Text = lbText;
            lb.Visible = true;
        }
        public void ddlSubMenuItems_OnSelectedIndexChanged(Object sender, EventArgs e)
        {
            var ddlSubNode = sender as DropDownList;
            int selectedSubNodeValue = int.Parse(ddlSubNode.SelectedValue.ToString());
            if (selectedSubNodeValue != Constants.INVALID_ID_ZERO)
            {
                this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("Add_New_Item_In_Sub_Menu");
            }
            else
            {
                this.lbNewMenuItemName.Text = BaseHelper.GetCaptionString("Add_New_Item_In_Main_Menu");
            }

        }

    }
}