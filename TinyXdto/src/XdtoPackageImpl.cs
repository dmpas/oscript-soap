using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass ("ПакетXDTO", "XDTOPackage")]
	public class XdtoPackageImpl : AutoContext<XdtoPackageImpl>, ICollectionContext, IEnumerable<IXdtoType>
	{
		// private readonly XmlSchema _schema;
		private readonly List<IXdtoType> _types = new List<IXdtoType> ();

		internal XdtoPackageImpl (XmlSchema schema)
		{
			// _schema = schema;

			NamespaceUri = schema.TargetNamespace;
			Dependencies = new XdtoPackageCollectionImpl (new XdtoPackageImpl [] { });

			foreach (var oType in schema.SchemaTypes) {
				var type = oType as XmlSchemaType;
				if (type is XmlSchemaSimpleType) {
					_types.Add (new XdtoValueTypeImpl (type as XmlSchemaSimpleType));
				} else
					if (type is XmlSchemaComplexType) {
					_types.Add (new XdtoObjectTypeImpl (type as XmlSchemaComplexType));
				}
			}

			var rootProperties = new List<XdtoPropertyImpl> ();
			foreach (var oElement in schema.Elements) {

				var elPair = (DictionaryEntry)oElement;
				var element = elPair.Value as XmlSchemaElement;

				// в XDTO под элемент создаётся свой тип с таким же именем
				IXdtoType elementType;
				if (element.SchemaType is XmlSchemaSimpleType) {

					elementType = new XdtoValueTypeImpl (element.SchemaType as XmlSchemaSimpleType);

				} else if (element.SchemaType is XmlSchemaComplexType) {

					elementType = new XdtoObjectTypeImpl (element.SchemaType as XmlSchemaComplexType);

				} else {
					// TODO: Присвоить anyType					throw new NotImplementedException ();				}

				var property = new XdtoPropertyImpl (null, XmlFormEnum.Element,
				                                     element.QualifiedName.Namespace,
				                                     element.QualifiedName.Name,
				                                     elementType);
				rootProperties.Add (property);
			}

			RootProperties = new XdtoPropertyCollectionImpl (rootProperties);
		}

		internal XdtoPackageImpl (string namespaceUri, IEnumerable<IXdtoType> types)
		{
			NamespaceUri = namespaceUri;
			_types.AddRange (types);
			Dependencies = new XdtoPackageCollectionImpl (new XdtoPackageImpl [] { });
			RootProperties = new XdtoPropertyCollectionImpl (new XdtoPropertyImpl [] { });
		}

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("Зависимости", "Dependencies")]
		public XdtoPackageCollectionImpl Dependencies { get; }

		[ContextProperty ("КорневыеСвойства", "RootProperties")]
		public XdtoPropertyCollectionImpl RootProperties { get; }

		public int Count ()
		{
			return _types.Count;
		}

		public CollectionEnumerator GetManagedIterator ()
		{
			return new CollectionEnumerator (GetIValueEnumerator ());
		}

		public IEnumerator<IValue> GetIValueEnumerator ()
		{
			foreach (var _type in _types) {
				yield return ValueFactory.Create (_type);
			}
		}

		public IEnumerator<IXdtoType> GetEnumerator ()
		{
			return _types.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}

