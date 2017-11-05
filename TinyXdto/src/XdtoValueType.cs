/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using System.Xml.Schema;
using System.Collections.Generic;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	[ContextClass("ТипЗначенияXDTO", "XDTOValueType")]
	public class XdtoValueType : AutoContext<XdtoValueType>, IXdtoType, IXdtoReader
	{
		private IXdtoType _baseType;

		public XdtoValueType (XmlSchemaSimpleType xmlType, XdtoFactory factory)
		{
			NamespaceUri = xmlType.QualifiedName.Namespace;
			Name = xmlType.QualifiedName.Name;

			if (xmlType.BaseXmlSchemaType is XmlSchemaSimpleType) {
				_baseType = new XdtoValueType (xmlType.BaseXmlSchemaType as XmlSchemaSimpleType, factory);
			}

			var memberTypes = new List<XdtoValueType> ();
			var facets = new List<XdtoFacet> ();

			if (xmlType.Content is XmlSchemaSimpleTypeUnion) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeList) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeRestriction) {
				var restriction = xmlType.Content as XmlSchemaSimpleTypeRestriction;
				_baseType = new TypeResolver (factory, restriction.BaseTypeName);
			}

			MemberTypes = new XdtoValueTypeCollection (memberTypes);
			Facets = new XdtoFacetCollection (facets);
			ListItemType = new UndefinedOr<XdtoValueType> (null);

			Reader = this;
		}

		internal XdtoValueType (XmlDataType primitiveType, IXdtoReader reader)
		{
			Name = primitiveType.TypeName;
			NamespaceUri = primitiveType.NamespaceUri;

			var memberTypes = new List<XdtoValueType> ();
			var facets = new List<XdtoFacet> ();

			MemberTypes = new XdtoValueTypeCollection (memberTypes);
			Facets = new XdtoFacetCollection (facets);
			ListItemType = new UndefinedOr<XdtoValueType> (null);

			Reader = reader;
		}


		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("БазовыйТип", "BaseType")]
		public XdtoValueType BaseType {
			get {
				if (_baseType is TypeResolver) {
					_baseType = (_baseType as TypeResolver).Resolve ();
				}
				return _baseType as XdtoValueType;
			}
		}

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("ТипыЧленовОбъединения", "MemberTypes")]
		public XdtoValueTypeCollection MemberTypes { get; }

		[ContextProperty("ТипЭлементаСписка", "ListItemType")]
		public UndefinedOr<XdtoValueType> ListItemType { get; }

		[ContextProperty("Фасеты", "Facets")]
		public XdtoFacetCollection Facets { get; }

		public IXdtoReader Reader { get; }

		[ContextMethod("Проверить", "Validate")]
		public void Validate (ContextIValueImpl value)
		{
			throw new NotImplementedException ("XDTOValueType.Validate");
		}

		[ContextMethod ("ЭтоПотомок", "IsDescendant")]
		public bool IsDescendant (XdtoValueType type)
		{
			if (BaseType == null)
				return false;
			
			if (BaseType.Equals (type))
				return true;
			
			return BaseType.IsDescendant (type);
		}

		public XmlDataType AsXmlDataType ()
		{
			return new XmlDataType (Name, NamespaceUri);
		}

		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType type, XdtoFactory factory)
		{
			var lexicalValue = reader.Value;
			var internalValue = ValueFactory.Create (lexicalValue);

			return new XdtoDataValue (this, lexicalValue, internalValue);
		}
		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoValueType;
			if (asThis == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(Name))
			{
				return object.ReferenceEquals(this, obj);
			}
			return asThis.NamespaceUri.Equals (NamespaceUri, StringComparison.Ordinal)
			       && asThis.Name.Equals (Name, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return NamespaceUri.GetHashCode () + Name.GetHashCode ();
		}

		public override string ToString ()
		{
			return string.Format ("{{{0}}}{1}", NamespaceUri, Name);
		}
	}
}

