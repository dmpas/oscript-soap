using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ПакетXDTO", "XDTOPackage")]
	public class XdtoPackageImpl : AutoContext<XdtoPackageImpl>
	{
		// TODO: XdtoPackageImpl - CollectionContext

		internal XdtoPackageImpl ()
		{
		}

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("Зависимости", "Dependencies")]
		public XdtoPackageCollectionImpl Dependencies { get; }

		[ContextProperty ("КорневыеСвойства", "RootProperties")]
		public XdtoPropertyCollectionImpl RootProperties { get; }

	}
}

