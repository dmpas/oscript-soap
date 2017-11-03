using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Whitespace", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public enum XdtoModelWhitespace
	{
		[XmlEnum(Name = "preserve")]
		Preserve,
		
		[XmlEnum(Name = "replace")]
		Replace,
		
		[XmlEnum(Name = "collapse")]
		Collapse
	}
}