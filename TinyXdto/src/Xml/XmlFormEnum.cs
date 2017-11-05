/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using ScriptEngine;
using System.Xml.Schema;

namespace TinyXdto
{
	[EnumerationType("ФормаXML", "XMLForm")]
	public enum XmlFormEnum
	{
		[EnumItem ("Атрибут", "Attribute")]
		Attribute,

		[EnumItem ("Текст", "Text")]
		Text,

		[EnumItem ("Элемент", "Element")]
		Element

	}
}

