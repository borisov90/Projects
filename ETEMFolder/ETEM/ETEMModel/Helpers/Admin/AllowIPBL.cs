using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Models.DataView.Admin;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Helpers.Admin
{
    public class AllowIPBL : BaseClassBL<AllowIP>
    {
        public AllowIPBL()
        {
            this.EntitySetName = "AllowIPs";
        }

        internal override AllowIP GetEntityById(int idEntity)
        {
            return this.dbContext.AllowIPs.Where(e => e.idAllowIP == idEntity).FirstOrDefault();
        }

        internal AllowIP GetEntityByIPAddress(string IP)
        {
            return this.dbContext.AllowIPs.Where(e => e.IP == IP).FirstOrDefault();
        }


        internal override void EntityToEntity(AllowIP sourceEntity, AllowIP targetEntity)
        {
            targetEntity.IP = sourceEntity.IP;
            targetEntity.Commnet = sourceEntity.Commnet;
            targetEntity.Allow = sourceEntity.Allow;
        }

        internal List<AllowIPDataView> GetAllAllowIP(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<AllowIPDataView> listView = (from u in dbContext.AllowIPs

                                              select new AllowIPDataView
                                              {
                                                  idAllowIP = u.idAllowIP,
                                                  IP = u.IP,
                                                  Commnet = u.Commnet,
                                                  Allow = u.Allow

                                              }).ApplySearchCriterias(searchCriteria).ToList();
            listView = BaseClassBL<AllowIPDataView>.Sort(listView, sortExpression, sortDirection).ToList();

            return listView;

        }
    }
}