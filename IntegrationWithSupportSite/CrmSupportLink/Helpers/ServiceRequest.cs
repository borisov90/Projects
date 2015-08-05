using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMServices.Helpers
{
    public class ServiceRequest 
    {

                
        public ServiceRequest()
        {
        }
        public string Person
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }

        public string CodeProject
        {
            get;
            set;
        }

        public string AscentCodeProject
        {
            get
            {
                if (ServiceRequestKind.Equals("Implementation"))
                {
                    return CodeProject + "_I";
                }
                else if (ServiceRequestKind.Equals("Support"))
                {
                    return CodeProject + "_S";
                }
                else
                {
                    return CodeProject;
                }
            }
        }

        public string ServiceRequestStatus
        {
            get;
            set;
        }
        public string ServiceRequestKind
        {
            get;
            set;
        }
        /// <summary>
        /// Статус на искането за обслужване
        /// </summary>
        public int AscentClass
        {
            get
            {
                int ascentClass = 0;
                switch (ServiceRequestStatus)
                {
                    case "Open":
                        ascentClass = 2;
                        break;
                    case "Closed":
                        ascentClass = 5;
                        break;
                    case "Reopened":
                        ascentClass = 6;
                        break;
                    case "Completed":
                        ascentClass = 7;
                        break;
                }
                return ascentClass;
            }
        }

        public string ServiceRequestType
        {
            get;
            set;
        }

        public int Area
        {
            get {

                if (ServiceRequestType.Equals("FuncQuestion"))
                {
                    return 1;
                }
                ///// 
                else
                {
                    return 1;
                }
            }
        }

        public string SRID
        {
            get;
            set;
        }

        public string companyName
        {
            get;
            set;
        }


        /// <summary>
        /// Дата на регистрация- date_reported
        /// </summary>
        public DateTime RegistrationDate
        {
            get;
            set;
        }

        public DateTime CompleteDate
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }
        public int IdModule
        {
            get;
            set;
        }
        
        public string ShortDescription
        {
            get;
            set;
        }
        /// <summary>
        /// Описание на проблема
        /// </summary>
        public string LongDescription
        {
            get;
            set;
        }

        public string CommentAfterCompleted
        {
            get;
            set;
        }
        public string AssignedPerson
        {
            get;
            set;
        }
        /// <summary>
        /// UserName from support site user who make assigning
        /// </summary>
        public string CreatedBy
        {
            get;
            set;
        }
        /// <summary>
        /// UserName from support site AssigningTo
        /// </summary>
        public string Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Срок за изпълнение
        /// </summary>
        public DateTime DueDate
        {
            get;
            set;
        }
        /// <summary>
        /// Лице направило искането
        /// </summary>
        public string AssignedToClient
        {
            get;
            set;
        }


        /// <summary>
        /// Описание към задачата
        /// </summary>
        public string CommentsMatricia
        {
            get;
            set;
        }
    }
}
