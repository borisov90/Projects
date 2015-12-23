using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Reflection;
using System.Web.SessionState;
using ETEM.Freamwork;

namespace ETEM.Admin
{
    public partial class TestShowAllSessions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sess = GetActiveSessions().ToList();

            List<UserProps> listUserProps = new List<UserProps>();

            foreach (var ses in sess)
            {
                UserProps up = ses["USER_PROPERTIES"] as UserProps;

                if (up != null)
                {
                    listUserProps.Add(up);
                }
            }

            this.GridView1.DataSource = listUserProps;
            this.GridView1.DataBind();


        }

        public IEnumerable<SessionStateItemCollection> GetActiveSessions()
        {
            object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            object[] obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);

            for (int i = 0; i < obj2.Length; i++)
            {
                Hashtable c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
                foreach (DictionaryEntry entry in c2)
                {
                    object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                    if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                    {
                        SessionStateItemCollection sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
                        if (sess != null)
                        {
                            yield return sess;
                        }
                    }
                }
            }
        }
    }
}