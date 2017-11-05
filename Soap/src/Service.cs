/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;

namespace OneScript.Soap
{

	[ContextClass ("WSСервис", "WSService")]
	public class Service : AutoContext<Service>, IWithName
	{
		internal Service (System.Web.Services.Description.Service service, TinyXdto.XdtoFactory factory)
		{
			Name = service.Name;
			NamespaceURI = service.ServiceDescription.TargetNamespace;
			Documentation = service.Documentation;
			Endpoints = EndpointCollection.Create (service.Ports, factory);
		}

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty ("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty ("ТочкиПодключения", "Endpoints")]
		public EndpointCollection Endpoints { get; }
	}
}

