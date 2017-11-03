using System;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Http;

namespace OneScript.Soap
{
	public class HttpTransportException : ScriptEngine.Machine.RuntimeException
	{
		public HttpTransportException (HttpResponseContext response)
			: base(response.GetBodyAsString (ValueFactory.Create("UTF-8")).AsString())
		{
			Data ["StatusCode"] = response.StatusCode;
		}

	}
}
