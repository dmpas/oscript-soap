/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using ScriptEngine;

namespace TinyXdto
{
	[EnumerationType("ВидФасетаXDTO", "XDTOFacetType")]
	public enum XdtoFacetTypeEnum
	{
		[EnumItem("Длина")]
		Length,

		[EnumItem ("МаксВключающее")]
		MaxInclusive,

		[EnumItem ("МаксДлина")]
		MaxLength,

		[EnumItem ("МаксИсключающее")]
		MaxExclusive,

		[EnumItem ("МинВключающее")]
		MinInclusive,

		[EnumItem ("МинДлина")]
		MinLength,

		[EnumItem ("МинИсключающее")]
		MinExclusive,

		[EnumItem ("Образец")]
		Pattern,

		[EnumItem ("Перечисление")]
		Enumeration,

		[EnumItem ("ПробельныеСимволые")]
		Whitespace,

		[EnumItem ("РазрядовВсего")]
		TotalDigits,

		[EnumItem ("РазрядовДробнойЧасти")]
		FractionDigits
	}
}

