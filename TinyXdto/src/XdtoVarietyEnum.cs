/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using ScriptEngine;

namespace TinyXdto
{
	[EnumerationType("ВариантXDTO", "XDTOVariety")]
	public enum XdtoVarietyEnum
	{
		[EnumItem ("Аторманый", "Atomic")]
		Atomic,

		[EnumItem ("Объединение", "Union")]
		Union,

		[EnumItem ("Список", "List")]
		List
	}
}

