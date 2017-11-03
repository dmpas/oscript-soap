using System;
using ScriptEngine.Machine.Contexts;
using System.Xml.Schema;
using System.Collections.Generic;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	[ContextClass("ТипЗначенияXDTO", "XDTOValueType")]
	public class XdtoValueTypeImpl : AutoContext<XdtoValueTypeImpl>, IXdtoType, IXdtoReader
	{
		private IXdtoType _baseType;

		public XdtoValueTypeImpl (XmlSchemaSimpleType xmlType, XdtoFactoryImpl factory)
		{
			NamespaceUri = xmlType.QualifiedName.Namespace;
			Name = xmlType.QualifiedName.Name;

			if (xmlType.BaseXmlSchemaType is XmlSchemaSimpleType) {
				_baseType = new XdtoValueTypeImpl (xmlType.BaseXmlSchemaType as XmlSchemaSimpleType, factory);
			}

			var memberTypes = new List<XdtoValueTypeImpl> ();
			var facets = new List<XdtoFacetImpl> ();

			if (xmlType.Content is XmlSchemaSimpleTypeUnion) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeList) {
			}
			if (xmlType.Content is XmlSchemaSimpleTypeRestriction) {
				var restriction = xmlType.Content as XmlSchemaSimpleTypeRestriction;
				_baseType = new TypeResolver (factory, restriction.BaseTypeName);
			}

			MemberTypes = new XdtoValueTypeCollectionImpl (memberTypes);
			Facets = new XdtoFacetCollectionImpl (facets);
			ListItemType = new UndefinedOr<XdtoValueTypeImpl> (null);

			Reader = this;
		}

		internal XdtoValueTypeImpl (XmlDataType primitiveType, IXdtoReader reader)
		{
			Name = primitiveType.TypeName;
			NamespaceUri = primitiveType.NamespaceUri;

			var memberTypes = new List<XdtoValueTypeImpl> ();
			var facets = new List<XdtoFacetImpl> ();

			MemberTypes = new XdtoValueTypeCollectionImpl (memberTypes);
			Facets = new XdtoFacetCollectionImpl (facets);
			ListItemType = new UndefinedOr<XdtoValueTypeImpl> (null);

			Reader = reader;
		}


		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("БазовыйТип", "BaseType")]
		public XdtoValueTypeImpl BaseType {
			get {
				if (_baseType is TypeResolver) {
					_baseType = (_baseType as TypeResolver).Resolve ();
				}
				return _baseType as XdtoValueTypeImpl;
			}
		}

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("ТипыЧленовОбъединения", "MemberTypes")]
		public XdtoValueTypeCollectionImpl MemberTypes { get; }

		[ContextProperty("ТипЭлементаСписка", "ListItemType")]
		public UndefinedOr<XdtoValueTypeImpl> ListItemType { get; }

		[ContextProperty("Фасеты", "Facets")]
		public XdtoFacetCollectionImpl Facets { get; }

		public IXdtoReader Reader { get; }

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

		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType type, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			var internalValue = ValueFactory.Create (lexicalValue);

			return new XdtoDataValueImpl (this, lexicalValue, internalValue);
		}
		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoValueTypeImpl;
			if (asThis == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(Name))
			{
				return object.ReferenceEquals(this, obj);
			}
			return asThis.NamespaceUri.Equals (NamespaceUri, StringComparison.Ordinal)
			       && asThis.Name.Equals (Name, StringComparison.Ordinal);		}

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

