using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияОпераций", "WSOperationCollection")]
	public class OperationCollectionImpl : FixedCollectionOf<OperationImpl>
	{

		internal OperationCollectionImpl(IEnumerable<OperationImpl> data) : base(data)
		{
		}

	}
}

