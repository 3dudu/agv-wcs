using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Canvas
{
	public class CursorCollection
	{
		Dictionary<object, Cursor> m_map = new Dictionary<object, Cursor>();
		public void AddCursor(object key, Cursor cursor)
		{
			m_map[key] = cursor;
		}
		public void AddCursor(object key, string resourcename)
		{
			string name = "Resources." + resourcename;
			Type type = GetType();
			Cursor cursor = new Cursor(GetType(), name);
			m_map[key] = cursor;
		}
		public Cursor GetCursor(object key)
		{
			if (m_map.ContainsKey(key))
				return m_map[key];
			return System.Windows.Forms.Cursors.Arrow;
		}
	}
}
