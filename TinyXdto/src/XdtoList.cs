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
			return Get(index);
		}
		
		public override void SetIndexedValue(IValue index, IValue val)
		{
			if (index.DataType == DataType.Number)
				Set((int)index.AsNumber(), val);
			else
				base.SetIndexedValue(index, val);
		}

		[ContextMethod ("Добавить", "Add")]
		public IXdtoValue Add (IValue value)
		{
			var rawValue = value.GetRawValue();
			if (rawValue is IXdtoValue)
			{
				_data.Add(rawValue as IXdtoValue);
				return rawValue as IXdtoValue;
			}
			var pv = new PrimitiveValuesSerializer();
			var xvalue = pv.SerializeXdto(value, null);
			_data.Add (xvalue);
			return xvalue;
		}

		[ContextMethod("Установить", "Set")]
		public IXdtoValue Set (int index, IValue value)
		{
			var pv = new PrimitiveValuesSerializer();
			var xvalue = pv.SerializeXdto(value, null);
			_data[index] = xvalue;
			return xvalue;
		}

		[ContextMethod("Получить", "Get")]
		public IValue Get(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				var value = GetXdto ((int)rawIndex.AsNumber ());
				if (value is XdtoDataValue)
				{
					return (value as XdtoDataValue).Value;
				}

				return ValueFactory.Create(value);
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}
		
		[ContextMethod("ПолучитьXDTO", "GetXDTO")]
		public IXdtoValue GetXdto(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				return GetXdto ((int)rawIndex.AsNumber ());
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}

		public IXdtoValue GetXdto(int index)
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
				if (value is XdtoDataValue)
				{
					yield return (value as XdtoDataValue).Value;
				}
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

