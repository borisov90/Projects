using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class RoleBL : BaseClassBL<Role>
    {

        public RoleBL()
        {
            this.EntitySetName = "Roles";
        }

        internal override Role GetEntityById(int idEntity)
        {
            return this.dbContext.Roles.Where(e => e.idRole == idEntity).FirstOrDefault();
        }


        internal override void EntityToEntity(Role entity, Role saveEntity)
        {

            saveEntity.Name = entity.Name;
            saveEntity.Description = entity.Description;
            saveEntity.RoleIntCode = entity.RoleIntCode;
            saveEntity.ViewNoActive = entity.ViewNoActive;
            saveEntity.CanDelete = entity.CanDelete;
        }


        public List<Role> GetAll(string sortExpression, string sortDirection)
        {
            List<Role> list = dbContext.Roles.Select(s => s).OrderBy(s => s.Name).ToList();
            list = BaseClassBL<Role>.Sort(list, sortExpression, sortDirection).ToList();
            return list;
        }





        internal CallContext AddPermittedAction(List<RolePermittedActionLink> listEntity, CallContext resultContext)
        {
            foreach (RolePermittedActionLink link in listEntity)
            {
                this.dbContext.AddToRolePermittedActionLinks(link);
            }
            this.dbContext.SaveChanges();
            return resultContext;
        }

        internal CallContext RemovePermittedAction(List<RolePermittedActionLink> listEntity, CallContext resultContext)
        {
            foreach (RolePermittedActionLink link in listEntity)
            {
                RolePermittedActionLink removeLink = this.dbContext.RolePermittedActionLinks.Where(l => l.idRolePermittedAction == link.idRolePermittedAction).FirstOrDefault();
                if (removeLink != null)
                {
                    this.dbContext.DeleteObject(removeLink);
                }
            }
            this.dbContext.SaveChanges();
            return resultContext;
        }

        internal List<Role> GetAllRolesByUser(string userID, string sortExpression, string sortDirection)
        {
            List<Role> entityList = new List<Role>();

            int tmpUserID;

            if (Int32.TryParse(userID, out tmpUserID))
            {

                entityList = (from r in this.dbContext.Roles
                              join l in this.dbContext.UserRoleLinks on r.idRole equals l.idRole
                              where l.idUser == tmpUserID
                              select r).ToList();
            }
            entityList = BaseClassBL<Role>.Sort(entityList, sortExpression, sortDirection).ToList();

            return entityList;
        }

        internal List<Role> GetAllRoleByUserNotAdded(string entityID, string sortExpression, string sortDirection, List<AbstractSearch> searchCriterias)
        {
            int idUser = Int32.Parse(entityID);

            List<int> listRolesID = (from p in this.dbContext.UserRoleLinks
                                     where p.idUser == idUser
                                     select p.idRole.Value).ToList();

            List<Role> list = (from p in this.dbContext.Roles
                               where !listRolesID.Contains(p.idRole)
                               select p).ApplySearchCriterias(searchCriterias).ToList();
            list = BaseClassBL<Role>.Sort(list, sortExpression, sortDirection).ToList();
            return list;
        }



        internal CallContext RemoveUserRole(string userID, string roleID, CallContext resultContext)
        {
            int tmpUserID, tmpRoleID;

            if (Int32.TryParse(userID, out tmpUserID) && Int32.TryParse(roleID, out tmpRoleID))
            {
                var listUserRoleLinksByIdUserAndIdRole = this.dbContext.UserRoleLinks.Where(l => l.idUser == tmpUserID && l.idRole == tmpRoleID).ToList<UserRoleLink>();
                foreach (UserRoleLink link in listUserRoleLinksByIdUserAndIdRole)
                {
                    if (link != null)
                    {
                        this.dbContext.DeleteObject(link);
                    }
                }
                this.dbContext.SaveChanges();
            }
            return resultContext;
        }

        internal CallContext AddUserRole(string userID, string roleID, CallContext resultContext)
        {
            int tmpUserID, tmpRoleID;

            if (Int32.TryParse(userID, out tmpUserID) && Int32.TryParse(roleID, out tmpRoleID))
            {
                this.dbContext.AddToUserRoleLinks(new UserRoleLink()
                   {
                       idRole = tmpRoleID,
                       idUser = tmpUserID
                   }
                );
                this.dbContext.SaveChanges();
            }

            return resultContext;
        }
    }
}