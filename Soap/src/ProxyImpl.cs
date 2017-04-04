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
	public class ProxyImpl : AutoContext<ProxyImpl>
	{

		private List<MethodInfo> _methods = new List<MethodInfo> ();
		private List<OperationImpl> _operations = new List<OperationImpl> ();
		private ISoapTransport _transport = null;

		// TODO: полный список аргументов
		public ProxyImpl (DefinitionsImpl definitions, EndpointImpl endpoint)
		{
			Definitions = definitions;
			Endpoint = endpoint;
			XdtoFactory = definitions?.XdtoFactory ?? new XdtoFactoryImpl();

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
		public DefinitionsImpl Definitions { get; }

		[ContextProperty ("Пароль", "Password")]
		public string Password { get; set; }

		[ContextProperty ("Пользователь", "User")]
		public string User { get; set; }

		[ContextProperty ("Прокси", "Proxy")]
		public IValue Proxy { get; }

		[ContextProperty ("Таймаут", "Timeout")]
		public decimal Timeout { get; }

		[ContextProperty ("ТочкаПодключения", "Endpoint")]
		public EndpointImpl Endpoint { get; }

		[ContextProperty ("ФабрикаXDTO", "XDTOFactory")]
		public XdtoFactoryImpl XdtoFactory { get; }

		private MethodInfo GetMethodInfo (OperationImpl operation)
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

		public override IEnumerable<MethodInfo> GetMethods ()
		{
			return _methods;
		}

		public override MethodInfo GetMethodInfo (int methodNumber)
		{
			return _methods [methodNumber];
		}

		private void ConnectIfNeeded ()
		{
			if (_transport == null) {
				_transport = Endpoint.Connect ();
			}
		}

		public override void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			var operation = _operations [methodNumber];
			retValue = ValueFactory.Create ();

			ConnectIfNeeded ();

			var headers = new MapImpl ();
			headers.Insert (ValueFactory.Create ("Content-Type"), ValueFactory.Create ("application/xml"));

			var xmlBody = XmlWriterImpl.Create ();
			xmlBody.SetString ("UTF-8");

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
			var responseText = _transport.Handle (requestString);
			var xmlResult = XmlReaderImpl.Create () as XmlReaderImpl;

			// TODO: Отдать на разбор фабрике
			xmlResult.SetString (responseText);

			var result = operation.ParseResponse (xmlResult, serializer);
			if (result is SoapExceptionResponse) {
				throw new RuntimeException ((result as SoapExceptionResponse).FaultMessage);
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
			DefinitionsImpl definitions,
			IValue serviceNamespaceURI,
			IValue serviceName,
			IValue endpointName
		)
		{
			return Constructor (definitions, serviceNamespaceURI.ToString (), serviceName.ToString (), endpointName.ToString ());
		}

		public static IRuntimeContextInstance Constructor(
			DefinitionsImpl definitions,
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

			return new ProxyImpl(definitions, endpoint);
		}
	}
}

