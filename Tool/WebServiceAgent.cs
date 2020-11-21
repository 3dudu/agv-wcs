using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;
using Tools;

namespace Tools
{
    /// <summary>
    /// webservice代理类
    /// </summary>
    public class WebServiceAgent
    {
        #region 属性
        /// <summary>
        /// 远程代理对象
        /// </summary>
        public static Type agentType;

        /// <summary>
        /// 远程代理
        /// </summary>
        public static object agent;

        /// <summary>
        /// 远程接口命名空间
        /// </summary>
        private const string CODE_NAMESPACE = @"Fbell.AGV";
        #endregion

        #region 方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebServiceAgent(string url)
        {
            try
            {
                XmlTextReader reader = new XmlTextReader(url + "?wsdl");
                //创建和格式化 WSDL 文档
                ServiceDescription sd = ServiceDescription.Read(reader);
                //创建客户端代理类
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, null, null);

                //使用 CodeDom 编译客户端代理类
                CodeNamespace cn = new CodeNamespace(CODE_NAMESPACE);
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider icc = new CSharpCodeProvider();
                CompilerParameters cp = new CompilerParameters();
                CompilerResults cr = icc.CompileAssemblyFromDom(cp, ccu);
                //得到代理对象的类型
                agentType = cr.CompiledAssembly.GetTypes()[0];
                //通过反射创建一个远程代理实例
                agent = Activator.CreateInstance(agentType);
            }
            catch (Exception ex)
            { throw ex; }
        }


        ///<summary<   
        ///根据方法名调用指定的方法   
        ///</summary<   
        ///<param name="methodName"<方法名，大小写敏感</param<   
        ///<param name="args"<参数，按照参数顺序赋值</param<   
        ///<returns<Web服务的返回值</returns<   
        public object Invoke(string methodName, params object[] args)
        {
            MethodInfo mi = agentType.GetMethod(methodName);
            return this.Invoke(mi, args);
        }

        public object Invoke(MethodInfo method, params object[] args)
        {
            return method.Invoke(agent, args);
        }
        #endregion


        #region WebAPI
        //webrequest 方式
        //POST方式
        public static string HttpPost(string url, string body)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";

                byte[] buffer = encoding.GetBytes(body);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            { LogHelper.WriteCallBoxLog("调用WebApi接口异常信息为:" + ex.Message); return ""; }
        }

        //get方式
        public static string HttpGet(string url)
        {
            try
            {
                // string ss = HttpGet("http://localhost:41558/api/Demo/GetXXX?Name=北京");
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            { LogHelper.WriteCallBoxLog("调用WebApi接口异常信息为:" + ex.Message); return ""; }
        }
        #endregion

    }
}
