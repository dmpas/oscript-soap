using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Linq;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSПрокси", "WSProxy")]
	public class ProxyImpl : AutoContext<ProxyImpl>
	{

		// TODO: проверить, сработает ли
		private static ParameterDirectionEnum parameterDirection = ParameterDirectionEnum.CreateInstance ();
		private List<MethodInfo> _methods = new List<MethodInfo> ();
		private List<OperationImpl> _operations = new List<OperationImpl> ();

		// TODO: полный список аргументов
		public ProxyImpl(DefinitionsImpl definitions, EndpointImpl endpoint)
		{
			Definitions = definitions;
			Endpoint = endpoint;

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

		[ContextProperty("ЗащищенноеСоединение", "SecuredConnection")]
		public IValue SecuredConnection { get; }

		[ContextProperty("Определение", "Definitions")]
		public DefinitionsImpl Definitions { get; }

		[ContextProperty("Пароль", "Password")]
		public string Password { get; set; }

		[ContextProperty("Пользователь", "User")]
		public string User { get; set; }

		[ContextProperty("Прокси", "Proxy")]
		public IValue Proxy { get; }

		[ContextProperty("Таймаут", "Timeout")]
		public decimal Timeout { get; }

		[ContextProperty("ТочкаПодключения", "Endpoint")]
		public EndpointImpl Endpoint { get; }

		[ContextProperty("ФабрикаXDTO", "XDTOFactory")]
		public IValue XdtoFactory { get; }

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
			return _methods.FindIndex((obj) => obj.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
		}

		public override IEnumerable<MethodInfo> GetMethods ()
		{
			return _methods;
		}

		public override MethodInfo GetMethodInfo (int methodNumber)
		{
			return _methods [methodNumber];
		}

		public override void CallAsFunction (int methodNumber, IValue [] arguments, out IValue retValue)
		{
			var operation = _operations [methodNumber];
			retValue = ValueFactory.Create ();

			throw new NotImplementedException ("WSProxy.[Call]");
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

