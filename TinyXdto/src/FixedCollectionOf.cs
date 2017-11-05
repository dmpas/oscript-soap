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
	public class FixedCollectionOf<Type>: PropertyNameIndexAccessor, ICollectionContext, IEnumerable<Type>
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

		public IEnumerator<Type> GetEnumerator()
		{
			foreach (var value in _data)
			{
				yield return value;
			}
		}

		private IEnumerator<IValue> GetEngineEnumerator ()
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
			return new CollectionEnumerator(GetEngineEnumerator());
		}

	}
}
