using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Neurocita.Reactive.Utilities
{
    public static class ObjectExtension
    {
        public static T DeepCopy<T>(this T source)
        {
            //Get the type of source object and create a new instance of that type
            Type typeSource = source.GetType();
            T target = (T)Activator.CreateInstance(typeSource);

            IEnumerable<FieldInfo> fieldInfos = typeSource
                                                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                    .Where(fieldInfo => !fieldInfo.IsInitOnly);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.FieldType.IsValueType || fieldInfo.FieldType.IsEnum || fieldInfo.FieldType.Equals(typeof(System.String)))
                    fieldInfo.SetValue(target, fieldInfo.GetValue(source));
                else
                {
                    object fieldValue = fieldInfo.GetValue(source);
                    fieldInfo.SetValue(target, fieldValue == null ? null : fieldValue.DeepCopy());
                }
            }

            //Get all the properties of source object type
            IEnumerable<PropertyInfo> propertyInfos = typeSource
                                                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                                        .Where(propertyInfo => propertyInfo.CanWrite);
            //Assign all source property to taget object 's properties
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //check whether property type is value type, enum or string type
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.IsEnum || propertyInfo.PropertyType.Equals(typeof(System.String)))
                    propertyInfo.SetValue(target, propertyInfo.GetValue(source, null), null);
                //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                else
                {
                    object propertyValue = propertyInfo.GetValue(source, null);
                    propertyInfo.SetValue(target, propertyValue == null ? null : propertyValue.DeepCopy(), null);
                }
            }
            return target;
        }
    }
}
