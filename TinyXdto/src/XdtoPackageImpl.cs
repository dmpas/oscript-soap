using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TinyXdto
{
	[ContextClass ("ПакетXDTO", "XDTOPackage")]
	public class XdtoPackageImpl : AutoContext<XdtoPackageImpl>, ICollectionContext, IEnumerable<IXdtoType>
	{
		// private readonly XmlSchema _schema;
		private readonly List<IXdtoType> _types = new List<IXdtoType> ();

		internal XdtoPackageImpl (XmlSchema schema, XdtoFactoryImpl factory)
		{
			// _schema = schema;

			NamespaceUri = schema.TargetNamespace;
			Dependencies = new XdtoPackageCollectionImpl (new XdtoPackageImpl [] { });

			foreach (var oType in schema.SchemaTypes) {
				var dElement = (DictionaryEntry)oType;
				var type = dElement.Value;
				if (type is XmlSchemaSimpleType) {
					_types.Add (new XdtoValueTypeImpl (type as XmlSchemaSimpleType, factory));
				} else
					if (type is XmlSchemaComplexType) {
					_types.Add (new XdtoObjectTypeImpl (type as XmlSchemaComplexType, factory));
				}
			}

			var rootProperties = new List<XdtoPropertyImpl> ();
			foreach (var oElement in schema.Elements) {

				var elPair = (DictionaryEntry)oElement;
				var element = elPair.Value as XmlSchemaElement;

				// в XDTO под элемент создаётся свой тип с таким же именем
				IXdtoType elementType;
				if (element.SchemaType is XmlSchemaSimpleType) {

					elementType = new XdtoValueTypeImpl (element.SchemaType as XmlSchemaSimpleType, factory);

				} else if (element.SchemaType is XmlSchemaComplexType) {

					elementType = new XdtoObjectTypeImpl (element, factory);

				} else {
					// TODO: Присвоить anyType					throw new NotImplementedException ();				}

				_types.Add (elementType);

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

		[ContextMethod("Количество")]
		public int Count ()
		{
			return _types.Count;
		}

		public IXdtoType Get (string name)
		{
			return _types.FirstOrDefault((arg) => arg.Name.Equals(name, StringComparison.Ordinal));
		}

		public IXdtoType Get (int index)
		{
			return _types [index];
		}

		[ContextMethod("Получить")]
		public IXdtoType Get (IValue index)
		{
			var rawIndex = index.GetRawValue ();
			if (rawIndex.DataType == DataType.String) {
				return Get (rawIndex.AsString ());
			}
			if (rawIndex.DataType == DataType.Number) {
				return Get ((int)rawIndex.AsNumber());
			}
			throw RuntimeException.InvalidArgumentType (nameof (index));
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

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoPackageImpl;
			if (asThis == null)
				return false;
			return asThis.NamespaceUri.Equals (NamespaceUri, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return NamespaceUri.GetHashCode ();
		}

		public override string ToString ()
		{
			return string.Format("{{{0}}}", NamespaceUri);
		}
	}
}

