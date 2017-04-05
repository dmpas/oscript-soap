using System;
using System.Xml;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.Machine;

namespace TinyXdto.W3Org.XmlSchema
{
	public class DateTimeReader : IXdtoReader
	{
		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory)
		{
			var lexicalValue = reader.Value;
			var xmlDateTimeValue = XmlConvert.ToDateTime (lexicalValue, XmlDateTimeSerializationMode.Unspecified);
			var value = ValueFactory.Create (xmlDateTimeValue);
			return new XdtoDataValueImpl (expectedType as XdtoValueTypeImpl, lexicalValue, value);
		}
	}
}
