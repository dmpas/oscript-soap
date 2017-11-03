using ScriptEngine;

namespace OneScript.Soap
{
	[EnumerationType("WSНаправлениеПараметра", "WSParameterDirection")]
	public enum ParameterDirectionEnum
	{

		[EnumItem("Входной", "In")]
		In,

		[EnumItem("ВходнойВыходной", "InOut")]
		InOut,

		[EnumItem("Выходной", "Out")]
		Out

	}
}

