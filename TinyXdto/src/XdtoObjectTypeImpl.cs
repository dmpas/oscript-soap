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
	public class XdtoObjectTypeImpl : AutoContext<XdtoObjectTypeImpl>, IXdtoType
	{
		internal XdtoObjectTypeImpl (XmlSchemaComplexType xmlType)
		{
			Name = xmlType.QualifiedName.Name;
			NamespaceUri = xmlType.QualifiedName.Namespace;

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

		public IXdtoValue ReadXml (XmlReaderImpl reader)
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
					// TODO: что делать, когда тип смешанный?!

					var textProperty = Properties.First ((p) => p.Form == XmlFormEnum.Text);
					IXdtoType type;
					if (textProperty == null) {
						if (!Open)
							throw new XdtoException ("Ошибка разбора XDTO!");

						textProperty = new XdtoPropertyImpl (result, XmlFormEnum.Text, NamespaceUri, "#text");
						type = new XdtoValueTypeImpl (new XmlDataType ("string"));
					} else {
						type = textProperty.Type;
					}

					var textValue = type.ReadXml (reader);
					result.Set (textProperty, textValue);

				} else if (reader.NodeType.Equals (xmlElementStart)) {

					var localName = reader.LocalName;
					var ns = reader.NamespaceURI;

					// TODO: reader.NamespaceContext
				}
			}

			throw new XdtoException ("Ошибка разбора XDTO!");
		}
	}
}

