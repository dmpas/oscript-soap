/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.IO;
using System.Web.Services.Description;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass ("WSОпределения", "WSDefinitions")]
	public class Definitions : AutoContext<Definitions>
	{

		private ServiceDescription _wsd;
		private string _wsdl;
		private string _userName;
		private string _password;
		private IValue _internetProxy;
		private decimal _timeout;
		private IValue _securedConnection;

		public Definitions (string wsdl,
			string userName = null,
			string password = null,
			IValue internetProxy = null,
			decimal timeout = 0,
			IValue securedConnection = null)
		{
			_wsdl = wsdl;
			_userName = userName;
			_password = password;
			_internetProxy = internetProxy;
			_timeout = timeout;
			_securedConnection = securedConnection;

			_wsd = ReachWsdl();

			XdtoFactory = new XdtoFactory (_wsd.Types.Schemas);

			Services = ServiceCollection.Create(_wsd.Services, XdtoFactory);
			InitializeStructures();
		}

		private void InitializeStructures()
		{

		}

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Сервисы", "Services")]
		public ServiceCollection Services { get; }

		[ContextProperty("ФабрикаXDTO", "XDTOFactory")]
		public XdtoFactory XdtoFactory { get; }

		// TODO: использовать InternetProxy, timeout и SecuredConnection
		private ServiceDescription ReachWsdl()
		{
			Stream stream = WsdlConnector.ReachWsdl(_wsdl, _userName, _password);
			return ServiceDescription.Read(stream);
		}

		[ScriptConstructor()]
		public static IRuntimeContextInstance Constructor (IValue wsdl,
			IValue userName = null,
			IValue password = null,
			IValue internetProxy = null,
			IValue timeout = null,
			IValue securedConnection = null
		)
		{
			var definitions = new Definitions(
				wsdl.ToString(),
				userName?.ToString(),
				password?.ToString(),
				internetProxy,
				0,
				securedConnection);
			return definitions;
		}
	}
}

