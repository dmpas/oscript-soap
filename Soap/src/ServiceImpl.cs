using System;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;

namespace OneScript.Soap
{

	[ContextClass ("WSСервис", "WSService")]
	public class ServiceImpl : AutoContext<ServiceImpl>, IWithName
	{
		internal ServiceImpl (Service service, TinyXdto.XdtoFactoryImpl factory)
		{
			Name = service.Name;
			NamespaceURI = service.ServiceDescription.TargetNamespace;
			Documentation = service.Documentation;
			Endpoints = EndpointCollectionImpl.Create (service.Ports, factory);
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

