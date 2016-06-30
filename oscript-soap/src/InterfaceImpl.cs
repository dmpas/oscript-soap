using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	[ContextClass("WSИнтерфейс", "WSInterface")]
	public class InterfaceImpl : AutoContext<InterfaceImpl>
	{
		internal InterfaceImpl()
		{
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

