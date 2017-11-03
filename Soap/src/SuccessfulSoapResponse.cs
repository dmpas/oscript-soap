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
