/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("TinyРасширенноеИмяXML", "TinyXMLExpandedName")]
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
		public static IRuntimeContextInstance Constructor (IValue namespaceUri, IValue localName)
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
