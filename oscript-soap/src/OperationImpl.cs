using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	[ContextClass("WSОперация", "WSOperation")]
	public class OperationImpl : AutoContext<OperationImpl>
	{
		internal OperationImpl()
		{
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

