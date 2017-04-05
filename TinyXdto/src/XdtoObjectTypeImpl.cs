using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Collections.Generic;
using ScriptEngine.HostedScript.Library.Xml;
using System.Linq;

namespace TinyXdto
{
	[ContextClass("ТипОбъектаXDTO", "XDTOObjectType")]
	public class XdtoObjectTypeImpl : AutoContext<XdtoObjectTypeImpl>, IXdtoType, IXdtoReader
	{
		// anyType
		internal XdtoObjectTypeImpl ()
		{
			Name = "anyType";
			NamespaceUri = XmlNs.xs;

			Open = true;
			Sequenced = true;
			Mixed = true;
			Properties = new XdtoPropertyCollectionImpl (new List<XdtoPropertyImpl> ());
		}

		public XdtoObjectTypeImpl (XmlSchemaComplexType xmlType)
		{
			Name = xmlType.QualifiedName.Name;
			NamespaceUri = xmlType.QualifiedName.Namespace;

			Abstract = xmlType.IsAbstract;
			Mixed = xmlType.IsMixed;

			Properties = new XdtoPropertyCollectionImpl (new List<XdtoPropertyImpl> ());
		}

		public XdtoObjectTypeImpl (XmlSchemaElement element)
		{
			var xmlType = element.SchemaType as XmlSchemaComplexType;

			Name = element.QualifiedName.Name;
			NamespaceUri = element.QualifiedName.Namespace;

			Abstract = xmlType.IsAbstract;
			Mixed = xmlType.IsMixed;

			Properties = new XdtoPropertyCollectionImpl (new List<XdtoPropertyImpl> ());
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty("БазовыйТип", "BaseType")]
		public XdtoObjectTypeImpl BaseType { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Абстрактный", "Abstract")]
		public bool Abstract { get; }

		[ContextProperty("Открытый", "Open")]
		public bool Open { get; }

		[ContextProperty("Последовательный", "Sequenced")]
		public bool Sequenced { get; }

		[ContextProperty("Смешанный", "Mixed")]
		public bool Mixed { get; }

		[ContextProperty("Упорядоченный", "Ordered")]
		public bool Ordered { get; }

		[ContextProperty("Свойства", "Properties")]
		public XdtoPropertyCollectionImpl Properties { get; }

		public IXdtoReader Reader {
			get {
				return this;
			}
		}

		[ContextMethod("Проверить", "Validate")]
		public void Validate (IValue value)
		{
			if (Open) {
				return;
			}

			throw new NotImplementedException ("Validate");
		}

		[ContextMethod ("ЭтоПотомок", "IsDescendant")]
		public bool IsDescendant (XdtoObjectTypeImpl type)
		{
			if (BaseType == null)
				return false;

			if (BaseType.Equals (type))
				return true;

			return BaseType.IsDescendant (type);
		}

		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory)
		{
			// TODO: Чтение атрибутов

			var result = new XdtoDataObjectImpl (this, null, null);

			// TODO: Перевести XML на простые перечисления
			var xmlNodeTypeEnum = XmlNodeTypeEnum.CreateInstance ();
			var xmlElementStart = xmlNodeTypeEnum.FromNativeValue (System.Xml.XmlNodeType.Element);
			var xmlText = xmlNodeTypeEnum.FromNativeValue (System.Xml.XmlNodeType.Text);
			var xmlElementEnd = xmlNodeTypeEnum.FromNativeValue (System.Xml.XmlNodeType.EndElement);

			reader.MoveToContent ();
			while (reader.Read ()) {
				
				if (reader.NodeType.Equals (xmlElementEnd)) {
					// TODO: result.Validate()
					return result;
				}

				if (reader.NodeType.Equals (xmlText)) {
					// надо найти свойство с Form=Text
					// оно должно быть одно

					var textProperty = Properties.FirstOrDefault ((p) => p.Form == XmlFormEnum.Text);
					IXdtoType type;
					IValue textValue;
					if (textProperty == null) {
						if (!Open)
							throw new XdtoException ("Ошибка разбора XDTO!");

						textProperty = new XdtoPropertyImpl (result, XmlFormEnum.Text, NamespaceUri, "#text");
						type = factory.Type(new XmlDataType ("string"));
						textValue = ValueFactory.Create (reader.Value);

					} else {
						type = textProperty.Type;
						textValue = type.Reader.ReadXml (reader, type, factory);
					}

					if (Sequenced) {

						result.Sequence ().Add (textValue.AsString ());

					} else {
						
						result.Set (textProperty, textValue);

					}

				} else if (reader.NodeType.Equals (xmlElementStart)) {

					var localName = reader.LocalName;
					var ns = reader.NamespaceURI;

					IXdtoType type;

					var property = Properties.First ((p) => p.LocalName.Equals(localName)
					                                 && p.NamespaceURI.Equals(ns));
					if (property != null) {

						type = property.Type;

					} else {
						
						if (!Open)
							throw new XdtoException ("Ошибка разбора XDTO!");

						property = new XdtoPropertyImpl (result, XmlFormEnum.Element, ns, localName);
						type = null;
					}

					var elementValue = factory.ReadXml (reader, type);

					if (Sequenced) {

						result.Sequence ().Add (property, elementValue);

					} else {
						
						result.Set (property, elementValue);

					}
				}
			}

			throw new XdtoException ("Ошибка разбора XDTO!");
		}

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoObjectTypeImpl;
			if (asThis == null)
				return false;
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

