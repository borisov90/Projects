using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ETEMModel.Models.DataView.Employees;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Helpers.Employees;
using ETEMModel.Helpers;



namespace ETEMModel.Services.Employe
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeRef" in code, svc and config file together.
    public class EmployeRef : IEmploye
    {
        #region Employe

        public List<EmployeDataView> GetEmployeDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection)
        {
            return new EmployeBL().GetEmployeDataView(searchCriteria, sortExpression, sortDirection);
        }

        public ETEMModel.Models.Employe GetEmployeByPersonId(string entityID)
        {
            return new EmployeBL().GetEmployeByPersonId(Int32.Parse(entityID));
        }

        public CallContext EmployeSave(ETEMModel.Models.Employe entity, CallContext resultContext)
        {
            resultContext.securitySettings = ETEMEnums.SecuritySettings.EmployeSave;
            CallContext resContext = new EmployeBL().EntitySave<ETEMModel.Models.Employe>(entity, resultContext);

            return resContext;
        }

        #endregion

        #region ExternalExpert

       

       

        #endregion
    }
}
