/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace OneScript.Soap
{
	public class SuccessfulSoapResponse : IParsedResponse
	{
		public SuccessfulSoapResponse (IValue retValue, IDictionary<int, IValue> outputParams)
		{
			RetValue = retValue;
			OutputParameters = outputParams;
		}

		public IValue RetValue { get; }
		public IDictionary<int, IValue> OutputParameters { get; }
	}
}
