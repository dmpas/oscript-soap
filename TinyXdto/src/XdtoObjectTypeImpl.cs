using System;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ТипОбъектаXDTO", "XDTOObjectType")]
	public class XdtoObjectTypeImpl : AutoContext<XdtoObjectTypeImpl>, IXdtoType
	{
		internal XdtoObjectTypeImpl ()
		{
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
		public void Validate (ContextIValueImpl value)
		{
			throw new NotImplementedException ("XDTOValueType.Validate");
		}

		[ContextMethod ("ЭтоПотомок", "IsDescendant")]
		public bool IsDescendant ()
		{
			throw new NotImplementedException ("XDTOValueType.IsDescendant");
		}
	}
}

