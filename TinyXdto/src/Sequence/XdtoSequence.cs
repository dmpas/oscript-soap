/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Collections;

namespace TinyXdto
{
	[ContextClass ("ПоследовательностьXDTO", "XDTOSeqence")]
	public sealed class XdtoSequence : AutoContext<XdtoSequence>, IEnumerable<IXdtoSequenceElement>
	{

		readonly List<IXdtoSequenceElement> elements = new List<IXdtoSequenceElement> ();
		readonly bool _isMixed;

		internal XdtoSequence (XdtoDataObject owner, bool isMixed)
		{
			Owner = owner;
			_isMixed = isMixed;
		}

		[ContextProperty ("Владелец", "Owner")]
		public XdtoDataObject Owner { get; }

		[ContextMethod ("Добавить", "Add")]
		public void Add (string text)
		{
			elements.Add (new XdtoSequenceStringElement (text));
		}

		[ContextMethod ("Добавить", "Add")]
		public void Add (XdtoProperty property, IValue dataElement)
		{
			elements.Add (new XdtoSequenceValueElement (property, dataElement as IXdtoValue));
		}

		[ContextMethod ("Вставить", "Insert")]
		public void Insert (int index, string text)
		{
			elements.Insert (index, new XdtoSequenceStringElement (text));
		}

		[ContextMethod ("Вставить", "Insert")]
		public void Insert (int index, XdtoProperty property, IValue dataElement)
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
		public XdtoProperty GetProperty (int index)
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
