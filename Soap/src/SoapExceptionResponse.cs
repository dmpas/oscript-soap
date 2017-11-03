using System;
namespace OneScript.Soap
{
	public class SoapExceptionResponse : IParsedResponse
	{
		public SoapExceptionResponse (string faultMessage)
		{
			FaultMessage = faultMessage;
		}

		public string FaultMessage { get; }
	}
}
