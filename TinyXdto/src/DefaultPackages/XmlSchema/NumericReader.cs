using System;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.Machine;

namespace TinyXdto.W3Org.XmlSchema
{
	public class NumericReader : IXdtoReader
	{
		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			var value = ValueFactory.Parse (lexicalValue, DataType.Number);
			return new XdtoDataValueImpl (expectedType as XdtoValueTypeImpl, lexicalValue, value);
		}
	}
}
