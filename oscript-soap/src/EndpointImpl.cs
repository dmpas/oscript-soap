using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	[ContextClass("WSТочкаПодключения", "WSEndpoint")]
	public class EndpointImpl : AutoContext<EndpointImpl>
	{
		internal EndpointImpl()
		{
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

