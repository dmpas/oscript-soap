/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System.Xml.Serialization;

namespace TinyXdto.Model
{
	[XmlType("Whitespace", Namespace = "http://v8.1c.ru/8.1/xdto")]
	public enum XdtoModelWhitespace
	{
		[XmlEnum(Name = "preserve")]
		Preserve,
		
		[XmlEnum(Name = "replace")]
		Replace,
		
		[XmlEnum(Name = "collapse")]
		Collapse
	}
}
