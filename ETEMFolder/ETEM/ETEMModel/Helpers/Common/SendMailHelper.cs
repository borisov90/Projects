using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Common
{
    public class SendMailHelper
    {
        private CallContext resultContext;
        private Person mailSystemPerson;
        private string subjectBG;
        private string subjectEN;
        private string bodyBG;
        private string bodyEN;

        public SendMailHelper()
        {

        }

        public SendMailHelper(CallContext _ResultContext)
        {
            this.resultContext = _ResultContext;
        }

        public CallContext ResultContext
        {
            get { return resultContext; }
            set { resultContext = value; }
        }

        public Person MailSystemPerson
        {
            get { return mailSystemPerson; }
            set { mailSystemPerson = value; }
        }

        public string FullName { get; set; }

        public string EmailTo { get; set; }

        public string SubjectBG
        {
            get { return subjectBG; }
            set { subjectBG = value; }
        }

        public string SubjectEN
        {
            get { return subjectEN; }
            set { subjectEN = value; }
        }

        public string BodyBG
        {
            get { return bodyBG; }
            set { bodyBG = value; }
        }

        public string BodyEN
        {
            get { return bodyEN; }
            set { bodyEN = value; }
        }

        public string LocalizedSubject
        {
            get
            {
                if (this.ResultContext != null)
                {
                    if (this.ResultContext.CurrentConsumerLang.Equals("bg"))
                    {
                        return this.SubjectBG;
                    }
                    else
                    {
                        return this.SubjectEN;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string LocalizedBody
        {
            get
            {
                if (this.ResultContext != null)
                {
                    if (this.ResultContext.CurrentConsumerLang.Equals("bg"))
                    {
                        return this.BodyBG;
                    }
                    else
                    {
                        return this.BodyEN;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string EMailToFromPerson
        {
            get
            {
                if (this.MailSystemPerson != null)
                {
                    return this.MailSystemPerson.EMail;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}