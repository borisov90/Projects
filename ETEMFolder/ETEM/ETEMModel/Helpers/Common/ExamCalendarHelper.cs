using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Common
{
    public class ExamCalendarHelper
    {
        public int idAcademicYear { get; set; }
        public int idSemester { get; set; }
        public int idSessionType { get; set; }
        public int idSessionPeriod { get; set; }

        public string AcademicYearStr { get; set; }
        public string SemesterStr { get; set; }
        public string SessionTypeStr { get; set; }
        public string SessionPeriodStr { get; set; }

    }
}