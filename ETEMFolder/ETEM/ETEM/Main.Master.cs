using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Models.DataView;
using ETEMModel.Models;
using System.Web.UI.HtmlControls;
using ETEMModel.Services.Common;
using ETEMModel.Helpers;

namespace ETEM
{
    public partial class Main : System.Web.UI.MasterPage
    {

        private Common commonClientRef;

        public Main()
        {
            this.commonClientRef = new Common();
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ProfileData.Visible = false;
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {


            var ownerPage = this.Page as GeneralPage;
            List<Role> roles = ownerPage.UserProps.Roles;

            string name = ownerPage.UserProps.PersonTwoNamePlusTitle;
            this.lbPersonName.Text = name;

            HashSet<MenuNodeDataView> allMenuNodes = new HashSet<MenuNodeDataView>();
            List<MenuNodeDataView> nodesForRoles = ownerPage.AdminClientRef.GetAllRoleMenuNodeByAllRoles(roles);
            for (int i = 0; i < roles.Count; i++)
            {
                List<MenuNodeDataView> menuNodes = nodesForRoles.Where(s => s.IdRole == roles[i].idRole).OrderBy(s => s.nodeOrder).ToList();
                for (int j = 0; j < menuNodes.Count; j++)
                {
                    var currentMenuNodeId = menuNodes[j].idNode;
                    bool isInHashSet = allMenuNodes.Any(n => n.idNode == currentMenuNodeId);
                    if (!isInHashSet)
                    {
                        allMenuNodes.Add(menuNodes[j]);
                    }
                }
            }

            List<MenuNodeDataView> rootNodes = allMenuNodes.Where(n => n.type == "root").OrderBy(s => s.nodeOrder).ToList();
            List<MenuNodeDataView> parentNodes = allMenuNodes.Where(n => n.type == "parent").OrderBy(s => s.nodeOrder).ToList();
            List<MenuNodeDataView> linkNodes = allMenuNodes.Where(n => n.type == "link").OrderBy(s => s.nodeOrder).ToList();
            for (int i = 0; i < rootNodes.Count; i++)
            {
                HtmlGenericControl listElement = new HtmlGenericControl("li");

                HtmlAnchor htmlanchor = new HtmlAnchor();
                htmlanchor.HRef = "#";
                htmlanchor.Attributes.Add("class", "mainNodeItem");
                htmlanchor.InnerText = rootNodes[i].name;
                listElement.Controls.Add(htmlanchor);

                HtmlGenericControl divHolder = new HtmlGenericControl("div");
                divHolder.Attributes.Add("class", "cbp-hrsub");

                HtmlGenericControl divInner = new HtmlGenericControl("div");
                divInner.Attributes.Add("class", "cbp-hrsub-inner");
                divHolder.Controls.Add(divInner);
                List<MenuNodeDataView> parentsFromCurrentRoot = parentNodes.Where(n => n.parentNode == rootNodes[i].idNode).ToList();
                for (int parentNodeIndex = 0; parentNodeIndex < parentsFromCurrentRoot.Count; parentNodeIndex++)
                {
                    HtmlGenericControl currntParentDiv = new HtmlGenericControl("div");
                    HtmlGenericControl currntParentName = new HtmlGenericControl("h4");
                    currntParentName.InnerText = parentsFromCurrentRoot[parentNodeIndex].name;
                    currntParentDiv.Controls.Add(currntParentName);
                    HtmlGenericControl currntLinkUl = new HtmlGenericControl("ul");
                    List<MenuNodeDataView> linkNodesOnCurrentParent = linkNodes.Where(n => n.parentNode ==
                        parentsFromCurrentRoot[parentNodeIndex].idNode).ToList();
                    for (int linkNodeIndex = 0; linkNodeIndex < linkNodesOnCurrentParent.Count; linkNodeIndex++)
                    {
                        HtmlGenericControl currentLinkListElement = new HtmlGenericControl("li");
                        HtmlAnchor htmlLink = new HtmlAnchor();
                        htmlLink.Attributes.Add("onclientclick", "makeMenuActive()");
                        htmlLink.InnerText = linkNodesOnCurrentParent[linkNodeIndex].name;
                        htmlLink.HRef = linkNodesOnCurrentParent[linkNodeIndex].URL;

                        string queryParams = "Node=" + linkNodesOnCurrentParent[linkNodeIndex].idNode;


                        //hardcode idNote Преглед на действията в системата
                        if (linkNodesOnCurrentParent[linkNodeIndex].idNode == 43)
                        {
                            htmlLink.Target = "_blank";
                        }

                        if (!string.IsNullOrEmpty(linkNodesOnCurrentParent[linkNodeIndex].QueryParams))
                        {
                            queryParams += "&" + linkNodesOnCurrentParent[linkNodeIndex].QueryParams;
                        }



                        htmlLink.HRef += "?" + BaseHelper.Encrypt(queryParams);




                        currentLinkListElement.Controls.Add(htmlLink);

                        currntLinkUl.Controls.Add(currentLinkListElement);

                    }

                    currntParentDiv.Controls.Add(currntLinkUl);
                    divInner.Controls.Add(currntParentDiv);
                }
                listElement.Controls.Add(divHolder);
                this.MainNavUl.Controls.Add(listElement);

                //пътя до формата на която сме в момента
                if (ownerPage.FormContext.QueryString["Node"] != null)
                {
                    int idNode = Int32.Parse(ownerPage.FormContext.QueryString["Node"].ToString());
                    this.lbNodeName.Text = ownerPage.AdminClientRef.GetMenuNodeFullPath(idNode, string.Empty);


                }
            }

            this.countMassages.Text = commonClientRef.GetNotificationCountByPersonID(ownerPage.UserProps.PersonID).ToString();
        }

        protected void lnkBtnLogFileWeb_Click(object sender, EventArgs e)
        {

        }



        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.countMassages.Text = "";
            //var ownerPage = this.Page as GeneralPage;
            this.countMassages.Text = commonClientRef.GetNotificationCountByPersonID("48").ToString();
        }

        protected void ToolkitScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {

        }

        protected void lnkBtnProfile_Click(object sender, EventArgs e)
        {
            this.ProfileData.Visible = true;
            this.ProfileData.UserControlLoad();
        }
    }
}