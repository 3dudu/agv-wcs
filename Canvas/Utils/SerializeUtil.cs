using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Drawing;

namespace Canvas
{
	interface ISerialize
	{
		void GetObjectData(XmlWriter wr);
		void AfterSerializedIn();
	}
	public class XmlSerializable : System.Attribute
	{
		public XmlSerializable() { }
	}
	class XmlUtil
	{
		public static void AddProperty(string name, object value, XmlWriter wr)
		{
			string svalue = string.Empty;
			if (value is string)
				svalue = value as string;
			if (svalue.Length == 0 && value.GetType() == typeof(float))
				svalue = XmlConvert.ToString(Math.Round((float)value, 8));
			if (svalue.Length == 0 && value.GetType() == typeof(double))
				svalue = XmlConvert.ToString(Math.Round((double)value, 8));
			if (svalue.Length == 0)
				svalue = value.ToString();
			
			wr.WriteStartElement("property");
			wr.WriteAttributeString("name", name);
			wr.WriteAttributeString("value", svalue);
			wr.WriteEndElement();
		}
		public static void ParseProperty(XmlElement node, object dataobject)
		{
			if (node.Name != "property")
				return;

			string fieldname = node.GetAttribute("name");
			string svalue = node.GetAttribute("value");
			if (fieldname.Length == 0 || svalue.Length == 0)
				return;

			PropertyInfo info = CommonTools.PropertyUtil.GetProperty(dataobject, fieldname);
			if (info == null || info.CanWrite == false)
				return;
			try
			{
				object value = PropertyUtil.ChangeType(svalue, info.PropertyType);
				if (value != null)
					info.SetValue(dataobject, value, null);
			}
			catch {};
		}
		public static void ParseProperties(XmlElement itemnode, object dataobject)
		{
			foreach (XmlElement propertynode in itemnode.ChildNodes)
				XmlUtil.ParseProperty(propertynode, dataobject);
		}
		public static void WriteProperties(object dataobject, XmlWriter wr)
		{
			foreach (PropertyInfo propertyInfo in dataobject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				XmlSerializable attr = (XmlSerializable)Attribute.GetCustomAttribute(propertyInfo, typeof(XmlSerializable));
				if (attr != null)
				{
					string name	= propertyInfo.Name;
					object value = propertyInfo.GetValue(dataobject, null);
					if (value != null)
						AddProperty(name, value, wr);
				}
			}
		}
	}

	class PropertyUtil
	{
		public static object ChangeType(object value, Type type)
		{
			if (type == typeof(UnitPoint))
				return Parse(value.ToString(), type);
			return CommonTools.PropertyUtil.ChangeType(value, type);
		}
		static public object Parse(string value, Type type)
		{
			if (type == typeof(UnitPoint))
				return CommonTools.PropertyUtil.Parse(new UnitPoint(0, 0), value);
			return CommonTools.PropertyUtil.Parse(value, type);
		}
	}

	class SerializeUtil
	{

	}
}
