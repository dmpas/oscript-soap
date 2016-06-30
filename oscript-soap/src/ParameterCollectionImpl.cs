using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияПараметров", "WSParameterCollection")]
	public class ParameterCollectionImpl : AutoContext<ParameterCollectionImpl>, ICollectionContext
	{

		private List<ParameterImpl> _data;

		internal ParameterCollectionImpl (IEnumerable<ParameterImpl> Data)
		{
			_data = new List<ParameterImpl> (Data);
		}

		public override bool IsIndexed
		{
			get { return true; }
		}

		public override IValue GetIndexedValue (IValue index)
		{
			return Get (index);
		}

		[ContextMethod("Получить", "Get")]
		public ParameterImpl Get(IValue index)
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

		public ParameterImpl Get(string index)
		{
			return _data.Find ((p) => (p.Name == index));
		}

		public ParameterImpl Get(int index)
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

