using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;
using ScriptEngine.HostedScript.Library.Http;
using System.Linq;

namespace OneScript.Soap
{
	[ContextClass("WSТочкаПодключения", "WSEndpoint")]
	public class EndpointImpl : AutoContext<EndpointImpl>, IWithName
	{
		private readonly ISoapTransport _transport = null;

		internal EndpointImpl(Port port, TinyXdto.XdtoFactoryImpl factory)
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
							Interface = new InterfaceImpl (portType, factory);
							break;
						}
					}
				}
			}

		}

		internal EndpointImpl (string name,
							   string documentation,
							   InterfaceImpl customInterface,
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
		public InterfaceImpl Interface { get; }

		[ContextProperty("Местоположение", "Location")]
		public string Location { get; }

		public ISoapTransport Connect ()
		{

			if (_transport != null)
				return _transport;

			var uri = new UriBuilder (Location);

			if (uri.Scheme.Equals ("http") || uri.Scheme.Equals ("https")) {
				var connection = new HttpConnectionContext (uri.Host, uri.Port, uri.UserName, uri.Password);
				return new HttpTransport (connection, uri.Path);
			}

			throw new RuntimeException (String.Format ("SOAP transport not supported: {0}", uri.Scheme));
		}
	}
}

