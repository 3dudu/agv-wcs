using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Tools
{
    public class XMLClass
    {
        /// <summary>
        /// 更新XML文件节点
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="rootnodeName">父节点名称</param>
        /// <param name="nodelist">需要增加的子节点信息</param>
        /// <returns></returns>
        public static bool AppendXML(string filepath, string rootnodeName, Hashtable nodelist)
        {
            if (!File.Exists(filepath))
            {
                if (!CreateXML(filepath))
                {
                    return false;
                }
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                XmlNodeList list = doc.GetElementsByTagName(rootnodeName);
                XmlNode rootNode = null;
                if (list.Count > 0)
                {
                    rootNode = list[0];
                    rootNode.ParentNode.RemoveChild(rootNode);
                }

                XmlNode pnode = doc.SelectSingleNode("config");

                rootNode = doc.CreateElement(rootnodeName);
                pnode.AppendChild(rootNode);

                foreach (DictionaryEntry d in nodelist)
                {
                    XmlElement tempnode = doc.CreateElement(d.Key.ToString());
                    tempnode.InnerText = d.Value.ToString();
                    rootNode.AppendChild(tempnode);
                }
                doc.Save(filepath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 新建XML文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool CreateXML(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(node);
            XmlNode rootNode = doc.CreateElement("config");
            doc.AppendChild(rootNode);
            try
            {
                doc.Save(filepath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 根据父节点得到所有对应的子节点信息
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="pNodeName"></param>
        /// <returns></returns>
        public static Hashtable GetXMLByParentNode(string filepath, string pNodeName)
        {
            Hashtable ht = new Hashtable();
            if (!File.Exists(filepath))
            {
                return ht;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlElement root = doc.DocumentElement;
            XmlNodeList list = root.ChildNodes;
            foreach (XmlNode node in list)
            {
                if (node.Name.ToLower() == pNodeName.ToLower())
                {
                    foreach (XmlNode subnode in node.ChildNodes)
                    {
                        ht.Add(subnode.Name, subnode.InnerText);
                    }
                }
            }
            return ht;


        }
    }
}
