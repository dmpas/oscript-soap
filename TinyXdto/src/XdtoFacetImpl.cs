using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ФасетXDTO", "XDTOFacet")]
	public class XdtoFacetImpl : AutoContext<XdtoFacetImpl>
	{

		internal XdtoFacetImpl (XdtoFacetTypeEnum type, string value)
		{
			Type = type;
			Value = value;
		}

		[ContextProperty("Вид", "Type")]
		public XdtoFacetTypeEnum Type { get; }

		[ContextProperty("Значение", "Value")]
		public string Value { get; }

		public override bool Equals (object obj)
		{
			var asThis = obj as XdtoFacetImpl;
			if (asThis == null)
				return false;
			
			return asThis.Type == Type
						 && asThis.Value.Equals (Value, StringComparison.Ordinal);
		}

		public override int GetHashCode ()
		{
			return Type.GetHashCode () + Value.GetHashCode ();
		}
	}
}
