using System;
using ScriptEngine.HostedScript.Library.Xml;

namespace TinyXdto
{
	public interface IXdtoReader
	{
		IXdtoValue ReadXml (XmlReaderImpl reader, IXdtoType expectedType, XdtoFactoryImpl factory);
	}
}
