using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияТочекПодключения", "WSEndpointCollection")]
	public class EndpointCollectionImpl : FixedCollectionOf<EndpointImpl>
	{

		internal EndpointCollectionImpl(IEnumerable<EndpointImpl> data) : base(data)
		{
		}

	}
}

