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
using System.Linq;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияСервисов", "WSServiceCollection")]
	public class ServiceCollection : FixedCollectionOf<Service>
	{
		ServiceCollection(IEnumerable<Service> data) : base (data)
		{
		}

		internal static ServiceCollection Create(System.Web.Services.Description.ServiceCollection collection, TinyXdto.XdtoFactory factory)
		{
			IList<Service> list = new List<Service>();
			foreach (System.Web.Services.Description.Service service in collection)
			{
				list.Add(new Service(service, factory));
			}
			return new ServiceCollection(list);
		}
	}
}

