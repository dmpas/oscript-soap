using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSПараметр", "WSParameter")]
	public class ParameterImpl : AutoContext<ParameterImpl>, IWithName
	{
		internal ParameterImpl ()
		{
		}

		[ContextProperty("ВозможноПустой", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("НаправлениеПараметра", "ParameterDirection")]
		public ParameterDirectionEnum ParameterDirection { get; }

		[ContextProperty("Тип", "Type")]
		public IValue Type { get; }
	}
}

