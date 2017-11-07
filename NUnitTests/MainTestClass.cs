/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
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
			// TODO: переключить тест на другой веб-сервис
			// host.RunTestScript ("NUnitTests.Tests.ws.os");
		}

		[Test]
		public void TestWsRouter()
		{
			host.RunTestScript ("NUnitTests.Tests.router.os");
		}
	}
}
