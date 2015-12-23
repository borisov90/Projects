using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Channels;
using ETEMModel.Helpers.AbstractSearchBLHolder;
using ETEMModel.Models.DataView.Admin;
using ETEMModel.Helpers.Admin;
using System.ServiceModel;

namespace ETEMModel.Services.Base
{
    public class BaseClassService
    {
        public bool CheckIP()
        {


            RemoteEndpointMessageProperty endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            string endpointAdress = endpoint.Address;
            ICollection<AbstractSearch> searchCriteria = new List<AbstractSearch>();

            searchCriteria.Add(
                   new BooleanSearch
                   {
                       Comparator = BooleanComparators.Equal,
                       Property = "Allow",
                       SearchTerm = true
                   });

            List<AllowIPDataView> listAllowIP = new AllowIPBL().GetAllAllowIP(searchCriteria, string.Empty, string.Empty);

            if (listAllowIP.Select(s => s.IP).Contains(endpointAdress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}