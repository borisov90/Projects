using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Crm.Sdk.Messages;
using CRMServices.Helpers;
using System.Globalization;
using System.Net.Mail;

namespace CrmSupportLink
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public string HelloWorld()
        {
            return "This service is working!";
        }
        [WebMethod]
        public void PublishInCRM(ServiceRequest serviceRequest)
        {
            //Connects to the database and Logs the User In
            var connection = ConnectToDatabase();
            var service = new OrganizationService(connection);
            var context = new CrmOrganizationServiceContext(connection);

            //const int hour = 60;

            EventLog.saveMessage("PublishIssue SRID:" + serviceRequest.SRID);

            //Creating the new Case
            Entity incident = new Entity("incident");

            try
            {
                //Filling the Data for the new case
                incident["createdon"] = serviceRequest.RegistrationDate;
                incident["description"] = serviceRequest.LongDescription;
                incident["statuscode"] = ReturnStatusCode(serviceRequest.ServiceRequestStatus);
                incident["subjectid"] = ReturnRequestType(serviceRequest.ServiceRequestType);
                incident["new_moduleoptionset"] = ReturnModuleCode("TS");
                //incident["ownerid"] = new EntityReference("systemuser", findConsultantID(serviceRequest.AssignedPerson, service));
                incident["new_caseasignedto"] = serviceRequest.AssignedPerson;
                incident["new_statushistory"] = serviceRequest.CommentsMatricia;
                incident["casetypecode"] = returnRequestKind(serviceRequest.ServiceRequestKind);
                incident["followupby"] = serviceRequest.DueDate;
                incident["new_supportrequestid"] = serviceRequest.SRID;
                incident["title"] = serviceRequest.AssignedToClient + " " + serviceRequest.SRID + " " + serviceRequest.companyName;
                //incident["customerid"] = new EntityReference("account", findCustomer((string)serviceRequest.companyName, service));
                incident["customerid"] = new EntityReference("account", findCustomerID(serviceRequest.companyName));
                incident["new_statushistory"] = serviceRequest.ShortDescription;
                incident["new_assignedfrom"] = serviceRequest.CreatedBy;

                Guid consultantID = findConsultantID(serviceRequest.AssignedPerson, service);

                
                //Adding the created case to CRM;
                var incidentGuid = service.Create(incident);

                //Assign a case!
                EventLog.saveMessage("Start of Assignment! to :" + consultantID);
                AssignRequest assignRequest = new AssignRequest();
                assignRequest.Assignee = new EntityReference("systemuser", consultantID);
                assignRequest.Target = new EntityReference(incident.LogicalName, incidentGuid);

                service.Execute(assignRequest);
            }
            catch (Exception)
            {
                EventLog.saveMessage("This case was not created in CRM " + serviceRequest.CreatedBy + "'" + serviceRequest.SRID); ;
            }



        }
        [WebMethod]
        public void UpdateStatus(ServiceRequest serviceRequest)
        {
            var idOfUpdatedItem = serviceRequest.SRID;

            //Login and connect to the server and create the context
            var connection = ConnectToDatabase();
            var service = new OrganizationService(connection);
            var context = new CrmOrganizationServiceContext(connection);

            //Gather the components for the "Retrieve" function
            ColumnSet set = new ColumnSet();
            set.AllColumns = true;
            Guid incidentGuid = GetGUIDByName(idOfUpdatedItem, service);

            //Retrieves the record that will be updated
            var incident = service.Retrieve("incident", incidentGuid, set);

            EventLog.saveMessage("Update Status of SRID:" + serviceRequest.SRID);

            try
            {
                // Actual UPDATE of the record.

                incident["description"] = serviceRequest.LongDescription;
                incident["statuscode"] = ReturnStatusCode(serviceRequest.ServiceRequestStatus);
                incident["subjectid"] = ReturnRequestType(serviceRequest.ServiceRequestType);
                incident["new_moduleoptionset"] = ReturnModuleCode("TS");
                //incident["ownerid"] = new EntityReference("systemuser", findConsultantID(serviceRequest.AssignedPerson,service));
                incident["new_statushistory"] = serviceRequest.CommentsMatricia;
                incident["casetypecode"] = returnRequestKind(serviceRequest.ServiceRequestKind);
                incident["new_caseasignedto"] = serviceRequest.AssignedPerson;
                //incident["followupby"] = serviceRequest.DueDate;
                incident["title"] = serviceRequest.AssignedToClient + " " + serviceRequest.SRID + " " + serviceRequest.companyName;
                //incident["customerid"] = new EntityReference("account", findCustomer((string)serviceRequest.companyName, service));
                incident["customerid"] = new EntityReference("account", findCustomerID(serviceRequest.companyName));
                incident["new_statushistory"] = serviceRequest.ShortDescription;
                incident["new_assignedfrom"] = serviceRequest.CreatedBy;

                Guid consultantID = findConsultantID(serviceRequest.AssignedPerson, service);

                //Assign a case!
               

                service.Update(incident);

                EventLog.saveMessage("Start of Assignment! to :" + consultantID);
                AssignRequest assignRequest = new AssignRequest();
                assignRequest.Assignee = new EntityReference("systemuser", consultantID);
                assignRequest.Target = new EntityReference(incident.LogicalName, incidentGuid);
                service.Execute(assignRequest);

            }
            catch (Exception)
            {
                EventLog.saveMessage("This record is unavailable for update right now!" + serviceRequest.SRID);
                return;
            }

        }
        [WebMethod]
        public void UpdateAssignedPerson(ServiceRequest serviceRequest)
        {
            var idOfUpdatedItem = serviceRequest.SRID;

            var connection = ConnectToDatabase();
            var service = new OrganizationService(connection);
            var context = new CrmOrganizationServiceContext(connection);

            Guid consultantID = findConsultantID(serviceRequest.AssignedPerson, service);
            ColumnSet set = new ColumnSet();
            set.AllColumns = true;

            //Gather the components for the "Retrieve" function
            Guid incidentGuid = GetGUIDByName(idOfUpdatedItem, service);

            //Retrieves the record that will be updated
            var incident = service.Retrieve("incident", incidentGuid, set);
            EventLog.saveMessage("Updating the consultant person of case: " + serviceRequest.SRID + "to " + serviceRequest.AssignedPerson);
            try
            {
                //Assign a case!
                AssignRequest assignRequest = new AssignRequest();
                assignRequest.Assignee = new EntityReference("systemuser", consultantID);
                assignRequest.Target = new EntityReference(incident.LogicalName, incidentGuid);

                //sets the new User.
                //incident["ownerid"] = new EntityReference("systemuser", consultantID);
               
                service.Update(incident);
                service.Execute(assignRequest);

            }
            catch (Exception)
            {
                EventLog.saveMessage("Updating the consultant person of case: " + serviceRequest.SRID + "to " + serviceRequest.AssignedPerson + " failed!");

                return;
            }
        }

        [WebMethod]


        public void SendMail(string mailFrom, string mailToSupport, string subject, string body)
        {
            try
            {
                EventLog.saveMessage("Sending mail to:" + mailToSupport);

                MailMessage messageForSupport = new MailMessage(mailFrom, mailToSupport, subject, body);
                SmtpClient clientSupport = new SmtpClient(MainFunctions.GetValueFromWebConfig("MailServer"));
                clientSupport.UseDefaultCredentials = true;
                clientSupport.Credentials = new System.Net.NetworkCredential(mailFrom, MainFunctions.GetValueFromWebConfig("MailFromPassword"));
                clientSupport.Send(messageForSupport);

                EventLog.saveMessage("Mail is sent to :" + mailToSupport);
            }
            catch (Exception exx)
            {
                EventLog.saveMassage("Грешка при изпращане на e-mail на: '" + mailToSupport + "'", exx);
            }

        }

        private bool checkAccess()
        {
            bool result = true;

            //EventLog.saveMassage("IP адрес:" +this.Context.Request.UserHostAddress);


            return result;
        }

        private OptionSetValue returnRequestKind(string serviceRequestKind)
        {
            switch (serviceRequestKind)
            {
                case "Implementation": return new OptionSetValue(2);
                case "Support": return new OptionSetValue(3);
                default: return new OptionSetValue(1);
            }
        }

        private EntityReference ReturnRequestType(string serviceRequestType)
        {
            var FunctionalityRequest = new EntityReference("subject", new Guid("D0BD4FAF-E56B-E411-B41E-00155D0A4E33"));
            var newQueryRequest = new EntityReference("subject", new Guid("7E8420F0-E56B-E411-B41E-00155D0A4E33"));
            var technicalQuestionRequest = new EntityReference("subject", new Guid("905EE10D-E66B-E411-B41E-00155D0A4E33"));
            var functionalQuestionRequest = new EntityReference("subject", new Guid("0C6AE716-E66B-E411-B41E-00155D0A4E33"));
            var typeOfRequest = new EntityReference("subject", new Guid("BE180831-FBCF-40C0-9D08-79D4B283C6F9"));

            switch (serviceRequestType)
            {
                case "FuncQuestion": return functionalQuestionRequest;
                case "Report": return newQueryRequest;
                case "TechSupport": return technicalQuestionRequest;
                case "Functionality": return FunctionalityRequest;
                default: return technicalQuestionRequest;
            }
        }

        private OptionSetValue ReturnModuleCode(string serviceRequestStatus)
        {

            OptionSetValue contractManagement = new OptionSetValue(100000000);//
            OptionSetValue generalLedger = new OptionSetValue(100000001);//
            OptionSetValue purchaseLedger = new OptionSetValue(100000003);//
            OptionSetValue salesLedger = new OptionSetValue(100000004);//
            OptionSetValue queriesReports = new OptionSetValue(100000005);
            OptionSetValue purchaseOrder = new OptionSetValue(100000006);//
            OptionSetValue salesOrder = new OptionSetValue(100000007);//
            OptionSetValue serviceManagement = new OptionSetValue(100000008);//
            OptionSetValue systemUtilities = new OptionSetValue(100000009);//
            OptionSetValue statistics = new OptionSetValue(100000010);//
            OptionSetValue assetManagement = new OptionSetValue(100000011);//
            OptionSetValue stockControl = new OptionSetValue(100000012);//
            OptionSetValue resourceManagement = new OptionSetValue(100000013);//
            OptionSetValue other = new OptionSetValue(100000002);


            switch (serviceRequestStatus)
            {
                case "AM": return assetManagement; //управление на активи
                case "GL": return generalLedger; //главна книга
                case "SC": return stockControl; //управление на запаси
                case "CM": return contractManagement; //управление на договори
                case "RM": return resourceManagement; //управление на ресурси
                case "PL": return purchaseLedger; //книга покупки
                case "SL": return salesLedger; //книга продажби
                case "PO": return purchaseOrder; //поръчка за покупка
                case "SO": return salesOrder; //поръчка за продажба
                case "TS": return serviceManagement; //управление на услуги/сервиз
                case "SU": return systemUtilities; //системни средства 
                case "ST": return statistics; //статистики
                default: return other;
            }

        }

        private OptionSetValue ReturnStatusCode(string serviceRequestStatus)
        {

            OptionSetValue openStatus = new OptionSetValue(100000002);
            OptionSetValue registeredStatus = new OptionSetValue(100000001);
            OptionSetValue reopenedStatus = new OptionSetValue(100000003);
            switch (serviceRequestStatus)
            {
                case "Open": return openStatus;
                case "Registered": return registeredStatus;
                case "Reopened": return reopenedStatus;
                default: return registeredStatus;
            }

        }

        /// <summary>
        /// This connects you to the database using the Domain name, username and password.
        /// </summary>
        /// <returns></returns>
        private CrmConnection ConnectToDatabase()
        {
            try
            {
                var connection = CrmConnection.Parse("Url=http://hv-mscrm-2013:5555/SMConsulta; Domain=SMC; Username=crm2013; Password=$cala230;");
                return connection;
            }
            catch (Exception)
            {

                throw new UnauthorizedAccessException();
            }

        }
        /// <summary>
        /// This method returns the GUID of a incident, by given Name.
        /// </summary>
        /// <param name="caseName">This is the string that holds the Name value of the entity</param>
        /// <param name="service">This is a reference to the service/context of the connection</param>
        /// <returns></returns>
        private Guid GetGUIDByName(string caseName, IOrganizationService service)
        {
            ConditionExpression condition = new ConditionExpression();
            condition.AttributeName = "new_supportrequestid";
            condition.Operator = ConditionOperator.Equal;
            condition.Values.Add(caseName);

            FilterExpression filter = new FilterExpression();
            filter.AddCondition(condition);

            QueryExpression query = new QueryExpression();
            query.EntityName = "incident";
            query.ColumnSet = new ColumnSet(true);
            query.Criteria = filter;

            EntityCollection result = service.RetrieveMultiple(query);

            var accountid = Guid.Empty;
            try
            {
                accountid = result.Entities.FirstOrDefault().Id;
            }
            catch (Exception)
            {

                Console.WriteLine("No item with that SRID was found!");
            }

            return accountid;
        }
        private Guid findCustomer(string customerName, IOrganizationService service)
        {

            ConditionExpression condition = new ConditionExpression();
            condition.AttributeName = "name";
            condition.Operator = ConditionOperator.Contains;
            condition.Values.Add(customerName);

            FilterExpression filter = new FilterExpression();
            filter.AddCondition(condition);

            QueryExpression query = new QueryExpression();
            query.EntityName = "account";
            query.ColumnSet = new ColumnSet(true);
            query.Criteria = filter;


            try
            {
                EntityCollection result = service.RetrieveMultiple(query);
                var accountid = Guid.Empty;

                accountid = result.Entities.FirstOrDefault().Id;
                return accountid;
            }
            catch (Exception)
            {
                Entity newAccount = new Entity("account");

                newAccount["name"] = customerName;

                var newAccID = service.Create(newAccount);
                return newAccID;
            }
        }
        private Guid findConsultantID(string consultantName, IOrganizationService service)
        {
            consultantName = findConsultantName(consultantName, service);
            EventLog.saveMessage("The consultant full name is " + consultantName);

            ConditionExpression condition = new ConditionExpression();
            condition.AttributeName = "fullname";
            condition.Operator = ConditionOperator.Equal;
            condition.Values.Add(consultantName);

            FilterExpression filter = new FilterExpression();
            filter.AddCondition(condition);

            QueryExpression query = new QueryExpression();
            query.EntityName = "systemuser";
            query.ColumnSet = new ColumnSet(true);
            query.Criteria = filter;


            try
            {
                EntityCollection result = service.RetrieveMultiple(query);
                var accountid = Guid.Empty;

                accountid = result.Entities.FirstOrDefault().Id;
                return accountid;
            }
            catch (Exception)
            {
                EventLog.saveMessage("No such user exist in the CRM Database - " + consultantName);
                return new Guid("64FA3858-E5B8-E311-ABB3-00155D0A4E33");
            }
        }
        private string findConsultantName(string consultantName, IOrganizationService service)
        {
            switch (consultantName)
            {
                case "elica": return "Elica Sokolova";
                case "veselina": return "Veselina Haralampieva";
                case "vesselin": return "Veselin Mitrev";
                case "zapryan": return "Zapryan Zapryanov";
                case "venko": return "Venko Popov";
                case "ESaraydarov": return "Emilian Saraydarov";
                case "Nina": return "Nina Milcheva";
                case "george": return "George Moskov";
                case "stoyana": return "Stoyana Daskalova";
                case "svetla": return "Svetla Manolova";
                case "ralica": return "Ralica Andonova";
                case "Adriana": return "Adriana Baeva";
                case "georgi": return "Georgi Borisov";
                case "svetlin": return "Svetlin Nikolov";
                case "irena": return "Irena Kandeva";
                default: return "Nina Milcheva";
            }
        }
        private Guid findCustomerID(string customerName)
        {
            switch (customerName)
            {
                case "Metropol Palace Beograd": return new Guid("B70A249B-8A0D-E411-BD19-00155D0A4E33"); //Metropol Palace Beograd
                case "Абот Продъктс": return new Guid("2F0B249B-8A0D-E411-BD19-00155D0A4E33"); //Abbot Products
                case "Авис Инженеринг": return new Guid("850A249B-8A0D-E411-BD19-00155D0A4E33"); //Avis Engineering
                case "Аграна Трейдинг ЕООД": return new Guid("458185FC-8A0D-E411-BD19-00155D0A4E33"); //Agrana Bulgaria will now be named Agrana Trading
                case "Ай Ен Джи": return new Guid("E50A249B-8A0D-E411-BD19-00155D0A4E33"); //ING
                case "Алукьонигщал": return new Guid("830A249B-8A0D-E411-BD19-00155D0A4E33"); //Alukonigstahl Bulgaria
                case "Алфа Лавал България": return new Guid("9D0A249B-8A0D-E411-BD19-00155D0A4E33"); //Alfa Laval EOOD
                case "Алфред С. Тьопфер": return new Guid("2D0B249B-8A0D-E411-BD19-00155D0A4E33"); //Alfred C.Toepfer International Bulgaria (АДМ България Трейдинг ЕООД)
                case "Атлас Копко": return new Guid("170B249B-8A0D-E411-BD19-00155D0A4E33"); //Atlas Copco
                case "АФИ Юръп": return new Guid("ED0A249B-8A0D-E411-BD19-00155D0A4E33"); //AFI Europe 
                case "Баренц България": return new Guid("230B249B-8A0D-E411-BD19-00155D0A4E33"); //Barentz
                case "Волво": return new Guid("D90A249B-8A0D-E411-BD19-00155D0A4E33"); //Volvo Bulgaria D90A249B-8A0D-E411-BD19-00155D0A4E33
                case "Деви": return new Guid("5F0B249B-8A0D-E411-BD19-00155D0A4E33"); //Devi Bulgaria
                case "ДНСК": return new Guid("1B0B249B-8A0D-E411-BD19-00155D0A4E33"); //DNSK
                case "Дусман": return new Guid("110B249B-8A0D-E411-BD19-00155D0A4E33"); //P.Dussman
                case "Евромедик България": return new Guid("0F0B249B-8A0D-E411-BD19-00155D0A4E33"); //Euromedic
                case "Европарт": return new Guid("430B249B-8A0D-E411-BD19-00155D0A4E33"); //Europart Bulgaria Gmbh
                case "ЕГИС": return new Guid("250B249B-8A0D-E411-BD19-00155D0A4E33"); // Egis Bulgaria
                case "Ей Кю Магнит": return new Guid("7F0A249B-8A0D-E411-BD19-00155D0A4E33"); //AQ Magnit
                case "Емералд": return new Guid("390B249B-8A0D-E411-BD19-00155D0A4E33"); //Emerald Hospitality 
                case "Интерсистемс": return new Guid("490B249B-8A0D-E411-BD19-00155D0A4E33"); //Intersystems
                case "Калиакра": return new Guid("270B249B-8A0D-E411-BD19-00155D0A4E33"); //BUNGE Kaliakra
                case "Кемпински": return new Guid("F10A249B-8A0D-E411-BD19-00155D0A4E33"); // Kempinski Hotel Zogravski
                case "Клиент": return new Guid("F70A249B-8A0D-E411-BD19-00155D0A4E33"); //Atlas Travels EOOD
                case "Лореал": return new Guid("CB0A249B-8A0D-E411-BD19-00155D0A4E33"); //Loreal Bulgaria
                case "Менпауър България": return new Guid("930A249B-8A0D-E411-BD19-00155D0A4E33"); //Manpower
                case "Неостил": return new Guid("AF0A249B-8A0D-E411-BD19-00155D0A4E33"); //Neoset
                case "Нова Броудкастинг Груп (Нова ТВ)": return new Guid("7B0A249B-8A0D-E411-BD19-00155D0A4E33"); //Nova Broadcasting
                case "Ондулин": return new Guid("A90A249B-8A0D-E411-BD19-00155D0A4E33"); //Onduline
                case "Органика": return new Guid("6B0B249B-8A0D-E411-BD19-00155D0A4E33"); //Organica
                case "Оркикем": return new Guid("610B249B-8A0D-E411-BD19-00155D0A4E33"); //Orkikem
                case "Перно Рикар": return new Guid("310B249B-8A0D-E411-BD19-00155D0A4E33"); //Pernod Ricard
                case "Радисън и Парк Инн (Интер Гранд Хотел София и Грийнвил България)": return new Guid("FCB6C3EA-8996-E411-B41E-00155D0A4E33"); //Radisson
                case "Сандвик": return new Guid("B90A249B-8A0D-E411-BD19-00155D0A4E33"); //Sandvik
                case "Селамор": return new Guid("F30A249B-8A0D-E411-BD19-00155D0A4E33"); //FLD/SELAMORE
                case "Синджента България": return new Guid("DD0A249B-8A0D-E411-BD19-00155D0A4E33"); //Syngenta Bulgaria
                case "Скала България": return new Guid("870A249B-8A0D-E411-BD19-00155D0A4E33"); //SM Consulta
                case "СМ Консулта ЕООД": return new Guid("870A249B-8A0D-E411-BD19-00155D0A4E33"); //SM Consulta
                case "Средна Гора": return new Guid("AB0A249B-8A0D-E411-BD19-00155D0A4E33"); //Sredna Gora
                case "ФАЕ София": return new Guid("910A249B-8A0D-E411-BD19-00155D0A4E33"); //VOESTALPINE VAE
                case "Фриготехника": return new Guid("530B249B-8A0D-E411-BD19-00155D0A4E33"); //Frigotechnica
                case "Шератон": return new Guid("210B249B-8A0D-E411-BD19-00155D0A4E33"); //Sofia Hotel Balkan (Sheraton)
                case "ЛЕСТО ПРОДУКТ ЕООД": return new Guid("879FDEA7-B79F-E411-B41E-00155D0A4E33");//ЛЕСТО ПРОДУКТ ЕООД
                default: return new Guid("870A249B-8A0D-E411-BD19-00155D0A4E33"); //SM Con will be the default Account
            }
        }
    }
}
