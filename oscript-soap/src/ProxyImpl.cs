using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	[ContextClass("WSПрокси", "WSProxy")]
	public class ProxyImpl : AutoContext<ProxyImpl> // TODO: Убрать AutoContext
	{
		// TODO: полный список аргументов
		public ProxyImpl(DefinitionsImpl definitions)
		{
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

		// TODO: полный список параметров
		[ScriptConstructor]
		public static IRuntimeContextInstance Constructor(DefinitionsImpl definitions)
		{
			return new ProxyImpl(definitions);
		}
	}
}

