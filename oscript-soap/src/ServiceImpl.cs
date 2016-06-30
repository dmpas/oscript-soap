using System;
using ScriptEngine.Machine.Contexts;

namespace OneScript.Soap
{

	[ContextClass ("WSСервис", "WSService")]
	public class ServiceImpl : AutoContext<ServiceImpl>, IWithName
	{
		internal ServiceImpl ()
		{
		}

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty ("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty ("ТочкиПодключения", "Endpoints")]
		public EndpointCollectionImpl Endpoints { get; }
	}
}

