using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("КоллекцияФасетовXDTO", "XDTOFacetCollection")]
	public class XdtoFacetCollectionImpl : FixedCollectionOf<XdtoFacetImpl>
	{
		internal XdtoFacetCollectionImpl (IEnumerable<XdtoFacetImpl> data) : base(data)
		{
		}
	}
}
