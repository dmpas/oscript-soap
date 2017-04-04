using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ТипДанныхXML", "XMLDataType")]
	public class XmlDataType : AutoContext<XmlDataType>
	{
		public XmlDataType (string typeName, string namespaceUri = XmlNs.xs)
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
			return new XmlDataType (typeName.AsString (), namespaceUri.AsString ());
		}

		public override bool Equals (object obj)
		{
			var asThis = obj as XmlDataType;
			if (asThis == null)
				return false;

			return string.Equals (NamespaceUri, asThis.NamespaceUri, StringComparison.Ordinal)
						 && string.Equals (TypeName, asThis.TypeName, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return (NamespaceUri?.GetHashCode () ?? 0)
				+ (TypeName?.GetHashCode () ?? 0);
		}

		public override string ToString ()
		{
			return string.Format ("{{{0}}}{1}", NamespaceUri, TypeName);
		}
	}
}
