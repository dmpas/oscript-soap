/*----------------------------------------------------------
This Source Code Form is subject to the terms of the 
Mozilla Public License, v.2.0. If a copy of the MPL 
was not distributed with this file, You can obtain one 
at http://mozilla.org/MPL/2.0/.
----------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace OneScript.Soap
{
	public class MessagePartProxy
	{
		public IList<Parameter> Parameters { get; set; }
		public string Name { get; set; }
		public string ElementName { get; set; }
		public string NamespaceUri { get; set; }
	}
}

