using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ФасетXDTO", "XDTOFacet")]
	public class XdtoFacetImpl : AutoContext<XdtoFacetImpl>
	{

		private static readonly XdtoFacetTypeEnum FacetType = XdtoFacetTypeEnum.CreateInstance ();

		internal XdtoFacetImpl (XdtoFacetTypeEnum type, string value)
		{
			// Type = FacetType.
			Value = value;
		}

		[ContextProperty("Вид", "Type")]
		public EnumerationValue Type { get; }

		[ContextProperty("Значение", "Value")]
		public string Value { get; }
	}
}
