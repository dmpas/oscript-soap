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
using System.Linq;

namespace TinyXdto
{
	public class FixedCollectionOf<ElementType>: ContextIValueImpl, ICollectionContext, IEnumerable<ElementType>
		where ElementType : IValue
	{
		private List<ElementType> _data;
		private ContextMethodsMapper<FixedCollectionOf<ElementType>> _methods = new ContextMethodsMapper<FixedCollectionOf<ElementType>>();

		internal FixedCollectionOf(IEnumerable<ElementType> Data)
		{
			_data = new List<ElementType>(Data);
		}

		public override IValue GetIndexedValue(IValue index)
		{
			return Get(index);
		}

		public override void SetIndexedValue(IValue index, IValue val)
		{
			throw RuntimeException.PropIsNotWritableException(index.AsString());
		}

		public override int FindProperty(string name)
		{
			if (typeof(ElementType).GetInterfaces().Contains(typeof(INamed)))
			{
				for (int i = 0; i < _data.Count; i++)
				{
					var namedValue = _data[i] as INamed;
					if (string.Equals(namedValue.GetComparableName(), name, StringComparison.InvariantCultureIgnoreCase))
					{
						return i;
					}
				}
				return -1;
			}
			throw RuntimeException.IndexedAccessIsNotSupportedException();
		}

		public override bool IsPropReadable(int propNum)
		{
			return true;
		}

		public override bool IsPropWritable(int propNum)
		{
			return false;
		}

		public override IValue GetPropValue(int propNum)
		{
			return _data[propNum];
		}

		public override void SetPropValue(int propNum, IValue newVal)
		{
			throw RuntimeException.PropIsNotWritableException($"{propNum}");
		}

		public override int GetPropCount()
		{
			return _data.Count;
		}

		public override string GetPropName(int propNum)
		{
			if (typeof(ElementType).IsAssignableFrom(typeof(INamed)))
			{
				return (_data[propNum] as INamed).GetComparableName();
			}
			throw new NotImplementedException();
		}

		public override int FindMethod(string name)
		{
			return _methods.FindMethod(name);
		}

		public override int GetMethodsCount()
		{
			return _methods.Count;
		}

		public override MethodInfo GetMethodInfo(int methodNumber)
		{
			return _methods.GetMethodInfo(methodNumber);
		}

		public override void CallAsProcedure(int methodNumber, IValue[] arguments)
		{
			var m = _methods.GetMethod(methodNumber);
			m.Invoke(this, arguments);
		}

		public override void CallAsFunction(int methodNumber, IValue[] arguments, out IValue retValue)
		{
			var m = _methods.GetMethod(methodNumber);
			retValue = m.Invoke(this, arguments);
		}

		public override bool IsIndexed { get; } = true;

		[ContextMethod("Получить", "Get", IsFunction = true)]
		public ElementType Get(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				return Get ((int)rawIndex.AsNumber ());
			}
			
			if (rawIndex.DataType == DataType.String)
			{
				return Get(rawIndex.AsString());
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}

		public ElementType Get(int index)
		{
			return _data [index];
		}

		public ElementType Get(string index)
		{
			var propertyIndex = FindProperty(index);
			return Get(propertyIndex);
		}

		[ContextMethod("Количество")]
		public int Count()
		{
			return _data.Count;
		}

		public IEnumerator<ElementType> GetEnumerator()
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
