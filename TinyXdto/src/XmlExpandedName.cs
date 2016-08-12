using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("РасширенноеИмяXML", "XMLExpandedName")]
	public class XmlExpandedName : AutoContext<XmlExpandedName>
	{
		internal XmlExpandedName (string namespaceUri, string localName)
		{
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty("ЛокальноеИмя", "LocalName")]
		public string LocalName { get; }

		[ScriptConstructor]
		public static IReflectableContext Constructor (IValue namespaceUri, IValue localName)
		{
			return new XmlExpandedName (namespaceUri.ToString (), localName.ToString ());
		}
	}
}
