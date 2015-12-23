using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.Admin;

namespace ETEMModel.Helpers.Admin
{
    public class GroupBL : BaseClassBL<Group>
    {

        public GroupBL()
        {
            this.EntitySetName = "Groups";
        }

        internal override void EntityToEntity(Group sourceEntity, Group targetEntity)
        {

            targetEntity.GroupName = sourceEntity.GroupName;
            targetEntity.SharedAccess = sourceEntity.SharedAccess;
        }

        internal override Group GetEntityById(int idEntity)
        {
            return this.dbContext.Groups.Where(e => e.idGroup == idEntity).FirstOrDefault();
        }

        internal List<GroupDataView> GetGroupDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {

            List<GroupDataView> listView = (from g in dbContext.Groups
                                            //GroupPersonLinks
                                            join gpl in this.dbContext.GroupPersonLinks on g.idGroup equals gpl.idGroup into gj
                                            from subpet in gj.DefaultIfEmpty()
                                            //Person
                                            join p in dbContext.Persons on (subpet != null ? subpet.idPerson : Constants.INVALID_ID) equals p.idPerson into groupPerson
                                            from grPerson in groupPerson.DefaultIfEmpty()

                                            select new GroupDataView
                                                {
                                                    idGroup = g.idGroup,
                                                    GroupName = g.GroupName,
                                                    SharedAccess = g.SharedAccess,
                                                    idPerson = grPerson.idPerson
                                                }
                                            ).ApplySearchCriterias(searchCriteria).Distinct().ToList();

            listView = BaseClassBL<GroupDataView>.Sort(listView, sortExpression, sortDirection).ToList();
            return listView;
        }



    }
}