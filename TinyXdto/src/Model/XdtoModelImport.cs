using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Import", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelImport
	{
		[XmlAttribute("namespace", DataType = "anyURI")]
		public string Namespace;
	}
}