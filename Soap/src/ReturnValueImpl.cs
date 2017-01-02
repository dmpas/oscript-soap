using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSВозвращаемоеЗначение", "WSReturnValue")]
	public class ReturnValueImpl : AutoContext<ReturnValueImpl>
	{
		internal ReturnValueImpl (OperationOutput returnValue)
		{
			Type = ValueFactory.Create ();
			Documentation = returnValue.Documentation;
			MessagePartName = "";
		}

		internal ReturnValueImpl (IValue type = null,
		                          string messagePartName = "",
		                          bool nillable = false,
		                          string documentation = "")
		{
			Type = type;
			Nillable = nillable;
			Documentation = documentation;
			MessagePartName = messagePartName;
		}

		[ContextProperty("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Тип", "Type")]
		public IValue Type { get; }

		public string MessagePartName { get; }
	}
}

