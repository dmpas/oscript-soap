/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Xml;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.Machine;

namespace TinyXdto.W3Org.XmlSchema
{
	public class DateTimeReader : IXdtoReader
	{
		public IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactory factory)
		{
			var lexicalValue = reader.Value;
			var xmlDateTimeValue = XmlConvert.ToDateTime (lexicalValue, XmlDateTimeSerializationMode.Unspecified);
			var value = ValueFactory.Create (xmlDateTimeValue);
			return new XdtoDataValue (expectedType as XdtoValueType, lexicalValue, value);
		}
	}
}
