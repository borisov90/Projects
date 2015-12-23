using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Models.DataView.CostCalculation
{
    public class ProfileSettingDataView : ProfileSetting
    {
        public string NumberOfCavitiesName  { get; set; }
        public string ProfileTypeName       { get; set; }
        public string ProfileCategoryName   { get; set; }
        public string ProfileComplexityName { get; set; }
    }
}