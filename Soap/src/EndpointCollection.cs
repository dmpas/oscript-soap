/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass ("WSКоллекцияТочекПодключения", "WSEndpointCollection")]
	public class EndpointCollection : FixedCollectionOf<Endpoint>
	{

		internal EndpointCollection (IEnumerable<Endpoint> data) : base (data)
		{
		}

		internal static EndpointCollection Create(PortCollection portCollection, TinyXdto.XdtoFactory factory)
		{
			var ports = new List<Endpoint> ();
			foreach (var mPort in portCollection) {
				var port = mPort as Port;
				ports.Add (new Endpoint (port, factory));
			}
			return new EndpointCollection (ports);
		}

	}
}

