using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[SystemEnum("ВидФасетаXDTO", "XDTOFacetType")]
	public class XdtoFacetTypeEnum : EnumerationContext
	{
		internal XdtoFacetTypeEnum (TypeDescriptor typeRepresentation, TypeDescriptor valuesType)
			: base(typeRepresentation, valuesType)
		{
		}


		public static XdtoFacetTypeEnum CreateInstance ()
		{
			return EnumContextHelper.CreateEnumInstance<XdtoFacetTypeEnum> ((t, v) => new XdtoFacetTypeEnum (t, v));
		}

	}
}

