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

