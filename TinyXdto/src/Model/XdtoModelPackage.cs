/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Package", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelPackage
	{
		[XmlAttribute("targetNamespace")]
		public string TargetNamespace;
	
		[XmlAttribute("elementFormQualified")]
		public bool ElementFormQualified;
		
		[XmlAttribute("attributeFormQualified")]
		public bool AttributeFormQualified;
		
		[XmlElement("import")]
		public XdtoModelImport[] Imports;

		[XmlElement("objectType", typeof(XdtoModelObjectType))]
		[XmlElement("valueType", typeof(XdtoModelValueType))]
		public XdtoModelAbstractType[] Types;
	}
}
