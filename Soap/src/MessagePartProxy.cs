using System;
using System.Collections.Generic;

namespace OneScript.Soap
{
	public class MessagePartProxy
	{
		public IList<ParameterImpl> Parameters { get; set; }
		public string Name { get; set; }
		public string ElementName { get; set; }
		public string NamespaceUri { get; set; }
	}
}

