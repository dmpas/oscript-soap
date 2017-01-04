using System;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.Machine;

namespace TinyXdto.W3Org.XmlSchema
{
	public class StringReader : IXdtoReader
	{
		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			var value = ValueFactory.Create (lexicalValue);
			return new XdtoDataValueImpl (expectedType as XdtoValueTypeImpl, lexicalValue, value);
		}
	}
}
