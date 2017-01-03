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

		internal XdtoPropertyImpl (XdtoDataObjectImpl owner,
								  XmlFormEnum form,
								  string namespaceUri,
								  string localName)
		{
			NamespaceURI = namespaceUri;
			LocalName = localName;
			Form = form;
			OwnerObject = owner;
			UpperBound = -1;
		}

		[ContextProperty ("URIПространстваИмен", "NamespaceURI")]
		public string NamespaceURI { get; }

		[ContextProperty ("ВерхняяГраница", "UpperBound")]
		public int UpperBound { get; }

		[ContextProperty ("ВозможноПустое", "Nillable")]
		public bool Nillable { get; }

		[ContextProperty ("ЗначениеПоУмолчанию", "DefaultValue")]
		public XdtoDataValueImpl DefaultValue { get; }

		[ContextProperty ("Имя", "Name")]
		public string Name { get; }

		[ContextProperty("Квалифицированное", "Qualified")]
		public bool Qualified { get; }

		[ContextProperty ("ЛокальноеИмя", "LocalName")]
		public string LocalName { get; }

		[ContextProperty ("НижняяГраница", "LowerBound")]
		public int LowerBound { get; }

		[ContextProperty ("ОбъектВладелец", "OwnerObject")]
		public XdtoDataObjectImpl OwnerObject { get; }

		[ContextProperty ("Тип", "Type")]
		public IXdtoType Type { get; }

		[ContextProperty ("ТипВладелец", "OwnerType")]
		public IXdtoType OwnerType { get; }

		[ContextProperty ("Фиксированное", "Fixed")]
		public bool Fixed { get; }

		[ContextProperty ("Форма", "Form")]
		public XmlFormEnum Form { get; }

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoPropertyImpl;
			if (asThis == null)
				return false;

			// TODO: вменяемое сравнение (СвойствоXDTO.Equals())

			return string.Equals (NamespaceURI, asThis.NamespaceURI, StringComparison.Ordinal)
						 && string.Equals (LocalName, asThis.LocalName, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return (NamespaceURI?.GetHashCode () ?? 0) + (LocalName?.GetHashCode () ?? 0);
		}
	}
}

