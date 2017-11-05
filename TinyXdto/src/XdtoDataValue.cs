/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("ЗначениеXDTO", "XDTODataValue")]
	public class XdtoDataValue : AutoContext<XdtoDataValue>, IXdtoValue
	{

		private readonly XdtoValueType _type;

		internal XdtoDataValue (XdtoValueType type,
		                            string lexicalValue,
		                            IValue value,
		                            XdtoDataValueCollection list = null)
		{
			_type = type;
			Value = value;
			LexicalValue = lexicalValue;
			List = list ?? new XdtoDataValueCollection();
		}

		[ContextProperty("Значение", "Value")]
		public IValue Value { get; }

		[ContextProperty("ЛексическоеЗначение", "LexicalValue")]
		public string LexicalValue { get; }

		[ContextProperty("Список", "List")]
		public XdtoDataValueCollection List { get; }

		[ContextMethod ("Тип", "Type")]
		public XdtoValueType Type ()
		{
			return _type;
		}

		public XmlDataType XmlType ()
		{
			return new XmlDataType (_type.Name, _type.NamespaceUri);
		}
	}
}

