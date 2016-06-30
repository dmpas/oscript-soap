using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.IO;
using System.Web.Services.Description;
using System.Xml;

namespace OneScript.Soap
{
	[ContextClass ("WSОпределения", "WSDefinitions")]
	public class DefinitionsImpl : AutoContext<DefinitionsImpl>
	{

		private ServiceDescription _wsd;
		private string _wsdl;
		private string _userName;
		private string _password;
		private IValue _internetProxy;
		private decimal _timeout;
		private IValue _securedConnection;

		public DefinitionsImpl (string wsdl,
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

			// TODO: Место для волшебства

		}

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Сервисы", "Services")]
		public ServiceCollectionImpl Services { get; }

		[ContextProperty("ФабрикаXDTO", "XDTOFactory")]
		public IValue XdtoFactory { get; }

		// TODO: использовать InternetProxy, timeout и SecuredConnection
		private ServiceDescription ReachWsdl()
		{
			Stream stream = WsdlConnector.ReachWsdl(_wsdl, _userName, _password);
			return ServiceDescription.Read(stream);
		}

		public static IRuntimeContextInstance Constructor (string wsdl,
		                                                   string userName = null,
		                                                   string password = null,
		                                                   IValue internetProxy = null,
		                                                   decimal timeout = 0,
		                                                   IValue securedConnection = null
		)
		{
			var definitions = new DefinitionsImpl(wsdl, userName, password, internetProxy, timeout, securedConnection);
			return definitions;
		}
	}
}

