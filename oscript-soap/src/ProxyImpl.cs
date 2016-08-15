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

		// TODO: проверить, сработает ли
		private static ParameterDirectionEnum parameterDirection = ParameterDirectionEnum.CreateInstance ();
		private List<MethodInfo> _methods = new List<MethodInfo> ();
		private List<OperationImpl> _operations = new List<OperationImpl> ();
		private HttpConnectionContext _conn = null;

		// TODO: полный список аргументов
		public ProxyImpl (DefinitionsImpl definitions, EndpointImpl endpoint)
		{
			Definitions = definitions;
			Endpoint = endpoint;

			XdtoFactory = XdtoFactoryImpl.Constructor () as XdtoFactoryImpl;

			FillMethods ();
		}

		private void FillMethods ()
		{
			foreach (var ivOperation in Endpoint.Interface.Operations) {
				var operation = ivOperation as OperationImpl;
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

			foreach (var ivParameter in operation.Parameters) {
				var param = ivParameter as ParameterImpl;
				var pdef = new ParameterDefinition ();

				if (param.ParameterDirection.Equals (parameterDirection.In)) {
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

		// TODO: SOAP может работать не только через HTTP/HTTPS
		private HttpConnectionContext GetConnection (UriBuilder uri)
		{
			if (_conn == null) {
				_conn = new HttpConnectionContext (uri.Host, uri.Port, uri.UserName, uri.Password);
			}

			return _conn;
		}

		public override void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			var operation = _operations [methodNumber];
			retValue = ValueFactory.Create ();

			var uri = new UriBuilder (Endpoint.Location);
			var conn = GetConnection (uri);

			var headers = new MapImpl ();
			headers.Insert (ValueFactory.Create ("Content-Type"), ValueFactory.Create ("application/xml"));

			var request = HttpRequestContext.Constructor (ValueFactory.Create(uri.Path), headers);

			var xmlBody = XmlWriterImpl.Create ();
			xmlBody.SetString ("UTF-8");

			xmlBody.WriteStartElement ("soap:Envelope");
			xmlBody.WriteNamespaceMapping ("xsi", "http://www.w3.org/2001/XMLSchema-instance");
			xmlBody.WriteNamespaceMapping ("xsd", "http://www.w3.org/2001/XMLSchema");
			xmlBody.WriteNamespaceMapping ("soap", "http://schemas.xmlsoap.org/soap/envelope/");
			xmlBody.WriteNamespaceMapping ("s", Endpoint.Interface.NamespaceURI);

			xmlBody.WriteStartElement ("soap:Body");


			xmlBody.WriteStartElement ("s:" + operation.Name);

			var serializer = new XdtoSerializerImpl (XdtoFactory);

			int paramIndex = 0;
			foreach (var argValue in arguments) {

				// TODO: отделить выходные параметры от входных и входных-выходных
				var param = operation.Parameters.Get (paramIndex);

				serializer.WriteXml (xmlBody, argValue, param.Name, Endpoint.Interface.NamespaceURI);

				paramIndex++;
			}

			xmlBody.WriteEndElement (); // s:<Operation>
			xmlBody.WriteEndElement (); // soap:Body
			xmlBody.WriteEndElement (); // soap:Envelope

			var requestString = xmlBody.Close ().ToString();
			request.SetBodyFromString (requestString);

			Console.WriteLine ("Request: {0}", requestString);
			var response = conn.Post (request);

			retValue = response.GetBodyAsString (ValueFactory.Create("UTF-8"));
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
			var service = definitions.Services.First ((it) => {
					var itService = it as ServiceImpl;
					return itService.Name.Equals (serviceName) && itService.NamespaceURI.Equals (serviceNamespaceURI);
				} ) as ServiceImpl;
			if (service == null) {
				throw new RuntimeException ("Service not found!");
			}

			var endpoint = service.Endpoints.Get (endpointName) as EndpointImpl;
			if (endpoint == null) {
				throw new RuntimeException ("Endpoint not found!");
			}

			return new ProxyImpl(definitions, endpoint);
		}
	}
}

