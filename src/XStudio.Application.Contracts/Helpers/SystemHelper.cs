using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// 枚举转元组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Tuple<int, string, string>> ToTupleList(this Enum theEnum)
        {
            var result = new List<Tuple<int, string, string>>();
            Type type = theEnum.GetType();
            foreach (var enumValue in Enum.GetValues(type))
            {
                var enumName = Enum.GetName(type, enumValue);
                if (enumName == null)
                {
                    continue;
                }
                var description = GetDescription(type, enumName);
                result.Add(Tuple.Create((int)enumValue, enumName, description));
            }

            return result;
        }

        /// <summary>
        /// 获取属性的DescriptionAttribute注释
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDescription(Type type, string name)
        {
            var field = type.GetField(name);
            var descriptionAttribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return descriptionAttribute?.Description ?? name;
        }
    }
}
