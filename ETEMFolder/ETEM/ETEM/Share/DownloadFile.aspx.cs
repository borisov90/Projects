using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ETEM.Freamwork;

namespace ETEM.Share
{
    public partial class DownloadFile : BasicPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string repFileName = this.FormContext.QueryString["FilePath"].ToString();

            FileStream readRepFile = File.OpenRead(repFileName);
            if (readRepFile.Length != 0)
            {
                byte[] repData = new byte[readRepFile.Length];
                readRepFile.Read(repData, 0, (int)readRepFile.Length);
                readRepFile.Flush();
                readRepFile.Close();

                if (this.FormContext.QueryString.Contains("Delete") &&
                    this.FormContext.QueryString["Delete"].ToString().Equals("true"))
                {
                    File.Delete(repFileName);
                }

                Response.Clear();
                string fileName = Path.GetFileName(repFileName).Replace(" ", "_");
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));

                if (this.FormContext.QueryString.ContainsKey("ContentType"))
                {
                    Response.ContentType = this.FormContext.QueryString["ContentType"].ToString();
                }
                Response.BinaryWrite(repData);
                Response.End();
            }

            Response.Write("<script type=\"text/javascript\" language=\"javascript\">window.close();</script>");
        }
    }
}