using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;
using System.Linq;

namespace OneScript.Soap
{
	[ContextClass("WSТочкаПодключения", "WSEndpoint")]
	public class EndpointImpl : AutoContext<EndpointImpl>, IWithName
	{
		internal EndpointImpl(Port port)
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
					Interface = new InterfaceImpl (binding);
					break;
				}
			}

		}

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Интерфейс", "Interface")]
		public InterfaceImpl Interface { get; }

		[ContextProperty("Местоположение", "Location")]
		public string Location { get; }
	}
}

