using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using AjaxControlToolkit;
using ETEMModel.Helpers.Admin;
using ETEMModel.Models.DataView;

using ETEMModel.Models.DataView.Common;
using MoreLinq;

using System.Web.UI.WebControls;


using ETEMModel.Helpers.AbstractSearchBLHolder;
using System.Net.Mail;
using System.Threading;

using System.IO;
using System.Text;
using ETEMModel.Models.DataView.Admin;

namespace ETEMModel.Helpers.Common
{
    public partial class CommonBL
    {
        private ETEMDataModelEntities dbContext;

        public CommonBL()
        {
            dbContext = new ETEMDataModelEntities();
        }

        public List<string> GetCompletionList(string prefixText, int count, string contextKey)
        {
            List<string> selectionList = new List<string>();

            string customCase = string.Empty;
            string tableForSelection = string.Empty;
            string columnIdForSelection = string.Empty;
            string columnNameForSelection = string.Empty;
            string columnNameForOrderBy = string.Empty;
            string additionalWhereParam = string.Empty;

            string[] arrContextParams = contextKey.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (contextKey.Contains("case"))
            {
                customCase = arrContextParams[0].Replace("case=", "");

                arrContextParams = contextKey.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                if (arrContextParams.Where(w => w.Contains("=")).Count() == 1)
                {
                    additionalWhereParam = arrContextParams.Where(w => w.Contains("=")).First();
                }
            }
            if (arrContextParams.Length == 2)
            {
                tableForSelection = arrContextParams[0];
                columnNameForSelection = arrContextParams[1];
            }
            else if (arrContextParams.Length == 3)
            {
                tableForSelection = arrContextParams[0];
                columnNameForSelection = arrContextParams[1];
                if (arrContextParams[2].Split('=', '!', '<', '>').Length > 1)
                {
                    additionalWhereParam = arrContextParams[2];
                }
                else
                {
                    columnIdForSelection = arrContextParams[2];
                }
            }
            else if (arrContextParams.Length == 4)
            {
                tableForSelection = arrContextParams[0];
                columnNameForSelection = arrContextParams[1];
                columnIdForSelection = arrContextParams[2];
                if (arrContextParams[3].Split('=', '!', '<', '>').Length > 1)
                {
                    additionalWhereParam = arrContextParams[3];
                }
                else
                {
                    columnNameForOrderBy = arrContextParams[3];
                }
            }
            else if (arrContextParams.Length == 5)
            {
                tableForSelection = arrContextParams[0];
                columnNameForSelection = arrContextParams[1];
                columnIdForSelection = arrContextParams[2];
                columnNameForOrderBy = arrContextParams[3];
                additionalWhereParam = arrContextParams[4];
            }
            if (!contextKey.Contains("case") && arrContextParams.Length == 0)
            {
                return selectionList;
            }

            if (string.IsNullOrEmpty(columnNameForOrderBy))
            {
                columnNameForOrderBy = columnNameForSelection;
            }

            using (this.dbContext = new ETEMDataModelEntities())
            {
                switch (customCase)
                {
                    

                    

                    

                   

                    case "PersonALLByName":
                        {
                            List<PersonDataView> list = (from p in this.dbContext.Persons
                                                         select new PersonDataView
                                                         {
                                                             idPerson = p.idPerson,
                                                             FirstName = p.FirstName,
                                                             SecondName = p.SecondName,
                                                             LastName = p.LastName
                                                         }).Distinct().ToList<PersonDataView>();

                            list = list.GroupBy(g => new { g.FirstNameTrimed, g.SecondNameTrimed, g.LastNameTrimed }).
                                        Select(s => new PersonDataView
                                                    {
                                                        idPerson = s.First().idPerson,
                                                        FirstName = s.Key.FirstNameTrimed,
                                                        SecondName = s.Key.SecondNameTrimed,
                                                        LastName = s.Key.LastNameTrimed
                                                    }).ToList();

                            string[] arrSearchTerms = prefixText.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

                            string searchItem = string.Join(" ", arrSearchTerms).Trim().ToLower();

                            //HashSet<PersonDataView> listPersons = new HashSet<PersonDataView>();

                            //for (int i = 0; i < arrSearchTerms.Length; i++)
                            //{
                            //    var term = arrSearchTerms[i].Trim().ToLower();

                            //    var listItems = (from p in list
                            //                     where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(term)) ||
                            //                             (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(term)) ||
                            //                             (p.LastName != null && p.LastName.Trim().ToLower().Contains(term)) ||
                            //                             (p.FullNameTwo != null && p.FullNameTwo.Trim().ToLower().Contains(term)) ||
                            //                             (p.FullNameLastTwo != null && p.FullNameLastTwo.Trim().ToLower().Contains(term)) ||
                            //                             (p.FullName != null && p.FullName.Trim().ToLower().Contains(term))
                            //                     select p).ToList();

                            //    listPersons.UnionWith(listItems);
                            //}

                            var listSearchedData = (from p in list
                                                    where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.LastName != null && p.LastName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameTwo != null && p.FullNameTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameLastTwo != null && p.FullNameLastTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullName != null && p.FullName.Trim().ToLower().Contains(searchItem))
                                                    select new
                                                    {
                                                        idPerson = p.idPerson,
                                                        FullText = p.FullName
                                                    }).OrderBy(o => o.FullText);

                            foreach (var completionResult in listSearchedData)
                            {
                                selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.FullText, completionResult.idPerson.ToString()));
                            }
                        }
                        break;

                    case "PersonALLByEGN":
                        {
                            var list = (from p in this.dbContext.Persons
                                        select new PersonDataView
                                        {
                                            idPerson = p.idPerson,
                                            FirstName = p.FirstName,
                                            SecondName = p.SecondName,
                                            LastName = p.LastName,
                                            EGN = p.EGN,
                                            IdentityNumber = p.IdentityNumber
                                        }).Distinct().ToList<PersonDataView>();

                            var listSearchedData = (from p in list
                                                    where (p.EGN != null && p.EGN.Contains(prefixText)) ||
                                                          (p.IdentityNumber != null && p.IdentityNumber.Contains(prefixText))
                                                    select new
                                                    {
                                                        idPerson = p.idPerson,
                                                        FullText = p.FullName
                                                    }).OrderBy(o => o.FullText);

                            foreach (var completionResult in listSearchedData)
                            {
                                selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.FullText, completionResult.idPerson.ToString()));
                            }
                        }
                        break;

                   

                  

                   

                   

                   

                

                   

                    

                   

                    case "NSICode":
                        {
                            List<KeyValue> allNSICode = dbContext.KeyValues.Where(k => k.KeyType.KeyTypeIntCode == "NSICode").ToList();

                            var listSearchedData = (from k in allNSICode
                                                    where k.Name.ToLower().Contains(prefixText.ToLower()) || k.KeyValueIntCode.ToLower().Contains(prefixText.ToLower())
                                                    select new
                                                    {
                                                        idKeyValue = k.idKeyValue,
                                                        FullText = k.KeyValueIntCode.Trim() + " | " + k.Name.Trim()
                                                    }).OrderBy(o => o.FullText);

                            foreach (var completionResult in listSearchedData)
                            {
                                selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.FullText, completionResult.idKeyValue.ToString()));
                            }
                        }
                        break;

                   

                    case "EmployeePerson":
                        {
                            List<PersonDataView> personList = new List<PersonDataView>();

                            personList = (from p in this.dbContext.Persons
                                          join u in this.dbContext.Users on p.idPerson equals u.idPerson
                                          select new PersonDataView
                                          {
                                              idPerson = p.idPerson,
                                              FirstName = p.FirstName,
                                              SecondName = p.SecondName,
                                              LastName = p.LastName,
                                              Title = p.Title
                                          }
                                          ).Distinct().ToList();

                            string[] arrSearchTerms = prefixText.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

                            string searchItem = string.Join(" ", arrSearchTerms).Trim().ToLower();

                            //HashSet<ArtModelDataView> listArtModels = new HashSet<ArtModelDataView>();

                            //for (int i = 0; i < arrSearchTerms.Length; i++)
                            //{
                            //    var term = arrSearchTerms[i].Trim().ToLower();

                            //    var listItems = (from p in allArtModel
                            //                     where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(term)) ||
                            //                             (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(term)) ||
                            //                             (p.LastName != null && p.LastName.Trim().ToLower().Contains(term)) ||
                            //                             (p.FullName != null && p.FullName.Trim().ToLower().Contains(term))
                            //                     select p).ToList();

                            //    listArtModels.UnionWith(listItems);
                            //}

                            var listSearchedData = (from p in personList
                                                    where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.LastName != null && p.LastName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameTwo != null && p.FullNameTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameLastTwo != null && p.FullNameLastTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullName != null && p.FullName.Trim().ToLower().Contains(searchItem))
                                                    select p).OrderBy(o => o.FullName).ToList();

                            foreach (var completionResult in listSearchedData)
                            {
                                selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.TitlePlusFullName, completionResult.idPerson.ToString()));
                            }
                        }
                        break;

                    case "PersonEmployee":
                        {
                            List<PersonDataView> personList = new List<PersonDataView>();

                            personList = (from p in this.dbContext.Persons
                                          join e in this.dbContext.Employees on p.idPerson equals e.idPerson
                                          select new PersonDataView
                                          {
                                              idPerson = p.idPerson,
                                              FirstName = p.FirstName,
                                              SecondName = p.SecondName,
                                              LastName = p.LastName,
                                              Title = p.Title
                                          }
                                          ).Distinct().ToList();

                            string[] arrSearchTerms = prefixText.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

                            string searchItem = string.Join(" ", arrSearchTerms).Trim().ToLower();

                            //HashSet<ArtModelDataView> listArtModels = new HashSet<ArtModelDataView>();

                            //for (int i = 0; i < arrSearchTerms.Length; i++)
                            //{
                            //    var term = arrSearchTerms[i].Trim().ToLower();

                            //    var listItems = (from p in allArtModel
                            //                     where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(term)) ||
                            //                             (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(term)) ||
                            //                             (p.LastName != null && p.LastName.Trim().ToLower().Contains(term)) ||
                            //                             (p.FullName != null && p.FullName.Trim().ToLower().Contains(term))
                            //                     select p).ToList();

                            //    listArtModels.UnionWith(listItems);
                            //}

                            var listSearchedData = (from p in personList
                                                    where (p.FirstName != null && p.FirstName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.SecondName != null && p.SecondName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.LastName != null && p.LastName.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameTwo != null && p.FullNameTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullNameLastTwo != null && p.FullNameLastTwo.Trim().ToLower().Contains(searchItem)) ||
                                                         (p.FullName != null && p.FullName.Trim().ToLower().Contains(searchItem))
                                                    select p).OrderBy(o => o.FullName).ToList();

                            foreach (var completionResult in listSearchedData)
                            {
                                selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.TitlePlusFullName, completionResult.idPerson.ToString()));
                            }
                        }
                        break;

                   

                    default:
                        {
                            string selectStmt = string.Empty;

                            selectStmt = "SELECT TOP " + count + (string.IsNullOrEmpty(columnIdForSelection) ? " [" : " [" + columnIdForSelection + "] AS Id, [") + columnNameForSelection + "] AS Name " +
                                         "FROM [" + tableForSelection + "] " +
                                         "WHERE LOWER([" + columnNameForOrderBy + "]) LIKE '" + BaseHelper.MySqlEscapeString(prefixText.ToLower().Trim()) + "%' " +
                                         (string.IsNullOrEmpty(additionalWhereParam) ? "" : "AND " + additionalWhereParam + " ") +
                                         "ORDER BY [" + columnNameForOrderBy + "] ASC";

                            if (string.IsNullOrEmpty(columnIdForSelection))
                            {
                                var result = this.dbContext.ExecuteStoreQuery<string>(selectStmt);

                                selectionList.AddRange(result.ToList());
                            }
                            else
                            {
                                var result = this.dbContext.ExecuteStoreQuery<CompletionResult>(selectStmt);

                                foreach (CompletionResult completionResult in result)
                                {
                                    selectionList.Add(AutoCompleteExtender.CreateAutoCompleteItem(completionResult.Name, completionResult.Id.ToString()));
                                }
                            }
                        }
                        break;


                }
            }

            return selectionList;
        }

        


       

        public List<SortDirectionClass> GetSortDirections()
        {
            List<SortDirectionClass> result = new List<SortDirectionClass>()
            {
                new SortDirectionClass()
                {
                    SortDirectionName = BaseHelper.GetCaptionString("SortDirectionAsc"),
                    SortDirectionCode = Constants.SORTING_ASC
                },
                new SortDirectionClass()
                {
                    SortDirectionName = BaseHelper.GetCaptionString("SortDirectionDesc"),
                    SortDirectionCode = Constants.SORTING_DESC
                }
            };

            return result;
        }

        /// <summary>
        /// Изпраща E-Mail на всеки от потребителите в списъка
        /// </summary>
        /// <param name="usersList"></param>
        /// <param name="resource"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public CallContext SendMail(List<SendMailHelper> listSendMailHelper, ETEMEnums.EmailTypeEnum emailType, CallContext resultContext)
        {
            if (resultContext.ListKvParams.Where(w => w.Key == "SendExternalMail").Count() != 1)
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                resultContext.Message = BaseHelper.GetCaptionString("EMail_External_Send_Not_Permitted");
                return resultContext;
            }
            else if (!Convert.ToBoolean(resultContext.ListKvParams.Where(w => w.Key == "SendExternalMail").First().Value))
            {
                resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                resultContext.Message = BaseHelper.GetCaptionString("EMail_External_Send_Not_Permitted");
                return resultContext;
            }

            List<string> listPersonsWithWrongEmails = new List<string>();

            string mailFrom = string.Empty;
            string mailTo = string.Empty;

            if (resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").Count() == 1)
            {
                mailFrom = resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").First().Value.ToString();
                mailTo = resultContext.ListKvParams.Where(w => w.Key == "DefaultEmail").First().Value.ToString();
            }

            switch (emailType)
            {
                case ETEMEnums.EmailTypeEnum.StudentCandidatesRanked:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.PotentialStudentCandidates:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.Students:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.StudentCandidates:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.Lecturers:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.GroupStudentsLecturersEmployeesPhds:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.Users:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

                case ETEMEnums.EmailTypeEnum.PhD:
                    {
                        foreach (SendMailHelper sendMailHelper in listSendMailHelper)
                        {
                            mailTo = sendMailHelper.EmailTo;

                            if (!string.IsNullOrEmpty(mailTo))
                            {
                                SendMailAction(mailFrom, mailTo,
                                               sendMailHelper.SubjectBG,
                                               sendMailHelper.BodyBG,
                                               sendMailHelper.FullName,
                                               listPersonsWithWrongEmails,
                                               resultContext);
                            }
                        }

                        if (listPersonsWithWrongEmails.Count > 0)
                        {
                            string personNames = string.Join(", ", listPersonsWithWrongEmails.OrderBy(o => o).ToArray());
                            resultContext.Message = string.Format(BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Error"),
                                                                  personNames);
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Warning;
                        }
                        else
                        {
                            resultContext.Message = BaseHelper.GetCaptionString("Form_Send_Email_To_Persons_Success");
                            resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                        }
                    }
                    break;

            }

            CallContext resContext = new CallContext();

            resContext.ListKvParams = resultContext.ListKvParams;

            CheckSentEmails(resContext);

            return resultContext;
        }

        public bool SendMailAction(string mailFrom, string mailTo, string subject, string body, string personName,
                                   List<string> listPersonsWithWrongEmails, CallContext resultContext, bool? isErrorMessage = null)
        {
            bool result = false;



            if (resultContext.ListKvParams.Where(w => w.Key == "SendExternalMail").Count() == 1)
            {
                string sendExternalMail = resultContext.ListKvParams.Where(w => w.Key == "SendExternalMail").First().Value.ToString();

                if (sendExternalMail != "true")
                {
                    return false;
                }
            }
            else
            {
                Setting sendExternalMail = new SettingBL().GetSettingByCode("SendExternalMail");

                if (sendExternalMail == null)
                {
                    return false;
                }

                if (sendExternalMail != null && sendExternalMail.SettingValue != "true")
                {
                    return false;
                }
            }


            try
            {
                MailMessage messageForSupport = new MailMessage(mailFrom, mailTo, subject, body);
                messageForSupport.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                string mailServer = string.Empty;
                int mailServerPort = 0;
                string password = string.Empty;

                if (resultContext.ListKvParams.Where(w => w.Key == "MailServer").Count() == 1)
                {
                    mailServer = resultContext.ListKvParams.Where(w => w.Key == "MailServer").First().Value.ToString();
                }
                if (resultContext.ListKvParams.Where(w => w.Key == "MailServerPort").Count() == 1)
                {
                    mailServerPort = Int32.Parse(resultContext.ListKvParams.Where(w => w.Key == "MailServerPort").First().Value.ToString());
                }

                //mean that the message is for error that has occured in the system, and them the email is being send from another mail respectivly another email
                if (isErrorMessage == true)
                {
                    if (resultContext.ListKvParams.Where(w => w.Key == "MailFromPasswordNew").Count() == 1)
                    {
                        password = resultContext.ListKvParams.Where(w => w.Key == "MailFromPasswordNew").First().Value.ToString();
                    }
                }
                else
                {
                    if (resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").Count() == 1)
                    {
                        password = resultContext.ListKvParams.Where(w => w.Key == "MailFromPassword").First().Value.ToString();
                    }

                }


                SmtpClient clientSupport = new SmtpClient(mailServer, mailServerPort);
                clientSupport.UseDefaultCredentials = false;
                clientSupport.Credentials = new System.Net.NetworkCredential(mailFrom, password);
                clientSupport.EnableSsl = true;
                clientSupport.Timeout = 120000;
                clientSupport.Send(messageForSupport);

                result = true;
            }
            catch (Exception ex)
            {
                listPersonsWithWrongEmails.Add(personName);

                string errorMessage = "Грешка при изпращане на E-Mail To: " + personName;
                BaseHelper.Log(errorMessage);
                BaseHelper.Log(ex.Message);
                BaseHelper.Log(ex.StackTrace);
            }

            return result;
        }

        public void CheckSentEmails(CallContext resultContext)
        {
            ThreadCheckForWrongEmails threadCheckForWrongEmails = new ThreadCheckForWrongEmails(resultContext, DateTime.Now);

            Thread checkThread = new Thread(new ThreadStart(threadCheckForWrongEmails.StartCheck));
            checkThread.Name = "ThreadCheckForWrongEmails";
            checkThread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            checkThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            checkThread.IsBackground = true;
            checkThread.Start();
        }

        public CallContext SendMailToAdministrator(SendMailHelper sendMailHelper)
        {
            CallContext callContext = new CallContext();



            SetDetaultKvParams(callContext);


            SendMailAction(GetSettingByCode(ETEMEnums.AppSettings.EmailForSending).SettingValue, GetSettingByCode(ETEMEnums.AppSettings.EmailForReciveError).SettingValue,
                sendMailHelper.SubjectBG, sendMailHelper.BodyBG, "Системен e-mail", new List<string>(), callContext, true);
            //
            return callContext;
        }

        public void SetDetaultKvParams(CallContext callContext)
        {
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.SendExternalMail.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.SendExternalMail.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.SendExternalMail).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.DefaultEmail.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.DefaultEmail.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.DefaultEmail).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServer.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServer.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailServer).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPort.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPort.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailServerPort).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailFromPassword.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailFromPassword.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailFromPassword).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPop3.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPop3.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailServerPop3).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailServerPop3Port.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailServerPop3Port.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailServerPop3Port).SettingValue));
            }
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.WaitCheckMailDeliveryInMinutes).SettingValue));
            }

            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.DefaultEmail.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.DefaultEmail.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.DefaultEmail).SettingValue));
            }

            //the password for the mails from which we are sending the erros messages
            if (callContext.ListKvParams.Where(w => w.Key == ETEMEnums.AppSettings.MailFromPasswordNew.ToString()).Count() == 0)
            {
                callContext.ListKvParams.Add(new KeyValuePair<string, object>(ETEMEnums.AppSettings.MailFromPasswordNew.ToString(),
                                                  GetSettingByCode(ETEMEnums.AppSettings.MailFromPasswordNew).SettingValue));
            }
        }


        public Setting GetSettingByCode(ETEMEnums.AppSettings appSettings)
        {
            string settingName = appSettings.ToString();
            Setting setting = this.dbContext.Settings.Where(k => k.SettingIntCode == settingName).FirstOrDefault();


            return setting;
        }









        //private CallContext DeleteOldScholarshipExport(int year)
        //{
        //    List<UniStudentGrant> listGrants = (from p in dbContext.UniStudentGrants where p.CalendarYear == year
        //                                        select p).ToList();
        //   CallContext callcontext = new CallContext();
        //   callcontext = new UniStudentGrantBL().EntityDelete<UniStudentGrant>(listGrants, callcontext);
        //   return callcontext;
        //}


        //private List<UniStudentGrantDataView> SaveNewScholarshipExport(List<UniStudentGrantDataView> listGrantsForAcademicPeriod, CallContext callContext)
        //{
        //    var groupsByProfGroup = listGrantsForAcademicPeriod.GroupBy(s => s.ProfGroupId);
        //    List<UniStudentGrant> listGrants = new List<UniStudentGrant>();
        //    List<UniStudentGrantDataView> listGrantsDataView = new List<UniStudentGrantDataView>();
        //    for (int i = 0; i < listGrants.Count; i++)
        //    {
        //        List<UniStudentGrantDataView> currentGroup = groupsByProfGroup.ElementAt(i).Select(s => s).ToList();

        //        UniStudentGrant grant = new UniStudentGrant()
        //        {
        //          CalendarYear= currentGroup.First().CalendarYear,
        //           idCodeProfGroup = currentGroup.First().idCodeProfGroup,
        //           idAcademicPeriod = currentGroup.First().idAcademicPeriod,
        //        };

        //        grant.TotalGrantsValue = currentGroup.Sum(s => s.TotalGrantsValue);


        //        UniStudentGrantDataView grantView = new UniStudentGrantDataView()
        //        {
        //            CalendarYear = currentGroup.First().CalendarYear,
        //            idCodeProfGroup = currentGroup.First().idCodeProfGroup,
        //        };

        //        grantView.TotalGrantsValue = currentGroup.Sum(s => s.TotalGrantsValue);

        //    }

        //    callContext = new UniStudentGrantBL().EntitySave<UniStudentGrant>(listGrants, callContext);


        //    return listGrantsDataView;
        //}




    }

    class CompletionResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}