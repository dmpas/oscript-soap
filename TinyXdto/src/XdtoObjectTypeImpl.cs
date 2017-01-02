using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Xml.Schema;
using System.Collections.Generic;

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
	}
}

