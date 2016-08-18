using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Web.Services.Description;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSОперация", "WSOperation")]
	public class OperationImpl : AutoContext<OperationImpl>, IWithName
	{
		internal OperationImpl(Operation operation)
		{
			Name = operation.Name;
			Documentation = operation.Documentation;
			ReturnValue = new ReturnValueImpl (operation.Messages.Output);

			Parameters = ParameterCollectionImpl.Create (operation.Messages.Input);
		}

		[ContextProperty("ВозвращаемоеЗначение", "ReturnValue")]
		public ReturnValueImpl ReturnValue { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Параметры", "Parameters")]
		public ParameterCollectionImpl Parameters { get; }
	}
}
