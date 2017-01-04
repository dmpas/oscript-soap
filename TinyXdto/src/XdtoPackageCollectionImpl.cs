using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("КоллекцияПакетовXDTO", "XDTOPackageCollection")]
	public class XdtoPackageCollectionImpl : FixedCollectionOf<XdtoPackageImpl>
	{
		internal XdtoPackageCollectionImpl (IEnumerable<XdtoPackageImpl> data) : base(data)
		{
		}

		public XdtoPackageImpl Get (string namespaceUri)
		{
			return this.FirstOrDefault((p) => p.NamespaceUri.Equals(namespaceUri, StringComparison.Ordinal));
		}

		[ContextMethod("Получить", "Get")]
		public new XdtoPackageImpl Get (IValue index)
		{
			if (index.DataType == DataType.Number)
				return this.Get ((int)index.AsNumber ());

			if (index.DataType == DataType.String)
				return Get (index.AsString ());
			
			throw RuntimeException.InvalidArgumentType (nameof (index));
		}
	}
}
