using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[SystemEnum("ВариантXDTO", "XDTOVariety")]
	public class XdtoVarietyEnum : EnumerationContext
	{
		internal XdtoVarietyEnum (TypeDescriptor typeRepresentation, TypeDescriptor valuesType)
			: base(typeRepresentation, valuesType)
		{
		}

		[EnumValue ("Аторманый", "Atomic")]
		public EnumerationValue Atomic {
			get { return this ["Atomic"]; }
		}

		[EnumValue ("Объединение", "Union")]
		public EnumerationValue Union {
			get { return this ["Union"]; }
		}

		[EnumValue ("Список", "List")]
		public EnumerationValue List {
			get { return this ["List"]; }
		}

		public static XdtoVarietyEnum CreateInstance ()
		{
			return EnumContextHelper.CreateEnumInstance<XdtoVarietyEnum> ((t, v) => new XdtoVarietyEnum (t, v));
		}
	}
}

