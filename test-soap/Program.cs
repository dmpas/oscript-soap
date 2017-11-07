/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using OneScript.Soap;
using ScriptEngine.Machine;
using ScriptEngine.HostedScript.Library.Xml;
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
			CheckIValueFor<Parameter>();
			CheckIValueFor<ParameterCollection>();

			CheckIValueFor<Service>();
			CheckIValueFor<ServiceCollection>();

			CheckIValueFor<ReturnValue>();

			CheckIValueFor<Definitions>();

			CheckIValueFor<Endpoint>();
			CheckIValueFor<EndpointCollection>();

			CheckIValueFor<Operation>();
			CheckIValueFor<OperationCollection>();

			CheckIValueFor<Interface>();
			CheckIValueFor<Proxy>();
		}

		public void TestWsdl ()
		{
			var def = new Definitions ("http://vm21297.hv8.ru:10080/httpservice/ws/complex.1cws?wsdl", "default");
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

			var proxy = Proxy.Constructor (def, "http://dmpas/complex", "Complex", "ComplexSoap") as Proxy;
			proxy.User = "default";
			int methodIndex = proxy.FindMethod ("DoOp");

			var callParams = new List<IValue> ();
			var OpParam = Variable.Create (ValueFactory.Create (), "Op");
			callParams.Add (OpParam);
			callParams.Add (ValueFactory.Create (1));
			callParams.Add (ValueFactory.Create (2));

			IValue result;
			proxy.CallAsFunction (methodIndex, callParams.ToArray(), out result);

			Console.WriteLine ("The DoOp result is '{0}', Op return is '{1}'", result, OpParam.AsString());
		}

		public void TestEchoService ()
		{
			var def = new Definitions ("http://vm21297.hv8.ru:10080/httpservice/ws/echo.1cws?wsdl", "default", "");
			var proxy = Proxy.Constructor (def, "http://dmpas/echo", "EchoService", "EchoServiceSoap") as Proxy;
			proxy.User = "default";

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

				Console.WriteLine ("Result for Echo{0}({2}) is {1} ({3})", callData.Key, result.AsString (), callData.Value, result.DataType);
			}
		}

		private void StartEngine ()
		{
			var engine = new ScriptEngine.HostedScript.HostedScriptEngine ();
			engine.Initialize ();
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (XdtoObjectType)));
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (Definitions)));
		}

		public void TestXdto ()
		{
			const string anyXml = @"<number>1</number>";
			var reader = XmlReaderImpl.Create () as XmlReaderImpl;
			reader.SetString (anyXml);

			var factory = XdtoFactory.Constructor() as XdtoFactory;
			var expectedType = factory.Type (new XmlDataType ("int"));
			var anyValue = factory.ReadXml (reader, expectedType) as XdtoDataValue;

			if (anyValue == null)
				throw new Exception ("XDTO не разобрался!");

			var writer = XmlWriterImpl.Create () as XmlWriterImpl;
			writer.SetString ();

			factory.WriteXml (writer, anyValue, "number");
			var serializedResult = writer.Close ().AsString();

			Console.WriteLine ("Original : {0}", anyXml);
			Console.WriteLine ("Generated: {0}", serializedResult);

			reader = XmlReaderImpl.Create () as XmlReaderImpl;
			reader.SetString (serializedResult);

			var anyValueTypeExtraction = factory.ReadXml (reader) as XdtoDataValue;
			if (anyValueTypeExtraction == null)
				throw new Exception ("Новый XDTO не разобрался!");
			Console.WriteLine ("Serialized:{0}", anyValueTypeExtraction.Value);
		}

		public void TestAllModels()
		{
			foreach (var fileName in Directory.EnumerateFiles(@"D:\temp\jar\v8.3.10\xdto"))
			{
				Console.Write($"trying {fileName}... ");
				var s = new XmlSerializer(typeof(TinyXdto.Model.XdtoModel), new XmlRootAttribute("model") {Namespace = "http://v8.1c.ru/8.1/xdto"});
				using (var fs = new FileStream(fileName, FileMode.Open))
				{
					var r = XmlReader.Create(fs);
					if (s.CanDeserialize(r))
					{
						var model = s.Deserialize(r) as TinyXdto.Model.XdtoModel;
						Console.WriteLine($"found {model.Packages?.Length} packages");
					}
					else
					{
						Console.WriteLine(" CANNOT DESERIALIZE!");
					}
				}
			}
		}

		public void TestModel()
		{
			var s = new XmlSerializer(typeof(TinyXdto.Model.XdtoModel));

			TinyXdto.Model.XdtoModel model;
			using (var fs = new FileStream(@"D:\temp\Model.xsd", FileMode.Open))
			{
				var r = XmlReader.Create(fs);
				model = s.Deserialize(r) as TinyXdto.Model.XdtoModel;
			}
			Console.WriteLine(model);
		}

		private static XmlSchemas LoadSchema(string fileName)
		{
			var schemas = new XmlSchemas();
			using (var fs = new FileStream(fileName, FileMode.Open))
			using (var r = XmlReader.Create(fs))
			{
				var sset = new XmlSchemaSet();
				sset.Add(null, r);
				
				foreach (var schema in sset.Schemas())
				{
					schemas.Add(schema as XmlSchema);
				}
			}
			return schemas;
		}

		public void TestUnnamedComplexType()
		{
			var ss = LoadSchema(@"TestData/Schema01.xsd");
			var f = new XdtoFactory(ss);
			var t = f.Type(uri: "unnamedComplexType", name: "TheComplexType") as XdtoObjectType;
			var v = f.Create(t) as XdtoDataObject;
			var p = t.Properties.Get("Element");
			var pv = f.Create(p.Type) as XdtoDataObject;
			var var1Value = f.Create(f.Type(XmlNs.xs, "string"), ValueFactory.Create("value of var1"));
			pv.Set("var1", ValueFactory.Create(var1Value));
			
			v.Set("Element", pv);
			
			var w = new XmlWriterImpl();
			w.SetString();

			f.WriteXml(w, v, "value");

			var serialized = w.Close().AsString();
			
			Console.WriteLine(serialized);
			
			var r = new XmlReaderImpl();
			r.SetString(serialized);

			var checkObject = f.ReadXml(r) as XdtoDataObject;
			var checkValue = (checkObject.Get("Element") as XdtoDataObject).Get("var1").AsString();
			
			Console.WriteLine($"got {checkValue}");
		}

		public void TestSomeInternals()
		{
			var ss = LoadSchema(@"TestData/Schema01.xsd");
			var f = new XdtoFactory(ss);
			var t = f.Type(uri: "unnamedComplexType", name: "TheComplexType") as XdtoObjectType;
			var v = f.Create(t) as XdtoDataObject;
			var fGet = t.Properties.FindMethod("Получить");
			IValue p;
			t.Properties.CallAsFunction(fGet, new[] {ValueFactory.Create("Element")}, out p);
			Console.WriteLine(p);
		}

		public void Run()
		{
			Check_AllClassesAreIValues();

			StartEngine ();
			
			TestSomeInternals();

			TestUnnamedComplexType();
			
			TestModel();
			TestAllModels();

			TestXdto ();

			// TestEchoService ();
			// TestWsdl ();
		}

		public static void Main(string[] args)
		{
			var main = new MainClass();
			main.Run();
			Console.Write ("Press any key...");
			Console.ReadKey ();
		}
	}
}
