using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass ("WSКоллекцияТочекПодключения", "WSEndpointCollection")]
	public class EndpointCollectionImpl : FixedCollectionOf<EndpointImpl>
	{

		internal EndpointCollectionImpl (IEnumerable<EndpointImpl> data) : base (data)
		{
		}

		internal static EndpointCollectionImpl Create(PortCollection portCollection, TinyXdto.XdtoFactoryImpl factory)
		{
			var ports = new List<EndpointImpl> ();
			foreach (var mPort in portCollection) {
				var port = mPort as Port;
				ports.Add (new EndpointImpl (port, factory));
			}
			return new EndpointCollectionImpl (ports);
		}

	}
}

