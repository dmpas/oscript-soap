/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
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
