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
using System.Linq;

namespace TinyXdto
{
	[ContextClass("КоллекцияПакетовXDTO", "XDTOPackageCollection")]
	public class XdtoPackageCollection : FixedCollectionOf<XdtoPackage>
	{
		internal XdtoPackageCollection (IEnumerable<XdtoPackage> data) : base(data)
		{
		}

		public XdtoPackage Get (string namespaceUri)
		{
			return this.FirstOrDefault((p) => p.NamespaceUri.Equals(namespaceUri, StringComparison.Ordinal));
		}

		[ContextMethod("Получить", "Get")]
		public new XdtoPackage Get (IValue index)
		{
			if (index.DataType == DataType.Number)
				return this.Get ((int)index.AsNumber ());

			if (index.DataType == DataType.String)
				return Get (index.AsString ());
			
			throw RuntimeException.InvalidArgumentType (nameof (index));
		}
	}
}
