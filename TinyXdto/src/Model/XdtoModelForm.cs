using System.Xml.Serialization;
 
 namespace TinyXdto.Model
 {
	 [XmlType("Form", Namespace = "http://v8.1c.ru/8.1/xdto")]
 	public enum XdtoModelForm
 	{
 		[XmlEnum(Name =  "Attribute")]
 		Attribute,
 		
 		[XmlEnum(Name =  "Element")]
 		Element,
 		
 		[XmlEnum(Name =  "Text")]
 		Text
 	}
 }