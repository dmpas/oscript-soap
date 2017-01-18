using System;
using System.IO;
using NUnit.Framework;
using ScriptEngine.HostedScript;
using ScriptEngine.Machine;
using ScriptEngine.Environment;
using TinyXdto;
using OneScript.Soap;

// Используется NUnit 3.6

namespace NUnitTests
{
	[TestFixture]
	public class MainTestClass
	{

		private EngineHelpWrapper host;

		[OneTimeSetUp]
		public void Initialize ()
		{
			host = new EngineHelpWrapper ();
			host.StartEngine ();
		}

		[Test]
		public void TestAsInternalObjects ()
		{
		}

		[Test]
		public void TestXdtoAsExternalObjects ()
		{
			host.RunTestScript ("NUnitTests.Tests.xdto.os");
		}

		[Test]
		public void TestWsExternalObjects ()
		{
			host.RunTestScript ("NUnitTests.Tests.ws.os");
		}
	}
}
