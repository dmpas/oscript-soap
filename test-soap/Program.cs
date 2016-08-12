using System;
using System.Linq;
using System.Collections.Generic;
using OneScript.Soap;
using ScriptEngine.Machine;
using TinyXdto;

namespace testsoap
{
	class MainClass
	{

		MainClass()
		{
		}

		void CheckIValueFor<Type>()
			where Type : IValue
		{
		}

		// В случае неправильного наследования код не скомпилируется
		void Check_AllClassesAreIValues()
		{
			CheckIValueFor<ParameterImpl>();
			CheckIValueFor<ParameterCollectionImpl>();

			CheckIValueFor<ServiceImpl>();
			CheckIValueFor<ServiceCollectionImpl>();

			CheckIValueFor<ReturnValueImpl>();

			CheckIValueFor<DefinitionsImpl>();

			CheckIValueFor<EndpointImpl>();
			CheckIValueFor<EndpointCollectionImpl>();

			CheckIValueFor<OperationImpl>();
			CheckIValueFor<OperationCollectionImpl>();

			CheckIValueFor<InterfaceImpl>();
			CheckIValueFor<ProxyImpl>();
		}

		public void TestWsdlNoAuth ()
		{
			var def = new DefinitionsImpl ("http://vm21297.hv8.ru:10080/httpservice/ws/complex.1cws?wsdl");
			Console.WriteLine ("Def has {0} services.", def.Services.Count ());
			foreach (var ivService in def.Services) {
				var service = ivService as ServiceImpl;
				Console.WriteLine ("\tService {0} has {1} endpoints", service.Name, service.Endpoints.Count ());

				foreach (var ivEndpoint in service.Endpoints) {
					var endpoint = ivEndpoint as EndpointImpl;
					Console.WriteLine ("\t\tEndpoint {0} as {1} operations", endpoint.Name, endpoint.Interface.Operations.Count ());
					foreach (var ivOperation in endpoint.Interface.Operations) {
						var operation = ivOperation as OperationImpl;
						Console.WriteLine ("\t\t\tOperation {0}", operation.Name);
					}
				}
			}

			var proxy = ProxyImpl.Constructor (def, "http://dmpas/complex", "Complex", "ComplexSoap");
			int methodIndex = proxy.FindMethod ("DoOp");

			var callParams = new List<IValue> ();
			callParams.Add (null);
			callParams.Add (ValueFactory.Create (1));
			callParams.Add (ValueFactory.Create (2));

			IValue result;
			proxy.CallAsFunction (methodIndex, callParams.ToArray(), out result);

			Console.WriteLine ("The DoOp result is {0}", result);
		}

		private void StartEngine ()
		{
			var engine = new ScriptEngine.HostedScript.HostedScriptEngine ();
			engine.Initialize ();
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (XdtoObjectTypeImpl)));
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (DefinitionsImpl)));
		}

		public void Run()
		{
			Check_AllClassesAreIValues();

			StartEngine ();
			TestWsdlNoAuth ();
		}

		public static void Main(string[] args)
		{
			var main = new MainClass();
			main.Run();
		}
	}
}
