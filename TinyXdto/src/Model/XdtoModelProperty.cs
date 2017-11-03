using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Property", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelProperty
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("ref")]
		public string Ref;

		[XmlAttribute("type")]
		public string Type;

		[XmlAttribute("lowerBound")]
		public int LowerBound;

		[XmlAttribute("upperBound")]
		public int UpperBound;

		[XmlAttribute("nillable")]
		public bool Nillable;

		[XmlAttribute("fixed")]
		public bool Fixed;

		[XmlAttribute("default")]
		public string Default;

		[XmlAttribute("form")]
		public XdtoModelForm Form;

		[XmlAttribute("localName")]
		public string LocalName;

		[XmlElement("typeDef")]
		public XdtoModelAbstractType TypeDef;

		[XmlAttribute("qualified")]
		public bool Qualified;
	}
}