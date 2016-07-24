using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияОпераций", "WSOperationCollection")]
	public class OperationCollectionImpl : FixedCollectionOf<OperationImpl>
	{

		internal OperationCollectionImpl(IEnumerable<OperationImpl> data) : base(data)
		{
		}

		internal static OperationCollectionImpl Create (OperationBindingCollection data)
		{
			var operations = new List<OperationImpl> ();

			foreach (var oOperation in data) {
				var operation = oOperation as OperationBinding;
				operations.Add (new OperationImpl (operation));
			}

			return new OperationCollectionImpl (operations);
		}

	}
}
