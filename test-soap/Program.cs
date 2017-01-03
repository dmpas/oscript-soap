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
			foreach (var service in def.Services) {
				
				Console.WriteLine ("\tService {0} has {1} endpoints", service.Name, service.Endpoints.Count ());
				foreach (var endpoint in service.Endpoints) {
					
					Console.WriteLine ("\t\tEndpoint {0} as {1} operations", endpoint.Name, endpoint.Interface.Operations.Count ());
					foreach (var operation in endpoint.Interface.Operations) {
						Console.WriteLine ("\t\t\tOperation {0}", operation.Name);
					}
				}
			}

			var proxy = ProxyImpl.Constructor (def, "http://dmpas/complex", "Complex", "ComplexSoap");
			int methodIndex = proxy.FindMethod ("DoOp");

			var callParams = new List<IValue> ();
			var OpParam = Variable.Create (ValueFactory.Create ());
			callParams.Add (OpParam);
			callParams.Add (ValueFactory.Create (1));
			callParams.Add (ValueFactory.Create (2));

			IValue result;
			proxy.CallAsFunction (methodIndex, callParams.ToArray(), out result);

			Console.WriteLine ("The DoOp result is '{0}', Op return is '{1}'", result, OpParam.AsString());
		}

		public void TestEchoService ()
		{
			var def = new DefinitionsImpl ("http://vm21297.hv8.ru:10080/httpservice/ws/echo.1cws?wsdl");
			var proxy = ProxyImpl.Constructor (def, "http://dmpas/echo", "EchoService", "EchoServiceSoap");

			decimal testValue = Decimal.Divide (152, 10);

			var calls = new Dictionary<string, IValue> ();
			calls ["Number"] = ValueFactory.Create (testValue);
			calls ["Float"] = ValueFactory.Create (testValue);
			calls ["String"] = ValueFactory.Create ("<&>");
			calls ["DateTime"] = ValueFactory.Create (DateTime.Now);
			calls ["Bool"] = ValueFactory.Create (false);
			// calls ["Fault"] = ValueFactory.Create ("123");


			foreach (var callData in calls) {
				int methodIndex = proxy.FindMethod ("Echo" + callData.Key);

				IValue result;
				proxy.CallAsFunction (methodIndex, new IValue [] { callData.Value }, out result);

				Console.WriteLine ("Result for Echo{0}({2}) is {1}", callData.Key, result.AsString (), callData.Value);
			}
		}

		private void StartEngine ()
		{
			var engine = new ScriptEngine.HostedScript.HostedScriptEngine ();
			engine.Initialize ();
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (XdtoObjectTypeImpl)));
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (DefinitionsImpl)));
		}

		public void TestXdto ()
		{
		}

		public void Run()
		{
			Check_AllClassesAreIValues();

			StartEngine ();

			TestXdto ();

			TestEchoService ();
			TestWsdlNoAuth ();
		}

		public static void Main(string[] args)
		{
			var main = new MainClass();
			main.Run();
		}
	}
}
