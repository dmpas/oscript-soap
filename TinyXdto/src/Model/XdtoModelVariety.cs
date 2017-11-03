using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Variety", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public enum XdtoModelVariety
	{
		[XmlEnum(Name = "Atomic")]
		Atomic,
		
		[XmlEnum(Name = "List")]
		List,
		
		[XmlEnum(Name = "Union")]
		Union
	}
}