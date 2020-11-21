using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Canvas.CanvasInterfaces;

namespace Canvas
{
	class EditCommandBase
	{
		public virtual bool DoUndo(IModel data)
		{
			return false;
		}
		public virtual bool DoRedo(IModel data)
		{
			return false;
		}
	}
	class EditCommandAdd : EditCommandBase
	{
		List<IDrawObject> m_objects = null;
		IDrawObject m_object;
		ICanvasLayer m_layer;
		public EditCommandAdd(ICanvasLayer layer, IDrawObject obj)
		{
			m_object = obj;
			m_layer = layer;
		}
		public EditCommandAdd(ICanvasLayer layer, List<IDrawObject> objects)
		{
			m_objects = new List<IDrawObject>(objects);
			m_layer = layer;
		}
		public override bool DoUndo(IModel data)
		{
			if (m_object != null)
				data.DeleteObjects(new IDrawObject[] { m_object });
			if (m_objects != null)
				data.DeleteObjects(m_objects);
			return true;
		}
		public override bool DoRedo(IModel data)
		{
			if (m_object != null)
				data.AddObject(m_layer, m_object);
			if (m_objects != null)
			{
				foreach (IDrawObject obj in m_objects)
					data.AddObject(m_layer, obj);
			}
			return true;
		}
	}
	class EditCommandRemove : EditCommandBase
	{
		Dictionary<ICanvasLayer, List<IDrawObject>> m_objects = new Dictionary<ICanvasLayer, List<IDrawObject>>();
		public EditCommandRemove()
		{
		}
		public void AddLayerObjects(ICanvasLayer layer, List<IDrawObject> objects)
		{
			m_objects.Add(layer, objects);
		}
		public override bool DoUndo(IModel data)
		{
			foreach (ICanvasLayer layer in m_objects.Keys)
			{
				foreach (IDrawObject obj in m_objects[layer])
					data.AddObject(layer, obj);
			}
			return true;
		}
		public override bool DoRedo(IModel data)
		{
			foreach (ICanvasLayer layer in m_objects.Keys)
				data.DeleteObjects(m_objects[layer]);
			return true;
		}
	}
	class EditCommandMove : EditCommandBase
	{
		List<IDrawObject> m_objects = new List<IDrawObject>();
		UnitPoint m_offset;
		public EditCommandMove(UnitPoint offset, IEnumerable<IDrawObject> objects)
		{
			m_objects = new List<IDrawObject>(objects);
			m_offset = offset;
		}
		public override bool DoUndo(IModel data)
		{
			foreach (IDrawObject obj in m_objects)
			{
				UnitPoint offset = new UnitPoint(-m_offset.X, -m_offset.Y);
				obj.Move(offset);
			}
			return true;
		}
		public override bool DoRedo(IModel data)
		{
			foreach (IDrawObject obj in m_objects)
				obj.Move(m_offset);
			return true;
		}
	}
	class EditCommandNodeMove : EditCommandBase
	{
		List<INodePoint> m_objects = new List<INodePoint>();
		public EditCommandNodeMove(IEnumerable<INodePoint> objects)
		{
			m_objects = new List<INodePoint>(objects);
		}
		public override bool DoUndo(IModel data)
		{
			foreach (INodePoint obj in m_objects)
				obj.Undo();
			return true;
		}
		public override bool DoRedo(IModel data)
		{
			foreach (INodePoint obj in m_objects)
				obj.Redo();
			return true;
		}
	}
	class EditCommandEditTool : EditCommandBase
	{
		IEditTool m_tool;
		public EditCommandEditTool(IEditTool tool)
		{
			m_tool = tool;
		}
		public override bool DoUndo(IModel data)
		{
			m_tool.Undo();
			return true;
		}
		public override bool DoRedo(IModel data)
		{
			m_tool.Redo();
			return true;
		}
	}
	class UndoRedoBuffer
	{
		List<EditCommandBase> m_undoBuffer = new List<EditCommandBase>();
		List<EditCommandBase> m_redoBuffer = new List<EditCommandBase>();
		bool m_canCapture = true;
		bool m_dirty = false;
		public UndoRedoBuffer()
		{
		}
		public void Clear()
		{
			m_undoBuffer.Clear();
			m_redoBuffer.Clear();
		}
		public bool Dirty
		{
			get { return m_dirty; }
			set { m_dirty = value;}
		}
		public bool CanCapture
		{
			get { return m_canCapture; }
		}
		public bool CanUndo
		{
			get { return m_undoBuffer.Count > 0; }
		}
		public bool CanRedo
		{
			get { return m_redoBuffer.Count > 0; }
		}
		public void AddCommand(EditCommandBase command)
		{
			if (m_canCapture && command != null)
			{
				m_undoBuffer.Add(command);
				m_redoBuffer.Clear();
				Dirty = true;
			}
		}
		public bool DoUndo(IModel data)
		{
			if (m_undoBuffer.Count == 0)
				return false;
			m_canCapture = false;
			EditCommandBase command = m_undoBuffer[m_undoBuffer.Count - 1];
			bool result = command.DoUndo(data);
			m_undoBuffer.RemoveAt(m_undoBuffer.Count - 1);
			m_redoBuffer.Add(command);
			m_canCapture = true;
			Dirty = true;
			return result;
		}
		public bool DoRedo(IModel data)
		{
			if (m_redoBuffer.Count == 0)
				return false;
			m_canCapture = false;
			EditCommandBase command = m_redoBuffer[m_redoBuffer.Count - 1];
			bool result = command.DoRedo(data);
			m_redoBuffer.RemoveAt(m_redoBuffer.Count - 1);
			m_undoBuffer.Add(command);
			m_canCapture = true;
			Dirty = true;
			return result;
		}
	}
}
