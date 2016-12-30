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

