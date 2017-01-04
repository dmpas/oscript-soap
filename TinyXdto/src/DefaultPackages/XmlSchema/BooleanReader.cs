using System;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.Machine;

namespace TinyXdto.W3Org.XmlSchema
{
	public class BooleanReader : IXdtoReader
	{
		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			var value = ValueFactory.Parse (lexicalValue, DataType.Boolean);
			return new XdtoDataValueImpl (expectedType as XdtoValueTypeImpl, lexicalValue, value);
		}
	}
}
