using ScriptEngine;
using System.Xml.Schema;

namespace TinyXdto
{
	[EnumerationType("ФормаXML", "XMLForm")]
	public enum XmlFormEnum
	{
		[EnumItem ("Атрибут", "Attribute")]
		Attribute,

		[EnumItem ("Текст", "Text")]
		Text,

		[EnumItem ("Элемент", "Element")]
		Element

	}
}

