using System;
using ScriptEngine.Machine.Contexts;
using System.Xml.Schema;
using System.Collections.Generic;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	[ContextClass("ТипЗначенияXDTO", "XDTOValueType")]
	public class XdtoValueTypeImpl : AutoContext<XdtoValueTypeImpl>, IXdtoType
	{
		internal XdtoValueTypeImpl (XmlSchemaSimpleType xmlType)
		{
			NamespaceUri = xmlType.QualifiedName.Namespace;
			Name = xmlType.QualifiedName.Name;

			if (xmlType.BaseXmlSchemaType is XmlSchemaSimpleType) {
				BaseType = new XdtoValueTypeImpl (xmlType.BaseXmlSchemaType as XmlSchemaSimpleType);
			}

			var memberTypes = new List<XdtoValueTypeImpl> ();
			var facets = new List<XdtoFacetImpl> ();

			if (xmlType.Content is XmlSchemaSimpleTypeUnion) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeList) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeRestriction) {
			}

			MemberTypes = new XdtoValueTypeCollectionImpl (memberTypes);
			Facets = new XdtoFacetCollectionImpl (facets);
			ListItemType = new UndefinedOr<XdtoValueTypeImpl> (null);
		}

		internal XdtoValueTypeImpl (XmlDataType primitiveType)
		{
			Name = primitiveType.TypeName;
			NamespaceUri = primitiveType.NamespaceUri;

			var memberTypes = new List<XdtoValueTypeImpl> ();
			var facets = new List<XdtoFacetImpl> ();

			MemberTypes = new XdtoValueTypeCollectionImpl (memberTypes);
			Facets = new XdtoFacetCollectionImpl (facets);
			ListItemType = new UndefinedOr<XdtoValueTypeImpl> (null);
		}


		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty("БазовыйТип", "BaseType")]
		public XdtoValueTypeImpl BaseType { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("ТипыЧленовОбъединения", "MemberTypes")]
		public XdtoValueTypeCollectionImpl MemberTypes { get; }

		[ContextProperty("ТипЭлементаСписка", "ListItemType")]
		public UndefinedOr<XdtoValueTypeImpl> ListItemType { get; }

		[ContextProperty("Фасеты", "Facets")]
		public XdtoFacetCollectionImpl Facets { get; }

		[ContextMethod("Проверить", "Validate")]
		public void Validate (ContextIValueImpl value)
		{
			throw new NotImplementedException ("XDTOValueType.Validate");
		}

		[ContextMethod ("ЭтоПотомок", "IsDescendant")]
		public bool IsDescendant (XdtoValueTypeImpl type)
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

		public IXdtoValue ReadXml (XmlReaderImpl reader, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			IValue internalValue = SerializedPrimitiveValue.Convert (AsXmlDataType(), lexicalValue);

			if (internalValue == null) {
				if (BaseType != null) {
					internalValue = SerializedPrimitiveValue.Convert (BaseType.AsXmlDataType (), lexicalValue);
				}
			}

			internalValue = internalValue ?? ValueFactory.Create (lexicalValue);

			return new XdtoDataValueImpl (this, lexicalValue, ValueFactory.Create (lexicalValue));
		}
	}
}

