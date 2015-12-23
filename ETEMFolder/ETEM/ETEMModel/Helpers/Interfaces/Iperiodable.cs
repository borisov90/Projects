using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Interfaces
{
    public class Iperiodable
    {
        string AcademicYear { get; set; }
        int Period { get; set; }
        int StartYear { get; set; }
        int EndYear { get; set; }
        int CurrentYear { get; set; }
    }
}