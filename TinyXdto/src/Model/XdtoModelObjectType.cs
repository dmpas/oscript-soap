using System.Xml;
using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("ObjectType", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelObjectType : XdtoModelAbstractType
	{
		[XmlAttribute("open")]
		public bool Open;
		
		[XmlAttribute("abstract")]
		public bool Abstract;
		
		[XmlAttribute("mixed")]
		public bool Mixed;
		
		[XmlAttribute("ordered")]
		public bool Ordered;
		
		[XmlAttribute("sequenced")]
		public bool Sequenced;

		[XmlElement("property")]
		public XdtoModelProperty[] Property;
	}
}