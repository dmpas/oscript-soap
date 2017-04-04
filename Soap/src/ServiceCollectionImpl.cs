using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Description;
using System.Linq;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияСервисов", "WSServiceCollection")]
	public class ServiceCollectionImpl : FixedCollectionOf<ServiceImpl>
	{
		ServiceCollectionImpl(IEnumerable<ServiceImpl> data) : base (data)
		{
		}

		internal static ServiceCollectionImpl Create(ServiceCollection collection, TinyXdto.XdtoFactoryImpl factory)
		{
			IList<ServiceImpl> list = new List<ServiceImpl>();
			foreach (Service service in collection)
			{
				list.Add(new ServiceImpl(service, factory));
			}
			return new ServiceCollectionImpl(list);
		}
	}
}

