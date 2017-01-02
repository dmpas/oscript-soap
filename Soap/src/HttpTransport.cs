using System;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library;
using ScriptEngine.HostedScript.Library.Xml;
using ScriptEngine.HostedScript.Library.Http;

namespace OneScript.Soap
{
	public class HttpTransport : ISoapTransport
	{
		public HttpTransport (HttpConnectionContext connection, string path)
		{
			Connection = connection;
			Path = path;
		}

		public HttpConnectionContext Connection { get; }
		public string Path { get; }

		public string Handle (string requestBody)
		{
			var headers = new MapImpl ();
			headers.Insert (ValueFactory.Create ("Content-Type"), ValueFactory.Create ("application/xml"));

			var request = HttpRequestContext.Constructor (ValueFactory.Create (Path), headers);
			request.SetBodyFromString (requestBody);

			var httpResponse = Connection.Post (request);

			return httpResponse.GetBodyAsString ().AsString ();
		}

	}
}
