/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Collections.Generic;
using ScriptEngine.HostedScript.Library.Xml;
using System.Linq;
using System.Xml;

namespace TinyXdto
{
	[ContextClass("ТипОбъектаXDTO", "XDTOObjectType")]
	public class XdtoObjectType : AutoContext<XdtoObjectType>, IXdtoType, IXdtoReader
	{
		// anyType
		internal XdtoObjectType ()
		{
			Name = "anyType";
			NamespaceUri = XmlNs.xs;

			Open = true;
			Sequenced = true;
			Mixed = true;
			Properties = new XdtoPropertyCollection (new List<XdtoProperty> ());
		}

		// TODO: переименовать
		private XmlQualifiedName Normalize(XmlQualifiedName name, string defaultUri)
		{
			if (string.IsNullOrEmpty(name.Namespace))
			{
				return new XmlQualifiedName(name.Name, defaultUri);
			}
			return name;
		}

		public XdtoObjectType (XmlSchemaComplexType xmlType, XdtoFactory factory)
		{
			Name = xmlType.QualifiedName.Name;
			NamespaceUri = xmlType.QualifiedName.Namespace;

			Abstract = xmlType.IsAbstract;
			Mixed = xmlType.IsMixed;

			var properties = new List<XdtoProperty> ();

			var particle = xmlType.Particle;

			if (xmlType.ContentModel is XmlSchemaComplexContent)
			{
				var complexContent = xmlType.ContentModel as XmlSchemaComplexContent;
				if (complexContent.Content is XmlSchemaComplexContentExtension)
				{
					var extension = complexContent.Content as XmlSchemaComplexContentExtension;
					BaseType = factory.Type(Normalize(extension.BaseTypeName, NamespaceUri)) as XdtoObjectType;
					particle = extension.Particle;
				}
				else
				{
					throw new NotImplementedException("Недоработочка в XDTO-объекте");
				}
			}

			if (particle is XmlSchemaSequence)
			{

				var sequence = particle as XmlSchemaSequence;

				foreach (var item in sequence.Items)
				{
					var element = item as XmlSchemaElement;
					IXdtoType propertyType;
					if (!element.SchemaTypeName.IsEmpty)
					{
						propertyType = new TypeResolver(factory, element.SchemaTypeName);
					}
					else
					{
						var type = element.SchemaType;
						if (type is XmlSchemaSimpleType)
						{
							propertyType = new XdtoValueType(type as XmlSchemaSimpleType, factory);
						}
						else if (type is XmlSchemaComplexType)
						{
							propertyType = new XdtoObjectType(type as XmlSchemaComplexType, factory);
						}
						else
						{
							propertyType = new XdtoObjectType();
						}
					}

					var minOccurs = element.MinOccurs;
					var maxOccurs = string.Equals(element.MaxOccursString, "unbounded", StringComparison.Ordinal)
							? -1
							: element.MaxOccurs
						;

					properties.Add(new XdtoProperty(this, null,
						XmlFormEnum.Element,
						element.QualifiedName.Namespace,
						element.QualifiedName.Name,
						(int)minOccurs, (int)maxOccurs,
						propertyType));

				}
			} else if (particle is XmlSchemaChoice)
			{
				var choice = particle as XmlSchemaChoice;
				foreach (var item in choice.Items)
				{
					var element = item as XmlSchemaElement;
					// TODO: копипаста
					IXdtoType propertyType;
					if (!element.SchemaTypeName.IsEmpty)
					{
						propertyType = new TypeResolver(factory, element.SchemaTypeName);
					}
					else
					{
						var type = element.SchemaType;
						if (type is XmlSchemaSimpleType)
						{
							propertyType = new XdtoValueType(type as XmlSchemaSimpleType, factory);
						}
						else if (type is XmlSchemaComplexType)
						{
							propertyType = new XdtoObjectType(type as XmlSchemaComplexType, factory);
						}
						else
						{
							throw new NotImplementedException("Anonymous type...");
						}
					}

					properties.Add(new XdtoProperty(this, null,
						XmlFormEnum.Element,
						element.QualifiedName.Namespace,
						element.QualifiedName.Name,
						0, 1,
						propertyType));

				}
			} else {
				throw new NotImplementedException("Недоработочка в XDTO-объекте");
			}

			Properties = new XdtoPropertyCollection (properties);
		}

		public XdtoObjectType (XmlSchemaElement element, XdtoFactory factory)
			: this(element.SchemaType as XmlSchemaComplexType, factory)
		{
			Name = element.QualifiedName.Name;
			NamespaceUri = element.QualifiedName.Namespace;
		}

		[ContextProperty("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceUri { get; }

		[ContextProperty("БазовыйТип", "BaseType")]
		public XdtoObjectType BaseType { get; }

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
		public XdtoPropertyCollection Properties { get; }

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

			// TODO: Validate
			// throw new NotImplementedException ("Validate");
		}

		[ContextMethod ("ЭтоПотомок", "IsDescendant")]
		public bool IsDescendant (XdtoObjectType type)
		{
			if (BaseType == null)
				return false;

			if (BaseType.Equals (type))
				return true;

			return BaseType.IsDescendant (type);
		}

		public IEnumerable<XdtoProperty> AllProperties()
		{
			var _baseType = BaseType;
			while (_baseType != null)
			{
				foreach (var property in _baseType.Properties)
				{
					yield return property;
				}
				_baseType = _baseType.BaseType;
			}
			foreach (var property in Properties)
			{
				yield return property;
			}
		}

		public IXdtoValue ReadXml(XmlReaderImpl reader, IXdtoType expectedType, XdtoFactory factory)
		{
			// TODO: дублирование кода в трёх ветках

			var result = new XdtoDataObject(this, null, null);

			// TODO: Перевести XML на простые перечисления
			var xmlNodeTypeEnum = XmlNodeTypeEnum.CreateInstance();
			var xmlElementStart = xmlNodeTypeEnum.FromNativeValue(System.Xml.XmlNodeType.Element);
			var xmlText = xmlNodeTypeEnum.FromNativeValue(System.Xml.XmlNodeType.Text);
			var xmlElementEnd = xmlNodeTypeEnum.FromNativeValue(System.Xml.XmlNodeType.EndElement);

			while (reader.ReadAttribute())
			{
				if (reader.NamespaceURI.Equals("http://www.w3.org/2000/xmlns/")
				    || reader.NamespaceURI.Equals(XmlNs.xsi) && reader.LocalName.Equals("type"))
				{
					continue;
				}
				var propertyName = reader.LocalName;
				var attributeNamespace = reader.NamespaceURI;
				var attributeProperty =
					AllProperties().FirstOrDefault(p => p.Form == XmlFormEnum.Attribute
					                                  && p.LocalName.Equals(propertyName)
					                                  && p.NamespaceURI.Equals(attributeNamespace));

				if (attributeProperty == null)
				{
					if (!Open)
					{
						throw new XdtoException($"Ошиба разбора XDTO: Получили неизвестный атрибут {propertyName}");
					}
					var type = factory.Type(new XmlDataType("string"));
					attributeProperty = new XdtoProperty(null, result, XmlFormEnum.Attribute,
						NamespaceUri, propertyName,
						0, -1, type);
				}

				var attributeValue = attributeProperty.Type.Reader.ReadXml(reader, attributeProperty.Type, factory);
				result.Set(attributeProperty, ValueFactory.Create(attributeValue));
			}

			while (reader.Read()) {

				if (reader.NodeType.Equals (xmlElementEnd)) {
					// TODO: result.Validate()
					return result;
				}

				if (reader.NodeType.Equals (xmlText)) {
					// надо найти свойство с Form=Text
					// оно должно быть одно

					var textProperty = AllProperties().FirstOrDefault ((p) => p.Form == XmlFormEnum.Text);
					IXdtoType type;
					IValue textValue;
					if (textProperty == null) {
						if (!Open)
							throw new XdtoException ($"Ошибка разбора XDTO: Текст {reader.Value} в неположенном месте при разборе типа {this}!");

						textProperty = new XdtoProperty (null, result, XmlFormEnum.Text, NamespaceUri, "#text");
						type = factory.Type(new XmlDataType ("string"));
						textValue = ValueFactory.Create (reader.Value);

					} else {
						type = textProperty.Type;
						textValue = ValueFactory.Create(type.Reader.ReadXml(reader, type, factory));
					}

					if (Sequenced) {

						result.Sequence ().Add (textValue.AsString ());

					} else {

						result.Set (textProperty, textValue);

					}

				} else if (reader.NodeType.Equals (xmlElementStart)) {

					var localName = reader.LocalName;
					var ns = reader.NamespaceURI;

					var property = AllProperties().FirstOrDefault ((p) => p.LocalName.Equals(localName)
					                                                       && p.NamespaceURI.Equals(ns)
					                                                       && p.Form == XmlFormEnum.Element);
					if (property == null) {
						if (!Open)
							throw new XdtoException ($"Ошибка разбора XDTO: Получили неизвестный элемент {localName}");

						property = new XdtoProperty (null, result, XmlFormEnum.Element, ns, localName);
					}

					var elementValue = factory.ReadXml (reader, property.Type);

					// TODO: Разбор anyType
//					if (Sequenced)
//					{
//
//						result.Sequence().Add(property, ValueFactory.Create(elementValue));
//
//					} else {
//
						result.Set (property, elementValue);
//
//					}

				}
			}

			throw new XdtoException ("Ошибка разбора XDTO!");
		}

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoObjectType;
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

