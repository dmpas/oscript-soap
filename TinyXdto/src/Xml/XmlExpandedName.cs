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
			NamespaceUri = namespaceUri;
			LocalName = localName;
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

		public override bool Equals (object obj)
		{
			var asThis = obj as XmlExpandedName;
			if (asThis == null)
				return false;

			return string.Equals (NamespaceUri, asThis.NamespaceUri, StringComparison.Ordinal)
						 && string.Equals (LocalName, asThis.LocalName, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return (NamespaceUri?.GetHashCode () ?? 0)
				+ (LocalName?.GetHashCode () ?? 0);
		}
	}
}
