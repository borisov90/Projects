using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView
{
    public class SpecialityCourseOKCDataView
    {
        //Използва се   LecturerTimeSheetDetailBL
        public int idWeekScheduleDetails { get; set; }
        public int? idCourse { get; set; }
        public int idSpeciality { get; set; }

        public string SpecialityInfo { get; set; }
        public int idDisciplineName { get; set; }
    }
}