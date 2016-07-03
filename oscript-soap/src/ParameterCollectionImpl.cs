using System;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияПараметров", "WSParameterCollection")]
	public class ParameterCollectionImpl : FixedCollectionOf<ParameterImpl>
	{

		internal ParameterCollectionImpl (IEnumerable<ParameterImpl> data) : base(data)
		{
		}

	}
}

