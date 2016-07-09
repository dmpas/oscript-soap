using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;


namespace TinyXdto
{
	[SystemEnum("НазначениеТипаXML", "XMLTypeAssignment")]
	public class XmlTypeAssignmentEnum : EnumerationContext
	{
		private XmlTypeAssignmentEnum (TypeDescriptor typeRepresentation, TypeDescriptor valuesType)
			: base(typeRepresentation, valuesType)
		{
		}

		[EnumValue ("Неявное", "Implicit")]
		public EnumerationValue Implicit
		{
			get
			{
				return this ["Implicit"];
			}
		}

		[EnumValue ("Явное", "Explicit")]
		public EnumerationValue Explicit
		{
			get
			{
				return this ["Explicit"];
			}
		}

		public static XmlTypeAssignmentEnum CreateInstance ()
		{
			return EnumContextHelper.CreateEnumInstance<XmlTypeAssignmentEnum> ((t, v) => new XmlTypeAssignmentEnum (t, v));
		}
	}
}

