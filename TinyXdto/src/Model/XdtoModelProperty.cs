/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System.Xml;
using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Property", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelProperty
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("ref")]
		public XmlQualifiedName Ref;

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
