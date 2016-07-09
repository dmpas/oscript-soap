using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace TinyXdto
{
	public class FixedCollectionOf<Type>: PropertyNameIndexAccessor, ICollectionContext
		where Type : IValue
	{
		private List<Type> _data;

		internal FixedCollectionOf(IEnumerable<Type> Data)
		{
			_data = new List<Type>(Data);
		}

		public override IValue GetIndexedValue (IValue index)
		{
			return Get (index);
		}

		public Type Get(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				return Get ((int)rawIndex.AsNumber ());
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}

		public Type Get(int index)
		{
			return _data [index];
		}

		public int Count()
		{
			return _data.Count;
		}

		public IEnumerator<IValue> GetEnumerator()
		{
			foreach (var value in _data)
			{
				yield return value;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public CollectionEnumerator GetManagedIterator()
		{
			return new CollectionEnumerator(GetEnumerator());
		}

	}
}

