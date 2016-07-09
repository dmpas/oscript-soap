using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("КоллекцияСвойствоXDTO", "XDTOPropertyCollection")]
	public class XdtoPropertyCollectionImpl : FixedCollectionOf<XdtoPropertyImpl>
	{
		internal XdtoPropertyCollectionImpl (IEnumerable<XdtoPropertyImpl> data) : base(data)
		{
		}
	}
}

