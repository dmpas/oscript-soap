using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Collections;

namespace TinyXdto
{
	[ContextClass ("ПоследовательностьXDTO", "XDTOSeqence")]
	public sealed class XdtoSequenceImpl : AutoContext<XdtoSequenceImpl>, IEnumerable<IXdtoSequenceElement>
	{

		readonly List<IXdtoSequenceElement> elements = new List<IXdtoSequenceElement> ();
		readonly bool _isMixed;

		internal XdtoSequenceImpl (XdtoDataObjectImpl owner, bool isMixed)
		{
			Owner = owner;
			_isMixed = isMixed;
		}

		[ContextProperty ("Владелец", "Owner")]
		public XdtoDataObjectImpl Owner { get; }

		[ContextMethod ("Добавить", "Add")]
		public void Add (string text)
		{
			elements.Add (new XdtoSequenceStringElement (text));
		}

		[ContextMethod ("Добавить", "Add")]
		public void Add (XdtoPropertyImpl property, IValue dataElement)
		{
			elements.Add (new XdtoSequenceValueElement (property, dataElement as IXdtoValue));
		}

		[ContextMethod ("Вставить", "Insert")]
		public void Insert (int index, string text)
		{
			elements.Insert (index, new XdtoSequenceStringElement (text));
		}

		[ContextMethod ("Вставить", "Insert")]
		public void Insert (int index, XdtoPropertyImpl property, IValue dataElement)
		{
			elements.Insert (index, new XdtoSequenceValueElement (property, dataElement as IXdtoValue));
		}

		[ContextMethod ("Количество", "Count")]
		public int Count ()
		{
			return elements.Count;
		}

		[ContextMethod ("Очистить", "Clear")]
		public void Clear ()
		{
			elements.Clear ();
		}

		[ContextMethod ("ПолучитьЗначение", "GetValue")]
		public IXdtoValue GetValue (int index)
		{
			// TODO: Разница между ПолучитьЗначение() и ПолучитьЗначение XDTO() ???
			return GetXdtoValue (index);
		}

		[ContextMethod ("ПолучитьЗначениеXDTO", "GetXDTOValue")]
		public IXdtoValue GetXdtoValue (int index)
		{
			var item = elements [index] as XdtoSequenceValueElement;
			if (item == null)
				throw new XdtoException ("Невозможно получить объект из текстового элемента!");

			return item.Value;
		}

		[ContextMethod ("ПолучитьСвойство", "GetProperty")]
		public XdtoPropertyImpl GetProperty (int index)
		{
			var item = elements [index] as XdtoSequenceValueElement;
			return item?.Property;
		}

		[ContextMethod ("ПолучитьТекст", "GetText")]
		public string GetText (int index = -1)
		{
			if (_isMixed && index == -1) {
				// не указывать номер можно только для не смешанных типов!
				throw RuntimeException.InvalidArgumentValue (); 
			}

			var item = elements [index] as XdtoSequenceStringElement;
			if (item == null)
				throw new XdtoException ("Невозможно получить строку из объекта!");

			return item.Text;
		}

		[ContextMethod ("Удалить", "Delete")]
		public void Delete (int index)
		{
			elements.RemoveAt (index);
		}

		[ContextMethod ("УстановитьЗначение", "SetValue")]
		public void SetValue (int index, IValue dataElement)
		{
			var item = elements [index] as XdtoSequenceValueElement;
			if (item == null)
				throw new XdtoException ("Невозможно установить значение текстовому элементу!");
			elements [index] = new XdtoSequenceValueElement (item.Property, dataElement as IXdtoValue);
		}

		public IEnumerator<IXdtoSequenceElement> GetEnumerator ()
		{
			return ((IEnumerable<IXdtoSequenceElement>)elements).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return ((IEnumerable<IXdtoSequenceElement>)elements).GetEnumerator ();
		}
	}
}
