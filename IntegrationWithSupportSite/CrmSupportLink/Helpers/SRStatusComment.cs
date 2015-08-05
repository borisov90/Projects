using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMServices.Helpers
{
    public class SRStatusComment
    {
        /// <summary>
        /// Коментар свързан със смяната на статуса
        /// </summary>
        public string CommentAfterCompleted
        {
            get;
            set;
        }
        /// <summary>
        /// Статус от support сайта
        /// </summary>
        public string ServiceRequestStatus
        {
            get;
            set;
        }

        public string SRID
        {
            get;
            set;
        }
        /// <summary>
        /// Приключено от потребителя сменил стасуса
        /// </summary>
        public string CreatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Статус на искането за обслужване в Ascent
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

        


    }
}
