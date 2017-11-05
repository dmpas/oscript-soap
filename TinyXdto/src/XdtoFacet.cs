/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace TinyXdto
{
	[ContextClass("ФасетXDTO", "XDTOFacet")]
	public class XdtoFacet : AutoContext<XdtoFacet>
	{

		internal XdtoFacet (XdtoFacetTypeEnum type, string value)
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
			var asThis = obj as XdtoFacet;
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
