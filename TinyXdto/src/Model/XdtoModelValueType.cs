/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("ValueType", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public class XdtoModelValueType : XdtoModelAbstractType
	{
		[XmlAttribute("variety")]
		public XdtoModelVariety Variety;

		[XmlAttribute("itemTyoe")]
		public string ItemType;

		[XmlAttribute("memberTypes")]
		public string MemberTypes;

		[XmlAttribute("length")]
		public int Length;
		
		[XmlAttribute("minLength")]
		public int MinLength;
		
		[XmlAttribute("maxLength")]
		public int MaxLength;

		[XmlAttribute("whiteSpace")]
		public XdtoModelWhitespace Whitespace;
		
		[XmlAttribute("minInclusive")]
		public decimal MinInclusive;
		
		[XmlAttribute("maxInclusive")]
		public decimal MaxInclusive;
		
		[XmlAttribute("minExclusive")]
		public decimal MinExclusive;
		
		[XmlAttribute("maxExclusive")]
		public decimal MaxExclusive;

		[XmlAttribute("totalDigits")]
		public int TotalDigits;

		[XmlAttribute("fractionDigits")]
		public int FractionDigits;

		[XmlElement("typeDef")]
		public XdtoModelValueType[] TypeDef;

		[XmlElement("pattern")]
		public string[] Pattern;
		
		[XmlElement("enumeration")]
		public string[] Enumeration;
	}
}
