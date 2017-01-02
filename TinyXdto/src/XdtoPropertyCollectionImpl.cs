using System;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("КоллекцияСвойствоXDTO", "XDTOPropertyCollection")]
	public class XdtoPropertyCollectionImpl : FixedCollectionOf<XdtoPropertyImpl>
	{
		internal XdtoPropertyCollectionImpl (IEnumerable<XdtoPropertyImpl> data) : base(data)
		{
		}

		public XdtoPropertyImpl Get (string name)
		{
			return this.First((p) => p.LocalName.Equals(name, StringComparison.Ordinal));
		}
	}
}

