using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;

namespace TinyXdto
{
	[ContextClass("КоллекцияТиповЗначенийXDTO", "XDTOValueTypeCollection")]
	public class XdtoValueTypeCollectionImpl : FixedCollectionOf<XdtoValueTypeImpl>
	{
		internal XdtoValueTypeCollectionImpl (IEnumerable<XdtoValueTypeImpl> data) : base(data)
		{
		}
	}
}

