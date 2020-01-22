/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("СписокXDTO", "XDTOList")]
	public class XdtoList : AutoContext<XdtoList>, ICollectionContext, IEnumerable<IXdtoValue>
	{
		private List<IXdtoValue> _data = new List<IXdtoValue> ();

		internal XdtoList (XdtoDataObject owner, XdtoProperty owningProperty)
		{
			Owner = owner;
			OwningProperty = owningProperty;
		}

		[ContextProperty("Владелец")]
		public XdtoDataObject Owner { get; }

		[ContextProperty ("ВладеющееСвойство")]
		public XdtoProperty OwningProperty { get; }

		public override IValue GetIndexedValue (IValue index)
		{
			return ValueFactory.Create(Get(index));
		}

		[ContextMethod ("Добавить", "Add")]
		public IXdtoValue Add (IXdtoValue value)
		{
			_data.Add (value);
			return value;
		}

		public IXdtoValue Add (IValue value)
		{
			var pv = new PrimitiveValuesSerializer();
			var xvalue = pv.SerializeXdto(value, null);
			_data.Add (xvalue);
			return xvalue;
		}

		[ContextMethod("Получить", "Get")]
		public IXdtoValue Get(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				return Get ((int)rawIndex.AsNumber ());
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}

		public IXdtoValue Get(int index)
		{
			return _data [index];
		}

		[ContextMethod("Количество", "Count")]
		public int Count()
		{
			return _data.Count;
		}

		public IEnumerator<IXdtoValue> GetEnumerator()
		{
			foreach (var value in _data)
			{
				yield return value;
			}
		}

		public IEnumerator<IValue> GetIValueEnumerator()
		{
			foreach (var value in _data)
			{
				yield return ValueFactory.Create(value);
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public CollectionEnumerator GetManagedIterator()
		{
			return new CollectionEnumerator(GetIValueEnumerator());
		}
	}
}

