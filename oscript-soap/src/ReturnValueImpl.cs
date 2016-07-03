using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSВозвращаемоеЗначение", "WSReturnValue")]
	public class ReturnValueImpl : AutoContext<ReturnValueImpl>
	{
		internal ReturnValueImpl(OperationOutput returnValue)
		{
		}

		[ContextProperty("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Тип", "Type")]
		public IValue Type { get; }
	}
}

