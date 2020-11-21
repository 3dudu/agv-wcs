using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AGVDispacth.SupperSocket
{
    public class SLRequestInfoParser : IRequestInfoParser<StringRequestInfo>
    {
        #region 属性
        private readonly string m_Spliter;
        private readonly string m_ParameterSpliters;
        private const string m_OneSpace = " ";
        public static readonly BasicRequestInfoParser DefaultInstance = new BasicRequestInfoParser();
        #endregion

        #region 函数方法
        public SLRequestInfoParser()
            : this(m_OneSpace, m_OneSpace)
        {
        }

        public SLRequestInfoParser(string spliter, string parameterSpliter)
        {
            m_Spliter = spliter;
            m_ParameterSpliters = parameterSpliter;
        }

        /// <summary>
        /// 解析请求
        /// </summary>
        public StringRequestInfo ParseRequestInfo(string source)
        {
            int pos = source.IndexOf(m_Spliter);
            string name = string.Empty;
            string param = string.Empty;
            if (pos > 0)
            {
                name = source.Substring(0, pos);
                param = source.Substring(pos + m_Spliter.Length);
            }
            else
            {
                name = source;
            }
            return new StringRequestInfo(name, param, Regex.Split(param, m_ParameterSpliters));
        }
        #endregion
    }
}
