/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library;
using ScriptEngine.HostedScript.Library.Http;
using ScriptEngine.HostedScript.Library.Xml;
using System.Xml;
using System.IO;
using System.Linq;
using TinyXdto;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass ("WSПрокси", "WSProxy")]
	public class Proxy : AutoContext<Proxy>
	{

		private List<MethodInfo> _methods = new List<MethodInfo> ();
		private List<Operation> _operations = new List<Operation> ();
		private ISoapTransport _transport = null;

		// TODO: полный список аргументов
		public Proxy (Definitions definitions, Endpoint endpoint)
		{
			Definitions = definitions;
			Endpoint = endpoint;
			XdtoFactory = definitions?.XdtoFactory ?? new XdtoFactory();

			FillMethods ();
		}

		private void FillMethods ()
		{
			foreach (var operation in Endpoint.Interface.Operations) {
				_operations.Add (operation);
				_methods.Add (GetMethodInfo (operation));
			}
		}

		[ContextProperty ("ЗащищенноеСоединение", "SecuredConnection")]
		public IValue SecuredConnection { get; }

		[ContextProperty ("Определение", "Definitions")]
		public Definitions Definitions { get; }

		[ContextProperty ("Пароль", "Password")]
		public string Password { get; set; }

		[ContextProperty ("Пользователь", "User")]
		public string User { get; set; }

		[ContextProperty ("Прокси", "Proxy")]
		public InternetProxyContext InternetProxy { get; }

		[ContextProperty ("Таймаут", "Timeout")]
		public decimal Timeout { get; }

		[ContextProperty ("ТочкаПодключения", "Endpoint")]
		public Endpoint Endpoint { get; }

		[ContextProperty ("ФабрикаXDTO", "XDTOFactory")]
		public XdtoFactory XdtoFactory { get; }

		private MethodInfo GetMethodInfo (Operation operation)
		{
			MethodInfo result;

			result = new MethodInfo ();
			result.Name = operation.Name;
			result.IsFunction = true; // TODO: всегда-ли IsFunction?

			var _params = new List<ParameterDefinition> ();

			foreach (var param in operation.Parameters) {
				var pdef = new ParameterDefinition ();

				if (param.ParameterDirection == ParameterDirectionEnum.In) {
					pdef.IsByValue = true;
				}
				_params.Add (pdef);
			}
			result.Params = _params.ToArray ();

			return result;
		}

		public override int FindMethod (string name)
		{
			return _methods.FindIndex ((obj) => obj.Name.Equals (name, StringComparison.InvariantCultureIgnoreCase));
		}

		public override MethodInfo GetMethodInfo (int methodNumber)
		{
			return _methods [methodNumber];
		}

		private void ConnectIfNeeded ()
		{
			_transport = Endpoint.Connect (User, Password, InternetProxy, (int)Timeout, SecuredConnection);
		}

		public override void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			var operation = _operations [methodNumber];
			retValue = ValueFactory.Create ();

			ConnectIfNeeded ();

			var headers = new MapImpl ();
			headers.Insert (ValueFactory.Create ("Content-Type"), ValueFactory.Create ("application/xml"));

			var xmlBody = XmlWriterImpl.Create ();
			xmlBody.SetString (ValueFactory.Create("UTF-8"));

			xmlBody.WriteStartElement ("soap:Envelope");
			xmlBody.WriteNamespaceMapping ("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			xmlBody.WriteNamespaceMapping ("xsd", "http://www.w3.org/2001/XMLSchema");
			xmlBody.WriteNamespaceMapping ("soap", "http://schemas.xmlsoap.org/soap/envelope/");
			xmlBody.WriteNamespaceMapping ("s", Endpoint.Interface.NamespaceURI);

			xmlBody.WriteStartElement ("soap:Body");

			var serializer = XdtoSerializerImpl.Constructor (XdtoFactory) as XdtoSerializerImpl;

			operation.WriteRequestBody (xmlBody, serializer, arguments);

			xmlBody.WriteEndElement (); // soap:Body
			xmlBody.WriteEndElement (); // soap:Envelope

			var requestString = xmlBody.Close ().ToString();
			// Console.WriteLine(requestString); // TODO: Debug
			var responseText = _transport.Handle (requestString);
			var xmlResult = XmlReaderImpl.Create () as XmlReaderImpl;

			// Console.WriteLine(responseText); // TODO: Debug
			// TODO: Отдать на разбор фабрике
			xmlResult.SetString (responseText);

			var result = operation.ParseResponse (xmlResult, serializer);
			if (result is SoapExceptionResponse) {
				throw new ScriptEngine.Machine.RuntimeException ((result as SoapExceptionResponse).FaultMessage);
			}

			var soapResponse = result as SuccessfulSoapResponse;
			retValue = soapResponse.RetValue;

			foreach (var outParamData in soapResponse.OutputParameters) {
				
				var argument = arguments [outParamData.Key] as IVariable;
				if (argument == null) {
					continue;
				}

				argument.Value = outParamData.Value;
			}
		}

		public override void CallAsProcedure (int methodNumber, IValue [] arguments)
		{
			IValue result;
			CallAsFunction (methodNumber, arguments, out result);
		}

		// TODO: полный список параметров
		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor (
			Definitions definitions,
			IValue serviceNamespaceURI,
			IValue serviceName,
			IValue endpointName
		)
		{
			return Constructor (definitions, serviceNamespaceURI.ToString (), serviceName.ToString (), endpointName.ToString ());
		}

		public static IRuntimeContextInstance Constructor(
			Definitions definitions,
			string serviceNamespaceURI,
			string serviceName,
			string endpointName
		)
		{
			var service = definitions.Services.First ((sv) => {
					return sv.Name.Equals (serviceName) && sv.NamespaceURI.Equals (serviceNamespaceURI);
				} );
			if (service == null) {
				throw new RuntimeException ("Service not found!");
			}

			var endpoint = service.Endpoints.Get (endpointName);
			if (endpoint == null) {
				throw new RuntimeException ("Endpoint not found!");
			}

			return new Proxy(definitions, endpoint);
		}
	}
}

