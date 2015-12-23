using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using OpenPop.Pop3;
using System.Net.Mail;
using OpenPop.Mime;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Common
{
    public class ThreadCheckForWrongEmails
    {
        private CallContext resultContext;
        private DateTime dtStartDateSend;

        public ThreadCheckForWrongEmails(CallContext _ResultContext, DateTime _StartDateSend)
        {
            this.resultContext = _ResultContext;
            this.dtStartDateSend = _StartDateSend;
        }

        public void StartCheck()
        {
            if (Thread.CurrentThread.Name == "ThreadCheckForWrongEmails")
            {
                int waitTimeInMinutes = 2;
                if (resultContext.ListKvParams.Where(w => w.Key == "WaitCheckMailDeliveryInMinutes").Count() == 1)
                {
                    waitTimeInMinutes = Int32.Parse(resultContext.ListKvParams.Where(w => w.Key == "WaitCheckMailDeliveryInMinutes").First().Value.ToString());
                }

                Thread.Sleep((waitTimeInMinutes * 60 * 1000));
            }

            Pop3Client pop3Client = null;

            try
            {
                pop3Client = new Pop3Client();

                string mailServerPop3 = string.Empty;
                int mailServerPop3Port = 0;
                string mailFromPassword = string.Empty;
                string mailDeliverySubsystemEmail = string.Empty;

                string mailFrom = string.Empty;
                string mailTo = string.Empty;

                if (resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").Count() == 1)
                {
                    mailFrom = resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").First().Value.ToString();
                    mailTo = resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").First().Value.ToString();
                }
                if (resultContext.ListKvParams.Where(w => w.Key == "MailServerPop3").Count() == 1)
                {
                    mailServerPop3 = resultContext.ListKvParams.Where(w => w.Key == "MailServerPop3").First().Value.ToString();
                }
                if (resultContext.ListKvParams.Where(w => w.Key == "MailServerPop3Port").Count() == 1)
                {
                    mailServerPop3Port = Int32.Parse(resultContext.ListKvParams.Where(w => w.Key == "MailServerPop3Port").First().Value.ToString());
                }
                if (resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").Count() == 1)
                {
                    mailFromPassword = resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").First().Value.ToString();
                }
                if (resultContext.ListKvParams.Where(w => w.Key == "MailDeliverySubsystemEmail").Count() == 1)
                {
                    mailDeliverySubsystemEmail = resultContext.ListKvParams.Where(w => w.Key == "MailDeliverySubsystemEmail").First().Value.ToString();
                }

                List<string> listWrongEmails = new List<string>();

                DateTime dtCurrDateSent = DateTime.Now;

                pop3Client.Connect(mailServerPop3, mailServerPop3Port, true);
                pop3Client.Authenticate(mailFrom, mailFromPassword);

                Message message = null;
                int countMessages = pop3Client.GetMessageCount();
                for (int i = 1; i <= countMessages; i++)
                {
                    message = pop3Client.GetMessage(i);
                    if (message != null && message.Headers != null && message.Headers.DateSent != null &&
                        message.Headers.From.Address == mailDeliverySubsystemEmail)
                    {
                        dtCurrDateSent = message.Headers.DateSent;
                        if (message.Headers.DateSent.Kind == DateTimeKind.Utc)
                        {
                            dtCurrDateSent = message.Headers.DateSent.ToLocalTime();
                        }

                        if (this.dtStartDateSend <= dtCurrDateSent && dtCurrDateSent <= DateTime.Now)
                        {
                            if (message.MessagePart != null && message.MessagePart.GetBodyAsText().Split('\r', '\n').Where(w => w.Contains("@")).Count() > 0)
                            {
                                string wrongEmail = message.MessagePart.GetBodyAsText().Split('\r', '\n').Where(w => w.Contains("@")).First().Trim();

                                listWrongEmails.Add(wrongEmail);
                            }
                        }
                    }
                }

                List<Person> listPersonsWithWrongEmails = new List<Person>();

                ETEMDataModelEntities dbContext = new ETEMDataModelEntities();

                listPersonsWithWrongEmails = (from p in dbContext.Persons
                                              where listWrongEmails.Contains(p.EMail)
                                              orderby p.FirstName ascending, p.SecondName ascending, p.LastName ascending
                                              select p).ToList<Person>();

                if (listPersonsWithWrongEmails.Count > 0)
                {
                    string subject = (from kv in dbContext.KeyValues
                                      join kt in dbContext.KeyTypes on kv.idKeyValue equals kt.idKeyType
                                      where kt.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.EmailSubject.ToString() &&
                                            kv.KeyValueIntCode == ETEMEnums.EmailSubjectEnum.WrongSentEmails.ToString()
                                      select kv.Description).FirstOrDefault();

                    string body = (from kv in dbContext.KeyValues
                                   join kt in dbContext.KeyTypes on kv.idKeyValue equals kt.idKeyType
                                   where kt.KeyTypeIntCode == ETEMEnums.KeyTypeEnum.EmailSubject.ToString() &&
                                         kv.KeyValueIntCode == ETEMEnums.EmailBodyEnum.WrongSentEmails.ToString()
                                   select kv.Description).FirstOrDefault();

                    if (!string.IsNullOrEmpty(subject) && !string.IsNullOrEmpty(body))
                    {
                        string bodyInnerText = string.Empty;
                        foreach (Models.Person person in listPersonsWithWrongEmails)
                        {
                            if (string.IsNullOrEmpty(bodyInnerText))
                            {
                                bodyInnerText += BaseHelper.GetCaptionString("Email_WrongSentEmail_Email") + " " + person.EMail + "\n";
                                bodyInnerText += BaseHelper.GetCaptionString("Email_WrongSentEmail_PersonName") + " " + person.FullName;
                            }
                            else
                            {
                                bodyInnerText += "\n" + BaseHelper.GetNumberOfCharAsString('-', 100) + "\n";

                                bodyInnerText += BaseHelper.GetCaptionString("Email_WrongSentEmail_Email") + " " + person.EMail + "\n";
                                bodyInnerText += BaseHelper.GetCaptionString("Email_WrongSentEmail_PersonName") + " " + person.FullName;
                            }
                        }

                        body = string.Format(body, bodyInnerText);

                        SendMailAction(mailFrom, mailTo, subject, body);
                    }
                }
            }
            catch (Exception ex)
            {
                BaseHelper.Log("Грешка при проверка за неуспешно изпратени имейли - (ThreadCheckForWrongEmails.StartCheck)!");
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }
            finally
            {
                if (pop3Client != null)
                {
                    if (pop3Client.Connected)
                    {
                        pop3Client.Disconnect();
                    }
                    pop3Client.Dispose();
                }
            }
        }

        public void SendMailAction(string mailFrom, string mailTo, string subject, string body)
        {
            MailMessage messageForSupport = new MailMessage(mailFrom, mailTo, subject, body);
            messageForSupport.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            string mailServer = string.Empty;
            int mailServerPort = 0;
            string password = string.Empty;

            if (this.resultContext.ListKvParams.Where(w => w.Key == "MailServer").Count() == 1)
            {
                mailServer = this.resultContext.ListKvParams.Where(w => w.Key == "MailServer").First().Value.ToString();
            }
            if (this.resultContext.ListKvParams.Where(w => w.Key == "MailServerPort").Count() == 1)
            {
                mailServerPort = Int32.Parse(this.resultContext.ListKvParams.Where(w => w.Key == "MailServerPort").First().Value.ToString());
            }
            if (this.resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").Count() == 1)
            {
                password = this.resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").First().Value.ToString();
            }

            SmtpClient clientSupport = new SmtpClient(mailServer, mailServerPort);
            clientSupport.UseDefaultCredentials = true;
            clientSupport.Credentials = new System.Net.NetworkCredential(mailFrom, password);
            clientSupport.EnableSsl = true;
            clientSupport.Timeout = 120000;
            clientSupport.Send(messageForSupport);
        }
    }
}