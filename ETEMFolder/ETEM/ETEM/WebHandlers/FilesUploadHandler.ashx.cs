using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Helpers;
using ETEMModel.Services.Common;
using ETEMModel.Models;
using ETEM.Freamwork;
using System.IO;

namespace ETEM.WebHandlers
{
    /// <summary>
    /// Summary description for FilesUploadHandler
    /// </summary>
    public class FilesUploadHandler : IHttpHandler
    {
        private Common CommonClientRef;
        private System.Collections.Hashtable queryString;
        private UploadedFile currentEntity;

        public void ProcessRequest(HttpContext context)
        {
            this.CommonClientRef = new Common();

            DecryptQueryString(context.Request);

            string path = string.Empty;
            if (this.queryString["UploadPath"] != null)
            {
                path = this.queryString["UploadPath"].ToString();
            }

            HttpPostedFile fileToUpload = context.Request.Files["Filedata"];
            string pathToSave = (path.EndsWith("\\") ? path : path + "\\") + fileToUpload.FileName;

            fileToUpload.SaveAs(pathToSave);

            #region Save Entity

            this.currentEntity = new UploadedFile();

            string[] user = path.Split('_');
            string[] resName = user.First().Split('\\');
            FileInfo fi = new FileInfo(pathToSave);

            currentEntity.idResource = Convert.ToInt32(user.Last());
            currentEntity.ResourceName = resName.Last();
            currentEntity.FilePath = pathToSave;
            currentEntity.FileName = fileToUpload.FileName;
            currentEntity.Extension = Path.GetExtension(fileToUpload.FileName).Substring(1);
            currentEntity.DateUpload = DateTime.Now;
            currentEntity.Size = Convert.ToInt32(fi.Length / (1024));// в KБ
            //currentEntity.ContentType = 

            CallContext resultContext = new CallContext();
            resultContext.CurrentConsumerID = user.Last();
            resultContext = CommonClientRef.UploadedFileSave(currentEntity, resultContext);

            #endregion
        }

        private void DecryptQueryString(HttpRequest _currRequest)
        {
            this.queryString = new System.Collections.Hashtable();
            if (_currRequest.QueryString.Count > 0)
            {
                string rowURL = _currRequest.RawUrl.Substring(_currRequest.RawUrl.IndexOf('?') + 1);
                string rawQueryString = System.Web.HttpUtility.UrlDecode(rowURL);

                if (rawQueryString.EndsWith("&"))
                {
                    rawQueryString = rawQueryString.Substring(0, rawQueryString.Length - 1);
                }

                string queryString = BaseHelper.Decrypt(rawQueryString);

                string val;
                string key;

                string[] keys_values = queryString.Split('&');

                foreach (string key_value in keys_values)
                {
                    string[] kv = key_value.Split('=');

                    key = kv[0];
                    val = kv[1];

                    this.queryString.Add(key, val);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}