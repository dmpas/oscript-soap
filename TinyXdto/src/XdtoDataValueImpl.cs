using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ЗначениеXDTO", "XDTODataValue")]
	public class XdtoDataValueImpl : AutoContext<XdtoDataValueImpl>
	{
		internal XdtoDataValueImpl ()
		{
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
			throw new NotImplementedException ("XDTODataValue.Type()");
		}

	}
}

