//using Newtonsoft.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class JosnTool
    {
        ///
        /// 把对象序列化 JSON 字符串
        ///
        /// 对象类型
        /// 对象实体
        /// JSON字符串
        public static string GetJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        ///
        /// 把JSON字符串还原为对象
        ///
        /// 对象类型
        /// JSON字符串
        /// 对象实体
        public static T ParseFormJson<T>(string szJson)
        {
            return (T)JsonConvert.DeserializeObject(szJson, typeof(T));
        }
    }
}
