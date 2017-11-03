using System.Xml.Serialization;

namespace TinyXdto.Model
{
	public abstract class XdtoModelAbstractType
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("base")]
		public string Base;
	}
}