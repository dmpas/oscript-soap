/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using ScriptEngine.Machine.Contexts;
using ScriptEngine.Machine;
using System.Collections.Generic;
using System.Web.Services.Description;

namespace OneScript.Soap
{
	[ContextClass("WSКоллекцияОпераций", "WSOperationCollection")]
	public class OperationCollection : FixedCollectionOf<Operation>
	{

		internal OperationCollection(IEnumerable<Operation> data) : base(data)
		{
		}

		internal static OperationCollection Create (System.Web.Services.Description.OperationCollection data, TinyXdto.XdtoFactory factory)
		{
			var operations = new List<Operation> ();

			foreach (var oOperation in data) {
				var operation = oOperation as System.Web.Services.Description.Operation;
				operations.Add (new Operation (operation, factory));
			}

			return new OperationCollection (operations);
		}

	}
}
