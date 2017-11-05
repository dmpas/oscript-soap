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
