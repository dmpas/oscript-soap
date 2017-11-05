/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
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
			if (httpResponse.StatusCode != 200) {
				throw new HttpTransportException (httpResponse);
			}

			var stringResponse = httpResponse.GetBodyAsString (ValueFactory.Create ("UTF-8")).AsString ();
			return stringResponse;
		}

	}
}
