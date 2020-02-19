/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.IO;
using NUnit.Framework;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine;
using ScriptEngine.Environment;
using ScriptEngine.HostedScript;

namespace NUnitTests
{
	public class EngineHelpWrapper : IHostApplication
	{

		private HostedScriptEngine engine;

		public EngineHelpWrapper ()
		{
		}

		public HostedScriptEngine Engine {
			get {
				return engine;
			}
		}

		public IValue TestRunner { get; private set; }

		public HostedScriptEngine StartEngine ()
		{
			engine = new HostedScriptEngine ();
			engine.Initialize ();

			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (TinyXdto.XdtoFactory)));
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (OneScript.Soap.Definitions)));

			// Подключаем тестовую оболочку
			engine.AttachAssembly (System.Reflection.Assembly.GetAssembly (typeof (EngineHelpWrapper)));

			var testrunnerSource = LoadFromAssemblyResource ("NUnitTests.Tests.testrunner.os");
			var moduleByteCode = engine.GetCompilerService().Compile(testrunnerSource);
			var testrunnerModule = engine.EngineInstance.LoadModuleImage(moduleByteCode);
			
			var testRunner = new UserScriptContextInstance(testrunnerModule);
			testRunner.AddProperty("ЭтотОбъект", "ThisObject", testRunner);
			testRunner.InitOwnData();
			testRunner.Initialize();
			
			TestRunner = ValueFactory.Create (testRunner);

			var testRootDir = ValueFactory.Create(TestContext.CurrentContext.TestDirectory);
			engine.InjectGlobalProperty("TestDataDirectory", testRootDir, readOnly: true);
			engine.InjectGlobalProperty("КаталогТестовыхДанных", testRootDir, readOnly: true);

			return engine;
		}

		public void RunTestScript (string resourceName)
		{
			var source = LoadFromAssemblyResource (resourceName);
			var byteCode = engine.GetCompilerService().Compile(source);
			var module = engine.EngineInstance.LoadModuleImage(byteCode);

			var test = new UserScriptContextInstance(module);
			test.AddProperty("ЭтотОбъект", "ThisObject", test);
			test.InitOwnData();
			test.Initialize();
			
			ArrayImpl testArray;
			{
				int methodIndex = test.FindMethod ("ПолучитьСписокТестов");

				{
					IValue ivTests;
					test.CallAsFunction (methodIndex, new IValue [] { TestRunner }, out ivTests);
					testArray = ivTests as ArrayImpl;
				}
			}

			foreach (var ivTestName in testArray) {
				string testName = ivTestName.AsString ();
				int methodIndex = test.FindMethod (testName);
				if (methodIndex == -1) {
					// Тест указан, но процедуры нет или она не экспортирована
					continue;
				}

				test.CallAsProcedure (methodIndex, new IValue [] { });
			}
		}

		public ICodeSource LoadFromAssemblyResource (string resourceName)
		{
			var asm = System.Reflection.Assembly.GetExecutingAssembly ();
			string codeSource;

			using (Stream s = asm.GetManifestResourceStream (resourceName)) {
				using (StreamReader r = new StreamReader (s)) {
					codeSource = r.ReadToEnd ();
				}
			}

			return engine.Loader.FromString (codeSource);
		}

		public void Echo (string str, MessageStatusEnum status = MessageStatusEnum.Ordinary)
		{
		}

		public string [] GetCommandLineArguments ()
		{
			return new string [] { };
		}

		public bool InputString (out string result, int maxLen)
		{
			result = "";
			return false;
		}

		public void ShowExceptionInfo (Exception exc)
		{
		}
	}
}
