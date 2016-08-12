using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ТипДанныхXML", "XMLDataType")]
	public class XmlDataType : AutoContext<XmlDataType>
	{
		internal XmlDataType (string typeName, string namespaceUri)
		{
			NamespaceUri = namespaceUri;
			TypeName = typeName;
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty("ИмяТипа", "TypeName")]
		public string TypeName { get; }

		[ScriptConstructor]
		public static IReflectableContext Constructor (IValue typeName, IValue namespaceUri)
		{
			return new XmlDataType (typeName.ToString(), namespaceUri.ToString());
		}
	}
}
