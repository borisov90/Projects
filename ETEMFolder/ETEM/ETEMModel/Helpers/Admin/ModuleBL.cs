using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.Admin;

namespace ETEMModel.Helpers.Admin
{
    public class ModuleBL : BaseClassBL<Module>
    {
        public ModuleBL()
        {
            this.EntitySetName = "Modules";
        }

        internal override Module GetEntityById(int idEntity)
        {
            return this.dbContext.Modules.Where(e => e.idModule == idEntity).FirstOrDefault();
        }

        internal Module GetEntityByIPModuleSysName(string ModuleSysName)
        {
            return this.dbContext.Modules.Where(e => e.ModuleSysName == ModuleSysName).FirstOrDefault();
        }




        internal override void EntityToEntity(Module sourceEntity, Module targetEntity)
        {
            targetEntity.ModuleName = sourceEntity.ModuleName;
            targetEntity.ModuleSysName = sourceEntity.ModuleSysName;
            targetEntity.Comment = sourceEntity.Comment;
            targetEntity.NeedCheck = sourceEntity.NeedCheck;
        }

        internal List<ModuleDataView> GetAllModules(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            List<ModuleDataView> listView = (from u in dbContext.Modules

                                             select new ModuleDataView
                                             {
                                                 idModule = u.idModule,
                                                 ModuleName = u.ModuleName,
                                                 ModuleSysName = u.ModuleSysName,
                                                 Comment = u.Comment,
                                                 NeedCheck = u.NeedCheck

                                             }).ApplySearchCriterias(searchCriteria).ToList();
            listView = BaseClassBL<ModuleDataView>.Sort(listView, sortExpression, sortDirection).ToList();

            return listView;

        }
    }
}