using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ETEMModel.Helpers.Common;
using System.Web.Services;


using ETEMModel.Helpers.AbstractSearchBLHolder;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using ETEMModel.Helpers.Admin;
using ETEMModel.Helpers;
using ETEM.Freamwork;
using ETEMModel.Models;


namespace ETEM.Services.Common
{
    [ServiceContract(Namespace = "Common")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Common
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        [OperationContract]
        public string DoWork()
        {
            // Add your operation implementation here
            return "test";
        }


       


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public string Log(string username, string pass)
        {
            //from android comes like this - ?-360
            CallContext resultContext = new UserBL().Login(username, pass, null);
            UserProps userProps = new UserProps();

            if (resultContext.ResultCode == ETEMEnums.ResultEnum.Success)
            {

                User currentUser = new UserBL().GetUserByUserID(resultContext.EntityID);

                if (currentUser != null)
                {

                    Person person = new PersonBL().GetPersonByPersonID(currentUser.idPerson.ToString());
                    userProps.PersonNamePlusTitle = person.FullNamePlusTitle;
                    userProps.PersonNameNoTitle = person.FullName;
                    userProps.PersonTwoNamePlusTitle = person.TwoNamesPlusTitle;
                    userProps.PersonID = person.idPerson.ToString();

                    userProps.LoginDateTime = DateTime.Now;
                    
                    
                    
                    userProps.idStudent = Constants.INVALID_ID;

                }

            }

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(userProps);

            return json;
        }


        

        /// <summary>
        /// Return list to auto complete control
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <param name="contextKey"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]

        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            //fiddler
            //http://localhost/ETEM/Services/Common/Common.svc/GetCompletionList
            //User-Agent: Fiddler
            //Host: localhost
            //Content-Length: 92
            //Content-Type: application/json;charset=UTF-8
            //{
            //    "prefixText":"геор",
            //    "count":"10",
            //    "contextKey":"case=StudentAndCandidateByName"
            //}
            if (count == 0)
            {
                count = 10;
            }

            List<string> items = new List<string>();

            items = new CommonBL().GetCompletionList(prefixText, count, contextKey);

            return items.ToArray<string>();
        }


        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public string[] TestCompletionList(string prefixText, int count, string contextKey) //string prefixText, int count, string contextKey
        {



            if (count == 0)
            {
                count = 10;
            }

            List<string> items = new List<string>();

            items = new CommonBL().GetCompletionList(prefixText, count, contextKey);

            //return "test"; 

            return items.ToArray<string>();
        }
        // Add more operations here and mark them with [OperationContract]
    }
}
