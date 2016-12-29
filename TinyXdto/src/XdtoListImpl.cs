using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("СписокXDTO", "XDTOList")]
	public class XdtoListImpl : PropertyNameIndexAccessor, ICollectionContext, IEnumerable<IXdtoValue>
	{
		private List<IXdtoValue> _data = new List<IXdtoValue> ();

		public override IValue GetIndexedValue (IValue index)
		{
			return Get (index);
		}

		[ContextMethod ("Добавить", "Add")]
		public IXdtoValue Add (IXdtoValue value)
		{
			_data.Add (value);
			return value;
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public CollectionEnumerator GetManagedIterator()
		{
			return new CollectionEnumerator(GetEnumerator());
		}
	}
}

