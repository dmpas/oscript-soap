/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
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
