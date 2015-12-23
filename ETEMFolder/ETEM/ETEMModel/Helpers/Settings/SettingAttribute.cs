using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers.Settings
{
    // Multiuse attribute.
    [System.AttributeUsage(System.AttributeTargets.Class |
                           System.AttributeTargets.Struct,
                           AllowMultiple = true)  // Multiuse attribute.
    ]

    public class SettingAttribute : System.Attribute
    {

        public string FrendlyName { get; set; }
        public string Description { get; set; }

        public SettingAttribute()
        {
        }


        public SettingAttribute(string frendlyName, string description)
        {
            FrendlyName = frendlyName;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SettingListInformation : Attribute
    {

        public string TextFieldName { get; set; }
        public string ValueFieldName { get; set; }



        public String ListProviderClass { get; set; }


        public String ListProviderProperty { get; set; }

        public SettingListInformation()
        {
        }
        public SettingListInformation(string textFieldName, string valueFieldName)
        {
            TextFieldName = textFieldName;
            ValueFieldName = valueFieldName;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SettingGroupAttribute : Attribute
    {

        public string GroupName { get; set; }

        public SettingGroupAttribute()
        {
        }
        public SettingGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }

}