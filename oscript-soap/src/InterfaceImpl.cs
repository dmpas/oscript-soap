using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSИнтерфейс", "WSInterface")]
	public class InterfaceImpl : AutoContext<InterfaceImpl>, IWithName
	{
		internal InterfaceImpl(Binding binding)
		{
			Documentation = binding.Documentation;
			Name = binding.Name;
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Операции", "Operations")]
		public OperationCollectionImpl Operations { get; }
	}
}

