using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Admin
{
    public class NavUrlBL : BaseClassBL<NavURL>
    {
        public NavUrlBL()
        {
            this.EntitySetName = "NavURLs";
        }
        internal override void EntityToEntity(NavURL sourceEntity, NavURL targetEntity)
        {

            targetEntity.URL = sourceEntity.URL;
            targetEntity.code = sourceEntity.code;
            targetEntity.QueryParams = sourceEntity.QueryParams;

        }

        internal override NavURL GetEntityById(int idEntity)
        {

            var navUrl = dbContext.NavURLs.FirstOrDefault(n => n.idNavURL == idEntity);
            return navUrl;
        }

    }
}