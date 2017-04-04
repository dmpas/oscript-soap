using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ЗначениеXDTO", "XDTODataValue")]
	public class XdtoDataValueImpl : AutoContext<XdtoDataValueImpl>, IXdtoValue
	{

		private readonly XdtoValueTypeImpl _type;

		internal XdtoDataValueImpl (XdtoValueTypeImpl type,
		                            string lexicalValue,
		                            IValue value,
		                            XdtoDataValueCollectionImpl list = null)
		{
			_type = type;
			Value = value;
			LexicalValue = lexicalValue;
			List = list ?? new XdtoDataValueCollectionImpl();
		}

		[ContextProperty("Значение", "Value")]
		public IValue Value { get; }

		[ContextProperty("ЛексическоеЗначение", "LexicalValue")]
		public string LexicalValue { get; }

		[ContextProperty("Список", "List")]
		public XdtoDataValueCollectionImpl List { get; }

		[ContextMethod ("Тип", "Type")]
		public XdtoValueTypeImpl Type ()
		{
			return _type;
		}

		public XmlDataType XmlType ()
		{
			return new XmlDataType (_type.Name, _type.NamespaceUri);
		}
	}
}

