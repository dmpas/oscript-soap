using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ПоследовательностьXDTO", "XDTOSeqence")]
	public class XdtoSequenceImpl : AutoContext<XdtoSequenceImpl>
	{
		internal XdtoSequenceImpl ()
		{
		}

		[ContextProperty("Владелец", "Owner")]
		public XdtoDataObjectImpl Owner { get; }

		[ContextMethod("Добавить", "Add")]
		public void Add (string text)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("Добавить", "Add")]
		public void Add (XdtoPropertyImpl property, IValue dataElement)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("Вставить", "Insert")]
		public void Insert (int index, string name)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("Вставить", "Insert")]
		public void Insert (int index, XdtoPropertyImpl property, IValue dataElement)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("Количество", "Count")]
		public int Count ()
		{
			return 0;
		}

		[ContextMethod("Очистить", "Clear")]
		public void Clear ()
		{
		}

		[ContextMethod("ПолучитьЗначение", "GetValue")]
		public IValue GetValue (int index)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("ПолучитьЗначениеXDTO", "GetXDTOValue")]
		public IValue GetXdtoValue (int index)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("ПолучитьСвойство", "GetProperty")]
		public IValue GetProperty (int index)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("ПолучитьТекст", "GetText")]
		public string GetText (int index)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("Удалить", "Delete")]
		public void Delete (int index)
		{
			throw new NotImplementedException ();
		}

		[ContextMethod("УстановитьЗначение", "SetValue")]
		public void SetValue (int index, IValue dataElement)
		{
			throw new NotImplementedException ();
		}
	}
}

