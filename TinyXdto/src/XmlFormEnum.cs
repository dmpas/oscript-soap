using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;

namespace TinyXdto
{
	[SystemEnum("ФормаXML", "XMLForm")]
	public class XmlFormEnum : EnumerationContext
	{
		private XmlFormEnum (TypeDescriptor typeRepresentation, TypeDescriptor valuesType)
			: base(typeRepresentation, valuesType)
		{
		}

		[EnumValue ("Атрибут", "Attribute")]
		public EnumerationValue Attribute
		{
			get
			{
				return this ["Attribute"];
			}
		}

		[EnumValue ("Текст", "Text")]
		public EnumerationValue Text
		{
			get
			{
				return this ["Text"];
			}
		}

		[EnumValue ("Элемент", "Element")]
		public EnumerationValue Element
		{
			get
			{
				return this ["Element"];
			}
		}

		public static XmlFormEnum CreateInstance ()
		{
			return EnumContextHelper.CreateEnumInstance<XmlFormEnum> ((t, v) => new XmlFormEnum (t, v));
		}

	}
}

