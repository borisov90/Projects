using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView.Admin;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class GroupPersonLinkBL : BaseClassBL<GroupPersonLink>
    {

        public GroupPersonLinkBL()
        {
            this.EntitySetName = "GroupPersonLinks";
        }

        internal override void EntityToEntity(GroupPersonLink sourceEntity, GroupPersonLink targetEntity)
        {
            targetEntity.idGroup = sourceEntity.idGroup;
            targetEntity.idPerson = sourceEntity.idPerson;
        }

        internal override GroupPersonLink GetEntityById(int idEntity)
        {
            return this.dbContext.GroupPersonLinks.Where(e => e.idGroupPersonLink == idEntity).FirstOrDefault();
        }

        internal CallContext GroupPersonLinkDelete(List<GroupPersonLink> list, CallContext resultContext)
        {

            foreach (GroupPersonLink entity in list)
            {
                GroupPersonLink removeEntity = this.dbContext.GroupPersonLinks.Where(a => a.idGroupPersonLink == entity.idGroupPersonLink).FirstOrDefault();

                if (removeEntity != null)
                {
                    try
                    {
                        resultContext.ResultCode = ETEMEnums.ResultEnum.Error;

                        this.dbContext.DeleteObject(removeEntity);

                        resultContext.ResultCode = ETEMEnums.ResultEnum.Success;
                    }
                    catch (ETEMModelException e)
                    {
                        BaseHelper.Log(e.Message);
                        BaseHelper.Log(e.StackTrace);
                    }
                }
            }

            this.dbContext.SaveChanges();
            return resultContext;

        }

        public bool IsUniqueRecordGroupPersonLink(int idGroup, int idPerson)
        {
            bool res = true;

            var record = (from gpl in this.dbContext.GroupPersonLinks
                          where gpl.idGroup == idGroup && gpl.idPerson == idPerson
                          select gpl
                            ).FirstOrDefault();

            if (record != null)
            {
                res = false;
            }

            return res;
        }

        internal List<GroupPersonLinkDataView> GetGroupPersonLinkDataViewByGroupID(int groupID)
        {

            List<GroupPersonLinkDataView> listView = (from gpl in dbContext.GroupPersonLinks
                                                      //Group
                                                      join g in this.dbContext.Groups on gpl.idGroup equals g.idGroup
                                                      //Person
                                                      join p in dbContext.Persons on gpl.idPerson equals p.idPerson

                                                      where gpl.idGroup == groupID

                                                      select new GroupPersonLinkDataView
                                                      {
                                                          idGroupPersonLink = gpl.idGroupPersonLink,
                                                          idGroup = gpl.idGroup,
                                                          idPerson = gpl.idPerson,
                                                          Title = p.Title,
                                                          FirstName = p.FirstName,
                                                          LastName = p.LastName
                                                      }
                                                    ).ToList();

            return listView;
        }

    }
}