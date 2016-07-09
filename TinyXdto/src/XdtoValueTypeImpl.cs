using System;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ТипЗначенияXDTO", "XDTOValueType")]
	public class XdtoValueTypeImpl : AutoContext<XdtoValueTypeImpl>, IXdtoType
	{
		internal XdtoValueTypeImpl ()
		{
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
		public XdtoValueTypeImpl ListItemType { get; }

		[ContextProperty("Фасеты", "Facets")]
		public ContextIValueImpl Facets { get; }

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

