﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nacos.V2;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Nacos
{
    public static class NacosJsonHelper
    {
        ///// <summary>
        ///// 对象转Json string
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static string ToJson(this object obj)
        //{
        //    return JsonConvert.SerializeObject(obj, Formatting.Indented);
        //}

        ///// <summary>
        ///// 把Json对象转为元对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="jsonContent"></param>
        ///// <returns></returns>
        //public static T? ToObject<T>(this string jsonContent)
        //{
        //    return JsonConvert.DeserializeObject<T>(jsonContent);
        //}
    }
}