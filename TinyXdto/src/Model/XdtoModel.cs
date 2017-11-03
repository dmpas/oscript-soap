using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Model", Namespace = "http://v8.1c.ru/8.1/xdto")]
	[XmlRoot("Model", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModel
	{
		[XmlElement("import", Namespace = "http://v8.1c.ru/8.1/xdto")]
		public XdtoModelImport[] Import;
	
		[XmlElement("package", Namespace = "http://v8.1c.ru/8.1/xdto")]
		public XdtoModelPackage[] Packages;
		
	}
}