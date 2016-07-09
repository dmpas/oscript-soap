using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("КоллекцияПакетовXDTO", "XDTOPackageCollection")]
	public class XdtoPackageCollectionImpl : FixedCollectionOf<XdtoPackageImpl>
	{
		internal XdtoPackageCollectionImpl (IEnumerable<XdtoPackageImpl> data) : base(data)
		{
		}
	}
}

