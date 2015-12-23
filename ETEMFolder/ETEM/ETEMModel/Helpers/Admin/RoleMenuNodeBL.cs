using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETEMModel.Models;

namespace ETEMModel.Helpers.Admin
{
    public class RoleMenuNodeBL : BaseClassBL<RoleMenuNode>
    {
        public RoleMenuNodeBL()
        {
            this.EntitySetName = "RoleMenuNodes";
        }

        internal override void EntityToEntity(RoleMenuNode sourceEntity, RoleMenuNode targetEntity)
        {
            throw new NotImplementedException();
        }

        internal override RoleMenuNode GetEntityById(int idEntity)
        {
            throw new NotImplementedException();
        }



        internal CallContext SaveRoleMenuNodes(List<KeyValuePair<string, string>> listRootMenuChecked, CallContext inputContext)
        {
            CallContext outputContext = new CallContext();

            outputContext.CurrentConsumerID = inputContext.CurrentConsumerID;
            outputContext.ResultCode = ETEMEnums.ResultEnum.Success;
            outputContext.securitySettings = inputContext.securitySettings;
            List<string> resultValidation = ValidateEntity(outputContext);
            if (listRootMenuChecked.Count > 0 && resultValidation.Count == 0)
            {
                var roleId = int.Parse(listRootMenuChecked.First().Key);
                var allRoleMenuNodes = this.dbContext.RoleMenuNodes.Where(rmn => rmn.idRole == roleId);
                foreach (var node in allRoleMenuNodes)
                {
                    dbContext.RoleMenuNodes.DeleteObject(node);
                }

                foreach (var keyValuePair in listRootMenuChecked)
                {
                    RoleMenuNode newNode = new RoleMenuNode
                    {
                        idRole = int.Parse(keyValuePair.Key),
                        idNode = int.Parse(keyValuePair.Value)

                    };
                    dbContext.RoleMenuNodes.AddObject(newNode);
                }

                dbContext.SaveChanges();

                return outputContext;
            }
            else
            {
                outputContext.ResultCode = ETEMEnums.ResultEnum.Error;
                outputContext.Message = string.Join(",", resultValidation);
                return outputContext;
            }


        }

        private List<string> ValidateEntity(CallContext outputContext)
        {
            //validate 
            return new List<string>();
        }

        internal bool IsCheckBoxChecked(int nodeId, int roleId)
        {
            bool isChecked = dbContext.RoleMenuNodes.Any(rnm => (rnm.idNode == nodeId && rnm.idRole == roleId));
            return isChecked;
        }
    }
}