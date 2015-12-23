using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Common
{
    public class MultipleIdKey
    {
        private int[] Ids { get; set; }
        public int idAcademicYear { get; set; }
        public int idPeriod { get; set; }
        public int idDisciplineName { get; set; }
        public string StreamNumber { get; set; }
        public int idCourse { get; set; }
        public int idAcademicPeriod { get; set; }
        public int? idSpecialty { get; set; }
        public int? idOKSFromSpecialty { get; set; }

        public MultipleIdKey(int idAcademicYear, int idPeriod, int idCourse)
        {
            this.idAcademicYear = idAcademicYear;
            this.idPeriod = idPeriod;
            this.idCourse = idCourse;
        }

        public MultipleIdKey(int idAcademicYear, int idPerid, int idDisciplineName, string streamNumber)
        {
            this.idAcademicYear = idAcademicYear;
            this.idPeriod = idPerid;
            this.idDisciplineName = idDisciplineName;
            this.StreamNumber = streamNumber;
        }
        public MultipleIdKey(string concatFourIdsData)
        {
            Ids = concatFourIdsData.Split(Constants.CHAR_SEPARATORS, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id)).ToArray();
            this.idAcademicYear = Ids[0];
            this.idPeriod = Ids[1];
            this.idDisciplineName = Ids[2];
            this.StreamNumber = Ids[3].ToString();
        }

        public MultipleIdKey()
        {

        }

        public static MultipleIdKey SetMultiKeyWithPeriod(int idAcademicPeriod, int idCourse, int idDisciplineName, string number)
        {
            MultipleIdKey multiKey = new MultipleIdKey();
            multiKey.idAcademicPeriod = idAcademicPeriod;
            multiKey.idCourse = idCourse;
            multiKey.idDisciplineName = idDisciplineName;
            multiKey.StreamNumber = number;

            return multiKey;
        }

        public static MultipleIdKey SetMultiKeyWithPeriod(int idAcademicPeriod, int idCourse, int idDisciplineName, string number, int? idSpecialty)
        {
            MultipleIdKey multiKey = SetMultiKeyWithPeriod(idAcademicPeriod, idCourse, idDisciplineName, number);
            if (idSpecialty != null)
            {
                multiKey.idSpecialty = (int)idSpecialty;
            }


            return multiKey;
        }
    }


}