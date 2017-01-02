using System;
using ScriptEngine.HostedScript.Library.Xml;

namespace OneScript.Soap
{
	public interface ISoapTransport
	{
		string Handle (string requestBody);
	}
}
