using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Web.Services.Description;
using System.Xml.Schema;
using TinyXdto;

namespace OneScript.Soap
{
	[ContextClass("WSПараметр", "WSParameter")]
	public class ParameterImpl : AutoContext<ParameterImpl>, IWithName
	{
		// TODO: проверить, сработает ли
		private static ParameterDirectionEnum parameterDirection = ParameterDirectionEnum.CreateInstance ();

		internal ParameterImpl (XmlSchemaElement element)
		{
			Name = element.Name;
			Nillable = element.IsNillable;
			ParameterDirection = parameterDirection.In;
			Documentation = "";
		}

		[ContextProperty("ВозможноПустой", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty("Документация", "Documentation")]
		public string Documentation { get; }

		[ContextProperty("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("НаправлениеПараметра", "ParameterDirection")]
		public EnumerationValue ParameterDirection { get; }

		[ContextProperty("Тип", "Type")]
		public IXdtoType Type { get; }
	}
}

