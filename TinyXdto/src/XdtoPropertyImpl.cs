using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[ContextClass("СвойствоXDTO", "XDTOProperty")]
	public class XdtoPropertyImpl : AutoContext<XdtoPropertyImpl>
	{
		internal XdtoPropertyImpl ()
		{
		}

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty ("ВерхняяГраница", "UpperBound")]
		public int UpperBound { get; }

		[ContextProperty ("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty ("ЗначениеПоУмолчанию", "DefaultValue")]
		public IValue DefaultValue { get; }

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextProperty ("ЛокальноеИмя", "LocaName")]
		public string LocaName { get; }

		[ContextProperty ("НижняяГраница", "LowerBound")]
		public int LowerBound { get; }

		[ContextProperty ("ОбъектВладелец", "OwnerObject")]
		public IValue OwnerObject { get; }

		[ContextProperty ("Тип", "Type")]
		public IValue Type { get; }

		[ContextProperty ("ТипВладелец", "OwnerType")]
		public IValue OwnerType { get; }

		[ContextProperty ("Фиксированное", "Fixed")]
		public bool Fixed { get; }

		[ContextProperty ("Форма", "Form")]
		public XmlFormEnum Form { get; }

	}
}

