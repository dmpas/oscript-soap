using System;
using System.Collections.Generic;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace OneScript.Soap
{
	[SystemEnum("WSНаправлениеПараметра", "WSParameterDirection")]
	public class ParameterDirectionEnum : EnumerationContext
	{

		private ParameterDirectionEnum(TypeDescriptor typeRepresentation, TypeDescriptor valuesType)
			: base(typeRepresentation, valuesType)
		{
		}

		[EnumValue("Входной", "In")]
		public EnumerationValue In
		{
			get
			{
				return this["In"];
			}
		}

		[EnumValue("ВходнойВыходной", "InOut")]
		public EnumerationValue InOut
		{
			get
			{
				return this["InOut"];
			}
		}

		[EnumValue("Выходной", "Out")]
		public EnumerationValue Out
		{
			get
			{
				return this["Out"];
			}
		}

		public static ParameterDirectionEnum CreateInstance()
		{
			return EnumContextHelper.CreateEnumInstance<ParameterDirectionEnum>((t, v) => new ParameterDirectionEnum(t, v));
		}
	}
}

