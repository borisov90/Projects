using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ETEM.Freamwork;
using ETEMModel.Helpers;

namespace ETEM.Admin
{
    public partial class OnlineUsersList : BasicPage
    {

        public static FormResources formResource = new FormResources
        {
            Module = Constants.MODULE_SUPPORT_HISTORY,
            PageFullName = Constants.UMS_ADMIN_ONLINE_USERSLIST,
            PagePath = "../Admin/OnlineUsersList.aspx"

        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.GridViewSortExpression == Constants.INVALID_ID_STRING)
                {
                    this.GridViewSortExpression = "LastRequestDateTime";
                    this.GridViewSortDirection = Constants.SORTING_DESC;
                }


                FormLoad();
            }
        }

        public override void FormLoad()
        {
            try
            {

                CheckUserActionPermission(ETEMEnums.SecuritySettings.ShowOnlineUsersList, true);

                List<UserProps> onlineUsers = (from u in GetOnlineUsers()
                                               select new UserProps
                                                  {
                                                      UserName = u.UserName,
                                                      PersonNamePlusTitle = u.PersonNamePlusTitle,
                                                      SessionID = u.SessionID,
                                                      LoginDateTime = u.LoginDateTime,
                                                      LastRequestDateTime = u.LastRequestDateTime,
                                                      IPAddress = u.IPAddress,
                                                      IsKilled = u.IsKilled,
                                                      LastPageName = u.LastPageName,
                                                      LastModuleName = u.LastModuleName
                                                  }).ToList();
                onlineUsers = BaseClassBL<UserProps>.Sort(onlineUsers, this.GridViewSortExpression, this.GridViewSortDirection).ToList();
                this.gvOnlineUsers.DataSource = onlineUsers;

                if (NewPageIndex.HasValue)
                {
                    this.gvOnlineUsers.PageIndex = NewPageIndex.Value;
                }
                this.gvOnlineUsers.DataBind();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
        }

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

        protected void gvOnlineUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            base.SetSortDirection();
            FormLoad();
        }

        protected void gvOnlineUsers_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.NewPageIndex = e.NewPageIndex;
            FormLoad();
        }


        protected void lnkBtnServerDelete_Click(object sender, EventArgs e)
        {

            try
            {


                if (!this.CheckUserActionPermission(ETEMEnums.SecuritySettings.OnlineUsersListDelete, false))
                {
                    return;
                }

                LinkButton lnkBtnServerDelete = sender as LinkButton;

                if (lnkBtnServerDelete == null)
                {
                    this.ShowMSG("lnkBtnServerDelete is null");
                    return;
                }
                string sessionID = BaseHelper.ParseStringByAmpersand(lnkBtnServerDelete.CommandArgument)["SessionID"].ToString();

                UserProps userProps = GetOnlineUsers().Where(s => s.SessionID == sessionID).FirstOrDefault() as UserProps;

                userProps.IsKilled = true;

                FormLoad();
            }
            catch (Exception ex)
            {

                BaseHelper.Log("Грешка в FormLoad " + formResource.PagePath);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }


        }
    }
}