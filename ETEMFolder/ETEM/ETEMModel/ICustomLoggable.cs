using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ETEMModel.Models.LogModel;

namespace ETEMModel
{
    interface ICustomLoggable
    {
        EventFullLog CreateMeaningfulEntityLogXML(EventFullLog eventFullLog);
    }
}
