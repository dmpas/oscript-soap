﻿/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace TinyXdto
{
	[ContextClass ("ПакетXDTO", "XDTOPackage")]
	public class XdtoPackage : AutoContext<XdtoPackage>, ICollectionContext, IEnumerable<IXdtoType>, INamed
	{
		private readonly XmlSchema _schema;
		private readonly XdtoFactory _factory;
		private readonly List<IXdtoType> _types = new List<IXdtoType> ();
		private bool _built = false;

		internal XdtoPackage (XmlSchema schema, XdtoFactory factory)
		{
			_schema = schema;
			_factory = factory;

			NamespaceUri = schema.TargetNamespace;
			Dependencies = new XdtoPackageCollection (new XdtoPackage [] { });

		}

		internal void BuildPackage()
		{
			if (_built)
			{
				return;
			}
			_built = true;

			foreach (var include in _schema.Includes)
			{
				if (include is XmlSchemaImport)
				{
					var import = include as XmlSchemaImport;
					var packagetoImport = _factory.Packages.Get(import.Namespace);
					// TODO: what if null ?
					packagetoImport?.BuildPackage();
				}
			}
			
			foreach (var oType in _schema.SchemaTypes) {
				var dElement = (DictionaryEntry)oType;
				var type = dElement.Value;
				if (type is XmlSchemaSimpleType) {
					_types.Add (new XdtoValueType (type as XmlSchemaSimpleType, _factory));
				} else
				if (type is XmlSchemaComplexType) {
					_types.Add (new XdtoObjectType (type as XmlSchemaComplexType, _factory));
				}
			}

			var rootProperties = new List<XdtoProperty> ();
			foreach (var oElement in _schema.Elements) {

				var elPair = (DictionaryEntry)oElement;
				var element = elPair.Value as XmlSchemaElement;

				// в XDTO под элемент создаётся свой тип с таким же именем
				IXdtoType elementType;
				if (element.SchemaType is XmlSchemaSimpleType) {

					elementType = new XdtoValueType (element.SchemaType as XmlSchemaSimpleType, _factory);

				} else if (element.SchemaType is XmlSchemaComplexType) {

					elementType = new XdtoObjectType (element, _factory);

				} else {
					// TODO: Присвоить anyType
					throw new NotImplementedException ();
				}

				_types.Add (elementType);

				var property = new XdtoProperty (XmlFormEnum.Element,
					element.QualifiedName.Namespace,
					element.QualifiedName.Name,
					elementType);
				rootProperties.Add (property);
			}

			RootProperties = new XdtoPropertyCollection (rootProperties);

		}

		internal XdtoPackage (string namespaceUri, IEnumerable<IXdtoType> types)
		{
			NamespaceUri = namespaceUri;
			_types.AddRange (types);
			Dependencies = new XdtoPackageCollection (new XdtoPackage [] { });
			RootProperties = new XdtoPropertyCollection (new XdtoProperty [] { });
			_built = true;
		}

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty ("Зависимости", "Dependencies")]
		public XdtoPackageCollection Dependencies { get; }

		[ContextProperty ("КорневыеСвойства", "RootProperties")]
		public XdtoPropertyCollection RootProperties { get; private set; }

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
			var asThis = obj as XdtoPackage;
			if (asThis == null)
				return false;
			return asThis.NamespaceUri.Equals (NamespaceUri, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return NamespaceUri.GetHashCode ();
		}

		public string GetComparableName()
		{
			return NamespaceUri;
		}

		public override string ToString ()
		{
			return string.Format("{{{0}}}", NamespaceUri);
		}
	}
}

