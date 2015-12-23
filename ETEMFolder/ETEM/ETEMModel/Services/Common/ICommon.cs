using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ETEMModel.Models.DataView;
using ETEMModel.Helpers.AbstractSearchBLHolder;

namespace ETEMModel.Services.Common
{
    [ServiceContract]
    public interface ICommon
    {

        [OperationContract]
        List<NotificationDataView> GetNotificationDataView(ICollection<AbstractSearch> searchCriteria, string sortExpression, string sortDirection);
    }
}
