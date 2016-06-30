using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияСервисов", "WSServiceCollection")]
	public class ServiceCollectionImpl : FixedCollectionOf<ServiceImpl>
	{
		ServiceCollectionImpl(IEnumerable<ServiceImpl> data) : base (data)
		{
		}
	}
}

