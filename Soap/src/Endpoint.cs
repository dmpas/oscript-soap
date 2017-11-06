/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;
using ScriptEngine.HostedScript.Library.Http;

namespace OneScript.Soap
{
	[ContextClass("WSТочкаПодключения", "WSEndpoint")]
	public class Endpoint : AutoContext<Endpoint>, IWithName
	{
		private readonly ISoapTransport _transport = null;

		internal Endpoint(Port port, TinyXdto.XdtoFactory factory)
		{
			Documentation = port.Documentation;
			Name = port.Name;

			foreach (var extension in port.Extensions)
			{
				if (extension is SoapAddressBinding)
				{
					Location = (extension as SoapAddressBinding).Location;
				}
			}

			// TODO: Проверить логику поиска
			foreach (var oBinding in port.Service.ServiceDescription.Bindings) {
				var binding = oBinding as Binding;
				if (binding.Name.Equals (port.Binding.Name)) {

					foreach (var oPortType in port.Service.ServiceDescription.PortTypes) {
						var portType = oPortType as PortType;
						if (portType.Name.Equals (binding.Type.Name)) {
							Interface = new Interface (portType, factory);
							break;
						}
					}
				}
			}

		}

		internal Endpoint (string name,
							   string documentation,
							   Interface customInterface,
							   ISoapTransport customTransport)
		{
			Documentation = documentation;
			Name = Name;
			Interface = customInterface;
			Location = "";
			_transport = customTransport;
		}

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Интерфейс", "Interface")]
		public Interface Interface { get; }

		[ContextProperty("Местоположение", "Location")]
		public string Location { get; }

		public ISoapTransport Connect (string userName,
		                               string password,
		                               InternetProxyContext internetProxy,
		                               int timeout,
		                               IValue ssl)
		{

			if (_transport != null)
				return _transport;

			var uri = new UriBuilder (Location);

			if (uri.Scheme.Equals ("http") || uri.Scheme.Equals ("https")) {
				var connection = new HttpConnectionContext (uri.Host,
				                                            uri.Port,
				                                            userName,
				                                            password,
				                                            internetProxy,
				                                            timeout,
				                                            ssl);
				return new HttpTransport (connection, uri.Path);
			}

			throw new RuntimeException (String.Format ("SOAP transport not supported: {0}", uri.Scheme));
		}
	}
}

