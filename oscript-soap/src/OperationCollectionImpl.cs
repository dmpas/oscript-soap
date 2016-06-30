using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияОпераций", "WSOperationCollection")]
	public class OperationCollectionImpl : AutoContext<OperationCollectionImpl>, ICollectionContext
	{

		private List<OperationImpl> _data;

		internal OperationCollectionImpl(IEnumerable<OperationImpl> Data)
		{
			_data = new List<OperationImpl>(Data);
		}

		public override IValue GetIndexedValue (IValue index)
		{
			return Get (index);
		}

		[ContextMethod("Получить", "Get")]
		public OperationImpl Get(IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.Number)
			{
				return Get ((int)rawIndex.AsNumber ());
			}
			if (rawIndex.DataType == DataType.String)
			{
				return Get (rawIndex.AsString());
			}

			throw RuntimeException.InvalidArgumentType ("index");
		}

		public OperationImpl Get(string index)
		{
			return _data.Find ((p) => (p.Name == index));
		}

		public OperationImpl Get(int index)
		{
			return _data [index];
		}

		[ContextMethod("Количество", "Count")]
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

