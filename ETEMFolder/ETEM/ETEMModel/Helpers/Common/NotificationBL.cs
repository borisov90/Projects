using System.Collections.Generic;
using System.Linq;
using ETEMModel.Models;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Admin;

using System;

using MoreLinq;
namespace ETEMModel.Helpers.Common
{
    public class NotificationBL : BaseClassBL<Notification>
    {
        public NotificationBL()
        {
            this.EntitySetName = "Notifications";
        }

        internal override void EntityToEntity(Notification entity, Notification saveEntity)
        {
            saveEntity.About = entity.About;
            saveEntity.Comment = entity.Comment;
            saveEntity.idSendFrom = entity.idSendFrom;
            saveEntity.idSendTo = entity.idSendTo;
            saveEntity.idStatus = entity.idStatus;
            saveEntity.LastUpdate = entity.LastUpdate;
            saveEntity.isReading = entity.isReading;
            saveEntity.ParentID = entity.ParentID;
            saveEntity.idGroup = entity.idGroup;
        }

        internal override Notification GetEntityById(int idEntity)
        {
            return this.dbContext.Notifications.Where(e => e.idNotification == idEntity).FirstOrDefault();
        }


        internal List<NotificationDataView> GetNotificationDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {

            List<NotificationDataView> listView = (from n in dbContext.Notifications
                                                   join pf in dbContext.Persons on n.idSendFrom equals pf.idPerson
                                                   join pt in dbContext.Persons on n.idSendTo equals pt.idPerson into personGroup
                                                   from subPerson in personGroup.DefaultIfEmpty()
                                                   join kv in dbContext.KeyValues on n.idStatus equals kv.idKeyValue
                                                   join kt in dbContext.KeyTypes on kv.idKeyType equals kt.idKeyType

                                                   select new NotificationDataView
                                                   {
                                                       idNotification = n.idNotification,
                                                       idSendFrom = n.idSendFrom,
                                                       idSendTo = n.idSendTo,
                                                       About = n.About,
                                                       SendDate = n.SendDate,
                                                       LastUpdate = n.LastUpdate,
                                                       FirstNameFrom = pf.FirstName,
                                                       LastNameFrom = pf.LastName,
                                                       FirstNameTo = subPerson.FirstName,
                                                       LastNameTo = subPerson.LastName,
                                                       Comment = n.Comment,
                                                       idStatus = n.idStatus,
                                                       StatusName = kv.Name,
                                                       idPerson = subPerson.idPerson,
                                                       isReading = n.isReading,
                                                       ParentID = n.ParentID,
                                                       idGroup = n.idGroup
                                                   }
                                              ).ApplySearchCriterias(searchCriteria).OrderByDescending(o => o.SendDate).ToList();

            listView = BaseClassBL<NotificationDataView>.Sort(listView, sortExpression, sortDirection).ToList();
            return listView;
        }


        internal int GetMaxNotificationID()
        {
            int? maxID = this.dbContext.Notifications.MaxBy(z => z.idNotification).idNotification;

            if (maxID == null)
            {
                return 1;
            }
            else
            {
                return maxID.Value + 1;
            }
        }

        internal int GetNotificationCountByPersonID(int idPerson)
        {
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

            searchCriteria.Add(
                    new NumericSearch
                    {
                        Comparator = NumericComparators.Equal,
                        Property = "idStatus",
                        SearchTerm = int.Parse(new KeyValueBL().GetKeyValueIdByIntCode("NotificationStatus", "Submitted").ToString())
                    });
            searchCriteria.Add(
                new NumericSearch
                {
                    Comparator = NumericComparators.Equal,
                    Property = "idPerson",
                    SearchTerm = idPerson
                });

            //само непрочетени писма
            searchCriteria.Add(
            new BooleanSearch
            {
                Comparator = BooleanComparators.Equal,
                Property = "isReading",
                SearchTerm = false
            });

            List<NotificationDataView> countNotification = GetNotificationDataView(searchCriteria, "", "");

            return countNotification.Count;
        }



       
    }
}