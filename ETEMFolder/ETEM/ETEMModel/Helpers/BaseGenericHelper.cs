using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETEMModel.Helpers
{
    public class BaseGenericHelper<T>
    {

        public static void EntityToEntityByReflection(T sourceEntity, T targetEntity, IEnumerable<string> propertiesToChange, bool? allowNullAsPropvalue = null)
        {
            Type sourseEntityType = sourceEntity.GetType();
            Type targetEntityType = sourceEntity.GetType();
            var sourseEntityProperties = sourseEntityType.GetProperties();
            var targetEntityProperties = targetEntityType.GetProperties();
            for (int i = 0; i < sourseEntityProperties.Length; i++)
            {
                var propSourse = sourseEntityProperties[i];
                var propTarget = targetEntityProperties[i];
                if (propertiesToChange.Any(p => p == propSourse.Name) && propSourse.CanRead && propSourse.CanWrite)
                {
                    var valueEntity = sourseEntityType.GetProperty(propSourse.Name);
                    var value = valueEntity.GetValue(sourceEntity, null);
                    if ((value != null && !string.IsNullOrEmpty(value.ToString())) || allowNullAsPropvalue == true)
                    {
                        propTarget.SetValue(targetEntity, value, null);
                    }
                }
            }

        }

        public static IEnumerable<T> ListToLowerByReflection(IEnumerable<T> listSourse)
        {


            for (int i = 0; i < listSourse.Count(); i++)
            {
                var currentEntity = listSourse.ElementAt(i);
                Type typeCurrentEntity = currentEntity.GetType();
                var properties = typeCurrentEntity.GetProperties();

                for (int j = 0; j < properties.Length; j++)
                {
                    var prop = properties[j];

                    if (prop.PropertyType.Name == typeof(System.String).Name && prop.CanRead && prop.CanWrite)
                    {
                        var target = currentEntity.GetType().GetProperty(prop.Name);
                        string value = (string)target.GetValue(currentEntity, null);
                        if (!string.IsNullOrEmpty(value))
                        {
                            prop.SetValue(currentEntity, value.ToLower(), null);
                        }



                    }
                }

            }

            return listSourse;

        }



        public static IEnumerable<T> ListToUpperFirstCharOfSentanseByReflection(IEnumerable<T> listSourse)
        {


            for (int i = 0; i < listSourse.Count(); i++)
            {
                var currentEntity = listSourse.ElementAt(i);
                Type typeCurrentEntity = currentEntity.GetType();
                var properties = typeCurrentEntity.GetProperties();

                for (int j = 0; j < properties.Length; j++)
                {
                    var prop = properties[j];

                    if (prop.PropertyType.Name == typeof(System.String).Name && prop.CanRead && prop.CanWrite)
                    {
                        var target = currentEntity.GetType().GetProperty(prop.Name);
                        string value = (string)target.GetValue(currentEntity, null);
                        if (!string.IsNullOrEmpty(value) && value.Length >= 2)
                        {
                            prop.SetValue(currentEntity, value[0].ToString().ToUpper() + value.Substring(1, value.Length - 1), null);
                        }



                    }
                }

            }

            return listSourse;

        }

        public static IEnumerable<T> ListToTitleCaseByReflection(IEnumerable<T> listSourse)
        {


            for (int i = 0; i < listSourse.Count(); i++)
            {
                var currentEntity = listSourse.ElementAt(i);
                Type typeCurrentEntity = currentEntity.GetType();
                var properties = typeCurrentEntity.GetProperties();

                for (int j = 0; j < properties.Length; j++)
                {
                    var prop = properties[j];

                    if (prop.PropertyType.Name == typeof(System.String).Name && prop.CanRead && prop.CanWrite)
                    {
                        var target = currentEntity.GetType().GetProperty(prop.Name);
                        string value = (string)target.GetValue(currentEntity, null);
                        if (!string.IsNullOrEmpty(value))
                        {
                            prop.SetValue(currentEntity, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower()), null);
                        }



                    }
                }

            }

            return listSourse;

        }



    }
}